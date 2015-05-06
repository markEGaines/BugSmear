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
using Microsoft.AspNet.Identity;
using System.IO;

namespace BugSmear.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tickets
        public async Task<ActionResult> Index()
        {
            //   var tickets = db.Tickets.Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            //   var tickets = db.Tickets.Include(t => t.Project);

            //ticket.OwnerUserId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;



            if (User.IsInRole("Administrator"))
            {
                ViewBag.asRole = "Administrator";
                var tickets = from t in db.Tickets
                              orderby t.TicketStatus.Status, t.TicketPriority.Priority, t.DueDate.Value
                              select t;

                return View(await tickets.ToListAsync());
            }
            else if (User.IsInRole("Project Manager"))
            {
                ViewBag.asRole = "Project Manager";
                var userId = User.Identity.GetUserId();
                var tickets = from u in db.Users
                              where u.Id == userId
                              from p in u.Projects
                              from t in p.Tickets
                              select t;

                return View(await tickets.ToListAsync());
            }
            else if (User.IsInRole("Developer"))
            {
                ViewBag.asRole = "Developer";
                //var tickets = from t in db.Tickets
                //              where (t.AssignedToUser.UserName == User.Identity.Name)
                //                  select t;
                var userId = User.Identity.GetUserId();
                var tickets = from u in db.Users
                              where u.Id == userId
                              from p in u.Projects
                              from t in p.Tickets
                              select t;

                return View(await tickets.ToListAsync());
            }
            else if (User.IsInRole("Submitter"))
            {
                ViewBag.asRole = "Submitter";
                var tickets = from t in db.Tickets
                              where (t.OwnerUser.UserName == User.Identity.Name)
                              select t;

                return View(await tickets.ToListAsync());
            }

            return View();

            //   return View(await tickets.ToListAsync());

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
            // sort comments in descending order
            ticket.TicketComments = ticket.TicketComments.OrderByDescending(p => p.Created).ToList();


            return View(ticket);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectName");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Priority");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Type");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description,Created,ProjectId,TicketTypeId,TicketStatusId,TicketPriorityId,OwnerUserId,EstHours,DueDate")] Ticket ticket, HttpPostedFileBase image)
        {

            if (image != null && image.ContentLength > 0)
            {
                var ext = Path.GetExtension(image.FileName).ToLower();                    // check file type is image
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg")
                    ModelState.AddModelError("image", "Invalid format.");
            }

            if (ModelState.IsValid)
            {
                ticket.Created = System.DateTimeOffset.Now;
                ticket.TicketStatusId = db.TicketStatus.FirstOrDefault(ts => ts.Status == "Open").Id;
                ticket.OwnerUserId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
                if (image != null && image.ContentLength > 0)
                {
                    TicketAttachment ta = new TicketAttachment();
                    var filePath = "/Uploads/tickets/images/";
                    var absPath = Server.MapPath("~" + filePath);
                    ta.FileUrl = filePath + image.FileName;
                    image.SaveAs(Path.Combine(absPath, image.FileName));
                    ta.Created = System.DateTimeOffset.Now;
                    ta.UserId = User.Identity.GetUserId();
                    db.TicketAttachments.Add(ta);
                }

                db.Tickets.Add(ticket);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectName", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Priority", ticket.TicketPriorityId);
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
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId,EstHours,DueDate,TicketAttachment")] Ticket ticket, HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                var ext = Path.GetExtension(image.FileName).ToLower();                    // check file type is image
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg")
                    ModelState.AddModelError("image", "Invalid format.");
            }

            if (ModelState.IsValid)
            {
                if (image != null && image.ContentLength > 0)
                {
                    TicketAttachment ta = new TicketAttachment();
                    var filePath = "/Uploads/tickets/images/";
                    var absPath = Server.MapPath("~" + filePath);
                    ta.FileUrl = filePath + image.FileName;
                    image.SaveAs(Path.Combine(absPath, image.FileName));
                    ta.Created = System.DateTimeOffset.Now;
                    ta.UserId = User.Identity.GetUserId();
                    ta.TicketId = ticket.Id;
                    db.TicketAttachments.Add(ta);
                }
                var transactionUpdateTimeStamp = System.DateTimeOffset.Now;
                ticket.Updated = transactionUpdateTimeStamp;
                db.Entry(ticket).State = EntityState.Modified;

                //build TicketHistory
                var oldTicket = (from t in db.Tickets.AsNoTracking()
                                 where t.Id == ticket.Id
                                 select t).FirstOrDefault();

                if (oldTicket.AssignedToUserId != ticket.AssignedToUserId)
                {
                    var assignedHistory = new TicketHistory
                    {
                        TicketId = ticket.Id,
                        UserId = User.Identity.GetUserId(),
                        Property = "AssignedUserId",
                        oldValue = oldTicket.AssignedToUserId,
                        newValue = ticket.AssignedToUserId,
                        Changed = transactionUpdateTimeStamp
                    };
                    db.TicketHistorys.Add(assignedHistory);
                    // notify user here
                }
                if (oldTicket.Description != ticket.Description)
                {
                    var descriptionHistory = new TicketHistory
                    {
                        TicketId = ticket.Id,
                        UserId = User.Identity.GetUserId(),
                        Property = "Description",
                        oldValue = oldTicket.Description,
                        newValue = ticket.Description,
                        Changed = transactionUpdateTimeStamp
                    };
                    db.TicketHistorys.Add(descriptionHistory);
                }
                if (oldTicket.DueDate != ticket.DueDate)
                {
                    var duedateHistory = new TicketHistory
                    {
                        TicketId = ticket.Id,
                        UserId = User.Identity.GetUserId(),
                        Property = "DueDate",
                        oldValue = oldTicket.DueDate.ToString(),
                        newValue = ticket.DueDate.ToString(),
                        Changed = transactionUpdateTimeStamp
                    };
                    db.TicketHistorys.Add(duedateHistory);
                }
                if (oldTicket.EstHours != ticket.EstHours)
                {
                    var esthoursHistory = new TicketHistory
                    {
                        TicketId = ticket.Id,
                        UserId = User.Identity.GetUserId(),
                        Property = "EstHours",
                        oldValue = oldTicket.EstHours.ToString(),
                        newValue = ticket.EstHours.ToString(),
                        Changed = transactionUpdateTimeStamp
                    };
                    db.TicketHistorys.Add(esthoursHistory);
                }

                if (oldTicket.TicketPriorityId != ticket.TicketPriorityId)
                {
                    var priorityHistory = new TicketHistory
                    {
                        TicketId = ticket.Id,
                        UserId = User.Identity.GetUserId(),
                        Property = "TicketPriority",
                        oldValue = oldTicket.TicketPriorityId.ToString(),
                        newValue = ticket.TicketPriorityId.ToString(),
                        Changed = transactionUpdateTimeStamp
                    };
                    db.TicketHistorys.Add(priorityHistory);
                }

                if (oldTicket.TicketStatusId != ticket.TicketStatusId)
                {
                    var statusHistory = new TicketHistory
                    {
                        TicketId = ticket.Id,
                        UserId = User.Identity.GetUserId(),
                        Property = "TicketStatus",
                        oldValue = oldTicket.TicketStatusId.ToString(),
                        newValue = ticket.TicketStatusId.ToString(),
                        Changed = transactionUpdateTimeStamp
                    };
                    db.TicketHistorys.Add(statusHistory);
                }

                if (oldTicket.TicketTypeId != ticket.TicketTypeId)
                {
                    var typeHistory = new TicketHistory
                    {
                        TicketId = ticket.Id,
                        UserId = User.Identity.GetUserId(),
                        Property = "TicketType",
                        oldValue = oldTicket.TicketTypeId.ToString(),
                        newValue = ticket.TicketTypeId.ToString(),
                        Changed = transactionUpdateTimeStamp
                    };
                    db.TicketHistorys.Add(typeHistory);
                }

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
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Ticket ticket = await db.Tickets.FindAsync(id);
        //    if (ticket == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ticket);
        //}

        // POST: Tickets/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Ticket ticket = await db.Tickets.FindAsync(id);
        //    db.Tickets.Remove(ticket);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        // POST: Tickets/CreateComment
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateComment([Bind(Include = "TicketId,AuthorId,Created,Comment")] TicketComment ticketcomment)
        {

            if (ModelState.IsValid)
            {
                if (String.IsNullOrWhiteSpace(ticketcomment.Comment))
                {
                    ModelState.AddModelError("Comment", "Missing Comment Text");
                    return RedirectToAction("Details", new { id = ticketcomment.TicketId });
                }



                ticketcomment.Created = System.DateTimeOffset.Now;
                ticketcomment.UserId = User.Identity.GetUserId();

                db.TicketComments.Add(ticketcomment);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = ticketcomment.TicketId });

            }
            return RedirectToAction("Details", new { id = ticketcomment.TicketId });
        }

        // GET: Posts/DeleteComment
        //[Authorize(Roles = "Administrator", "Project Manager")]
        public async Task<ActionResult> DeleteComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment comment = await db.TicketComments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Posts/DeleteComment
        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> DeleteCommentConfirmed(int id)
        {
            TicketComment ticketcomment = await db.TicketComments.FindAsync(id);
            Ticket post = db.Tickets.Find(ticketcomment.TicketId);
            db.TicketComments.Remove(ticketcomment);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = ticketcomment.TicketId });
        }

        // GET: Posts/EditComment
        //[Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> EditComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment comment = await db.TicketComments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }


        // POST: Posts/EditComment
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin, Moderator")]
        //public async Task<ActionResult> Edit([Bind(Include = "Id,Created,Updated,Title,Body,MediaUrl,Slug")] Post post)
        public async Task<ActionResult> EditComment([Bind(Include = "Created,Id,UserId,Comment,TicketId,Hours,PercentComplete")] TicketComment ticketcomment)
        {
            if (ModelState.IsValid)
            {
                db.TicketComments.Attach(ticketcomment);
                db.Entry(ticketcomment).State = EntityState.Modified;

                Ticket ticket = db.Tickets.Find(ticketcomment.TicketId);

                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = ticketcomment.TicketId });
                //return View();
            }
            return View(ticketcomment);
        }









    }
}
