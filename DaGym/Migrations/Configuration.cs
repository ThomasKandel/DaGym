namespace DaGym.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DaGym.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DaGym.Models.ApplicationDbContext context)
        {
            var AdminRoleName = "admin";
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            if (!context.Roles.Any(r => r.Name == AdminRoleName))
            {
                var role = new IdentityRole { Name = AdminRoleName };
                var result = roleManager.Create(role);
                if (!result.Succeeded)
                {
                    throw new Exception("seeding '" + AdminRoleName + "' to Roles/n" + string.Join("/n", result.Errors));
                }
            }

            var hasher = new PasswordHasher();
            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    UserName = "seededUser",
                    Email = "seededUser@hush.com",
                    FirstName = "Snurre",
                    LastName = "Sprätt",
                    TimeOfRegistration = DateTime.Now,
                    PasswordHash = hasher.HashPassword("pass")
                });

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            ApplicationUser user;

            if (!context.Users.Any(u => u.UserName == "admin@Gymbokning.se"))
            {
                user = new ApplicationUser
                {
                    UserName = "admin@Gymbokning.se",
                    Email = "sickan@Gymbokning.se",
                    FirstName = "Charles Ingvar",
                    LastName = "Jönsson",
                    TimeOfRegistration = DateTime.Now
                };
                var result = userManager.Create(user, "password");
                if (!result.Succeeded)
                {
                    throw new Exception("seeding 'admin@Gymbokning.se'/n" + string.Join("/n", result.Errors));
                }
            }

            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@Gymbokning.se",
                    FirstName = "Ragnar",
                    LastName = "Vanheden",
                    TimeOfRegistration = DateTime.Now
                };
                var result = userManager.Create(user, "123456");
                if (!result.Succeeded)
                {
                    throw new Exception("seeding 'admin'/n" + string.Join("/n", result.Errors));
                }
            }


            user = userManager.FindByName("admin@Gymbokning.se");
            if (!userManager.IsInRole(user.Id, AdminRoleName))
            {
                var result = userManager.AddToRole(user.Id, AdminRoleName);

                if (!result.Succeeded)
                {
                    throw new Exception("seed ading 'admin@Gymbokning.se' to role 'admin'/n" + string.Join("/n", result.Errors));
                }
            }

            user = userManager.FindByName("admin");
            if (!userManager.IsInRole(user.Id, AdminRoleName))
            {
                var result = userManager.AddToRole(user.Id, AdminRoleName);

                if (!result.Succeeded)
                {
                    throw new Exception("seed ading 'admin' to role 'admin'/n" + string.Join("/n", result.Errors));
                }
            }
        }

        //private bool SeedUserToRole(string RoleName, string UserId)
        //{
        //    user = userManager.FindByName("admin@Gymbokning.se");
        //    if (!userManager.IsInRole(user.Id, AdminRoleName))
        //    {
        //        var result = userManager.AddToRole(user.Id, AdminRoleName);

        //        if (!result.Succeeded)
        //        {
        //            throw new Exception("seed ading 'admin@Gymbokning.se' to role 'admin'/n" + string.Join("/n", resultx.Errors));
        //        }
        //    }
        //}
    }
}
