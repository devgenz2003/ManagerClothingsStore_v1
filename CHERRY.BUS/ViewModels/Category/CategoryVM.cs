
namespace CHERRY.BUS.ViewModels.Category
{
    public class CategoryVM
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int Status { get; set; }
    }
}
