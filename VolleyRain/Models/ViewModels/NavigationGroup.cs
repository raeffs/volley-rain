using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class NavigationGroup
    {
        public string DisplayText { get; set; }

        public List<NavigationLink> Links { get; set; }

        public bool IsActive { get { return Links.Any(l => l.IsActive); } }

        public bool HideIfOnlyOneLink { get; set; }

        public bool AlignRight { get; set; }

        public NavigationGroup()
        {
            Links = new List<NavigationLink>();
        }
    }
}