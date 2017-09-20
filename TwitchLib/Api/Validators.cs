namespace TwitchLib.Api
{
    public static class Validators
    {
            #region ClientIdValidation
            public static bool SkipClientIdValidation { get; set; } = false;
            #endregion
            #region AccessTokenValidation
            public static bool SkipAccessTokenValidation { get; set; } = false;
            #endregion
            #region DynamicScopeValidation
            public static bool SkipDynamicScopeValidation { get; set; } = false;
            #endregion
        }
}
