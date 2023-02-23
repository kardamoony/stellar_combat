using Newtonsoft.Json;

namespace StaticHelpers;

public static class NetworkingHelper
{
    public static byte[] ObjectToBytes(object obj)
    {
        var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        var jsonStr = JsonConvert.SerializeObject(obj, settings);
        return System.Text.Encoding.UTF8.GetBytes(jsonStr);
    }
}