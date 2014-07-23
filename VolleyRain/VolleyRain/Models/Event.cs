﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public enum EventType
    {
        Match,
        Training
    }

    public class Event
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public EventType Type { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}