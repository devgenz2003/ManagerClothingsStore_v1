using CHERRY.BUS.ViewModels.Cart;
using CHERRY.UI.Repositorys._1_Interface;

namespace CHERRY.UI.Repositorys._2_Implement
{
    public class CartRepository : ICartRepository
    {
        private readonly HttpClient _httpClient;

        public CartRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> CreateAsync(CartCreateVM request)
        {
            var resutl = await _httpClient.PostAsJsonAsync("api/Cart/create", request);
            return resutl.IsSuccessStatusCode;
        }

        public async Task<List<CartVM>> GetAllActiveAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CartVM>>("api/Cart/getallactive");
        }

        public async Task<List<CartVM>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CartVM>>("api/Cart/all");
        }

        public async Task<CartVM> GetByIDAsync(Guid ID)
        {
            return await _httpClient.GetFromJsonAsync<CartVM>($"api/Cart/GetByID/{ID}");
        }

        public async Task<CartVM> GetCartByUserIDAsync(string ID_USER)
        {
            return await _httpClient.GetFromJsonAsync<CartVM>($"api/Cart/GetCartByUserIDAsync/{ID_USER}");
        }

        public async Task<bool> RemoveAsync(Guid ID, Guid IDUserdelete)
        {
            var resutl = await _httpClient.DeleteAsync($"api/Cart/{ID}/{IDUserdelete}");
            return resutl.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(Guid ID, CartUpdateVM request)
        {
            var resutl = await _httpClient.PutAsJsonAsync($"api/Cart/{ID}", request);
            return resutl.IsSuccessStatusCode;
        }
    }
}
