using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugSmear.Models;

namespace BugSmear.Controllers
{
    public class TicketHistoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketHistories
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var ticketHistorys = db.TicketHistorys.Include(t => t.Ticket).Include(t => t.User);
            return View(await ticketHistorys.ToListAsync());
        }

        // GET: TicketHistories/Details/5
        [Authorize]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketHistory ticketHistory = await db.TicketHistorys.FindAsync(id);
            if (ticketHistory == null)
            {
                return HttpNotFound();
            }
            return View(ticketHistory);
        }
    }
}
