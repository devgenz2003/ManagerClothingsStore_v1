using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CHERRY.DAL.Configurations
{
    public class PromotionVariantConfiguration : IEntityTypeConfiguration<PromotionVariant>
    {
        public void Configure(EntityTypeBuilder<PromotionVariant> builder)
        {
            builder.HasKey(c => new { c.IDPromotion, c.IDVariant });

            //BASE
            builder.Property(c => c.CreateDate).IsRequired();
            builder.Property(c => c.CreateBy).IsRequired();
            builder.Property(c => c.ModifieBy).IsRequired(false);
            builder.Property(c => c.ModifieDate).IsRequired(false);
            builder.Property(c => c.DeleteBy).IsRequired(false);
            builder.Property(c => c.DeleteDate).IsRequired(false);
            builder.Property(c => c.Status).IsRequired();

            builder.HasOne<Variants>(c => c.Variants)
               .WithMany(c => c.PromotionVariants)
               .HasForeignKey(c => c.IDVariant)
               .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne<Promotion>(c => c.Promotion)
                .WithMany(c => c.PromotionVariants)
                .HasForeignKey(c => c.IDPromotion)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
