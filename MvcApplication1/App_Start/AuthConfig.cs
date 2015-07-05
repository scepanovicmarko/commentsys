using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using MvcApplication1.Models;

namespace MvcApplication1
{
    public static class AuthConfig
    {
        private static string FB_IMAGE_URL = "/Images/fb.png";
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");
            Dictionary<string, object> facebookSocialData = new Dictionary<string, object>();
            facebookSocialData.Add("Icon", FB_IMAGE_URL);
            OAuthWebSecurity.RegisterFacebookClient(
                appId: "1602279993393828",
                appSecret: "784e48419887b07dbdd6e700977d869f", 
                displayName: "Facebook",
                extraData: facebookSocialData);
        }
    }
}
