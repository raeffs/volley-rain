using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VolleyRain.Models;

namespace VolleyRain.Controllers
{
    public class NavigationController : BaseController
    {
        public readonly List<NavigationGroup> LinkGroups = new List<NavigationGroup>
        {
            new NavigationGroup 
            { 
                DisplayText = "News", 
                HideIfOnlyOneLink = true, 
                Links = new List<NavigationLink> 
                {
                    new NavigationLink { Controller = "News", Action = "Index", DisplayText = "News", Role = string.Empty },
                }
            },
            new NavigationGroup 
            { 
                DisplayText = "Rangliste", 
                HideIfOnlyOneLink = true, 
                Links = new List<NavigationLink> 
                {
                    new NavigationLink { Controller = "Ranking", Action = "Index", DisplayText = "Rangliste", Role = string.Empty },
                }
            },
            new NavigationGroup 
            { 
                DisplayText = "Kalender", 
                HideIfOnlyOneLink = true, 
                Links = new List<NavigationLink> 
                {
                    new NavigationLink { Controller = "Calendar", Action = "Index", DisplayText = "Übersicht", Role = "User" },
                    new NavigationLink { Controller = "Game", Action = "Index", DisplayText = "Spielplan", Role = string.Empty },
                    new NavigationLink { Controller = "Attendance", Action = "Index", DisplayText = "Anwesenheitskontrolle", Role = "User" },
                }
            },
            new NavigationGroup 
            { 
                DisplayText = "Dokumente", 
                HideIfOnlyOneLink = true, 
                Links = new List<NavigationLink> 
                {
                    new NavigationLink { Controller = "Document", Action = "Index", DisplayText = "Dokumente", Role = "User" },
                    new NavigationLink { Controller = "Document", Action = "Upload", DisplayText = "Dokument hochladen", Role = "User", Hide = true },
                }
            },
            new NavigationGroup 
            { 
                DisplayText = "Verwaltung", 
                HideIfOnlyOneLink = false, 
                Links = new List<NavigationLink> 
                {
                    new NavigationLink { Controller = "Team", Action = "Members", DisplayText = "Teammitglieder", Role = "Team-Administrator" },
                    //new NavigationLink { Controller = "Attendance", Action = "Types", DisplayText = "Anwesenheits-Typen", Role = "Team-Administrator" },
                }
            },
            new NavigationGroup 
            { 
                DisplayText = "Administration", 
                HideIfOnlyOneLink = false, 
                Links = new List<NavigationLink> 
                {
                    new NavigationLink { Controller = "Log", Action = "Index", DisplayText = "Log", Role = "Administrator", RouteValues = new { fixedID = "", page = "" } },
                }
            },
        };

        public ActionResult Render()
        {
            var model = GetLinkGroups();

            var currentController = HttpContext.Request.RequestContext.RouteData.Values["controller"] as string ?? "News";
            var currentAction = HttpContext.Request.RequestContext.RouteData.Values["action"] as string ?? "Index";
            var active = model.SelectMany(g => g.Links).SingleOrDefault(l => l.Controller == currentController && l.Action == currentAction)
                ?? model.SelectMany(g => g.Links).SingleOrDefault(l => l.Controller == currentController);
            if (active != null) active.IsActive = true;

            foreach (var group in model)
            {
                List<NavigationLink> toRemove = null;
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    toRemove = group.Links.Where(l => !string.IsNullOrWhiteSpace(l.Role) && !HttpContext.User.IsInRole(l.Role)).ToList();
                }
                else
                {
                    toRemove = group.Links.Where(l => !string.IsNullOrWhiteSpace(l.Role)).ToList();
                }
                toRemove.ForEach(l => group.Links.Remove(l));
            }
            return PartialView(model.Where(g => g.Links.Count > 0));
        }

        private List<NavigationGroup> GetLinkGroups()
        {
            var model = LinkGroups.ToList();

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                model.Add(new NavigationGroup
                {
                    DisplayText = string.Format("Angemeldet als {0}", Session.UserName),
                    HideIfOnlyOneLink = false,
                    AlignRight = true,
                    Links = new List<NavigationLink>
                    {
                        new NavigationLink { Controller = "Account", Action = "ChangePassword", DisplayText = "Passwort ändern", Role = string.Empty },
                        new NavigationLink { Controller = "Account", Action = "Logout", DisplayText = "Logout", Role = string.Empty },
                    }
                });
            }
            else
            {
                model.Add(new NavigationGroup
                {
                    DisplayText = "Login",
                    HideIfOnlyOneLink = true,
                    AlignRight = true,
                    Links = new List<NavigationLink>
                    {
                        new NavigationLink { Controller = "Account", Action = "Login", DisplayText = "Login", Role = string.Empty },
                    }
                });
            }

            return model;
        }
    }
}