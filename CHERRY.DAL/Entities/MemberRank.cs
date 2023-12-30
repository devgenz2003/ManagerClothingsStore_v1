using CHERRY.DAL.Entities.Base;

namespace CHERRY.DAL.Entities
{
    public partial class MemberRank : EntityBase
    {
        public MemberRank() : base() { }

        public string MemberName { get; set; } = null!;
        public string? Description { get; set; }
        public virtual Member_Rank Rank { get; set; }
        public virtual ICollection<User> Users { get; set; } = null!;
    }
    public enum Member_Rank
    {
        Base = 0,
        Silver = 1,
        Gold = 2,
        Platinum = 3,
    }
}
