using System.Threading.Tasks;

namespace TwitchLib.Api
{
    public class Root : IFluentInterface
    {
        public Root(Settings settings, Requests requests)
        {
            V3 = new v3(settings, requests);
            V5 = new v5(settings, requests);
        }

        public v3 V3 { get; }
        public v5 V5 { get; }

        public class v3 : ApiMethod
        {
            public v3(Settings settings, Requests requests) : base(settings, requests)
            {
            }
            #region Root
            public async Task<Models.API.v3.Root.RootResponse> GetRootAsync(string accessToken = null, string clientId = null)
            {
                return await Requests.GetGenericAsync<Models.API.v3.Root.RootResponse>("https://api.twitch.tv/kraken", accessToken, Requests.API.v3, clientId);
            }
            #endregion
        }

        public class v5 : ApiMethod
        {
            public v5(Settings settings, Requests requests) : base(settings, requests)
            {
            }
            #region GetRoot
            public Models.API.v5.Root.Root GetRoot(string authToken = null, string clientId = null)
            {
                return Requests.GetGenericAsync<Models.API.v5.Root.Root>("https://api.twitch.tv/kraken", authToken, Requests.API.v5, clientId).Result;
            }
            #endregion
        }

    }
}