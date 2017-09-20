using System.Collections.Generic;
using TwitchLib.Exceptions.API;

namespace TwitchLib.Api
{
    public class Settings : IFluentInterface
    {
        #region Private variables
        private string clientIdInternal;
        private string accessTokenInternal;

        private Requests requests;
        #endregion
        #region Public property fields
        public string ClientId { get { return clientIdInternal; } set { setClientId(value); } }
        public string AccessToken { get { return accessTokenInternal; } set { setAccessToken(value); } }

        public Settings()
        {
            requests = new Requests(this);
        }

        public List<Enums.AuthScopes> Scopes { get; set; } = new List<Enums.AuthScopes>() { Enums.AuthScopes.None };
        #endregion
        #region Public methods
        #region DynamicScopeValidation
        public void DynamicScopeValidation(Enums.AuthScopes requiredScope, string accessToken = null)
        {
            if (!Validators.SkipDynamicScopeValidation && accessToken == null)
                if (!Scopes.Contains(requiredScope) || (requiredScope == Enums.AuthScopes.Any && Scopes.Count == 0))
                    throw new InvalidCredentialException($"The current access token does not support this call. Missing required scope: {requiredScope.ToString().ToLower()}. You can skip this check by using: TwitchLib.TwitchAPI.Settings.Validators.SkipDynamicScopeValidation = true . You can also generate a new token with this scope here: https://twitchtokengenerator.com");
        }
        #endregion
        #endregion
        #region Private methods
        #region setClientId
        private void setClientId(string clientId)
        {
            if (!Validators.SkipClientIdValidation)
            {
                //if (string.IsNullOrEmpty(clientId))
                //    throw new InvalidCredentialException("Client Id cannot be empty or null. Set it using TwitchLib.TwitchAPI.Settings.ClientId = {clientid}");
                if ((!string.IsNullOrWhiteSpace(clientId) || !string.IsNullOrWhiteSpace(AccessToken)) && !(validClientId(clientId)))
                    throw new InvalidCredentialException("The passed Client Id was not valid. To get a valid Client Id, register an application here: https://www.twitch.tv/kraken/oauth2/clients/new");
            }
            clientIdInternal = clientId;
        }
        #endregion
        #region setAccessToken
        private void setAccessToken(string accessToken)
        {
            if (!Validators.SkipAccessTokenValidation)
            {
                if (string.IsNullOrEmpty(accessToken))
                    throw new InvalidCredentialException("Access Token cannot be empty or null. Set it using: TwitchLib.TwitchAPI.Settings.AccessToken = {access_token}");
                if (!(validAccessToken(accessToken)))
                    throw new InvalidCredentialException("The passed Access Token was not valid. To get an access token, go here:  https://twitchtokengenerator.com/");
            }
            accessTokenInternal = accessToken;
        }
        #endregion
        #region validClientId
        private bool validClientId(string clientId)
        {
            try
            {
                var result = requests.GetGenericAsync<Models.API.v5.Root.Root>("https://api.twitch.tv/kraken", null, Requests.API.v5, ClientId).Result;
                return true;
            }
            catch (BadRequestException)
            {
                return false;
            }
        }
        #endregion
        #region validAccessToken
        private bool validAccessToken(string accessToken)
        {
            try
            {
                var resp = requests.GetGenericAsync<Models.API.v5.Root.Root>("https://api.twitch.tv/kraken", accessToken, Requests.API.v5, ClientId).Result;
                if (resp.Token != null)
                {
                    Scopes = buildScopesList(resp.Token);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region buildScopesList
        private List<Enums.AuthScopes> buildScopesList(Models.API.v5.Root.RootToken token)
        {
            List<Enums.AuthScopes> scopes = new List<Enums.AuthScopes>();
            foreach (string scope in token.Auth.Scopes)
            {
                switch (scope)
                {
                    case "channel_check_subscription":
                        scopes.Add(Enums.AuthScopes.Channel_Check_Subscription);
                        break;
                    case "channel_commercial":
                        scopes.Add(Enums.AuthScopes.Channel_Commercial);
                        break;
                    case "channel_editor":
                        scopes.Add(Enums.AuthScopes.Channel_Editor);
                        break;
                    case "channel_feed_edit":
                        scopes.Add(Enums.AuthScopes.Channel_Feed_Edit);
                        break;
                    case "channel_feed_read":
                        scopes.Add(Enums.AuthScopes.Channel_Feed_Read);
                        break;
                    case "channel_read":
                        scopes.Add(Enums.AuthScopes.Channel_Read);
                        break;
                    case "channel_stream":
                        scopes.Add(Enums.AuthScopes.Channel_Stream);
                        break;
                    case "channel_subscriptions":
                        scopes.Add(Enums.AuthScopes.Channel_Subscriptions);
                        break;
                    case "chat_login":
                        scopes.Add(Enums.AuthScopes.Chat_Login);
                        break;
                    case "collections_edit":
                        scopes.Add(Enums.AuthScopes.Collections_Edit);
                        break;
                    case "communities_edit":
                        scopes.Add(Enums.AuthScopes.Communities_Edit);
                        break;
                    case "communities_moderate":
                        scopes.Add(Enums.AuthScopes.Communities_Moderate);
                        break;
                    case "user_blocks_edit":
                        scopes.Add(Enums.AuthScopes.User_Blocks_Edit);
                        break;
                    case "user_blocks_read":
                        scopes.Add(Enums.AuthScopes.User_Blocks_Read);
                        break;
                    case "user_follows_edit":
                        scopes.Add(Enums.AuthScopes.User_Follows_Edit);
                        break;
                    case "user_read":
                        scopes.Add(Enums.AuthScopes.User_Read);
                        break;
                    case "user_subscriptions":
                        scopes.Add(Enums.AuthScopes.User_Subscriptions);
                        break;
                    case "viewing_activity_read":
                        scopes.Add(Enums.AuthScopes.Viewing_Activity_Read);
                        break;
                }
            }

            if (scopes.Count == 0)
                scopes.Add(Enums.AuthScopes.None);
            return scopes;
        }
        #endregion
        #endregion
    }
}