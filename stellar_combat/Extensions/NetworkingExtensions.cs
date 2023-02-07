using System.Text;
using Newtonsoft.Json;

namespace StellarCombat.Extensions
{
    public static class NetworkingExtensions
    {
        public static T Deserialize<T>(this byte[] bytes)
        {
            var jsonStr = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }
    }
}

