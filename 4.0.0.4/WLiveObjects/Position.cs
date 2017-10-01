using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLive.WLiveObjects
{
    public class Position
    {
        [WLive.WLive("name", true)]
        public string Name { get; set; }
    }
}
