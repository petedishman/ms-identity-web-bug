namespace WebApp
{
    public static class CallCounts
    {
        public static int MsIdentityOptionsConfiguration { get; set; } = 0;
        public static int ClientAppOptionsConfiguration { get; set; } = 0;
        public static int RedirectToIdProvider { get; set; } = 0;

        internal static void Reset()
        {
            MsIdentityOptionsConfiguration = 0;
            ClientAppOptionsConfiguration = 0;
            RedirectToIdProvider = 0;
        }
    }
}
