namespace Limbus_wordle_backend.Util.Environment
{
    public static class EnvironmentVariables
    {
        public static string identitiesFilePath = System.Environment.GetEnvironmentVariable("IdentityJSONFile") ?? "Data/Identities.json";
        public static string dailyIdentityFilePath = System.Environment.GetEnvironmentVariable("DailyIdentityJSONFile") ?? "Data/DailyIdentities.json";
        public static string identityImgFilePath = System.Environment.GetEnvironmentVariable("IdentityImgFilePath") ?? "Content/Images/Identities/";
        public static string cloudinaryUrl = System.Environment.GetEnvironmentVariable("CLOUDINARY_URL") ?? "";
        public static string frontendUrl = System.Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "http://localhost:3000";
        public static string listenOn = System.Environment.GetEnvironmentVariable("LISTEN_ON") ?? "http://localhost:8080";
    }
}