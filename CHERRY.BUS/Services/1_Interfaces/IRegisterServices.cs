using CHERRY.BUS.ViewModels;
using CHERRY.BUS.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHERRY.BUS.Services._1_Interfaces
{
    public interface IRegisterServices
    {
        Task<Response> RegisterAsync(RegisterUser registerUser, string role);
    }
}
