namespace Sales.API.Helpers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Configuration;
    using Sales.Common.Models;
    using Sales.Domain.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    public class UsersHelper : IDisposable
    {
        private static ApplicationDbContext userContext = new ApplicationDbContext();
        private static DataContext db = new DataContext();

        public static bool DeleteUser(string userName,string roleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(userName);

            if (userASP == null)
            {
                return false;
            }

            var response = userManager.RemoveFromRole(userASP.Id, roleName);
            return response.Succeeded;
        }

        public static ApplicationUser GetUserASP(string email)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);

            return userASP;
        }

        public static Response CreateUserASP(UserRequest userRequest)
        {
            try
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
                var oldUserASP = userManager.FindByEmail(userRequest.EMail);
                if (oldUserASP != null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "001. User already exists."
                    };
                }
                var userASP = new ApplicationUser
                {
                    Email = userRequest.EMail,
                    UserName = userRequest.EMail,
                    PhoneNumber = userRequest.Phone,
                };

                var result = userManager.Create(userASP, userRequest.Password);
                if (result.Succeeded)
                {
                    var newUserASP = userManager.FindByEmail(userRequest.EMail);
                    userManager.AddClaim(newUserASP.Id, new System.Security.Claims.Claim(ClaimTypes.GivenName, userRequest.FirstName));
                    userManager.AddClaim(newUserASP.Id, new System.Security.Claims.Claim(ClaimTypes.Name, userRequest.LastName));

                    if (!string.IsNullOrEmpty(userRequest.Address))
                    {
                        userManager.AddClaim(newUserASP.Id, new System.Security.Claims.Claim(ClaimTypes.StreetAddress, userRequest.Address));
                    }

                    if (!string.IsNullOrEmpty(userRequest.ImagePath))
                    {
                        userManager.AddClaim(newUserASP.Id, new System.Security.Claims.Claim(ClaimTypes.Uri, userRequest.ImagePath));
                    }

                    return new Response
                    {
                        IsSuccess = true
                    };

                }

                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error}, ";
                }

                return new Response
                {
                    IsSuccess = false,
                    Message = errors,
                };

            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
           
        }

        //Pendiente UpdateUserName

        public void Dispose()
        {
            userContext.Dispose();
            db.Dispose();
        }
    }
}