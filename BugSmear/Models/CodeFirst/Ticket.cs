using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugSmear.Models
{
    public class Ticket
    {
        public Ticket()                 
        {
            this.TicketAttachments = new HashSet<TicketAttachment>();
            this.TicketComments = new HashSet<TicketComment>();
            this.TicketHistorys = new HashSet<TicketHistory>();
            this.TicketNotifications = new HashSet<TicketNotification>();
        }

        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }   //? makes it nullable
        public int ProjectId { get; set; }
        public int TicketTypeId { get; set; }
        public int TicketPriorityId { get; set; }
        public int TicketStatusId { get; set; }
        public string OwnerUserId { get; set; }
        public string AssignedToUserId { get; set; }

        public float EstHours { get; set; }            // scope creep
        public DateTimeOffset? DueDate { get; set; }   // scope creep

        public virtual TicketStatus TicketStatus { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual TicketType TicketType { get; set; }
        public virtual Project Project { get; set; }

        public virtual ApplicationUser OwnerUser { get; set; }
        public virtual ApplicationUser AssignedToUser { get; set; }

        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }
        public virtual ICollection<TicketComment> TicketComments { get; set; }
        public virtual ICollection<TicketHistory> TicketHistorys { get; set; }
        public virtual ICollection<TicketNotification> TicketNotifications { get; set; }
    }

    public class TicketStatus
    {
        public int Id { get; set; }
        public string Status { get; set; }
    }

    public class TicketPriority
    {
        public int Id { get; set; }
        public string Priority { get; set; }
    }
    
    public class TicketType
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }

    public class Project
    {
        public Project()                 
        {
            this.ApplicationUsers = new HashSet<ApplicationUser>();
            this.Tickets = new HashSet<Ticket>();
        }
        public int Id { get; set; }
        public string ProjectName { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }

    }

    public class TicketAttachment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public int UserId { get; set; }
        public string FileUrl { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

    public class TicketComment
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTimeOffset Created { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }

        public bool Hide { get; set; }                  // scope creep
        public float Hours { get; set; }                // scope creep
        public float PercentComplete { get; set; }      // scope creep

        public virtual ApplicationUser User { get; set; }
        public virtual Ticket Ticket { get; set; }
    }

    public class TicketHistory
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Property { get; set; }
        public string oldValue { set; get; }
        public string newValue { set; get; }
        public DateTimeOffset Changed { get; set; }
        public int UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
    public class TicketNotification
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}