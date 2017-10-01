using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLive.WLiveObjects
{
    public class Location
    {
        [WLive.WLive("latitude")]
        public double Latitude { get; set; }
        [WLive.WLive("longitude")]
        public double Longitude { get; set; }
        [WLive.WLive("altitude")]
        public double Altitude { get; set; }
    }
}
