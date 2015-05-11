using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugSmear.Models;
using BugSmear.Helpers;

namespace BugSmear.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return (RedirectToAction("Login", "Account"));
            }
            else
            {
                // return (RedirectToAction("Index", "Tickets"));

                // build DASHBOARD

                var model = new DashboardViewModel();
                var projects = db.Projects.Include("Tickets").OrderByDescending(p => p.Tickets.Count).Take(5);
                foreach (var p in projects)
                {
                    model.projInfo.Add(new ProjectInfo { ProjectName = p.ProjectName, NumTickets = p.Tickets.Count });
                }


                var helper = new UserHelpers();
                var devs = helper.UsersInRole("Developer");
                
                foreach (var d in devs)
                {
                    var ticketCount = db.Tickets.Where(t => t.AssignedToUserId == d.Id).Count();
                    model.devInfo.Add(new DevInfo {DevName = d.Email, NumTickets = ticketCount});
                }

                model.devInfo = model.devInfo.OrderByDescending(d => d.NumTickets).Take(3).ToList();

                model.TicketsAssigned = db.Tickets.Where(t => t.AssignedToUserId != null).Count();
                model.TicketsNotAssigned = db.Tickets.Where(t => t.AssignedToUserId == null).Count();
                model.TicketsResolved = db.Tickets.Where(t => t.TicketStatus.Status  ==  "Resolved").Count();
                model.TicketsOpen = db.Tickets.Where(t => t.TicketStatus.Status == "Open").Count();
                var today3 = System.DateTimeOffset.Now.AddDays(3);
                model.TicketsDue3 = db.Tickets.Where(t => t.DueDate <= today3 && t.DueDate >= System.DateTimeOffset.Now).Count();
                model.TicketsOverDue = db.Tickets.Where(t => t.DueDate < System.DateTimeOffset.Now).Count();
                
                return View(model);

            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



    }
}