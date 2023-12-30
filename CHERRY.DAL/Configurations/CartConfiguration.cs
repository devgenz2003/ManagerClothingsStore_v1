using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CHERRY.DAL.Configurations
{
    public partial class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
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
                .WithOne(c => c.Cart)
                .HasForeignKey<Cart>(c => c.ID_User)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
