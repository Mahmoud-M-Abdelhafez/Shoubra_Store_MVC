using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebAppStore.Data;
using WebAppStore.DataSeeder;
using WebAppStore.Interfaces;
using WebAppStore.Models;
using WebAppStore.Repository;

namespace WebAppStore
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //Server=(localdb)\\MSSQLLocalDB;  Database=Store_DB;  Trusted_Connection=True; Encrypt=False;
            var connectionString = builder.Configuration.GetConnectionString("default");

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<StoreContextDB>
                (
                options => options.UseSqlServer(connectionString)
                );
            builder.Services.AddIdentity<AppUser, IdentityRole>(
                options =>
                {
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                }).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreContextDB>().AddDefaultTokenProviders();

            //Custom Service --REgister
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=home}/{action=start}/{ id?}");

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<StoreContextDB>();
                    context.Database.Migrate(); // This applies any pending migrations

                    // Call the Seeder
                    await Seeder.SeedRolesAndUsers(services);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error applying migrations: {ex.Message}");
                }
               
            }
            app.Run();
        }
    }
}
