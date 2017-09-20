using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchLib.Internal;

namespace TwitchLib.Api
{
    public class Games : IFluentInterface
    {
        public v3 V3 { get; }
        public v5 V5 { get; }

        public Games(Settings settings, Requests requests)
        {
            V3 = new v3(settings, requests);
            V5 = new v5(settings, requests);
        }

        public class v3 : ApiMethod
        {
            public v3(Settings settings, Requests requests) : base(settings, requests)
            {
            }
            #region GetTopGames
            public async Task<Models.API.v3.Games.TopGamesResponse> GetTopGamesAsync(int limit = 10, int offset = 0)
            {
                string paramsStr = $"?limit={limit}&offset={offset}";
                return await Requests.GetGenericAsync<Models.API.v3.Games.TopGamesResponse>($"https://api.twitch.tv/kraken/games/top{paramsStr}", null, Requests.API.v3);
            }
            #endregion
        }

        public class v5 : ApiMethod
        {
            public v5(Settings settings, Requests requests) : base(settings, requests)
            {
            }
            #region GetTopGames
            public async Task<Models.API.v5.Games.TopGames> GetTopGamesAsync(int? limit = null, int? offset = null)
            {
                List<KeyValuePair<string, string>> queryParameters = new List<KeyValuePair<string, string>>();
                if (limit != null)
                    queryParameters.Add(new KeyValuePair<string, string>("limit", limit.ToString()));
                if (offset != null)
                    queryParameters.Add(new KeyValuePair<string, string>("offset", offset.ToString()));

                string optionalQuery = string.Empty;
                if (queryParameters.Count > 0)
                {
                    for (int i = 0; i < queryParameters.Count; i++)
                    {
                        if (i == 0) { optionalQuery = $"?{queryParameters[i].Key}={queryParameters[i].Value}"; }
                        else { optionalQuery += $"&{queryParameters[i].Key}={queryParameters[i].Value}"; }
                    }
                }
                return await Requests.GetGenericAsync<Models.API.v5.Games.TopGames>($"https://api.twitch.tv/kraken/games/top{optionalQuery}", null, Requests.API.v5);
            }
            #endregion
        }
    }
}