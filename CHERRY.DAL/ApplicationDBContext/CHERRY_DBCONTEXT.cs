using Microsoft.EntityFrameworkCore;
using CHERRY.DAL.Entities;
using CHERRY.DAL.Configurations;

namespace CHERRY.DAL.ApplicationDBContext
{
    public partial class CHERRY_DBCONTEXT : DbContext
    {
        public CHERRY_DBCONTEXT()
        {
        }

        public CHERRY_DBCONTEXT(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(
                        "Data Source=.;Initial Catalog=CHERRY.v2023.POL_23.ED_24.v1.1;Integrated Security=True"
                        );
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new CartProductVariantsConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriesConfiguration());
            modelBuilder.ApplyConfiguration(new ColorsConfiguration());
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
            modelBuilder.ApplyConfiguration(new CategoriesVariantsConfiguration());
            modelBuilder.ApplyConfiguration(new ReviewConfiguration());

            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<VoucherHistory> VoucherHistory { get; set; } = null!;
        public virtual DbSet<VoucherUser> VoucherUser { get; set; } = null!;
        public virtual DbSet<Order> Order { get; set; } = null!;
        public virtual DbSet<OrderVariant> OrderVariant { get; set; } = null!;
        public virtual DbSet<Cart> Cart { get; set; } = null!;
        public virtual DbSet<CartProductVariants> CartProductVariants { get; set; } = null!;
        public virtual DbSet<Categories> Categories { get; set; } = null!;
        public virtual DbSet<Colors> Colors { get; set; } = null!; 
        public virtual DbSet<Review> Review { get; set; } = null!; 
        public virtual DbSet<MediaAssets> MediaAssets { get; set; } = null!;
        public virtual DbSet<MemberRank> MemberRank { get; set; } = null!;
        public virtual DbSet<Options>? Options { get; set; } 
        public virtual DbSet<Brand> Brand { get; set; } = null!;
        public virtual DbSet<Material> Material { get; set; } = null!;
        public virtual DbSet<Promotion> Promotion { get; set; } = null!;
        public virtual DbSet<Sizes> Sizes { get; set; } = null!;
        public virtual DbSet<Variants> Variants { get; set; } = null!;
        public virtual DbSet<PromotionVariant> PromotionVariant { get; set; } = null!;
        public virtual DbSet<CategoriesVariants> CategoriesVariants { get; set; } = null!;
        public virtual DbSet<Voucher> Vouchers { get; set; }

    }
}
