using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchLib.Internal;

namespace TwitchLib.Api
{
    public class Chat : IFluentInterface
    {
        public v3 V3 { get; }
        public v5 V5 { get; }

        public Chat(Settings settings, Requests requests)
        {
            V3 = new v3(settings, requests);
            V5 = new v5(settings, requests);
        }

        public class v3 : ApiMethod
        {
            public v3(Settings settings, Requests requests) : base(settings, requests)
            {
            }
            #region GetBadges
            public async Task<Models.API.v3.Chat.BadgesResponse> GetBadgesAsync(string channel)
            {
                return await Requests.GetGenericAsync<Models.API.v3.Chat.BadgesResponse>($"https://api.twitch.tv/kraken/chat/{channel}/badges", null, Requests.API.v3);
            }
            #endregion
            #region GetAllEmoticons
            public async Task<Models.API.v3.Chat.AllEmoticonsResponse> GetAllEmoticonsAsync()
            {
                return await Requests.GetGenericAsync<Models.API.v3.Chat.AllEmoticonsResponse>("https://api.twitch.tv/kraken/chat/emoticons", null, Requests.API.v3);
            }
            #endregion
            #region GetEmoticonsBySets
            public async Task<Models.API.v3.Chat.EmoticonSetsResponse> GetEmoticonsBySetsAsync(List<int> emotesets)
            {
                return await Requests.GetGenericAsync<Models.API.v3.Chat.EmoticonSetsResponse>($"https://api.twitch.tv/kraken/chat/emoticon_images?emotesets={string.Join(",", emotesets)}", null, Requests.API.v3);
            }
            #endregion
        }

        public class v5 : ApiMethod
        {
            public v5(Settings settings, Requests requests) : base(settings, requests)
            {
            }

            #region GetChatBadgesByChannel
            public async Task<Models.API.v5.Chat.ChannelBadges> GetChatBadgesByChannelAsync(string channelId)
            {
                if (string.IsNullOrWhiteSpace(channelId)) { throw new Exceptions.API.BadParameterException("The channel id is not valid for catching the channel badges. It is not allowed to be null, empty or filled with whitespaces."); }
                return await Requests.GetGenericAsync<Models.API.v5.Chat.ChannelBadges>($"https://api.twitch.tv/kraken/chat/{channelId}/badges", null, Requests.API.v5);
            }
            #endregion
            #region GetChatEmoticonsBySet
            public async Task<Models.API.v5.Chat.EmoteSet> GetChatEmoticonsBySetAsync(List<int> emotesets = null)
            {
                string payload = string.Empty;
                if (emotesets != null && emotesets.Count > 0)
                {
                    for (int i = 0; i < emotesets.Count; i++)
                    {
                        if (i == 0) { payload = $"?emotesets={emotesets[i]}"; }
                        else { payload += $",{emotesets[i]}"; }
                    }
                }
                return await Requests.GetGenericAsync<Models.API.v5.Chat.EmoteSet>($"https://api.twitch.tv/kraken/chat/emoticon_images{payload}", null, Requests.API.v5);
            }
            #endregion
            #region GetAllChatEmoticons
            public async Task<Models.API.v5.Chat.AllChatEmotes> GetAllChatEmoticonsAsync()
            {
                return await Requests.GetGenericAsync<Models.API.v5.Chat.AllChatEmotes>("https://api.twitch.tv/kraken/chat/emoticons", null, Requests.API.v5);
            }
            #endregion
        }
    }
}