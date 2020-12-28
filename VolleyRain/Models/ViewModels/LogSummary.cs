using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class LogSummary
    {
        public int ID { get; set; }

        public DateTime TimeStamp { get; set; }

        public string Level { get; set; }

        public string Logger { get; set; }

        public string ShortLogger { get { return Logger.Split('.').Last(); } }

        public string Message { get; set; }

        public bool HasException { get; set; }

        public string SessionID { get; set; }
    }
}