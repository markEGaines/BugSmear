using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugSmear.Models
{
    public class ProjectUsersViewModel
    {
        public int projectId { get; set; }
        public string projectName { get; set; }
        [Display(Name = "Available Users")]
        public System.Web.Mvc.MultiSelectList Users { get; set; }       // populates listbox
        public string[] SelectedUsers { get; set; }                     // receives selections

    }
}