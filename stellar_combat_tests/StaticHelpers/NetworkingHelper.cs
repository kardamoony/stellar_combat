using Newtonsoft.Json;

namespace StaticHelpers;

public static class NetworkingHelper
{
    public static byte[] ObjectToBytes(object obj)
    {
        var jsonStr = JsonConvert.SerializeObject(obj);
        return System.Text.Encoding.UTF8.GetBytes(jsonStr);
    }
}