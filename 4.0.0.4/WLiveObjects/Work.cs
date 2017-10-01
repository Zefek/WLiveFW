using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLive.WLiveObjects
{
    public class Work
    {
        [WLive.WLive("employer", true)]
        public Employer Employer { get; set; }
        [WLive.WLive("position", true)]
        public Position Position { get; set; }
    }
}
