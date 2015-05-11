using BugSmear.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugSmear.Helpers
{
    public class UserProjectsHelper
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public bool isOnProject(string userId, int projectId)
        {
            // if (db.Projects.Find(projectId).ApplicationUsers.Contains(db.Users.Find(userId)))

            return db.Projects.Find(projectId).ApplicationUsers.Any(u=>u.Id == userId); // alternate usage with only 1 db hit

            //var project = db.Projects.Find(projectId);
            //var user = db.Users.Find(userId);                    //any is more efficient but need lamda
            //var userlist = project.ApplicationUsers;   // or project.Users;
            //if (userlist.Contains(user))
            //{
            //    return true;
            //}
            //return false;
        }

        public void AddUserToProject(string userId, int projectId)
        {
            if (!isOnProject(userId, projectId))
            {
                var project = db.Projects.Find(projectId);                        // get project from database
                project.ApplicationUsers.Add(db.Users.Find(userId));                    // add user to the project I just got
                db.Entry(project).State = System.Data.Entity.EntityState.Modified;      // change state of this project so that only it gets saved
                db.SaveChanges();                                                       // db will save just the project

            }

        }
        public void RemoveUserFromProject(string userId, int projectId)
        {
            if (isOnProject(userId, projectId))
            {
                var project = db.Projects.Find(projectId);                        // get project from database
                project.ApplicationUsers.Remove(db.Users.Find(userId));                    // rem user from the project I just got
                db.Entry(project).State = System.Data.Entity.EntityState.Modified;      // change state of this project so that only it gets saved
                db.SaveChanges();                                                       // db will save just the project

            }

        }
        public ICollection<ApplicationUser> ListUsersOnProject(int projectId)
        {
            return db.Projects.Find(projectId).ApplicationUsers;
        }

        public ICollection<Project> ListProjectsForUser(string userId)
        {
            return db.Users.Find(userId).Projects;
        }

        public ICollection<ApplicationUser> ListUsersNotOnProject(int projectId)
        {
            return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
        }



    }


    public class UserHelpers
    {
        private UserManager<ApplicationUser> manager =
            new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(
                    new ApplicationDbContext()));

        public bool IsUserInRole(string userId, string roleName)
        {
            return manager.IsInRole(userId, roleName);
        }

        public IList<string> ListUserRoles(string userId)
        {
            return manager.GetRoles(userId);
        }

        public bool AddUserToRole(string userId, string roleName)
        {
            var result = manager.AddToRole(userId, roleName);
            return result.Succeeded;
        }

        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = manager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }

        //public IList<ApplicationUser> UsersInRole(string roleName)
        //{
        //    var db = new ApplicationDbContext();
        //    var resultList = new List<ApplicationUser>();
        //    foreach (var user in db.Users)
        //    {
        //        if (IsUserInRole(user.Id, roleName))
        //        {
        //            resultList.Add(user);
        //        }
        //    }
        //    return resultList;
        //}

        //public IList<ApplicationUser> UsersNotInRole(string roleName)
        //{
        //    var db = new ApplicationDbContext();
        //    var resultList = new List<ApplicationUser>();
        //    foreach (var user in manager.Users)
        //    {
        //        if (!IsUserInRole(user.Id, roleName))
        //        {
        //            resultList.Add(user);
        //        }
        //    }
        //    return resultList;
        //}

        public IList<ApplicationUser> UsersInRole(string roleName)
        {
            var db = new ApplicationDbContext();
            var resultList = new List<ApplicationUser>();

            foreach (var user in db.Users)
            {
                if (IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }
    }
}