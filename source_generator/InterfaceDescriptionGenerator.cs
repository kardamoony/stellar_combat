﻿using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SourceGenerator
{
    [Generator]
    public class InterfaceDescriptionGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            //no initialization needed
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var sb = new StringBuilder();
            sb.AppendLine("// <auto-generated/>");
            sb.AppendLine("public static class InterfacesDescription");
            sb.AppendLine("{");

            foreach (var syntaxTree in context.Compilation.SyntaxTrees)
            {
                foreach (var interfaceDeclaration in syntaxTree.GetRoot().DescendantNodes().OfType<InterfaceDeclarationSyntax>())
                {
                    var interfaceName = interfaceDeclaration.Identifier.Text;

                    foreach (var property in interfaceDeclaration.Members.OfType<PropertyDeclarationSyntax>())
                    {
                        var propertyName = property.Identifier.Text;
                        
                        if (property.AccessorList == null) continue;

                        foreach (var accessor in property.AccessorList.Accessors)
                        {
                            switch (accessor.Kind())
                            {
                                case SyntaxKind.GetAccessorDeclaration:
                                {
                                    var variableName = $"{interfaceName}{propertyName}Get";
                                    var variableValue = $"{interfaceName}.{propertyName}.Get";
                                    
                                    sb.AppendLine($"\tpublic static readonly string {variableName} = \"{variableValue}\";");
                                    break;
                                }
                                case SyntaxKind.SetAccessorDeclaration:
                                {
                                    var variableName = $"{interfaceName}{propertyName}Set";
                                    var variableValue = $"{interfaceName}.{propertyName}.Set";
                                    
                                    sb.AppendLine($"\tpublic static readonly string {variableName} = \"{variableValue}\";");
                                    break;
                                }
                            }
                        }
                    }
                    
                }
            }
            
            sb.AppendLine("}");
            context.AddSource($"InterfacesDescription.Generated.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }
    }
}
