using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLive.WLiveObjects
{
    public class Tag :AGetOp<Tag>
    {
        [WLive.WLive("id")]
        public string Id { get; set; }
        [WLive.WLive("user", true)]
        public From User { get; set; }
        [WLive.WLive("x")]
        public string X { get; set; }
        [WLive.WLive("y")]
        public string Y { get; set; }
        [WLive.WLive("created_time")]
        public string CreatedTime { get; set; }
    }
}
