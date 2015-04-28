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
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tickets
        public async Task<ActionResult> Index()
        {
            var tickets = db.Tickets.Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(await tickets.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectName");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Priority");
            //ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Status");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Type");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description,Created,ProjectId,TicketTypeId,TicketStatusId,TicketPriorityId,OwnerUserId,EstHours,DueDate")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Created = System.DateTimeOffset.Now;
                ticket.TicketStatusId = db.TicketStatus.FirstOrDefault(ts => ts.Status == "Open").Id;
                ticket.OwnerUserId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
                db.Tickets.Add(ticket);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectName", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Priority", ticket.TicketPriorityId);
            //ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Status", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Type", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectName", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Priority", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Status", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Type", ticket.TicketTypeId);
            ViewBag.AssignedToUserId = new SelectList(db.Users.Where(u => u.Roles.Any(ur => ur.RoleId == db.Roles.FirstOrDefault(r => r.Name == "Developer").Id)), "Id", "UserName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users.Where(u => u.Roles.Any(ur => ur.RoleId == db.Roles.FirstOrDefault(r => r.Name == "Submitter").Id)), "Id", "UserName", ticket.OwnerUserId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId,EstHours,DueDate")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Updated = System.DateTimeOffset.Now;
                db.Entry(ticket).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectName", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Priority", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Status", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Type", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Ticket ticket = await db.Tickets.FindAsync(id);
            db.Tickets.Remove(ticket);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
