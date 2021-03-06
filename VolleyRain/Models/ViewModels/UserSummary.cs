﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class UserSummary
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsCoach { get; set; }
        public bool IsTemporary { get; set; }
        public int NumberOfAttendings { get; set; }
        public int PossibleAttendings { get; set; }
        public int AttendanceInPercentage
        {
            get
            {
                return PossibleAttendings == 0 ? 0 : (int)Math.Round((double)NumberOfAttendings / (double)PossibleAttendings * 100);
            }
        }
    }
}