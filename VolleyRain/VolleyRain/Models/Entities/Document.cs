using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class Document
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
    }
}