
using CHERRY.BUS.ViewModels.Review;

namespace CHERRY.BUS.Services._1_Interfaces
{
    public interface IReviewService
    {
        public Task<List<ReviewVM>> GetAllAsync();
        public Task<List<ReviewVM>> GetAllActiveAsync();
        public Task<ReviewVM> GetByIDAsync(Guid ID);
        public Task<List<ReviewVM>> GetByVariant(Guid IDVariant);
        public Task<ReviewVM> GetByUser(string IDUser);
        public Task<bool> CreateAsync(ReviewCreateVM request);
        public Task<bool> RemoveAsync(Guid ID, string IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, ReviewUpdateVM request);
    }
}
