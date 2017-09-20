using Newtonsoft.Json;

namespace TwitchLib.Api
{
    /// <summary>These methods are intended to aid in developing the library.</summary>
    public static class Debugging
    {
        public static T BuildModel<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}