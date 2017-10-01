using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLive.WLiveObjects
{
    public class Image
    {
        [WLive.WLive("width")]
        public int Width { get; set; }
        [WLive.WLive("height")]
        public int Height { get; set; }
        [WLive.WLive("type")]
        public string FType { get; set; }
        [WLive.WLive("source")]
        public string Source { get; set; }
    }
}
