﻿namespace TwitchLib.Events.Client
{
    #region using directives
    using System;
    #endregion
    /// <summary>Args representing on channel state changed event.</summary>
    public class OnSendReceiveDataArgs : EventArgs
    {
        /// <summary>Property representing the direction of the data.</summary>
        public Enums.SendReceiveDirection Direction;
        /// <summary>Property representing the data that was either sent or received.</summary>
        public string Data;
    }
}
