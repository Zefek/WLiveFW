using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLive.WLiveObjects
{
    public class WLEvent:AGetOp<WLEvent>
    {
        [WLive.WLive("id")]
        public string Id { get; set; }
        [WLive.WLive("name", true)]
        public string Name { get; set; }
        [WLive.WLive("created_time")]
        public DateTime CreatedTime { get; set; }
        [WLive.WLive("updated_time")]
        public DateTime UpdatedTime { get; set; }
        [WLive.WLive("description", true)]
        public string Description { get; set; }
        [WLive.WLive("calendar_id")]
        public string CalendarId { get; set; }
        [WLive.WLive("from")]
        public From From { get; set; }
        [WLive.WLive("start_time", true)]
        public DateTime StartTime { get; set; }
        [WLive.WLive("end_time", true)]
        public DateTime EndTime { get; set; }
        [WLive.WLive("location", true)]
        public string Location { get; set; }
        [WLive.WLive("is_all_day_event", true)]
        public bool IsAllDayEvent { get; set; }
        [WLive.WLive("is_recurrent")]
        public bool IsRecurrent { get; set; }
        [WLive.WLive("recurrence")]
        public string Recurrence { get; set; }
        [WLive.WLive("reminder_time", true)]
        public int ReminderTime { get; set; }
        [WLive.WLive("availability", true)]
        public string Availability { get; set; }
        [WLive.WLive("visibility", true)]
        public string Visibility { get; set; }

        //create, delete
    }
}
