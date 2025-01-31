namespace Duende.Bff
{
    /// <summary>
    /// Constants for Duende.BFF
    /// </summary>
    public static class Constants
    {
        internal const string BffMiddlewareMarker = "Duende.Bff.BffMiddlewareMarker";

        /// <summary>
        /// Constants used for YARP
        /// </summary>
        public static class Yarp
        {
            /// <summary>
            /// Name of toke type metadata
            /// </summary>
            public const string TokenTypeMetadata = "Duende.Bff.Yarp.TokenType";
        }
        
        /// <summary>
        /// Custom claim types used by Duende.BFF
        /// </summary>
        public static class ClaimTypes
        {
            /// <summary>
            /// Claim type for logout URL including session id
            /// </summary>
            public const string LogoutUrl = "bff:logout_url";
            
            /// <summary>
            /// Claim type for session expiration in seconds
            /// </summary>
            public const string SessionExpiresIn = "bff:session_expires_in";

            /// <summary>
            /// Claim type for authorize response session state value
            /// </summary>
            public const string SessionState = "bff:session_state";
        }

        /// <summary>
        /// Paths used for management endpoints.
        /// </summary>
        public static class ManagementEndpoints
        {
            /// <summary>
            /// Login path
            /// </summary>
            public const string Login = "/login";
            
            /// <summary>
            /// Logout path
            /// </summary>
            public const string Logout = "/logout";
            
            /// <summary>
            /// User path
            /// </summary>
            public const string User = "/user";
            
            /// <summary>
            /// Back channel logout path
            /// </summary>
            public const string BackChannelLogout = "/backchannel";
            
            /// <summary>
            /// Diagnostics path
            /// </summary>
            public const string Diagnostics = "/diagnostics";
        }

        /// <summary>
        /// Request parameter names
        /// </summary>
        public static class RequestParameters
        {
            /// <summary>
            /// Used to prevent cookie sliding on user endpoint
            /// </summary>
            public const string SlideCookie = "slide";
            
            /// <summary>
            /// Used to pass a return URL to login/logout
            /// </summary>
            public const string ReturnUrl = "returnUrl";
        }
    }
}