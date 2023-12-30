
using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CHERRY.DAL.Configurations
{
    public partial class CartProductVariantsConfiguration : IEntityTypeConfiguration<CartProductVariants>
    {
        public void Configure(EntityTypeBuilder<CartProductVariants> builder)
        {
            builder.HasKey(c => new { c.IDCart, c.IDOptions });

            //BASE
            builder.Property(c => c.CreateDate).IsRequired();
            builder.Property(c => c.CreateBy).IsRequired();
            builder.Property(c => c.ModifieBy).IsRequired(false);
            builder.Property(c => c.ModifieDate).IsRequired(false);
            builder.Property(c => c.DeleteBy).IsRequired(false);
            builder.Property(c => c.DeleteDate).IsRequired(false);
            builder.Property(c => c.Status).IsRequired();

            builder.HasOne<Options>(c => c.Options)
               .WithMany(c => c.CartProductVariants)
               .HasForeignKey(c => c.IDOptions)
               .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne<Cart>(c => c.Cart)
                .WithMany(c => c.CartProductVariants)
                .HasForeignKey(c => c.IDCart)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
