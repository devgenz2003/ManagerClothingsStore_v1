using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CHERRY.DAL.Configurations
{
    public partial class CategoriesVariantsConfiguration : IEntityTypeConfiguration<CategoriesVariants>
    {
        public void Configure(EntityTypeBuilder<CategoriesVariants> builder)
        {
            builder.HasKey(c => new { c.IDVariants, c.IDCategories });

            //BASE
            builder.Property(c => c.CreateDate).IsRequired();
            builder.Property(c => c.CreateBy).IsRequired();
            builder.Property(c => c.ModifieBy).IsRequired(false);
            builder.Property(c => c.ModifieDate).IsRequired(false);
            builder.Property(c => c.DeleteBy).IsRequired(false);
            builder.Property(c => c.DeleteDate).IsRequired(false);
            builder.Property(c => c.Status).IsRequired();

            builder.HasOne<Variants>(c => c.Variants)
               .WithMany(c => c.CategoriesVariants)
               .HasForeignKey(c => c.IDVariants)
               .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne<Categories>(c => c.Categories)
                .WithMany(c => c.CategoriesVariants)
                .HasForeignKey(c => c.IDCategories)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
