namespace BugSmear.Migrations
{
    using BugSmear.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugSmear.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugSmear.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            
            if(!context.Roles.Any(r=>r.Name == "Administrator"))
            {
                roleManager.Create(new IdentityRole { Name = "Administrator"});
            }

            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }
            
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }
            
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if(!context.Users.Any(r=>r.Email == "markegaines@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                { 
                    UserName = "markegaines@gmail.com",
                    Email="markegaines@gmail.com",
                }, "Plugh4!");               
            }

            var userId = userManager.FindByEmail("markegaines@gmail.com").Id;
            userManager.AddToRole(userId, "Administrator");
// add Administrators
            if (!context.Users.Any(r => r.Email == "lreaves@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "lreaves@coderfoundry.com",
                    Email = "lreaves@coderfoundry.com",
                }, "Password-1");
            }

                userId = userManager.FindByEmail("lreaves@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Administrator");
//------------------------
            if (!context.Users.Any(r => r.Email == "bdavis@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "bdavis@coderfoundry.com",
                    Email = "bdavis@coderfoundry.com",
                }, "Password-1");
            }

            userId = userManager.FindByEmail("bdavis@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Administrator");
//------------------------
            if (!context.Users.Any(r => r.Email == "ajensen@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ajensen@coderfoundry.com",
                    Email = "ajensen@coderfoundry.com",
                }, "Password-1");
            }

            userId = userManager.FindByEmail("ajensen@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Administrator");
//------------------------
            if (!context.Users.Any(r => r.Email == "tjones@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "tjones@coderfoundry.com",
                    Email = "tjones@coderfoundry.com",
                }, "Password-1");
            }

            userId = userManager.FindByEmail("tjones@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Administrator");
//------------------------
            if (!context.Users.Any(r => r.Email == "tparrish@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "tparrish@coderfoundry.com",
                    Email = "tparrish@coderfoundry.com",
                }, "Password-1");
            }

            userId = userManager.FindByEmail("tparrish@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Administrator");
        }
    }
}
