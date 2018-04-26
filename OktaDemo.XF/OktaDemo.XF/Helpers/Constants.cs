namespace OktaDemo.XF.Helpers
{
    public class Constants
    {
        public const string AuthStateKey = "authState";
        public const string AuthServiceDiscoveryKey = "authServiceDiscovery";

        public const string ClientId = "{clientId}";
        public const string RedirectUri = "{redirectUri}";
        public const string OrgUrl = "https://{yourOktaDomain}.com";
        public const string AuthorizationServerId = "default";

        public static readonly string DiscoveryEndpoint =
            $"{OrgUrl}/oauth2/{AuthorizationServerId}/.well-known/openid-configuration";


        public static readonly string[] Scopes = new string[] {
            "openid", "profile", "email", "offline_access" };
    }
}