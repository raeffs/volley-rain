using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VolleyRain.DataAccess;

namespace VolleyRain.Controllers
{
    public abstract class BaseController : Controller
    {
        protected DatabaseContext Context { get; private set; }

        public BaseController()
        {
            Context = new DatabaseContext();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}