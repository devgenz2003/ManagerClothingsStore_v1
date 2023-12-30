using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CHERRY.DAL.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
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

            builder.HasOne<User>(c => c.User)
                .WithMany(c => c.Reviews)
                .HasForeignKey(c => c.IDUser)
                .OnDelete(DeleteBehavior.NoAction); 
            
            builder.HasOne<Variants>(c => c.Variants)
                .WithMany(c => c.Reviews)
                .HasForeignKey(c => c.IDVariant)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.HasOne<OrderVariant>(c => c.OrderVariant)
                .WithMany(c => c.Review)
                .HasForeignKey(c => c.IDOrderVariant)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
