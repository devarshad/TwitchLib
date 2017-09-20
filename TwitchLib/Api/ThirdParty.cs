using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace TwitchLib.Api
{
    /// <summary>These endpoints are offered by third party services (NOT TWITCH), but are still pretty cool.</summary>
    public class ThirdParty : IFluentInterface
    {
        public ThirdParty(Settings settings, Requests requests)
        {
            UsernameChangesApi = new UsernameChanges(settings, requests);
            ModLookupApi = new ModLookup(settings, requests);
        }

        public UsernameChanges UsernameChangesApi { get; }
        public ModLookup ModLookupApi { get; }

        public class UsernameChanges : ApiMethod
        {
            public UsernameChanges(Settings settings, Requests requests) : base(settings, requests)
            {
            }

            public async Task<List<Models.API.ThirdParty.UsernameChange.UsernameChangeListing>> GetUsernameChangesAsync(string username)
            {
                return await Requests.GetGenericAsync<List<Models.API.ThirdParty.UsernameChange.UsernameChangeListing>>($"https://twitch-tools.rootonline.de/username_changelogs_search.php?q={username}&format=json", null, Requests.API.Void);
            }
        }

        public class ModLookup : ApiMethod
        {
            public ModLookup(Settings settings, Requests requests) : base(settings, requests)
            {
            }

            public async Task<Models.API.ThirdParty.ModLookup.ModLookupResponse> GetChannelsModdedForByNameAsync(string username, int offset = 0, int limit = 100, bool useTls12 = true)
            {
                if (useTls12)
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                return await Requests.GetGenericAsync<Models.API.ThirdParty.ModLookup.ModLookupResponse>($"https://twitchstuff.3v.fi/modlookup/api/user/{username}?offset={offset}&limit={limit}");
            }

            public async Task<Models.API.ThirdParty.ModLookup.TopResponse> GetChannelsModdedForByTopAsync(bool useTls12 = true)
            {
                if (useTls12)
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                return await Requests.GetGenericAsync<Models.API.ThirdParty.ModLookup.TopResponse>($"https://twitchstuff.3v.fi/modlookup/api/top");
            }

            public async Task<Models.API.ThirdParty.ModLookup.StatsResponse> GetChannelsModdedForStatsAsync(bool useTls12 = true)
            {
                if (useTls12)
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                return await Requests.GetGenericAsync<Models.API.ThirdParty.ModLookup.StatsResponse>($"https://twitchstuff.3v.fi/modlookup/api/stats");
            }
        }

        public class AuthorizationFlow
        {
            public event EventHandler<Events.API.ThirdParty.AuthorizationFlow.OnUserAuthorizationDetectedArgs> OnUserAuthorizationDetected;
            public event EventHandler<Events.API.ThirdParty.AuthorizationFlow.OnErrorArgs> OnError;

            private string baseUrl = "https://twitchtokengenerator.com/api";
            private System.Timers.Timer pingTimer;
            private string apiId;

            public Models.API.ThirdParty.AuthorizationFlow.CreatedFlow CreateFlow(string applicationTitle, List<Enums.AuthScopes> scopes)
            {
                string scopesStr = null;
                foreach (var scope in scopes)
                {
                    if (scopesStr == null)
                        scopesStr = Common.Helpers.AuthScopesToString(scope);
                    else
                        scopesStr += $"+{Common.Helpers.AuthScopesToString(scope)}";
                }

                string createUrl = $"{baseUrl}/create/{Common.Helpers.Base64Encode(applicationTitle)}/{scopesStr}";

                var resp = new System.Net.WebClient().DownloadString(createUrl);
                return JsonConvert.DeserializeObject<Models.API.ThirdParty.AuthorizationFlow.CreatedFlow>(resp);
            }

            public void BeginPingingStatus(string id, int intervalMs = 5000)
            {
                apiId = id;
                pingTimer = new System.Timers.Timer(intervalMs);
                pingTimer.Elapsed += onPingTimerElapsed;
                pingTimer.Start();
            }

            public Models.API.ThirdParty.AuthorizationFlow.PingResponse PingStatus(string id = null)
            {
                if (id != null)
                    apiId = id;

                var resp = new System.Net.WebClient().DownloadString($"{baseUrl}/status/{apiId}");
                var model = new Models.API.ThirdParty.AuthorizationFlow.PingResponse(resp);

                return model;
            }

            private void onPingTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
            {
                var ping = PingStatus();
                if (ping.Success)
                {
                    pingTimer.Stop();
                    OnUserAuthorizationDetected?.Invoke(null, new Events.API.ThirdParty.AuthorizationFlow.OnUserAuthorizationDetectedArgs { Id = ping.Id, Scopes = ping.Scopes, Token = ping.Token, Username = ping.Username });
                }
                else
                {
                    if (ping.Error != 3)
                    {
                        pingTimer.Stop();
                        OnError?.Invoke(null, new Events.API.ThirdParty.AuthorizationFlow.OnErrorArgs { Error = ping.Error, Message = ping.Message });
                    }
                }
            }
        }
    }
}