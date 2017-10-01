using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLive.Utils;

namespace WLive.WLiveObjects
{
    public class Calendar:AGetOp<Calendar>
    {
        [WLive.WLive("id")]
        public string Id { get; set; }
        [WLive.WLive("name", true)]
        public string Name { get; set; }
        [WLive.WLive("description", true)]
        public string Description { get; set; }
        [WLive.WLive("created_time")]
        public DateTime CreatedTime { get; set; }
        [WLive.WLive("updated_time")]
        public DateTime UpdatedTime { get; set; }
        [WLive.WLive("from")]
        public From From { get; set; }
        [WLive.WLive("is_default")]
        public bool IsDefault { get; set; }
        [WLive.WLive("substription_location")]
        public string SubscriptionLocation { get; set; }
        [WLive.WLive("permissions")]
        public string Permissions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">USER_ID</param>
        /// <returns>Returns all calendars of user by user_id when parameter is not empty else returns all calendars of current user</returns>
        public static IEnumerable<Calendar> GetCalendars(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                userId = "me";
            try
            {
                return Requester.Request<Calendar>(new RequestObject { Url = UrlBuilder1.Build(userId + "/calendars") });
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Creates new calendar
        /// </summary>
        /// <param name="userId">USER_ID</param>
        /// <param name="name">Name of calendar</param>
        /// <param name="summary">Summary of calendar</param>
        /// <returns>Returns new calendar object</returns>
        public static Calendar Create(string userId, string name, string summary)
        {
            if (string.IsNullOrEmpty(userId))
                userId = "me";
            string data = "{\"name\": \"" + name + "\",\r\n \"summary\": \"" + summary + "\"}";
            try
            {
                RequestObject ro = new RequestObject { Url = UrlBuilder1.Build(userId + "/calendars"), Method = WebRequestMethods.Http.Post, ContentType = ContentType.ApplicationJson };
                ro.SetData(data);
                return Requester.Request<Calendar>(ro).ElementAt(0);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Update properties of calendar
        /// </summary>
        public void Update()
        {
            string data = "{\"name\": \"" + Name + "\"}";
            try
            {
                RequestObject ro = new RequestObject { Url = UrlBuilder1.Build(Id), Method=WebRequestMethods.Http.Put, ContentType = ContentType.ApplicationJson };
                ro.SetData(data);
                Requester.Request<Calendar>(ro).ElementAt(0);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Subscribe calendar
        /// </summary>
        /// <param name="userId">USER_ID</param>
        /// <param name="name">Name of calendar</param>
        /// <param name="subscribeLocation">Subscribe location</param>
        public static void Subscribe(string userId, string name, string subscribeLocation)
        {
            if (string.IsNullOrEmpty(userId))
                userId = "me";
            string data = "{\"name\": \"" + name + "\", \r\n \"subscription_location\": \""+subscribeLocation+"\"}";
            try
            {
                RequestObject ro = new RequestObject { Url = UrlBuilder1.Build(userId+"/calendars"), Method = WebRequestMethods.Http.Post, ContentType = ContentType.ApplicationJson };
                ro.SetData(data);
                Requester.Request<Calendar>(ro).ElementAt(0);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Delete current calendar
        /// </summary>
        public void Delete()
        {
            try
            {
                Requester.Request<Calendar>(new RequestObject { Url = UrlBuilder1.Build(Id), Method = WebRequestMethods.Http.Delete });
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Gets all events of calendar
        /// </summary>
        /// <returns>Events collection of calendar</returns>
        public IEnumerable<WLEvent> Events
        {
            get
            {
                try
                {
                    return Requester.Request<WLEvent>(new RequestObject { Url = UrlBuilder1.Build(Id + "/events") });
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
