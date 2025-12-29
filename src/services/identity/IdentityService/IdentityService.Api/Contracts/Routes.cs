namespace IdentityService.Api.Contracts
{
    public static class Routes
    {
        public const string Service = "identity";
        public const string Version = "v1";

        public static class Auth
        {
            private const string Base = $"/{Service}/{Version}/auth";
            public const string Register = $"{Base}/register";
            public const string Login = $"{Base}/login";
        }
    }
}
