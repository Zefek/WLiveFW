using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLive.WLiveObjects
{
    public class Address
    {
        [WLive.WLive("street")]
        public string Street { get; set; }
        [WLive.WLive("street_2")]
        public string Street2 { get; set; }
        [WLive.WLive("city")]
        public string City { get; set; }
        [WLive.WLive("state")]
        public string State { get; set; }
        [WLive.WLive("postal_code")]
        public string PostalCode { get; set; }
        [WLive.WLive("region")]
        public string Region { get; set; }
    }

    public class WLAddress
    {
        [WLive.WLive("personal")]
        public Address Personal { get; set; }
        [WLive.WLive("business")]
        public Address Business { get; set; }
    }
}
