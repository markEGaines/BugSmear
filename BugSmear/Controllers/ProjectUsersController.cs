using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugSmear.Models;
using BugSmear.Helpers;

namespace BugSmear.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProjectUsersController : Controller
    {
        private UserProjectsHelper helper = new UserProjectsHelper();
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult AssignRemoveUsers(int Id)
        {

            var model = new ProjectUsersViewModel();
            var project = db.Projects.Find(Id);
            model.projectId = project.Id;
            model.projectName = project.ProjectName;
            var selUsers = helper.ListUsersOnProject(Id).OrderBy(u => u.UserName).Select(n => n.Id);
            model.Users = new MultiSelectList(db.Users.OrderBy(u => u.UserName), "Id", "UserName", selUsers);

            return View(model);
        }

        // POST: AssignRemoveUsers
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult AssignRemoveUsers(ProjectUsersViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userlist = db.Users;
                foreach (var usr in userlist)
                {
                    if (model.SelectedUsers != null && model.SelectedUsers.Contains(usr.Id))
                    {
                        helper.AddUserToProject(usr.Id, model.projectId);
                    }
                    else
                    {
                        helper.RemoveUserFromProject(usr.Id, model.projectId);
                    }
                }
                return RedirectToAction("Index", "Projects", new { id = model.projectId });
                //return View(model);
            }
            return View("Error");
        }

        // GET: AssignUsers
        public ActionResult AssignUsers(int Id)
        {
            var model = new ProjectUsersViewModel();
            var project = db.Projects.Find(Id);
            model.projectId = project.Id;
            model.projectName = project.ProjectName;
            model.Users = new MultiSelectList(helper.ListUsersNotOnProject(Id).OrderBy(u => u.UserName), "Id", "UserName");
            return View(model);
        }

        // POST: AssignUsers
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult AssignUsers(ProjectUsersViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedUsers != null)
                {
                    foreach (string id in model.SelectedUsers)
                    {
                        helper.AddUserToProject(id, model.projectId);
                    }
                    return RedirectToAction("Details", "Projects", new { id = model.projectId });
                }
                else
                {
                    return View("Error");
                }
            }
            return View(model);
        }

        // GET: RemoveUsers
        [Authorize(Roles = "Administrator")]
        public ActionResult RemoveUsers(int Id)
        {
            var model = new ProjectUsersViewModel();
            var project = db.Projects.Find(Id);
            model.projectId = project.Id;
            model.projectName = project.ProjectName;
            model.Users = new MultiSelectList(helper.ListUsersOnProject(Id).OrderBy(u => u.UserName), "Id", "UserName");
            return View(model);
        }

        // POST: RemoveUsers
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult RemoveUsers(ProjectUsersViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedUsers != null)
                {
                    foreach (string id in model.SelectedUsers)
                    {
                        helper.RemoveUserFromProject(id, model.projectId);
                    }
                    return RedirectToAction("Details", "Projects", new { id = model.projectId });
                }
                else
                {
                    return View("Error");
                }
            }
            return View(model);
        }
    }
}