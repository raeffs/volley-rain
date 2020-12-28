using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace VolleyRain.Validation
{
    public class FileTypesAttribute : ValidationAttribute
    {
        private readonly List<string> _types;

        public FileTypesAttribute(params string[] types)
        {
            _types = types.ToList();
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            var fileExt = Path.GetExtension((value as HttpPostedFileBase).FileName).Trim('.');
            return _types.Contains(fileExt, StringComparer.InvariantCultureIgnoreCase);
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("Ungültiger Datei-Typ. Nur die folgenden Typen sind zulässig: {0}", string.Join(", ", _types));
        }
    }
}