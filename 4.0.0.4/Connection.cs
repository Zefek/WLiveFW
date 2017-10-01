using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WLive
{
    public class Connection
    {
        static DateTime tokenExpiresDate;

        static bool expired = true;

        [WLive("token_type")]
        public string TokenType { get; set; }
        [WLive("scope")]
        public string Scope { get; set; }
        [WLive("refresh_token")]
        public static string RefreshToken { get; set; }
        [WLive("authentication_token")]
        public string AuthenticationToken { get; set; }
        [WLive("access_token")]
        public string AccessToken { get; set; }
        [WLive("user_id")]
        public static string UserId { get; set; }

        public static string ClientId { get; set; }
        public static string ClientSecret { get; set; }
        public static Uri RedirectUri { get; set; }
        public static string Code{ get; set; }
        public static string GrantType { get; set; }
        

        public const string UrlToken = "https://login.live.com/oauth20_token.srf";
        public const string UrlAuthorize = "https://login.live.com/oauth20_authorize.srf";

        static Connection connection;
        
        public static string Token
        {
            get
            {
                if (DateTime.Now > tokenExpiresDate)
                {
                    expired = true;
                    do
                    {
                        if (Expired != null)
                            Expired(null, new EventArgs());
                    } while (expired);
                }
                return connection.AccessToken;
            }
        }
        [WLive("expires_in")]
        public static string ExpiresIn
        {
            set
            {
                tokenExpiresDate = DateTime.Now.AddSeconds(double.Parse(value, CultureInfo.InvariantCulture) - 10);
                expired = false;
            }
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Event is raised when access token expired
        /// </summary>
        public static event EventHandler<EventArgs> Expired;
        /// <summary>
        /// Gives new access token. Use this method for login or refreshing access token
        /// </summary>
        public static void LogOn()
        {
            
            QueryParameters qp = new QueryParameters();
            if (!string.IsNullOrEmpty(Connection.RefreshToken))
            {
                qp.Add("refresh_token", Connection.RefreshToken);
                GrantType = "refresh_token";
            }
            else
            {
                qp.Add("code", Code);
                GrantType = "authorization_code";
            }
            qp.Add("grant_type", GrantType);
            qp.Add("client_id", ClientId);
            qp.Add("client_secret", ClientSecret);
            qp.Add("redirect_uri", RedirectUri.ToString());
            try
            {
                connection = Requester.Request<Connection>(new RequestObject { Url = UrlBuilder1.Concat(new Uri(UrlToken), qp) }).ElementAt(0);
            }
            catch
            {
                throw;
            }
        }

        public static Uri GetAuthorizationUrl(string[] scopes, string responseType)
        {
            return GetAuthorizationUrl(scopes, responseType, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scopes">Scopes which you want to use</param>
        /// <param name="response_type">Access token, code, ...</param>
        /// <param name="isMobileApps">True when your app is mobile otherwise false</param>
        /// <returns>Returns Url for authorization</returns>
        public static Uri GetAuthorizationUrl(string[] scopes, string responseType, bool isMobileApps)
        {
            if (scopes == null)
                throw new ArgumentNullException("scopes");
            if (string.IsNullOrEmpty(responseType))
                throw new ArgumentNullException("responseType");
            QueryParameters qp = new QueryParameters();
            string ret = "";
            foreach (string s in scopes)
                ret += s + ",";
            if (ret.EndsWith(",", StringComparison.Ordinal))
                ret = ret.Substring(0, ret.Length - 1);
            qp.Add("scope", ret);
            qp.Add("response_type", responseType);
            if (isMobileApps)
                qp.Add("display", "touch");
            qp.Add("client_id", ClientId);
            qp.Add("client_secret", ClientSecret);
            qp.Add("redirect_uri", RedirectUri.ToString());
            return UrlBuilder1.Concat(new Uri(UrlAuthorize), qp);
        }
        /// <summary>
        /// Logout user
        /// </summary>
        public static void LogOff()
        { }
    }
}
