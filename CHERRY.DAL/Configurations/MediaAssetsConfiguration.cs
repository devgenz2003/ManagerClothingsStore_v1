using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CHERRY.DAL.Configurations
{
    public partial class MediaAssetsConfiguration : IEntityTypeConfiguration<MediaAssets>
    {
        public void Configure(EntityTypeBuilder<MediaAssets> builder)
        {
            //BASE
            builder.HasKey(c => c.ID);
            builder.Property(c => c.CreateDate).IsRequired();
            builder.Property(c => c.CreateBy).IsRequired();
            builder.Property(c => c.ModifieBy).IsRequired(false);
            builder.Property(c => c.ModifieDate).IsRequired(false);
            builder.Property(c => c.DeleteBy).IsRequired(false);
            builder.Property(c => c.DeleteDate).IsRequired(false);
            builder.Property(c => c.Status).IsRequired();

            builder.HasOne<Variants>(c => c.Variants)
                .WithMany(c => c.MediaAssets)
                .HasForeignKey(c => c.IDVariant)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Review>(c => c.Review)
               .WithMany(c => c.MediaAssets)
               .HasForeignKey(c => c.IDReview)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
