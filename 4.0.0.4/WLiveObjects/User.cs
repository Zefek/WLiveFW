using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WLive.Utils;

namespace WLive.WLiveObjects
{
    public class User:AGetOp<User>
    {
        [WLive.WLive("id")]
        public string Id { get; set; }
        [WLive.WLive("name")]
        public string Name { get; set; }
        [WLive.WLive("first_name")]
        public string FirstName { get; set; }
        [WLive.WLive("last_name")]
        public string LastName { get; set; }
        [WLive.WLive("link")]
        public string Link { get; set; }
        [WLive.WLive("birth_day")]
        public int Birthday { get; set; }
        [WLive.WLive("birth_month")]
        public int BirthMonth { get; set; }
        [WLive.WLive("birth_year")]
        public int BirthYear { get; set; }
        [WLive.WLive("work")]
        public IEnumerable<Work> Work { get; set; }
        [WLive.WLive("emails")]
        public Email Emails { get; set; }
        [WLive.WLive("addresses")]
        public WLAddress Addresses { get; set; }
        [WLive.WLive("phones")]
        public Phone Phones { get; set; }
        [WLive.WLive("locale")]
        public string Locale { get; set; }
        [WLive.WLive("updated_time")]
        public DateTime UpdatedTime { get; set; }

    /// <summary>
    /// Gets folders in root of current user
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Folder> GetFolders()
        {
            try
            {
                return GetFolders(System.IO.SearchOption.TopDirectoryOnly);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Get user by userid
        /// </summary>
        /// <param name="usrid">USER_ID</param>
        /// <returns>Return user object</returns>
        public new static User Get(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return Get("me");
                return AGetOp<User>.Get(userId);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Gets folders or subfolders of current user
        /// </summary>
        /// <param name="option">Option if you want to folders with subfolders or not</param>
        /// <returns>List of folders or subfolders of current user</returns>
        public IEnumerable<Folder> GetFolders(System.IO.SearchOption option)
        {
            if (string.IsNullOrEmpty(Id))
                Id = "me";
            Filter f = new Filter(FilterTypes.Folder);
            QueryParameters qp = new QueryParameters();
            qp.Add("filter", f.GetTypesForQuery);
            try
            {
                if (option == System.IO.SearchOption.TopDirectoryOnly)
                    return Requester.Request<Folder>(new RequestObject { Url = UrlBuilder1.Build(Id + "/skydrive/files", qp) }).ToList();
                else
                {
                    List<Folder> k = new List<Folder>();
                    foreach (Folder folder in Requester.Request<Folder>(new RequestObject { Url = UrlBuilder1.Build(Id + "/files", qp) }))
                    {
                        k.Add(folder);
                        k.AddRange(folder.GetFolders(option));
                    }
                    return k;
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Gets shared folders of current user
        /// </summary>
        /// <returns>List of shared folders of current user</returns>
        public IEnumerable<Folder> SharedFolders
        {
            get
            {
                if (string.IsNullOrEmpty(Id))
                    Id = "me";
                Filter f = new Filter(FilterTypes.Folder);
                QueryParameters qp = new QueryParameters();
                qp.Add("filter", f.GetTypesForQuery);
                try
                {
                    return Requester.Request<Folder>(new RequestObject { Url = UrlBuilder1.Build(Id + "/skydrive/shared/files", qp) }).ToList();
                }
                catch
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Gets album of current user
        /// </summary>
        /// <returns>List of album of current user</returns>
        public IEnumerable<Album> Albums
        {
            get
            {
                if (string.IsNullOrEmpty(Id))
                    Id = "me";
                Filter f = new Filter(FilterTypes.Album);
                QueryParameters qp = new QueryParameters();
                qp.Add("filter", f.GetTypesForQuery);
                try
                {
                    return Requester.Request<Album>(new RequestObject { Url = UrlBuilder1.Build(Id + "/skydrive/files", qp) }).ToList();
                }
                catch
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Gets all events of user
        /// </summary>
        /// <param name="usrid">USER_ID</param>
        /// <returns>Events collection of user</returns>
        public static IEnumerable<WLEvent> GetEvents(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                userId = "me";
            try
            {
                return Requester.Request<WLEvent>(new RequestObject { Url = UrlBuilder1.Build(userId + "/events") }).ToList();
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns friends of current user</returns>
        public IEnumerable<WLFriend> Friends
        {
            get
            {
                try
                {
                    return Requester.Request<WLFriend>(new RequestObject { Url = UrlBuilder1.Build(string.IsNullOrEmpty(Id) ? "me/friends" : Id + "/friends") }).ToList();
                }
                catch
                {
                    throw;
                }
            }
        }

        internal class UserContact
        {
            [WLive("contact", true)]
            public Contact ContactInfo { get; set; }
            [WLive("emails", true)]
            public Email Emails { get; set;}
            [WLive("work", true)]
            public Work[] Works { get; set; }

        }

        /// <summary>
        /// Create new contact for current user. At least one parameter must be filled.
        /// </summary>
        /// <param name="firstname">Contact firstname</param>
        /// <param name="lastname">Contact lastname</param>
        /// <param name="emails">Contact emails</param>
        /// <param name="works">Contact works</param>
        public void CreateContact(Contact contact, Email emails, Work[] work)
        {
            UserContact uc = new UserContact { ContactInfo = contact, Emails = emails, Works = work };
            /*
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            if (contact != null)
            {
                if (!string.IsNullOrEmpty(contact.FirstName))
                    sb.Append("\"first_name\": \"" + contact.FirstName + "\"\r\n");
                if(!string.IsNullOrEmpty(contact.LastName))
                    sb.Append("\"last_name\": \"" + contact.LastName + "\"\r\n");
            }
            string email = "";
            if (emails != null)
                email = "\"emails\": " + JSONDataBuilder.Build(emails);
            if (!string.IsNullOrEmpty(email))
                sb.Append(email);
            string wrk = "";
            if (work!=null && work.Length > 0)
            {
                wrk = "\"work\": [ { ";
                for (int i = 0; i < work.Length; i++)
                {
                    wrk += "\"employer\": " + JSONDataBuilder.Build(work[i].Employer);
                }
                wrk += "} ]";
            }
            if (!string.IsNullOrEmpty(wrk))
                sb.Append(wrk);
            sb.Append("}");
            */
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(uc, AGetOpConverter<UserContact>.Settings);
            try
            {
                RequestObject ro = new RequestObject();
                ro.Method = WebRequestMethods.Http.Post;
                ro.SetData(data);
                ro.Url = UrlBuilder1.Build(Id + "/contacts");
                Requester.Request<Contact>(ro);
            }
            catch
            { throw; }
        }
        /// <summary>
        /// Gets all user contacts
        /// </summary>
        /// <returns>Returns current user contacts</returns>
        public IEnumerable<Contact> Contacts
        {
            get
            {
                try
                {
                    return Requester.Request<Contact>(new RequestObject { Url = UrlBuilder1.Build(Id + "/contacts") }).ToList();
                }
                catch
                { throw; }
            }
        }

    }
}
