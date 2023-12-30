using CHERRY.BUS.Services._1_Interface;
using CHERRY.BUS.ViewModels.MediaAssets;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace CHERRY.UI.Repositorys._2_Implement
{
    public class MediaAssetsRepository : IMediaAssetsRepository
    {
        private readonly HttpClient _httpClient;

        public MediaAssetsRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Guid>> CreateAsync(MediaAssetsCreateVM request, Cloudinary cloudinary)
        {
            var response = await _httpClient.PostAsJsonAsync("api/MediaAssets/create", request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<Guid>>();
                return result;
            }
            return new List<Guid>();
        }
        public async Task<string> UploadImageToCloudinaryAsync(IFormFile imageFile, Cloudinary cloudinary)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            using var stream = imageFile.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, stream),
                PublicId = fileName
            };
            var uploadResult = await cloudinary.UploadAsync(uploadParams);
            return uploadResult.Url.ToString();
        }

        public Task<List<MediaAssetsVM>> GetAllActiveAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<MediaAssetsVM>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<MediaAssetsVM> GetByIDAsync(Guid ID)
        {
            throw new NotImplementedException();
        }

        public Task<MediaAssetsVM> GetCartByUserIDAsync(Guid ID_USER)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(Guid ID, Guid IDUserdelete)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Guid ID, MediaAssetsUpdateVM request)
        {
            throw new NotImplementedException();
        }
    }
}
