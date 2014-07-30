using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VolleyRain.DataAccess;
using VolleyRain.Models;

namespace VolleyRain.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly Lazy<CacheDecorator> _cache;
        private readonly Lazy<DatabaseContext> _context;
        private readonly Lazy<SessionDecorator> _session;

        public BaseController()
        {
            _cache = new Lazy<CacheDecorator>(() => new CacheDecorator(HttpContext.Cache));
            _context = new Lazy<DatabaseContext>(() => new DatabaseContext());
            _session = new Lazy<SessionDecorator>(() => new SessionDecorator(HttpContext.Session));
        }

        protected CacheDecorator Cache { get { return _cache.Value; } }

        protected DatabaseContext Context { get { return _context.Value; } }

        protected new SessionDecorator Session { get { return _session.Value; } }

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