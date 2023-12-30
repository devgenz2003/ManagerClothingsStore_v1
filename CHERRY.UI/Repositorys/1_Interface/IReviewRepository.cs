using CHERRY.BUS.ViewModels.Review;

namespace CHERRY.UI.Repositorys._1_Interface
{
    public interface IReviewRepository
    {
        public Task<List<ReviewVM>> GetAllAsync();
        public Task<List<ReviewVM>> GetAllActiveAsync();
        public Task<ReviewVM> GetByIDAsync(Guid ID);
        public Task<bool> CreateAsync(ReviewCreateVM request);
        public Task<bool> RemoveAsync(Guid ID, Guid IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, ReviewUpdateVM request); 
        public Task<List<ReviewVM>> GetByVariant(Guid IDVariant);
        public Task<ReviewVM> GetByUser(string IDUser);
    }
}
