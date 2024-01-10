using CHERRY.DAL.Configurations;
using CHERRY.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CHERRY.DAL.ApplicationDBContext
{
    public class CHERRY_IdentityDbContext : IdentityDbContext<IdentityUser>
    {
        public CHERRY_IdentityDbContext()
        {

        }
        public CHERRY_IdentityDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Gán Index vào cột Email của bảng AspNetUsers
            modelBuilder.Entity<IdentityUser>()
                .ToTable("AspNetUsers")
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.ApplyConfiguration(new DiscountHistoryConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new CartProductVariantsConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriesConfiguration());
            modelBuilder.ApplyConfiguration(new ColorsConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriesVariantsConfiguration());
            modelBuilder.ApplyConfiguration(new MediaAssetsConfiguration());
            modelBuilder.ApplyConfiguration(new MemberRankConfiguration());
            modelBuilder.ApplyConfiguration(new OptionsConfiguration());
            modelBuilder.ApplyConfiguration(new PromotionConfiguration());
            modelBuilder.ApplyConfiguration(new SizesConfiguration());
            modelBuilder.ApplyConfiguration(new VariantsConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderVariantConfiguration()); 
            modelBuilder.ApplyConfiguration(new VoucherUserConfiguration());
            modelBuilder.ApplyConfiguration(new PromotionVariantConfiguration());
            modelBuilder.ApplyConfiguration(new VoucherHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new ReviewConfiguration());


            base.OnModelCreating(modelBuilder);
            CreateRoles(modelBuilder);
        }
        private void CreateRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                    new IdentityRole() { Name = "Admin", NormalizedName = "Admin" },
                    new IdentityRole() { Name = "Client", NormalizedName = "Client" }
                );
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(
                        "Data Source=.;Initial Catalog=CHERRY.v2023;Integrated Security=True"
                        );
        }
        public virtual DbSet<DiscountHistory> DiscountHistory { get; set; } = null!;
        public virtual DbSet<VoucherUser> VoucherUser { get; set; } = null!;
        public virtual DbSet<GhtkOrderRequest> GhtkOrderRequest { get; set; } = null!;
        public virtual DbSet<Brand> Brand { get; set; } = null!;
        public virtual DbSet<Review> Review { get; set; } = null!;
        public virtual DbSet<Material> Material { get; set; } = null!;
        public virtual DbSet<CategoriesVariants> CategoriesVariants { get; set; } = null!;
        public virtual DbSet<VoucherHistory> VoucherHistory { get; set; } = null!;
        public virtual DbSet<Order> Order { get; set; } = null!;
        public virtual DbSet<OrderVariant> OrderVariant { get; set; } = null!;
        public virtual DbSet<Cart> Cart { get; set; } = null!;
        public virtual DbSet<CartProductVariants> CartProductVariants { get; set; } = null!;
        public virtual DbSet<Categories> Categories { get; set; } = null!;
        public virtual DbSet<Colors> Colors { get; set; } = null!;
        public virtual DbSet<MediaAssets> MediaAssets { get; set; } = null!;
        public virtual DbSet<MemberRank> MemberRank { get; set; } = null!;
        public virtual DbSet<Options>? Options { get; set; }
        public virtual DbSet<Promotion> Promotion { get; set; } = null!;
        public virtual DbSet<Sizes> Sizes { get; set; } = null!;
        public virtual DbSet<Variants> Variants { get; set; } = null!;
        public virtual DbSet<PromotionVariant> PromotionVariant { get; set; } = null!;
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }

    }
}
