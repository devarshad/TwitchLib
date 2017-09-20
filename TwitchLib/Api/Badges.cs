using System.Threading.Tasks;

namespace TwitchLib.Api
{
        public class Badges : IFluentInterface
        {
            public v5 V5 { get; private set; }
            public Badges(Settings settings, Requests requests)
            {
                V5 = new v5(settings, requests);
            }

            public class v5 : ApiMethod
        {
            public v5(Settings settings, Requests requests) : base(settings, requests)
            { }

            #region GetSubscriberBadgesForChannel
            public async Task<Models.API.v5.Badges.ChannelDisplayBadges> GetSubscriberBadgesForChannelAsync(string channelId)
                {
                    if (string.IsNullOrWhiteSpace(channelId)) { throw new Exceptions.API.BadParameterException("The channel id is not valid. It is not allowed to be null, empty or filled with whitespaces."); }
                    return await Requests.GetGenericAsync<Models.API.v5.Badges.ChannelDisplayBadges>($"https://badges.twitch.tv/v1/badges/channels/{channelId}/display", null, Requests.API.v5);
                }
                #endregion

                #region GetGlobalBadges
                public async Task<Models.API.v5.Badges.GlobalBadgesResponse> GetGlobalBadgesAsync()
                {
                    return await Requests.GetGenericAsync<Models.API.v5.Badges.GlobalBadgesResponse>("https://badges.twitch.tv/v1/badges/global/display", null, Requests.API.v5);
                }
                #endregion
            }
    }
}