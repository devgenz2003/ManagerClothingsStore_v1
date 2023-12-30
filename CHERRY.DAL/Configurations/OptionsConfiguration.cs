using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CHERRY.DAL.Configurations
{
    public partial class OptionsConfiguration : IEntityTypeConfiguration<Options>
    {
        public void Configure(EntityTypeBuilder<Options> builder)
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

            builder.HasOne<Colors>(c => c.Color)
               .WithMany(c => c.Options)
               .HasForeignKey(c => c.IDColor)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Sizes>(c => c.Sizes)
                .WithMany(c => c.Options)
                .HasForeignKey(c => c.IDSizes)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Variants>(c => c.Variants)
                .WithMany(c => c.Options)
                .HasForeignKey(c => c.IDVariant)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
