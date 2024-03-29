﻿using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;
using IoC.Interfaces;

namespace IoC.CodeGeneration
{
    public static class AdapterGenerator
    {
        private const string AssemblyName = "AutoGeneratedAsm";
        private const string ModuleName = "AutoGenerated.dll";

        private static readonly ModuleBuilder _module;
        private static readonly ConcurrentDictionary<string, Type> _adapterTypes;

        static AdapterGenerator()
        {
            var assemblyName = new AssemblyName
            {
                Name = AssemblyName
            };

            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);
            _module = assemblyBuilder.DefineDynamicModule(ModuleName);
            _adapterTypes = new ConcurrentDictionary<string, Type>();
        }

        public static Type GenerateAdapterType<T>()
        {
            var interfaceType = typeof(T);

            if (!interfaceType.IsInterface)
            {
                throw new ArgumentException("Generic type argument must be an interface");
            }
            
            var typeName = $"{typeof(T).Name}Adapter";

            if (_adapterTypes.TryGetValue(typeName, out var type))
            {
                return type;
            }

            var typeBuilder = _module.DefineType(typeName, TypeAttributes.Class);
            var adapteeField = typeBuilder.DefineField("_adaptee", typeof(object), FieldAttributes.Private | FieldAttributes.InitOnly);
            
            typeBuilder.AddInterfaceImplementation(interfaceType);
            
            DefineConstructor(typeBuilder, adapteeField);
            DefineProperties(typeBuilder, interfaceType, adapteeField, out var propertyAccessors);
            DefineMethods(typeBuilder, interfaceType, propertyAccessors);

            type = typeBuilder.CreateType();
            _adapterTypes.TryAdd(typeName, type);

            return type;
        }
        
        private static void DefineConstructor(TypeBuilder typeBuilder, FieldBuilder adapteeField)
        {
            var ctorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public, 
                CallingConventions.Standard,
                new[] { typeof(object) });

            var ctorIl = ctorBuilder.GetILGenerator();
             
            ctorIl.Emit(OpCodes.Ldarg_0); //this
            ctorIl.Emit(OpCodes.Ldarg_1); //adaptee object
            ctorIl.Emit(OpCodes.Stfld, adapteeField); //setup field
            ctorIl.Emit(OpCodes.Ret); //return
        }
        
        private static void DefineProperties(TypeBuilder typeBuilder, Type baseType, FieldBuilder adapteeField, out List<MethodInfo> propertyAccessors)
        {
            propertyAccessors = new List<MethodInfo>();
            var properties = baseType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            var resolveMethod = typeof(Container).GetMethod(nameof(Container.Resolve));
            
            var getSetAttributes = MethodAttributes.Final |
                                   MethodAttributes.HideBySig |
                                   MethodAttributes.NewSlot |
                                   MethodAttributes.SpecialName | 
                                   MethodAttributes.Public |
                                   MethodAttributes.Virtual;

            foreach (var property in properties)
            {
                var propertyBuilder = typeBuilder.DefineProperty(property.Name, property.Attributes, property.PropertyType, null);

                var getMethodInfo = property.GetGetMethod();
                var setMethodInfo = property.GetSetMethod();
    
                if (getMethodInfo != null)
                {
                    var methodName = getMethodInfo.Name;
                    var methodBuilder = typeBuilder.DefineMethod(methodName, getSetAttributes, property.PropertyType, null);

                    var genericResolveMethod = resolveMethod.MakeGenericMethod(property.PropertyType);
                    
                    var ilGenerator = methodBuilder.GetILGenerator();

                    ilGenerator.Emit(OpCodes.Ldstr, $"{baseType.Name}.{property.Name}.Get");
                    ilGenerator.Emit(OpCodes.Ldc_I4_1);
                    ilGenerator.Emit(OpCodes.Newarr, typeof(object));
                    ilGenerator.Emit(OpCodes.Dup);
                    ilGenerator.Emit(OpCodes.Ldc_I4_0);
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldfld, adapteeField);
                    ilGenerator.Emit(OpCodes.Stelem_Ref);
                    ilGenerator.Emit(OpCodes.Call, genericResolveMethod);
                    ilGenerator.Emit(OpCodes.Ret);
                    
                    propertyBuilder.SetGetMethod(methodBuilder);
                    typeBuilder.DefineMethodOverride(methodBuilder, getMethodInfo);
                    propertyAccessors.Add(getMethodInfo);
                }

                if (setMethodInfo != null)
                {
                    var methodName = setMethodInfo.Name;
                    var methodBuilder = typeBuilder.DefineMethod(methodName, getSetAttributes, typeof(void), new []{property.PropertyType});
                    var genericResolveMethod = resolveMethod.MakeGenericMethod(typeof(ICommand));
                    var executeMethod = typeof(ICommand).GetMethod(nameof(ICommand.Execute));

                    var ilGenerator = methodBuilder.GetILGenerator();

                    ilGenerator.Emit(OpCodes.Ldstr, $"{baseType.Name}.{property.Name}.Set");
                    ilGenerator.Emit(OpCodes.Ldc_I4_2);
                    ilGenerator.Emit(OpCodes.Newarr, typeof(object));
                    ilGenerator.Emit(OpCodes.Dup);
                    ilGenerator.Emit(OpCodes.Ldc_I4_0);
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldfld, adapteeField);
                    ilGenerator.Emit(OpCodes.Stelem_Ref);
                    ilGenerator.Emit(OpCodes.Dup);
                    ilGenerator.Emit(OpCodes.Ldc_I4_1);
                    ilGenerator.Emit(OpCodes.Ldarg_1);
                    ilGenerator.Emit(OpCodes.Box, property.PropertyType);
                    ilGenerator.Emit(OpCodes.Stelem_Ref);
                    ilGenerator.Emit(OpCodes.Call, genericResolveMethod);
                    ilGenerator.Emit(OpCodes.Callvirt, executeMethod);
                    ilGenerator.Emit(OpCodes.Nop);
                    ilGenerator.Emit(OpCodes.Ret);
                    
                    propertyBuilder.SetSetMethod(methodBuilder);
                    typeBuilder.DefineMethodOverride(methodBuilder, setMethodInfo);
                    propertyAccessors.Add(setMethodInfo);
                }
            }
        }

        private static void DefineMethods(TypeBuilder typeBuilder, Type baseType, List<MethodInfo> propertyAccessors)
        {
            var methods = baseType.GetMethods();

            var attributes = MethodAttributes.Final |
                             MethodAttributes.HideBySig |
                             MethodAttributes.NewSlot |
                             MethodAttributes.Virtual |
                             MethodAttributes.SpecialName;
        
            foreach (var method in methods)
            {
                if (propertyAccessors.Contains(method))
                {
                    continue;
                }
                
                var parameterTypes = method.GetParameters().Select(p => p.ParameterType);
                var methodBuilder = typeBuilder.DefineMethod(method.Name, attributes, method.ReturnType, parameterTypes.ToArray());
                
                var ilGenerator = methodBuilder.GetILGenerator();
                
                ilGenerator.Emit(OpCodes.Nop);
                ilGenerator.Emit(OpCodes.Newobj, typeof(NotImplementedException).GetConstructors()[0]);
                ilGenerator.Emit(OpCodes.Throw);
                
                typeBuilder.DefineMethodOverride(methodBuilder, method);
            }
        }
    }
}

