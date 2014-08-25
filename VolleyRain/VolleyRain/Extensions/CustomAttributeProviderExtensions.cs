using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace VolleyRain
{
    public static class CustomAttributeProviderExtensions
    {
        public static bool HasCustomAttributes<T>(this ICustomAttributeProvider provider, bool inherit = false) where T : Attribute
        {
            return provider.GetCustomAttributes<T>(inherit).Any();
        }

        public static IEnumerable<T> GetCustomAttributes<T>(this ICustomAttributeProvider provider, bool inherit = false) where T : Attribute
        {
            return provider.GetCustomAttributes(typeof(T), inherit).OfType<T>();
        }
    }
}