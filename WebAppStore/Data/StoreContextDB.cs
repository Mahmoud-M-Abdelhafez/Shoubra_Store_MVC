using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAppStore.Models;

namespace WebAppStore.Data
{
    public class StoreContextDB : IdentityDbContext<AppUser>
    {
        public StoreContextDB(DbContextOptions<StoreContextDB> options) : base(options)
        {

        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }

        public virtual DbSet<Cart> Carts { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer("Server=db16041.public.databaseasp.net; Database=db16041; User Id=db16041; Password=6As+%w7Pd5Z!; Encrypt=False; MultipleActiveResultSets=True;");

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }

    }
}
