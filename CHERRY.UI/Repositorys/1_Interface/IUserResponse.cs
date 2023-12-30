using CHERRY.BUS.ViewModels.User;

namespace CHERRY.UI.Repositorys._1_Interface
{
    public interface IUserResponse
    {
        public Task<List<UserVM>> GetAllAsync();
        public Task<List<UserVM>> GetAllActiveAsync();
        public Task<UserVM> GetByIDAsync(string ID);
        public Task<bool> CreateAsync(UserCreateVM request);
        public Task<bool> UpdateAsync(string ID, UserUpdateVM request);
        public Task<bool> RemoveAsync(string ID, Guid IDUserdelete);
        public Task<UserVM> GetByGmailAsync(string Gmail); 
        public Task<bool> ChangeUserRoleAsync(string IDUser, string newRole);
        public Task<bool> ChangePasswordAsync(string IDUser, ChangePasswordViewModel model);

    }
}
