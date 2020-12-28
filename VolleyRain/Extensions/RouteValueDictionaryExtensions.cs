using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace VolleyRain
{
    public static class RouteValueDictionaryExtensions
    {
        public static RouteValueDictionary Extend(this RouteValueDictionary dest, IEnumerable<KeyValuePair<string, object>> src)
        {
            src.ToList().ForEach(x => { dest[x.Key] = x.Value; });
            return dest;
        }

        public static RouteValueDictionary Merge(this RouteValueDictionary source, IEnumerable<KeyValuePair<string, object>> newData)
        {
            return (new RouteValueDictionary(source)).Extend(newData);
        }
    }
}