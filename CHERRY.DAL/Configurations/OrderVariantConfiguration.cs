
using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CHERRY.DAL.Configurations
{
    public class OrderVariantConfiguration : IEntityTypeConfiguration<OrderVariant>
    {
        public void Configure(EntityTypeBuilder<OrderVariant> builder)
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

            builder.HasOne<Order>(c=>c.Order).WithMany(c=>c.OrderVariant)
                .HasForeignKey(c=>c.IDOrder).OnDelete(DeleteBehavior.NoAction);

            
            builder.HasOne<Options>(c=>c.Options).WithMany(c=>c.OrderVariant)
                .HasForeignKey(c=>c.IDOptions).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
