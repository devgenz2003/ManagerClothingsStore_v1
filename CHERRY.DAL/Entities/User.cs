using Microsoft.AspNetCore.Identity;

namespace CHERRY.DAL.Entities
{
    public partial class User : IdentityUser
    {
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public Guid? ID_MemberRank { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? SurName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }
        public int Status { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual ICollection<VoucherUser> VoucherUser { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
