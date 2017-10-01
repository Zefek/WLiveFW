using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLive.WLiveObjects
{
    public class Video : AWithTag<Video>
    {
        [WLive.WLive("tags_count")]
        public string TagsCount { get; set; }
        [WLive.WLive("tags_enabled")]
        public string TagsEnabled { get; set; }
        [WLive.WLive("picture")]
        public string Picture { get; set; }
        [WLive.WLive("height")]
        public string Height { get; set; }
        [WLive.WLive("width")]
        public string Width { get; set; }
        [WLive.WLive("duration")]
        public string Duration { get; set; }
        [WLive.WLive("bitrate")]
        public string Bitrate { get; set; }
    }
}
