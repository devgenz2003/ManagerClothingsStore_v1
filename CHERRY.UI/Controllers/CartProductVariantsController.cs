using CHERRY.BUS.ViewModels.CartProductVariants;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.UI.Models;
using CHERRY.UI.Repositorys._1_Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CHERRY.UI.Controllers
{
    public class CartProductVariantsController : Controller
    {
        private readonly ILogger<CartProductVariantsController> _logger;
        private readonly ICartRepository _ICartRepository;
        private readonly IVariantRepository _IVariantRepository;
        private readonly IOptionsRepository _IOptionsRepository;
        private readonly ICartProductVariantsRepository _ICartProductVariantsRepository;
        private readonly CHERRY_DBCONTEXT _dbcontext;
        public CartProductVariantsController(ICartRepository ICartRepository, CHERRY_DBCONTEXT dbcontext,
            ICartProductVariantsRepository ICartProductVariantsRepository,
            IOptionsRepository IOptionsRepository,
            ILogger<CartProductVariantsController> logger,
            IVariantRepository IVariantRepository)
        {
            _ICartRepository = ICartRepository;
            _dbcontext = dbcontext;
            _IVariantRepository = IVariantRepository;
            _IOptionsRepository = IOptionsRepository;
            _ICartProductVariantsRepository = ICartProductVariantsRepository;
            _logger = logger;
        }
        private string ExtractUserIDFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken?.Claims != null)
                {
                    var userIDClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
                    if (userIDClaim != null)
                    {
                        return userIDClaim.Value;
                    }
                }

                return "Failed";
            }
            catch (Exception ex)
            {
                return "Failed";
            }
        }
        [HttpGet]
        [Route("CartIndex")]
        public async Task<IActionResult> Index()
        {
            try
            {
                string token = HttpContext.Request.Cookies["token"];
                string userID = ExtractUserIDFromToken(token);

                if (userID == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                string cartJson = Request.Cookies["CartProduct"];
                List<CartProductVariantsVM> cartItems;

                if (!string.IsNullOrEmpty(cartJson))
                {
                    cartItems = JsonConvert.DeserializeObject<List<CartProductVariantsVM>>(cartJson);
                    ViewBag.CartItems = cartItems;
                }
                else
                {
                    cartItems = new List<CartProductVariantsVM>();
                }

                foreach (var item in cartItems)
                {

                    var variant = await _IOptionsRepository.GetByIDAsync(item.IDOptions.Value);
                }

                return View(cartItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CartProductVariantsController Index action.");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(Guid IDOptions, int Quantity)
        {
            try
            {
                string token = HttpContext.Request.Cookies["token"];
                string userID = ExtractUserIDFromToken(token);

                if (userID == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                string cartJson = Request.Cookies["CartProduct"];
                List<CartProductVariantsVM> cartList = string.IsNullOrEmpty(cartJson)
                    ? new List<CartProductVariantsVM>()
                    : JsonConvert.DeserializeObject<List<CartProductVariantsVM>>(cartJson);
                var existingCartItem = cartList.FirstOrDefault(item => item.IDOptions == IDOptions);

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += 1; 
                    existingCartItem.Total_Amount = (existingCartItem.Quantity * existingCartItem.UnitPrice) - (existingCartItem.DiscountedPrice * existingCartItem.Quantity);
                }
                else
                {
                    var cartUser = await _ICartRepository.GetCartByUserIDAsync(userID);
                    var options = await _dbcontext.Options.FirstOrDefaultAsync(c => c.ID == IDOptions);

                    if (options != null)
                    {
                        var cartItem = _dbcontext.Options
                            .Where(vo => vo.ID == IDOptions)
                            .Select(vov => new CartProductVariantsVM
                            {
                                ID_Cart = cartUser.ID,
                                IDOptions = vov.ID,
                                ProductName = vov.Variants.VariantName,
                                SizeName = string.Join(", ", vov.Sizes.Name),
                                ColorName = string.Join(", ", vov.Color.Name),
                                UnitPrice = options.RetailPrice,
                                Quantity = Quantity,
                                DiscountedPrice = options.DiscountedPrice.Value * Quantity,
                                Status = 1,
                                Imagepaths = vov.ImageURL,
                                Total_Amount = (options.RetailPrice * 1) - options.DiscountedPrice.Value,
                            })
                            .FirstOrDefault();

                        if (cartItem != null)
                        {
                            cartItem.ColorName = cartItem.ColorName ?? "Default Color"; // Add default color if null
                            cartItem.SizeName = cartItem.SizeName ?? "Default Size"; // Add default size if null

                            cartList.Add(cartItem);
                        }
                    }
                    else
                    {
                        return RedirectToAction("ProductNotFound", "Home");
                    }
                }

                var cartJsonUpdated = JsonConvert.SerializeObject(cartList);
                Response.Cookies.Append("CartProduct", cartJsonUpdated, new CookieOptions { HttpOnly = true, Expires = DateTime.Now.AddDays(30) });

                return RedirectToAction("Index", "CartProductVariants");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CartProductVariantsController AddToCart action.");
                return RedirectToAction("Error", "Home");
            }
        }


        public async Task<IActionResult> RemoveFromCart(Guid IDOptions)
        {
            string cartJson = Request.Cookies["CartProduct"];
            if (!string.IsNullOrEmpty(cartJson))
            {
                List<CartProductVariantsVM> cartList = JsonConvert.DeserializeObject<List<CartProductVariantsVM>>(cartJson);

                var productToRemove = cartList.FirstOrDefault(p => p.IDOptions == IDOptions);
                if (productToRemove != null)
                {
                    cartList.Remove(productToRemove);

                    // Lưu trở lại vào cookie
                    string updatedCartJson = JsonConvert.SerializeObject(cartList);
                    Response.Cookies.Append("CartProduct", updatedCartJson, new CookieOptions { Expires = DateTime.Now.AddDays(1) });
                }
                else
                {
                }
            }

            return RedirectToAction("Index", "CartProductVariants");
        }

        private CheckoutViewModel MapCartItemsToCheckoutViewModel(IEnumerable<CartProductVariantsVM> cartItems)
        {
            var checkoutViewModel = new CheckoutViewModel
            {
                CartItems = cartItems.Select(cp => new CartItemViewModel
                {
                    IDOptions = cp.IDOptions.Value,
                    ProductName = cp.ProductName,
                    Quantity = cp.Quantity,
                    Price = cp.UnitPrice 
                }).ToList(),
                TotalAmount = cartItems.Sum(cp => cp.Total_Amount),
            };

            return checkoutViewModel;
        }

    }
}
