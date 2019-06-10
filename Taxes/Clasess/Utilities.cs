using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Taxes.Models;

namespace Taxes.Clasess
{
    public class Utilities : IDisposable
    {
        private static ApplicationDbContext userContext = new ApplicationDbContext();

        public static void CheckRole(string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));

            // Check to see if role exist, if not create it
            if (!roleManager.RoleExists(roleName))
            {
                var result = roleManager.Create(new IdentityRole(roleName));
            }

        }

        public static void CheckSuperUser()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail("carmelo5mayen@gmail.com");

            if (userASP == null)
            {
                CreateUserASP("carmelo5mayen@gmail.com", "Admin");
                return;
            }

            userManager.AddToRole(userASP.Id, "Admin");

        }

        public static void CreateUserASP(String email, String roleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            var userASP = new ApplicationUser
            {
                Email = email,
                UserName = email
            };

            userManager.Create(userASP, email);
            userManager.AddToRole(userASP.Id, roleName);
        }

        public void Dispose()
        {
            userContext.Dispose();
        }
    }
}