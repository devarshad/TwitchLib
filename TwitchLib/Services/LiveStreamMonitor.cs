﻿namespace TwitchLib.Services
{
    #region using directives
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Timers;

    using Enums;
    using Events.Services.LiveStreamMonitor;
    using Exceptions.API;
    using Exceptions.Services;
    #endregion

    /// <summary>Service that allows customizability and subscribing to detection of channels going online/offline.</summary>
    public class LiveStreamMonitor
    {
        #region Private Variables
        private string _clientId;
        private int _checkIntervalSeconds;
        private List<string> _channels;
        private Dictionary<string, bool> _statuses = new Dictionary<string, bool>();
        private readonly Timer _streamMonitorTimer = new Timer();
        private StreamIdentifierType _identifierType;
        #endregion
        #region Public Variables
        /// <summary>Property representing Twitch channels service is monitoring.</summary>
        public List<string> Channels { get { return _channels; } protected set { _channels = value; } }
        /// <summary>Property representing application client Id, also updates it in TwitchApi.</summary>
        public string ClientId { get { return _clientId; } set { _clientId = value; twitchAPI.ClientId = value; } }
        /// <summary>Property representing interval between Twitch Api calls, in seconds. Recommended: 60</summary>
        public int CheckIntervalSeconds { get { return _checkIntervalSeconds; } set { _checkIntervalSeconds = value; _streamMonitorTimer.Interval = value * 1000; } }
        /// <summary>Property representing whether streams are represented by usernames or userids</summary>
        public StreamIdentifierType IdentifierType { get { return _identifierType; } protected set { _identifierType = value; } }
        private readonly ITwitchAPI twitchAPI;
        #endregion
        #region EVENTS
        /// <summary>Event fires when Stream goes online</summary>
        public event EventHandler<OnStreamOnlineArgs> OnStreamOnline;
        /// <summary>Event fires when Stream goes online</summary>
        public event EventHandler<OnStreamOfflineArgs> OnStreamOffline;
        /// <summary>Event fires when service stops.</summary>
        public event EventHandler<OnStreamMonitorStartedArgs> OnStreamMonitorStarted;
        /// <summary>Event fires when service starts.</summary>
        public event EventHandler<OnStreamMonitorEndedArgs> OnStreamMonitorEnded;
        /// <summary>Event fires when channels to monitor are intitialized.</summary>
        public event EventHandler<OnStreamsSetArgs> OnStreamsSet;
        #endregion
        /// <summary>Service constructor.</summary>
        /// <exception cref="BadResourceException">If channel is invalid, an InvalidChannelException will be thrown.</exception>
        /// <param name="checkIntervalSeconds">Param representing number of seconds between calls to Twitch Api.</param>
        /// <param name="clientId">Optional param representing Twitch Api-required application client id, not required if already set.</param>
        public LiveStreamMonitor(ITwitchAPI twitchApi, int checkIntervalSeconds = 60, string clientId = "")
        {
            twitchAPI = twitchApi;
            CheckIntervalSeconds = checkIntervalSeconds;
            _streamMonitorTimer.Elapsed += _streamMonitorTimerElapsed;
            if (clientId != "")
                ClientId = clientId;
        }

        #region CONTROLS
        /// <summary>Starts service, updates status of all channels, fires OnStreamMonitorStarted event.</summary>
        public async void StartService()
        {
            if (Channels == null)
            {
                throw new UnintializedChannelListException("Channel list must be initialized prior to service starting");
            }
            foreach (string channel in Channels)
            {
                var initialStatus = await _checkStreamOnline(channel);
                _statuses.Add(channel, initialStatus);
            }
            _streamMonitorTimer.Start();
            OnStreamMonitorStarted?.Invoke(this,
                new OnStreamMonitorStartedArgs { Channels = Channels, IdentifierType = IdentifierType, CheckIntervalSeconds = CheckIntervalSeconds });
        }

        /// <summary>Stops service and fires OnStreamMonitorStopped event.</summary>
        public void StopService()
        {
            _streamMonitorTimer.Stop();
            OnStreamMonitorEnded?.Invoke(this,
               new OnStreamMonitorEndedArgs { Channels = Channels, IdentifierType = IdentifierType, CheckIntervalSeconds = CheckIntervalSeconds });
        }
        /// <summary> Sets the list of channels to monitor by username </summary>
        /// <param name="usernames">List of channels to monitor as usernames</param>
        public void SetStreamsByUsername(List<string> usernames)
        {
            _statuses = new Dictionary<string, bool>();
            _channels = usernames;
            _identifierType = StreamIdentifierType.Usernames;
            OnStreamsSet?.Invoke(this,
                new OnStreamsSetArgs { Channels = Channels, IdentifierType = IdentifierType, CheckIntervalSeconds = CheckIntervalSeconds });
        }
        /// <summary> Sets the list of channels to monitor by username </summary>
        /// <param name="userids">List of channels to monitor as userids</param>
        public void SetStreamsByUserId(List<string> userids)
        {
            _statuses = new Dictionary<string, bool>();
            _channels = userids;
            _identifierType = StreamIdentifierType.UserIds;
            OnStreamsSet?.Invoke(this,
                new OnStreamsSetArgs { Channels = Channels, IdentifierType = IdentifierType, CheckIntervalSeconds = CheckIntervalSeconds });
        }
        #endregion

        private async void _streamMonitorTimerElapsed(object sender, ElapsedEventArgs e)
        {
            foreach (string channel in Channels)
            {
                bool current = await _checkStreamOnline(channel);
                if (current && !_statuses[channel])
                {
                    OnStreamOnline?.Invoke(this,
                        new OnStreamOnlineArgs { Channel = channel, IdentifierType = IdentifierType, CheckIntervalSeconds = CheckIntervalSeconds });
                    _statuses[channel] = true;
                }
                else if (!current && _statuses[channel])
                {
                    OnStreamOffline?.Invoke(this,
                        new OnStreamOfflineArgs { Channel = channel, IdentifierType = IdentifierType, CheckIntervalSeconds = CheckIntervalSeconds });
                    _statuses[channel] = false;
                }
            }
        }
        private async Task<bool> _checkStreamOnline(string channel)
        {
            switch (_identifierType)
            {
                case StreamIdentifierType.Usernames:
                    try
                    {
                        var resp = await twitchAPI.Streams.V3.GetStreamAsync(channel);
                        if (resp == null || resp.Stream == null)
                            return false;
                        else
                            return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                case StreamIdentifierType.UserIds:
                    try
                    {
                        var resp = await twitchAPI.Streams.V5.GetStreamByUserAsync(channel);
                        if (resp == null || resp.Stream == null)
                            return false;
                        else
                            return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                default:
                    throw new UnintializedChannelListException("Channel list must be initialized prior to service starting");
            }
        }

    }
}
