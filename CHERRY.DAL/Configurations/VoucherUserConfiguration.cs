using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CHERRY.DAL.Configurations
{
    public partial class VoucherUserConfiguration : IEntityTypeConfiguration<VoucherUser>
    {
        public void Configure(EntityTypeBuilder<VoucherUser> builder)
        {
            builder.HasKey(c => new { c.IDUser, c.IDVoucher });

            //BASE
            builder.Property(c => c.CreateDate).IsRequired();
            builder.Property(c => c.CreateBy).IsRequired();
            builder.Property(c => c.ModifieBy).IsRequired(false);
            builder.Property(c => c.ModifieDate).IsRequired(false);
            builder.Property(c => c.DeleteBy).IsRequired(false);
            builder.Property(c => c.DeleteDate).IsRequired(false);
            builder.Property(c => c.Status).IsRequired();

            builder.HasOne<Voucher>(c => c.Voucher)
               .WithMany(c => c.VoucherUser)
               .HasForeignKey(c => c.IDVoucher)
               .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne<User>(c => c.Users)
                .WithMany(c => c.VoucherUser)
                .HasForeignKey(c => c.IDUser)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
