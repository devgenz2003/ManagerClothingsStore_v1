using CHERRY.BUS.ViewModels.Category;
using CHERRY.BUS.ViewModels.Options;
using CHERRY.BUS.ViewModels.Order;
using CHERRY.BUS.ViewModels.OrderVariant;
using CHERRY.BUS.ViewModels.Promotion;
using CHERRY.BUS.ViewModels.PromotionVariants;
using CHERRY.BUS.ViewModels.User;
using CHERRY.BUS.ViewModels.Variants;
using CHERRY.BUS.ViewModels.Voucher;
using CHERRY.BUS.ViewModels.VoucherUser;
using CHERRY.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace CHERRY.UI.Areas.Admin.Models
{
    public class ModelCompositeShare
    {
        public VariantsVM VariantsVM { get; set; }
        public OptionsVM OptionsVM { get; set; }
        public UserVM UserVM { get; set; }
        public OrderVM OrderVM { get; set; }
        public PromotionVariantsVM PromotionVariantsVM { get; set; }
        public VoucherVM VoucherVM { get; set; }
        public OrderVariantVM OrderVariantVM { get; set; }
        public List<OrderVM> LstOrderVM { get; set; }
        public List<PromotionVariantsVM> LstPromotionVariantsVM { get; set; }
        public List<PromotionVariant> LstPromotionVariants { get; set; }
        public List<OrderVariantVM> LstOrderVariantVM { get; set; }
        public List<UserVM> LstUser { get; set; }
        public List<VoucherUserVM> LstVoucherUserVM { get; set; }
        public List<VoucherVM> LstVoucherVM { get; set; }
        public List<CategoryVM> LstCategoryVM { get; set; }
        public List<VariantsVM> LstVariantsVM { get; set; }
        public List<OptionsVM> LstOptionsVM { get; set; }
        public PromotionVM PromotionVM { get; set; }
        //Create
        public VariantsCreateVM VariantsCreateVM { get; set; }
        public OptionsCreateVM OptionsCreateVM { get; set; }
        public PromotionCreateVM PromotionCreateVM { get; set; }
        public PromotionUpdateVM PromotionUpdateVM { get; set; }
        public VoucherCreateVM VoucherCreateVM { get; set; }
    }
}
