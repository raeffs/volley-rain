using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VolleyRain.Validation;

namespace VolleyRain.Models
{
    public class DocumentUpload
    {
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Dokument")]
        [FileSize(10)]
        [FileTypes("doc", "docx", "pdf", "xls", "xlsx", "ppt", "pptx", "txt")]
        public HttpPostedFileBase Document { get; set; }
    }
}