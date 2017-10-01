using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLive.WLiveObjects
{
    public class Comment:AGetOp<Comment>
    {
        [WLive.WLive("id")]
        public string Id { get; set; }
        [WLive.WLive("from")]
        public From From { get; set; }
        [WLive.WLive("message", true)]
        public string Message { get; set; }
        [WLive.WLive("created_time")]
        public DateTime CreatedTime { get; set; }
    }
}
