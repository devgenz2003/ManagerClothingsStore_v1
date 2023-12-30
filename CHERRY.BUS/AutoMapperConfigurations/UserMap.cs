using AutoMapper;
using CHERRY.BUS.ViewModels.User;
using CHERRY.DAL.Entities;

namespace CHERRY.BUS.AutoMapperConfigurations
{
    public class UserMap : Profile
    {
        public UserMap()
        {
            CreateMap<User, UserVM>()
                .ReverseMap();
        }
    }
}
