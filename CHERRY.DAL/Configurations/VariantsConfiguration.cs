using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CHERRY.DAL.Configurations
{
    public partial class VariantsConfiguration : IEntityTypeConfiguration<Variants>
    {
        public void Configure(EntityTypeBuilder<Variants> builder)
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
          
            builder.HasOne<Brand>(c => c.Brand)
                .WithMany(c => c.Variants)
                .HasForeignKey(c => c.IDBrand)
                .OnDelete(DeleteBehavior.Cascade);  
            
            builder.HasOne<Material>(c => c.Material)
                .WithMany(c => c.Variants)
                .HasForeignKey(c => c.IDMaterial)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
