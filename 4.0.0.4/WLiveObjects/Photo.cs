using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLive.WLiveObjects
{
    public class Photo : AWithTag<Photo>
    {
        [WLive.WLive("tags_enabled")]
        public bool TagsEnabled { get; set; }
        [WLive.WLive("tag_counts")]
        public int TagCounts { get; set; }
        [WLive.WLive("picture")]
        public string Picture { get; set; }
        [WLive.WLive("images")]
        public IEnumerable<Image> Images { get; set; }
        [WLive.WLive("when_taken")]
        public string WhenTaken { get; set; }
        [WLive.WLive("height")]
        public int Height { get; set; }
        [WLive.WLive("width")]
        public int Width { get; set; }
        [WLive.WLive("location")]
        public Location Location { get; set; }
        [WLive.WLive("camera_make")]
        public string CameraMake { get; set; }
        [WLive.WLive("camera_model")]
        public string CameraModel { get; set; }
        [WLive.WLive("focal_ratio")]
        public string FocalRatio { get; set; }
        [WLive.WLive("exposure_numerator")]
        public string ExposureNumerator { get; set; }
        [WLive.WLive("exposure_denominator")]
        public string ExposureDenominator { get; set; }
        [WLive.WLive("focal_length")]
        public string FocalLength { get; set; }
    }
}
