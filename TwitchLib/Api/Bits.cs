using System.Threading.Tasks;

namespace TwitchLib.Api
{
    public class Bits : IFluentInterface
    {
        public v5 V5 { get; private set; }
        public Bits(Settings settings, Requests requests)
        {
            V5 = new v5(settings, requests);
        }

        public class v5 : ApiMethod
        {
            public v5(Settings settings, Requests requests) : base(settings, requests)
            { }

            #region GetCheermotes
            public async Task<Models.API.v5.Bits.Cheermotes> GetCheermotesAsync(string channelId = null)
            {
                string optionalQuery = (channelId != null) ? $"?channel_id={channelId}" : string.Empty;
                return await Requests.GetGenericAsync<Models.API.v5.Bits.Cheermotes>($"https://api.twitch.tv/kraken/bits/actions{optionalQuery}");
            }
            #endregion
        }
    }
}