using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VolleyRain.Validation
{
    public class FileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSizeInMB;

        public FileSizeAttribute(int maxFileSizeInMB)
        {
            _maxFileSizeInMB = maxFileSizeInMB;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            return (value as HttpPostedFileBase).ContentLength <= (_maxFileSizeInMB * 1024 * 1024);
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("Die Datei darf nicht grösser als {0} MB sein.", _maxFileSizeInMB);
        }
    }
}