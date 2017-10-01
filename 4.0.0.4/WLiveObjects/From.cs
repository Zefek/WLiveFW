using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLive.WLiveObjects
{
    public class From
    {
        [WLive.WLive("id")]
        public string Id { get; set; }
        [WLive.WLive("name")]
        public string Name { get; set; }
    }

    /*
    public class DataObject<T>
    {
        [WLive.WLive("data")]
        public T[] data;
    }
    */
}
