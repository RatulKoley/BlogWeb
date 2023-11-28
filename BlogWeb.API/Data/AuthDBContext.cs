using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogWeb.API.Data
{
    public class AuthDBContext : IdentityDbContext
    {
        public AuthDBContext(DbContextOptions<AuthDBContext> opt) : base(opt) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //Seed The Roles
            var AdminRoleId = "bda86f04-6987-41d7-8a1b-de46e060691f";
            var SuperRoleId = "b02638d5-ae5a-40b3-8600-8e3efeb32fa5";
            var UserRoleId = "4bbff1d1-fe85-491f-8ac2-fcbae25aac83";
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = AdminRoleId,
                    ConcurrencyStamp = AdminRoleId
                },
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = SuperRoleId,
                    ConcurrencyStamp = SuperRoleId
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = UserRoleId,
                    ConcurrencyStamp = UserRoleId
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);

            //Seed SuperAdmin User
            var SuperUserId = "0583440f-d199-4f41-be75-c4965c19fbed";
            var superUser = new IdentityUser
            {
                UserName = "SuperAdmin",
                Email = "SuperAdmin@gmail.com",
                NormalizedEmail = "SuperAdmin".ToUpper(),
                NormalizedUserName = "SuperAdmin@gmail.com".ToUpper(),
                Id = SuperUserId
            };
            superUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superUser, "Admin@123");
            builder.Entity<IdentityUser>().HasData(superUser);

            //Add All Roles To SuperAdmin

            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = AdminRoleId,
                    UserId = SuperUserId
                },
                new IdentityUserRole<string>
                {
                    RoleId = SuperRoleId,
                    UserId = SuperUserId
                },
                new IdentityUserRole<string>
                {
                    RoleId = UserRoleId,
                    UserId = SuperUserId
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
