using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class Log
    {
        public int ID { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }

        [Required]
        public string Application { get; set; }

        [Required]
        public string Level { get; set; }

        [Required]
        public string Logger { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string SessionID { get; set; }

        [Required]
        public int ThreadID { get; set; }

        [Required]
        public string UserIdentity { get; set; }

        [Required]
        public string Exception { get; set; }
    }
}