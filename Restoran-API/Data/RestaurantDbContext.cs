
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using Restaurant_Reservation_Management_System_Api.Model;
namespace Restaurant_Reservation_Management_System_Api.Data
{
    public class RestaurantDbContext : IdentityDbContext<ApplicationUser>
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {

        }


        public DbSet <Admin> Admins { get; set; }  
        
        public DbSet <Customer> Customers { get; set; } 

        public DbSet<Reservation> Reservations { get; set;}

        public DbSet<Table> Tables { get; set; }    

        public DbSet<MenuCategory> MenuCategories { get; set; } 

        public DbSet<FoodItem> FoodItems { get; set; }  

        public DbSet<Order>  Orders { get; set; }   

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<RegisteredCustomer> RegisteredCustomers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminRoleId = "1";

            var customerRoleId = "2";

            //create new Admin and Customer Role
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = adminRoleId ,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper(),
                    ConcurrencyStamp = adminRoleId ,

                },
                new IdentityRole()
                {
                    Id = customerRoleId ,
                    Name = "Customer",
                    NormalizedName = "Customer".ToUpper(),
                    ConcurrencyStamp= customerRoleId ,
                }
            };

            //Seed the roles

            builder.Entity<IdentityRole>().HasData(roles);

            //Create an Admin User


            var adminUserId = "1";

            var admin = new ApplicationUser()
            {
                Id = adminUserId,
                UserName = "vignesh",
                Email = "vignesh123@gmail.com",
                NormalizedEmail = "vignesh123@gmail.com".ToUpper(),
                NormalizedUserName = "vignesh".ToUpper(),
                Name = "vignesh",

            };

            admin.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(admin, "Admin@123");


            builder.Entity<ApplicationUser>().HasData(admin);


            //Give Roles To Admin

            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId ,
                    RoleId = adminRoleId ,
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = customerRoleId ,
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }


    }
    
}
