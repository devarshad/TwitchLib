using System.Collections.Generic;
using TwitchLib.Api;
using TwitchLib.Exceptions.API;

namespace TwitchLib
{
    public class TwitchAPI : ITwitchAPI
    {
        private Settings _settings;
        private Requests _requests;

        public Blocks Blocks { get; }
        public Badges Badges { get; }
        public Bits Bits { get; }
        public ChannelFeeds ChannelFeeds { get; }
        public Channels Channels { get; }
        public Chat Chat { get; }
        public Clips Clips { get; }
        public Collections Collections { get; }
        public Communities Communities { get; }
        public Follows Follows { get; }
        public Games Games { get; }
        public Ingests Ingests { get; }
        public Root Root { get; }
        public Search Search { get; }
        public Streams Streams { get; }
        public Subscriptions Subscriptions { get; }
        public Teams Teams { get; }

        public TwitchAPI()
        {
            _settings = new Settings();
            _requests = new Requests(_settings);
            Blocks = new Blocks(_settings, _requests);
            Badges = new Badges(_settings, _requests);
            Bits = new Bits(_settings, _requests);
            ChannelFeeds = new ChannelFeeds(_settings, _requests);
            Channels = new Channels(_settings, _requests);
            Chat = new Chat(_settings, _requests);
            Clips = new Clips(_settings, _requests);
            Collections = new Collections(_settings, _requests);
            Communities = new Communities(_settings, _requests);
            Follows = new Follows(_settings, _requests);
            Games = new Games(_settings, _requests);
            Ingests = new Ingests(_settings, _requests);
            Root = new Root(_settings, _requests);
            Search = new Search(_settings, _requests);
            Streams = new Streams(_settings, _requests);
            Subscriptions = new Subscriptions(_settings, _requests);
            Teams = new Teams(_settings, _requests);
            ThirdParty = new ThirdParty(_settings, _requests);
            Undocumented = new Undocumented(_settings, _requests);
            Users = new Users(_settings, _requests);
            Videos = new Videos(_settings, _requests);

        }

        #region ClientId
        public string ClientId { get { return _settings.ClientId; } set { _settings.ClientId = value; } }
        #endregion
        #region AccessToken
        public string AccessToken { get { return _settings.AccessToken; } set { _settings.AccessToken = value; } }
        #endregion

        #region Scopes
        public List<Enums.AuthScopes> Scopes { get { return _settings.Scopes; } }

        public ThirdParty ThirdParty { get; }
        public Undocumented Undocumented { get; }
        public Users Users { get; }
        public Videos Videos { get; }
        #endregion

        public void ValidateScope(Enums.AuthScopes requiredScope, string accessToken = null)
        {
            if (accessToken != null)
                return;
            if (!_settings.Scopes.Contains(requiredScope))
                throw new InvalidCredentialException($"The call you attempted was blocked because you are missing required scope: {requiredScope.ToString().ToLower()}. You can ignore this protection by using TwitchLib.TwitchAPI.Settings.Validators.SkipDynamicScopeValidation = false . You can also generate a new token with the required scope here: https://twitchtokengenerator.com");
        }

    }
}