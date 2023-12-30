using CHERRY.BUS.ViewModels.CartProductVariants;
using CHERRY.DAL.Entities;
using CHERRY.UI.Repositorys._1_Interface;

namespace CHERRY.UI.Repositorys._2_Implement
{
    public class CartProductVariantsRepository : ICartProductVariantsRepository
    {
        private readonly HttpClient _httpClient;

        public CartProductVariantsRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CreateAsync(CartProductVariantsCreateVM request)
        {
            var resutl = await _httpClient.PostAsJsonAsync("api/CartProductVariants/AddToCart", request);
            return resutl.IsSuccessStatusCode;
        }

        public async Task<List<CartProductVariantsVM>> GetAllActiveAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CartProductVariantsVM>>("api/CartProductVariants/allactive");
        }

        public async Task<List<CartProductVariantsVM>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CartProductVariantsVM>>("api/CartProductVariants/all");
        }

        public async Task<List<CartProductVariantsVM>> GetAllByCartIdAsync(Guid ID_Cart)
        {
            return await _httpClient.GetFromJsonAsync<List<CartProductVariantsVM>>($"api/CartProductVariants/GetAllByCartIdAsync/{ID_Cart}");
        }

        public async Task<CartProductVariantsVM> GetByIDAsync(Guid ID_Cart, Guid ID_ClassifyItem)
        {
            return await _httpClient.GetFromJsonAsync<CartProductVariantsVM>($"api/CartProductVariants/{ID_Cart}/{ID_ClassifyItem}");
        }

        public async Task<bool> RemoveAsync(Guid ID_Cart, Guid ID_ClassifyItem, Guid idUserdelete)
        {
            var resutl = await _httpClient.DeleteAsync($"api/CartProductVariants/{ID_Cart}/{ID_ClassifyItem}/{idUserdelete}");
            return resutl.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(Guid ID_Cart, Guid ID_ClassifyItem, CartProductVariantsUpdateVM request)
        {
            var resutl = await _httpClient.PutAsJsonAsync($"api/CartProductVariants/{ID_Cart}/{ID_ClassifyItem}", request);
            return resutl.IsSuccessStatusCode;
        }
    }
}
