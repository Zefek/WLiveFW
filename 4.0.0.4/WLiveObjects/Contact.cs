using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLive.WLiveObjects
{
    public class Contact:AGetOp<Contact>
    {
        [WLive.WLive("id")]
        public string Id { get; set; }
        [WLive.WLive("first_name", true)]
        public string FirstName { get; set; }
        [WLive.WLive("last_name", true)]
        public string LastName { get; set; }
        [WLive.WLive("name")]
        public string Name { get; set; }
        [WLive.WLive("is_friend")]
        public bool IsFriend { get; set; }
        [WLive.WLive("is_favorite", true)]
        public bool IsFavorite { get; set; }
        [WLive.WLive("user_id")]
        public string UserId { get; set; }
        [WLive.WLive("email_hashes")]
        public IEnumerable<string> EmailHashes { get; set; }
        [WLive.WLive("updated_time")]
        public DateTime UpdatedTime { get; set; }
        [WLive.WLive("birth_day")]
        public int Birthday { get; set; }
        [WLive.WLive("birth_month")]
        public int BirthMonth { get; set; }
    }
}
