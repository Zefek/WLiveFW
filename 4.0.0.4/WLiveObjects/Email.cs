using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLive.WLiveObjects
{
    public class Email
    {
        [WLive.WLive("preferred", true)]
        public string Preferred { get; set; }
        [WLive.WLive("account", true)]
        public string Account { get; set; }
        [WLive.WLive("personal", true)]
        public string Personal { get; set; }
        [WLive.WLive("business", true)]
        public string Business { get; set; }
        [WLive.WLive("other", true)]
        public string Other { get; set; }
    }
}
