using System.Threading.Tasks;

namespace TwitchLib.Api
{
    public class Ingests : IFluentInterface
    {
        public Ingests(Settings settings, Requests requests)
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
            #region GetIngests
            public async Task<Models.API.v3.Ingests.IngestsResponse> GetIngestsAsync()
            {
                return await Requests.GetGenericAsync<Models.API.v3.Ingests.IngestsResponse>("https://api.twitch.tv/kraken/ingests", null, Requests.API.v3);
            }
            #endregion
        }

        public class v5 : ApiMethod
        {
            public v5(Settings settings, Requests requests) : base(settings, requests)
            {
            }
            #region GetIngestServerList
            public async Task<Models.API.v5.Ingests.Ingests> GetIngestServerListAsync()
            {
                return await Requests.GetGenericAsync<Models.API.v5.Ingests.Ingests>("https://api.twitch.tv/kraken/ingests", null, Requests.API.v5);
            }
            #endregion
        }
    }
}