using CHERRY.BUS.ViewModels.MediaAssets;
using CloudinaryDotNet;

namespace CHERRY.BUS.Services._1_Interface
{
	public interface IMediaAssetsRepository
	{
		public Task<List<MediaAssetsVM>> GetAllAsync();
		public Task<List<MediaAssetsVM>> GetAllActiveAsync();
		public Task<MediaAssetsVM> GetByIDAsync(Guid ID);
		public Task<MediaAssetsVM> GetCartByUserIDAsync(Guid ID_USER);
		public Task<List<Guid>> CreateAsync(MediaAssetsCreateVM request, Cloudinary cloudinary);
		public Task<bool> RemoveAsync(Guid ID, Guid IDUserdelete);
		public Task<bool> UpdateAsync(Guid ID, MediaAssetsUpdateVM request);
	}
}
