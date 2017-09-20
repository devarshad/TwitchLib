using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchLib.Internal;

namespace TwitchLib.Api
{
    public class Users : IFluentInterface
    {
        public v3 V3 { get; }
        public v5 V5 { get; }
        public helix Helix { get; }

        public Users(Settings settings, Requests requests)
        {
            V3 = new v3(settings, requests);
            V5 = new v5(settings, requests);
            Helix = new helix(settings, requests);
        }

        public class v3 : ApiMethod
        {
            public v3(Settings settings, Requests requests) : base(settings, requests)
            {
            }
            #region GetUserFromUsername
            public async Task<Models.API.v3.Users.User> GetUserFromUsernameAsync(string username)
            {
                return await Requests.GetGenericAsync<Models.API.v3.Users.User>($"https://api.twitch.tv/kraken/users/{username}", null, Requests.API.v3);
            }
            #endregion
            #region GetEmotes
            public async Task<Models.API.v3.Users.UserEmotesResponse> GetEmotesAsync(string username, string accessToken = null)
            {
                Settings.DynamicScopeValidation(Enums.AuthScopes.User_Subscriptions, accessToken);
                return await Requests.GetGenericAsync<Models.API.v3.Users.UserEmotesResponse>($"https://api.twitch.tv/kraken/users/{username}/emotes", accessToken, Requests.API.v3);
            }
            #endregion
            #region GetUserFromToken
            public async Task<Models.API.v3.Users.FullUser> GetUserFromTokenAsync(string accessToken = null)
            {
                Settings.DynamicScopeValidation(Enums.AuthScopes.User_Read, accessToken);
                return await Requests.GetGenericAsync<Models.API.v3.Users.FullUser>("https://api.twitch.tv/kraken/user", accessToken, Requests.API.v3);
            }
            #endregion
            #region GetFollowedStreams
            public async Task<Models.API.v3.Users.FollowedStreamsResponse> GetFollowedStreamsAsync(int limit = 25, int offset = 0, Enums.StreamType type = Enums.StreamType.All, string accessToken = null)
            {
                Settings.DynamicScopeValidation(Enums.AuthScopes.User_Read, accessToken);
                string paramsStr = $"?limit={offset}&offset={offset}";
                switch (type)
                {
                    case Enums.StreamType.All:
                        paramsStr += "&stream_type=all";
                        break;
                    case Enums.StreamType.Live:
                        paramsStr += "&stream_type=live";
                        break;
                    case Enums.StreamType.Playlist:
                        paramsStr += "&stream_type=playlist";
                        break;
                }

                return await Requests.GetGenericAsync<Models.API.v3.Users.FollowedStreamsResponse>($"https://api.twitch.tv/kraken/streams/followed{paramsStr}", accessToken, Requests.API.v3);
            }
            #endregion
            #region GetFollowedVideos
            public async Task<Models.API.v3.Users.FollowedVideosResponse> GetFollowedVideosAsync(int limit = 25, int offset = 0, Enums.BroadcastType broadcastType = Enums.BroadcastType.All, string accessToken = null)
            {
                Settings.DynamicScopeValidation(Enums.AuthScopes.User_Read, accessToken);
                string paramsStr = $"?limit={limit}&offset={offset}";
                switch (broadcastType)
                {
                    case Enums.BroadcastType.All:
                        paramsStr += "&broadcast_type=all";
                        break;
                    case Enums.BroadcastType.Archive:
                        paramsStr += "&broadcast_type=archive";
                        break;
                    case Enums.BroadcastType.Highlight:
                        paramsStr += "&broadcast_type=highlight";
                        break;
                }

                return await Requests.GetGenericAsync<Models.API.v3.Users.FollowedVideosResponse>($"https://api.twitch.tv/kraken/videos/followed{paramsStr}", accessToken, Requests.API.v3);
            }
            #endregion
        }

        public class v5 : ApiMethod
        {
            public v5(Settings settings, Requests requests) : base(settings, requests)
            {
            }
            #region GetUsersByName
            public async Task<Models.API.v5.Users.Users> GetUsersByNameAsync(List<string> usernames)
            {
                if (usernames == null || usernames.Count == 0) { throw new Exceptions.API.BadParameterException("The username list is not valid. It is not allowed to be null or empty."); }
                string payload = "?login=" + string.Join(",", usernames);
                return await Requests.GetGenericAsync<Models.API.v5.Users.Users>($"https://api.twitch.tv/kraken/users{payload}", null, Requests.API.v5);
            }
            #endregion
            #region GetUser
            public async Task<Models.API.v5.Users.UserAuthed> GetUserAsync(string authToken = null)
            {
                return await Requests.GetGenericAsync<Models.API.v5.Users.UserAuthed>("https://api.twitch.tv/kraken/user", authToken, Requests.API.v5);
            }
            #endregion
            #region GetUserByID
            public async Task<Models.API.v5.Users.User> GetUserByIDAsync(string userId)
            {
                if (string.IsNullOrWhiteSpace(userId)) { throw new Exceptions.API.BadParameterException("The user id is not valid. It is not allowed to be null, empty or filled with whitespaces."); }
                return await Requests.GetGenericAsync<Models.API.v5.Users.User>($"https://api.twitch.tv/kraken/users/{userId}", null, Requests.API.v5);
            }
            #endregion
            #region GetUserByName
            public async Task<Models.API.v5.Users.Users> GetUserByNameAsync(string username)
            {
                if (string.IsNullOrEmpty(username)) { throw new Exceptions.API.BadParameterException("The username is not valid."); }
                return await Requests.GetGenericAsync<Models.API.v5.Users.Users>($"https://api.twitch.tv/kraken/users?login={username}");
            }
            #endregion
            #region GetUserEmotes
            public async Task<Models.API.v5.Users.UserEmotes> GetUserEmotesAsync(string userId, string authToken = null)
            {
                if (string.IsNullOrWhiteSpace(userId)) { throw new Exceptions.API.BadParameterException("The user id is not valid. It is not allowed to be null, empty or filled with whitespaces."); }
                return await Requests.GetGenericAsync<Models.API.v5.Users.UserEmotes>($"https://api.twitch.tv/kraken/users/{userId}/emotes", authToken, Requests.API.v5);
            }
            #endregion
            #region CheckUserSubscriptionByChannel
            public async Task<Models.API.v5.Subscriptions.Subscription> CheckUserSubscriptionByChannelAsync(string userId, string channelId, string authToken = null)
            {
                if (string.IsNullOrWhiteSpace(userId)) { throw new Exceptions.API.BadParameterException("The user id is not valid. It is not allowed to be null, empty or filled with whitespaces."); }
                if (string.IsNullOrWhiteSpace(channelId)) { throw new Exceptions.API.BadParameterException("The channel id is not valid. It is not allowed to be null, empty or filled with whitespaces."); }
                return await Requests.GetGenericAsync<Models.API.v5.Subscriptions.Subscription>($"https://api.twitch.tv/kraken/users/{userId}/subscriptions/{channelId}", authToken, Requests.API.v5);
            }
            #endregion
            #region GetUserFollows
            public async Task<Models.API.v5.Users.UserFollows> GetUserFollowsAsync(string userId, int? limit = null, int? offset = null, string direction = null, string sortby = null)
            {
                if (string.IsNullOrWhiteSpace(userId)) { throw new Exceptions.API.BadParameterException("The user id is not valid. It is not allowed to be null, empty or filled with whitespaces."); }
                List<KeyValuePair<string, string>> queryParameters = new List<KeyValuePair<string, string>>();
                if (limit != null)
                    queryParameters.Add(new KeyValuePair<string, string>("limit", limit.ToString()));
                if (offset != null)
                    queryParameters.Add(new KeyValuePair<string, string>("offset", offset.ToString()));
                if (!string.IsNullOrEmpty(direction) && (direction == "asc" || direction == "desc"))
                    queryParameters.Add(new KeyValuePair<string, string>("direction", direction));
                if (!string.IsNullOrEmpty(sortby) && (sortby == "created_at" || sortby == "last_broadcast" || sortby == "login"))
                    queryParameters.Add(new KeyValuePair<string, string>("sortby", sortby));

                string optionalQuery = string.Empty;
                if (queryParameters.Count > 0)
                {
                    for (int i = 0; i < queryParameters.Count; i++)
                    {
                        if (i == 0) { optionalQuery = $"?{queryParameters[i].Key}={queryParameters[i].Value}"; }
                        else { optionalQuery += $"&{queryParameters[i].Key}={queryParameters[i].Value}"; }
                    }
                }
                return await Requests.GetGenericAsync<Models.API.v5.Users.UserFollows>($"https://api.twitch.tv/kraken/users/{userId}/follows/channels{optionalQuery}", null, Requests.API.v5);
            }
            #endregion
            #region CheckUserFollowsByChannel
            public async Task<Models.API.v5.Users.UserFollow> CheckUserFollowsByChannelAsync(string userId, string channelId)
            {
                if (string.IsNullOrWhiteSpace(userId)) { throw new Exceptions.API.BadParameterException("The user id is not valid. It is not allowed to be null, empty or filled with whitespaces."); }
                if (string.IsNullOrWhiteSpace(channelId)) { throw new Exceptions.API.BadParameterException("The channel id is not valid. It is not allowed to be null, empty or filled with whitespaces."); }
                return await Requests.GetGenericAsync<Models.API.v5.Users.UserFollow>($"https://api.twitch.tv/kraken/users/{userId}/follows/channels/{channelId}", null, Requests.API.v5);
            }
            #endregion
            #region UserFollowsChannel
            public async Task<bool> UserFollowsChannelAsync(string userId, string channelId)
            {
                if (string.IsNullOrWhiteSpace(userId)) { throw new Exceptions.API.BadParameterException("The user id is not valid. It is not allowed to be null, empty or filled with whitespaces."); }
                if (string.IsNullOrWhiteSpace(channelId)) { throw new Exceptions.API.BadParameterException("The channel id is not valid. It is not allowed to be null, empty or filled with whitespaces."); }
                try
                {
                    await Requests.GetGenericAsync<Models.API.v5.Users.UserFollow>($"https://api.twitch.tv/kraken/users/{userId}/follows/channels/{channelId}", null, Requests.API.v5);
                    return true;
                }
                catch (Exceptions.API.BadResourceException)
                {
                    return false;
                }
            }
            #endregion
            #region FollowChannel
            public async Task<Models.API.v5.Users.UserFollow> FollowChannelAsync(string userId, string channelId, bool? notifications = null, string authToken = null)
            {
                if (string.IsNullOrWhiteSpace(userId)) { throw new Exceptions.API.BadParameterException("The user id is not valid. It is not allowed to be null, empty or filled with whitespaces."); }
                if (string.IsNullOrWhiteSpace(channelId)) { throw new Exceptions.API.BadParameterException("The channel id is not valid. It is not allowed to be null, empty or filled with whitespaces."); }
                string optionalRequestBody = (notifications != null) ? "{\"notifications\": " + notifications + "}" : null;
                return await Requests.PutGenericAsync<Models.API.v5.Users.UserFollow>($"https://api.twitch.tv/kraken/users/{userId}/follows/channels/{channelId}", optionalRequestBody, authToken, Requests.API.v5);
            }
            #endregion
            #region UnfollowChannel
            public async Task UnfollowChannelAsync(string userId, string channelId, string authToken = null)
            {
                if (string.IsNullOrWhiteSpace(userId)) { throw new Exceptions.API.BadParameterException("The user id is not valid. It is not allowed to be null, empty or filled with whitespaces."); }
                if (string.IsNullOrWhiteSpace(channelId)) { throw new Exceptions.API.BadParameterException("The channel id is not valid. It is not allowed to be null, empty or filled with whitespaces."); }
                await Requests.DeleteAsync($"https://api.twitch.tv/kraken/users/{userId}/follows/channels/{channelId}", authToken, Requests.API.v5);
            }
            #endregion
            #region GetUserBlockList
            public async Task<Models.API.v5.Users.UserBlocks> GetUserBlockListAsync(string userId, int? limit = null, int? offset = null, string authToken = null)
            {
                if (string.IsNullOrWhiteSpace(userId)) { throw new Exceptions.API.BadParameterException("The user id is not valid. It is not allowed to be null, empty or filled with whitespaces."); }
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
                return await Requests.GetGenericAsync<Models.API.v5.Users.UserBlocks>($"https://api.twitch.tv/kraken/users/{userId}/blocks{optionalQuery}", authToken, Requests.API.v5);
            }
            #endregion
            #region BlockUser
            public async Task<Models.API.v5.Users.UserBlock> BlockUserAsync(string sourceUserId, string targetUserId, string authToken = null)
            {
                if (string.IsNullOrWhiteSpace(sourceUserId)) { throw new Exceptions.API.BadParameterException("The source user id is not valid. It is not allowed to be null, empty or filled with whitespaces."); }
                if (string.IsNullOrWhiteSpace(targetUserId)) { throw new Exceptions.API.BadParameterException("The target user id is not valid. It is not allowed to be null, empty or filled with whitespaces."); }
                return await Requests.PutGenericAsync<Models.API.v5.Users.UserBlock>($"https://api.twitch.tv/kraken/users/{sourceUserId}/blocks/{targetUserId}", null, authToken, Requests.API.v5);
            }
            #endregion
            #region UnblockUser
            public async Task UnblockUserAsync(string sourceUserId, string targetUserId, string authToken = null)
            {
                if (string.IsNullOrWhiteSpace(sourceUserId)) { throw new Exceptions.API.BadParameterException("The source user id is not valid. It is not allowed to be null, empty or filled with whitespaces."); }
                if (string.IsNullOrWhiteSpace(targetUserId)) { throw new Exceptions.API.BadParameterException("The target user id is not valid. It is not allowed to be null, empty or filled with whitespaces."); }
                await Requests.DeleteAsync($"https://api.twitch.tv/kraken/users/{sourceUserId}/blocks/{targetUserId}", authToken, Requests.API.v5);
            }
            #endregion
            #region ViewerHeartbeatService
            #region CreateUserConnectionToViewerHeartbeatService
            public async Task CreateUserConnectionToViewerHeartbeatServiceAsync(string identifier, string authToken = null)
            {
                if (string.IsNullOrWhiteSpace(identifier)) { throw new Exceptions.API.BadParameterException("The identifier is not valid. It is not allowed to be null, empty or filled with whitespaces."); }
                await Requests.PutAsync("https://api.twitch.tv/kraken/user/vhs", "{\"identifier\": \"" + identifier + "\"}", authToken, Requests.API.v5);
            }
            #endregion
            #region CheckUserConnectionToViewerHeartbeatService
            public async Task<Models.API.v5.ViewerHeartbeatService.VHSConnectionCheck> CheckUserConnectionToViewerHeartbeatServiceAsync(string authToken = null)
            {
                return await Requests.GetGenericAsync<Models.API.v5.ViewerHeartbeatService.VHSConnectionCheck>("https://api.twitch.tv/kraken/user/vhs", authToken, Requests.API.v5);
            }
            #endregion
            #region DeleteUserConnectionToViewerHeartbeatService
            public async Task DeleteUserConnectionToViewerHeartbeatServicechStreamsAsync(string authToken = null)
            {
                await Requests.DeleteAsync("https://api.twitch.tv/kraken/user/vhs", authToken, Requests.API.v5);
            }
            #endregion
            #endregion
        }

        public class helix : ApiMethod
        {
            public helix(Settings settings, Requests requests) : base(settings, requests)
            {
            }

            public async Task<Models.API.Helix.Users.GetUsers.GetUsersResponse> GetUsersAsync(List<string> ids = null, List<string> logins = null, string accessToken = null)
            {
                string getParams = "";
                if (ids != null && ids.Count > 0)
                {
                    string idParams = string.Join("&login=", ids);
                    getParams = $"?id={idParams}";
                }
                if (logins != null && logins.Count > 0)
                {
                    string loginParams = string.Join("&id=", logins);
                    if (getParams == "")
                        getParams = $"?login={loginParams}";
                    else
                        getParams += $"&login={loginParams}";
                }
                return await Requests.GetGenericAsync<Models.API.Helix.Users.GetUsers.GetUsersResponse>($"https://api.twitch.tv/helix/users{getParams}", accessToken, Requests.API.Helix);
            }

            public async Task<Models.API.Helix.Users.GetUsersFollows.GetUsersFollowsResponse> GetUsersFollows(string after = null, string before = null, int first = 20, string fromId = null, string toId = null)
            {
                string getParams = $"?first={first}";
                if (after != null)
                    getParams += $"&after={after}";
                if (before != null)
                    getParams += $"&before={before}";
                if (fromId != null)
                    getParams += $"&from_id={fromId}";
                if (toId != null)
                    getParams += $"&to_id={toId}";

                return await Requests.GetGenericAsync<Models.API.Helix.Users.GetUsersFollows.GetUsersFollowsResponse>($"https://api.twitch.tv/helix/users/follows{getParams}", api: Requests.API.Helix);
            }

            public async Task PutUsers(string description, string accessToken = null)
            {
                await Requests.PutAsync($"https://api.twitch.tv/helix/users?description={description}", null, accessToken, Requests.API.Helix);
            }
        }
    }
}