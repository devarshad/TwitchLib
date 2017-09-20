using System.Threading.Tasks;
using TwitchLib.Internal;

namespace TwitchLib.Api
{
    public class Clips : IFluentInterface
    {
        public v5 V5 { get; }

        public Clips(Settings settings, Requests requests)
        {
            V5 = new v5(settings, requests);
        }

        public class v5 : ApiMethod
        {
            public v5(Settings settings, Requests requests) : base(settings, requests)
            {
            }
            #region GetClip
            public async Task<Models.API.v5.Clips.Clip> GetClipAsync(string slug)
            {
                return await Requests.GetGenericAsync<Models.API.v5.Clips.Clip>($"https://api.twitch.tv/kraken/clips/{slug}", null);
            }
            #endregion
            #region GetTopClips
            public async Task<Models.API.v5.Clips.TopClipsResponse> GetTopClipsAsync(string channel = null, string cursor = null, string game = null, long limit = 10, Models.API.v5.Clips.Period period = Models.API.v5.Clips.Period.Week, bool trending = false)
            {
                string paramsStr = $"?limit={limit}";
                if (channel != null)
                    paramsStr += $"&channel={channel}";
                if (cursor != null)
                    paramsStr += $"&cursor={cursor}";
                if (game != null)
                    paramsStr += $"&game={game}";
                if (trending)
                    paramsStr += "&trending=true";
                else
                    paramsStr += "&trending=false";
                switch (period)
                {
                    case Models.API.v5.Clips.Period.All:
                        paramsStr += "&period=all";
                        break;
                    case Models.API.v5.Clips.Period.Month:
                        paramsStr += "&period=month";
                        break;
                    case Models.API.v5.Clips.Period.Week:
                        paramsStr += "&period=week";
                        break;
                    case Models.API.v5.Clips.Period.Day:
                        paramsStr += "&period=day";
                        break;
                }

                return await Requests.GetGenericAsync<Models.API.v5.Clips.TopClipsResponse>($"https://api.twitch.tv/kraken/clips/top{paramsStr}", null);
            }
            #endregion
            #region GetFollowedClips
            public async Task<Models.API.v5.Clips.FollowClipsResponse> GetFollowedClipsAsync(long limit = 10, string cursor = null, bool trending = false, string authToken = null)
            {
                string paramsStr = $"?limit={limit}";
                if (cursor != null)
                    paramsStr += $"&cursor={cursor}";
                if (trending)
                    paramsStr += "&trending=true";
                else
                    paramsStr += "&trending=false";

                return await Requests.GetGenericAsync<Models.API.v5.Clips.FollowClipsResponse>($"https://api.twitch.tv/kraken/clips/followed{paramsStr}", authToken);
            }
            #endregion
        }

    }
}