using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLive.WLiveObjects
{
    public class WLFriend:AGetOp<WLFriend>
    {
        [WLive.WLive("id")]
        public string Id { get; set; }
        [WLive.WLive("name")]
        public string Name { get; set; }
    }
}
