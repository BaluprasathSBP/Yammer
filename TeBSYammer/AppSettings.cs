using System;
namespace TeBSYammer
{

    public class IAppSettings
    {
        public string ClientID { get; }

        public string ClientSecret { get; }

        public string BaseURL { get; }

        public string AuthBaseURL { get; }

        public string PortalURL { get; }

        public string FacebookAppId { get; }

        public string TwitterApiKey { get; }

        public string TwitterApiSecret { get; }
    }

    public class AppSettings : IAppSettings
    {

        public static string ClientID { get; private set; }

        public static string ClientSecret { get; private set; }

        public static string BaseURL { get; private set; }

        public static string AuthBaseURL { get; private set; }

        public static string RedirectURL { get; private set; }

        public static string PortalURL { get; }

        public static string FacebookAppId { get; }

        public static string TwitterApiKey { get; }

        public static string TwitterApiSecret { get; }

        public static string Token { get; set; }

        public static string UserId { get; set; }



        public static IAppSettings Current { get; private set; }

        public AppSettings()
        {
            ClientID = "I1Wk1J76wXjt0MBNDmOipg";
            ClientSecret = "EmMj8kZUnLK0ehbS5SIUMLGetwvGjukAut0zSagix58";
            RedirectURL = "TeBSYammer://com.TebsYammer";
            BaseURL = $"https://www.yammer.com/api/v1/";
        }

    }
}
