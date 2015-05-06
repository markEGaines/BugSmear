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

            if (!context.Roles.Any(r => r.Name == "Administrator"))
                roleManager.Create(new IdentityRole { Name = "Administrator" });

            if (!context.Roles.Any(r => r.Name == "Project Manager"))
                roleManager.Create(new IdentityRole { Name = "Project Manager" });

            if (!context.Roles.Any(r => r.Name == "Developer"))
                roleManager.Create(new IdentityRole { Name = "Developer" });

            if (!context.Roles.Any(r => r.Name == "Submitter"))
                roleManager.Create(new IdentityRole { Name = "Submitter" });


            if (!context.TicketStatus.Any(ts => ts.Status == "Open"))
            {
                var status = new TicketStatus
                {
                    Status = "Open"
                };
                context.TicketStatus.Add(status);
                context.SaveChanges();
            }

            if (!context.TicketStatus.Any(ts => ts.Status == "Resolved"))
            {
                var status = new TicketStatus
                {
                    Status = "Resolved"
                };
                context.TicketStatus.Add(status);
                context.SaveChanges();
            }

            if (!context.TicketPriorities.Any(tp => tp.Priority == "Critical"))
            {
                var status = new TicketPriority
                {
                    Priority = "Critical"
                };
                context.TicketPriorities.Add(status);
                context.SaveChanges();
            }

            if (!context.TicketPriorities.Any(tp => tp.Priority == "Moderate"))
            {
                var status = new TicketPriority
                {
                    Priority = "Moderate"
                };
                context.TicketPriorities.Add(status);
                context.SaveChanges();
            }

            if (!context.TicketPriorities.Any(tp => tp.Priority == "Low"))
            {
                var status = new TicketPriority
                {
                    Priority = "Low"
                };
                context.TicketPriorities.Add(status);
                context.SaveChanges();
            }

            if (!context.TicketTypes.Any(tt => tt.Type == "Defect - User Interface"))
            {
                var status = new TicketType
                {
                    Type = "Defect - User Interface"
                };
                context.TicketTypes.Add(status);
                context.SaveChanges();
            }

            if (!context.TicketTypes.Any(tt => tt.Type == "Defect - Business Logic"))
            {
                var status = new TicketType
                {
                    Type = "Defect - Business Logic"
                };
                context.TicketTypes.Add(status);
                context.SaveChanges();
            }

            if (!context.TicketTypes.Any(tt => tt.Type == "Enhancement"))
            {
                var status = new TicketType
                {
                    Type = "Enhancement"
                };
                context.TicketTypes.Add(status);
                context.SaveChanges();
            }

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(r => r.Email == "markegaines@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "markegaines@gmail.com",
                    Email = "markegaines@gmail.com",
                    EmailConfirmed = true,
                }, "Plugh4!");
            }

            var userId = userManager.FindByEmail("markegaines@gmail.com").Id;
            userManager.AddToRole(userId, "Administrator");
            userManager.AddToRole(userId, "Developer");
            userManager.AddToRole(userId, "Project Manager");
            userManager.AddToRole(userId, "Submitter");

            // add Administrators
            if (!context.Users.Any(r => r.Email == "donaldfgaines@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "donaldfgaines@gmail.com",
                    Email = "donaldfgaines@gmail.com",
                    EmailConfirmed = true,
                }, "Password-1");
            }

            userId = userManager.FindByEmail("donaldfgaines@gmail.com").Id;
            userManager.AddToRole(userId, "Administrator");
            //------------------------
            if (!context.Users.Any(r => r.Email == "Guest@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Guest@coderfoundry.com",
                    Email = "Guest@coderfoundry.com",
                    EmailConfirmed = true,
                }, "Password-1");
            }

            userId = userManager.FindByEmail("Guest@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Administrator");
            //------------------------
            if (!context.Users.Any(r => r.Email == "lreaves@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "lreaves@coderfoundry.com",
                    Email = "lreaves@coderfoundry.com",
                    EmailConfirmed = true,
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
                    EmailConfirmed = true,
                }, "Password-1");
            }

            userId = userManager.FindByEmail("bdavis@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Administrator");
            userManager.AddToRole(userId, "Project Manager");
            //------------------------
            if (!context.Users.Any(r => r.Email == "ajensen@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ajensen@coderfoundry.com",
                    Email = "ajensen@coderfoundry.com",
                    EmailConfirmed = true,
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
                    EmailConfirmed = true,
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
                    EmailConfirmed = true,
                }, "Password-1");
            }

            userId = userManager.FindByEmail("tparrish@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Administrator");




            // add test data

            if (!context.Users.Any(r => r.Email == "SamSubmitter@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "SamSubmitter@coderfoundry.com",
                    Email = "SamSubmitter@coderfoundry.com",
                    EmailConfirmed = true,
                }, "Password-1");
            }

            userId = userManager.FindByEmail("SamSubmitter@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Submitter");

            //------------------------

            if (!context.Users.Any(r => r.Email == "PamProjectManager@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "PamProjectManager@coderfoundry.com",
                    Email = "PamProjectManager@coderfoundry.com",
                    EmailConfirmed = true,
                }, "Password-1");
            }

            userId = userManager.FindByEmail("PamProjectManager@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Project Manager");

            //------------------------

            if (!context.Users.Any(r => r.Email == "DorothyDeveloper@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DorothyDeveloper@coderfoundry.com",
                    Email = "DorothyDeveloper@coderfoundry.com",
                    EmailConfirmed = true,
                }, "Password-1");
            }

            userId = userManager.FindByEmail("DorothyDeveloper@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Developer");

            //------------------------

            if (!context.Users.Any(r => r.Email == "ArtieAdministrator@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ArtieAdministrator@coderfoundry.com",
                    Email = "ArtieAdministrator@coderfoundry.com",
                    EmailConfirmed = true,
                }, "Password-1");
            }

            userId = userManager.FindByEmail("ArtieAdministrator@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Administrator");

        }
    }
}
