namespace WalletService.Api.Contracts
{
    public static class Routes
    {
        public const string Service = "wallet";
        public const string Version = "v1";

        public static class Wallet
        {
            private const string Base = $"/{Service}/{Version}/wallets";

            public const string Create = Base;
            public const string Get = Base;
            public const string Credit = $"{Base}/credit";
            public const string Debit = $"{Base}/debit";
        }
    }
}
