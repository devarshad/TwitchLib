using System.Collections.Generic;
using TwitchLib.Api;
using TwitchLib.Enums;

namespace TwitchLib
{
    public interface ITwitchAPI
    {
        string AccessToken { get; set; }
        Badges Badges { get; }
        Bits Bits { get; }
        Blocks Blocks { get; }
        ChannelFeeds ChannelFeeds { get; }
        Channels Channels { get; }
        Chat Chat { get; }
        string ClientId { get; set; }
        Clips Clips { get; }
        Collections Collections { get; }
        Communities Communities { get; }
        Follows Follows { get; }
        Games Games { get; }
        Ingests Ingests { get; }
        Root Root { get; }
        List<AuthScopes> Scopes { get; }
        Search Search { get; }
        Streams Streams { get; }
        Subscriptions Subscriptions { get; }
        Teams Teams { get; }
        ThirdParty ThirdParty { get; }
        Undocumented Undocumented { get; }
        Users Users { get; }
        Videos Videos { get; }

        void ValidateScope(AuthScopes requiredScope, string accessToken = null);
    }
}