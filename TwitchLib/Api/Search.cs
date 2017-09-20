using System.Collections.Generic;
using System.Threading.Tasks;

namespace TwitchLib.Api
{
    public class Search : IFluentInterface
    {
        public v3 V3 { get; }
        public v5 V5 { get; }

        public Search(Settings settings, Requests requests)
        {
            V3 = new v3(settings, requests);
            V5 = new v5(settings, requests);
        }

        public class v3 : ApiMethod
        {
            public v3(Settings settings, Requests requests) : base(settings, requests)
            {
            }
            #region SearchChannels
            public async Task<Models.API.v3.Search.SearchChannelsResponse> SearchChannelsAsync(string query, int limit = 25, int offset = 0)
            {
                string paramsStr = $"?query={query}&limit={limit}&offset={0}";
                return await Requests.GetGenericAsync<Models.API.v3.Search.SearchChannelsResponse>($"https://api.twitch.tv/kraken/search/channels{paramsStr}", null, Requests.API.v3);
            }
            #endregion
            #region SearchStreams
            public async Task<Models.API.v3.Search.SearchStreamsResponse> SearchStreamsAsync(string query, int limit = 25, int offset = 0, bool? hls = null)
            {
                string opHls = "";
                if (hls != null)
                {
                    if ((bool)hls)
                        opHls = "&hls=true";
                    else
                        opHls = "&hls=false";
                }

                string paramsStr = $"?query={query}&limit={limit}&offset={offset}{opHls}";
                return await Requests.GetGenericAsync<Models.API.v3.Search.SearchStreamsResponse>($"https://api.twitch.tv/kraken/search/streams{paramsStr}", null, Requests.API.v3);
            }
            #endregion
            #region SearchGames
            public async Task<Models.API.v3.Search.SearchGamesResponse> SearchGamesAsync(string query, Enums.GameSearchType type = Enums.GameSearchType.Suggest, bool live = false)
            {
                string paramsStr = $"?query={query}&live={live.ToString().ToLower()}";
                switch (type)
                {
                    case Enums.GameSearchType.Suggest:
                        paramsStr += $"&type=suggest";
                        break;
                }

                return await Requests.GetGenericAsync<Models.API.v3.Search.SearchGamesResponse>($"https://api.twitch.tv/kraken/search/games{paramsStr}", null, Requests.API.v3);
            }
            #endregion
        }

        public class v5 : ApiMethod
        {
            public v5(Settings settings, Requests requests) : base(settings, requests)
            {
            }
            #region SearchChannels
            public async Task<Models.API.v5.Search.SearchChannels> SearchChannelsAsync(string encodedSearchQuery, int? limit = null, int? offset = null)
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
                return await Requests.GetGenericAsync<Models.API.v5.Search.SearchChannels>($"https://api.twitch.tv/kraken/search/channels?query={encodedSearchQuery}{optionalQuery}", null, Requests.API.v5);
            }
            #endregion
            #region SearchGames
            public async Task<Models.API.v5.Search.SearchGames> SearchGamesAsync(string encodedSearchQuery, bool? live = null)
            {
                string optionalQuery = (live != null) ? $"?live={live}" : string.Empty;
                return await Requests.GetGenericAsync<Models.API.v5.Search.SearchGames>($"https://api.twitch.tv/kraken/search/games?query={encodedSearchQuery}{optionalQuery}", null, Requests.API.v5);
            }
            #endregion
            #region SearchStreams
            public async Task<Models.API.v5.Search.SearchStreams> SearchStreamsAsync(string encodedSearchQuery, int? limit = null, int? offset = null, bool? hls = null)
            {
                List<KeyValuePair<string, string>> queryParameters = new List<KeyValuePair<string, string>>();
                if (limit != null)
                    queryParameters.Add(new KeyValuePair<string, string>("limit", limit.ToString()));
                if (offset != null)
                    queryParameters.Add(new KeyValuePair<string, string>("offset", offset.ToString()));
                if (hls != null)
                    queryParameters.Add(new KeyValuePair<string, string>("hls", hls.ToString()));

                string optionalQuery = string.Empty;
                if (queryParameters.Count > 0)
                {
                    for (int i = 0; i < queryParameters.Count; i++)
                    {
                        if (i == 0) { optionalQuery = $"?{queryParameters[i].Key}={queryParameters[i].Value}"; }
                        else { optionalQuery += $"&{queryParameters[i].Key}={queryParameters[i].Value}"; }
                    }
                }
                return await Requests.GetGenericAsync<Models.API.v5.Search.SearchStreams>($"https://api.twitch.tv/kraken/search/streams?query={encodedSearchQuery}{optionalQuery}", null, Requests.API.v5);
            }
            #endregion
        }
    }
}