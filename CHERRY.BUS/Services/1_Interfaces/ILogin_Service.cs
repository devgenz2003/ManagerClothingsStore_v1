
using CHERRY.BUS.ViewModels;
using CHERRY.BUS.ViewModels.User;

namespace CHERRY.BUS.Services._1_Interfaces
{
    public partial interface ILogin_Service
    {
        public Task<Response> Login(UserLoginModel model);
    }
}
