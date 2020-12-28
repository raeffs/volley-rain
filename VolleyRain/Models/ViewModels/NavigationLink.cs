using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class NavigationLink
    {
        public string Controller { get; set; }

        public string Action { get; set; }

        public string DisplayText { get; set; }

        public string Role { get; set; }

        public bool IsActive { get; set; }

        public object RouteValues { get; set; }

        public bool Hide { get; set; }
    }
}