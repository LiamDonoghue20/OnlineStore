
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            //check if there aren't any users currently existing
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Liam",
                    Email = "Liam@test.com",
                    UserName = "LiamDono20",
                    Address = new Address
                    {
                        FirstName = "Liam",
                        LastName = "Donoghue",
                        Street = "Cornhill Street",
                        City = "Liverpool",
                        State = "Merseyside",
                        ZipCode = "L376BZ"
                    }
                };
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}