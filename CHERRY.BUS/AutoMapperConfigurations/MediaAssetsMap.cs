using AutoMapper;
using CHERRY.BUS.ViewModels.MediaAssets;
using CHERRY.DAL.Entities;

namespace CHERRY.BUS.AutoMapperConfigurations
{
	public class MediaAssetsMap : Profile
	{
        public MediaAssetsMap()
        {
            CreateMap<MediaAssets, MediaAssetsVM>().ReverseMap();
        }
    }
}
