using System.Threading.Tasks;

namespace TwitchLib.Api
{
    public class Blocks : IFluentInterface
    {
        public v3 V3 { get; private set; }
        public Blocks(Settings settings, Requests requests)
        {
            V3 = new v3(settings, requests);
        }

        public class v3 : ApiMethod
        {
            public v3(Settings settings, Requests requests) : base(settings, requests)
            { }

            #region GetBlocks
            public async Task<Models.API.v3.Blocks.GetBlocksResponse> GetBlocksAsync(string channel, int limit = 25, int offset = 0, string accessToken = null)
            {
            Settings.DynamicScopeValidation(Enums.AuthScopes.User_Blocks_Read, accessToken);
                string pm = $"?limit={limit}&offset={offset}";
                return await Requests.GetGenericAsync<Models.API.v3.Blocks.GetBlocksResponse>($"https://api.twitch.tv/kraken/users/{channel}/blocks{pm}", accessToken, Requests.API.v3);
            }
            #endregion
            #region CreateBlock
            public async Task<Models.API.v3.Blocks.Block> CreateBlockAsync(string channel, string target, string accessToken = null)
            {
                Settings.DynamicScopeValidation(Enums.AuthScopes.User_Blocks_Edit, accessToken);
                return await Requests.PutGenericAsync<Models.API.v3.Blocks.Block>($"https://api.twitch.tv/kraken/users/{channel}/blocks/{target}", null, accessToken, Requests.API.v3);
            }
            #endregion
            #region RemoveBlock
            public async Task RemoveBlockAsync(string channel, string target, string accessToken = null)
            {
                Settings.DynamicScopeValidation(Enums.AuthScopes.User_Blocks_Edit, accessToken);
                await Requests.DeleteAsync($"https://api.twitch.tv/kraken/users/{channel}/blocks/{target}", accessToken, Requests.API.v3);
            }
            #endregion
        }

    }
}