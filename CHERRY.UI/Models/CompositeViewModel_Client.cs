using CHERRY.BUS.ViewModels.CartProductVariants;
using CHERRY.BUS.ViewModels.CategoriesVariants;
using CHERRY.BUS.ViewModels.Category;
using CHERRY.BUS.ViewModels.Options;
using CHERRY.BUS.ViewModels.Order;
using CHERRY.BUS.ViewModels.OrderVariant;
using CHERRY.BUS.ViewModels.Review;
using CHERRY.BUS.ViewModels.Variants;
using CHERRY.DAL.Entities;

namespace CHERRY.UI.Models
{
    public class CompositeViewModel_Client
    {

        public List<CategoryVM>? CategoryVM { get; set; }
        public List<CartProductVariantsVM> CartProductVariantsVM { get; set; }
        public List<VariantsVM> LstVariantsVM { get; set; }
        public List<CategoriesVariantsVM> LstCategoriesVariantsVM { get; set; }
        public List<CategoryVM> LstCategoryVM { get; set; }
        public List<OptionsVM> LstOptionsVM { get; set; }
        public List<ReviewVM> LstReviewVM { get; set; }
        public List<OrderVM> LstOrderVM { get; set; }
        public VariantsVM VariantsVM { get; set; }
        public OptionsVM OptionsVM { get; set; }
        public OrderCreateVM OrderCreateVM { get; set; }
        public OrderVM orderVM { get; set; }    
        public List<OrderVariantVM> orderVariantVMs { get; set; }
        public string OrderDetailsJson { get; set; }
    }
}
