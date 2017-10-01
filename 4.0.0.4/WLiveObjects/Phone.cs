using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLive.WLiveObjects
{
    public class Phone
    {
        [WLive.WLive("personal")]
        public string Personal { get; set; }
        [WLive.WLive("business")]
        public string Business { get; set; }
        [WLive.WLive("mobile")]
        public string Mobile { get; set; }
    }
}
