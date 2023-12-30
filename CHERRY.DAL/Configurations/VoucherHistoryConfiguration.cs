using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CHERRY.DAL.Configurations
{
    public partial class VoucherHistoryConfiguration : IEntityTypeConfiguration<VoucherHistory>
    {
        public void Configure(EntityTypeBuilder<VoucherHistory> builder)
        {
            builder.HasKey(c => new { c.IDOrder, c.IDVoucher });

            //BASE
            builder.Property(c => c.CreateDate).IsRequired();
            builder.Property(c => c.CreateBy).IsRequired();
            builder.Property(c => c.ModifieBy).IsRequired(false);
            builder.Property(c => c.ModifieDate).IsRequired(false);
            builder.Property(c => c.DeleteBy).IsRequired(false);
            builder.Property(c => c.DeleteDate).IsRequired(false);
            builder.Property(c => c.Status).IsRequired();

            builder.HasOne<Order>(c => c.Order)
               .WithOne(c => c.VoucherHistory)
               .HasForeignKey<VoucherHistory>(c => c.IDOrder)
               .OnDelete(DeleteBehavior.Restrict);

              builder.HasOne<Voucher>(c => c.Voucher)
               .WithMany(c => c.VoucherHistory)
               .HasForeignKey(c => c.IDVoucher)
               .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
