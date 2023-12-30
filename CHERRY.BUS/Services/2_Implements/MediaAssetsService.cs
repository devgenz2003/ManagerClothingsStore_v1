using AutoMapper;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.MediaAssets;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.DAL.Entities;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace CHERRY.BUS.Services._2_Implements
{
    public class MediaAssetsService : IMediaAssetsService
    {
        private readonly Cloudinary cloudinary;

        private readonly CHERRY_DBCONTEXT _dbcontext;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnvironment;
        public MediaAssetsService(CHERRY_DBCONTEXT dbcontext, IMapper mapper, IHostingEnvironment hostingEnvironment, Cloudinary cloudinary)
        {
            _dbcontext = dbcontext;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _hostingEnvironment = hostingEnvironment;
            this.cloudinary = cloudinary;
        }
        public async Task<List<Guid>> CreateAsync(MediaAssetsCreateVM request, Cloudinary cloudinary)
        {
            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {

                    Guid idToUse;

                    if (request.IDVariant != null)
                    {
                        var checkProduct = await _dbcontext.Variants.FirstOrDefaultAsync(c => c.ID == request.IDVariant);
                        if (checkProduct != null)
                        {
                            idToUse = request.IDVariant.Value;
                        }
                        else
                        {
                            throw new ArgumentException("Không tìm thấy bản ghi Product với ID_PRODUCT đã cung cấp.");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Phải cung cấp ID_PRODUCT hoặc ID_REVIEW, IDProductVariants");
                    }


                    List<Guid> imageIds = new List<Guid>();

                    foreach (var file in request.ImageFile)
                    {
                        var imageId = Guid.NewGuid();
                        imageIds.Add(imageId);

                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        using var stream = file.OpenReadStream();
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(fileName, stream),
                            PublicId = fileName
                        };

                        var uploadResult = await cloudinary.UploadAsync(uploadParams);
                        var imageUrl = uploadResult.Url.ToString();

                        var imageEntity = new MediaAssets
                        {
                            ID = imageId,
                            IDVariant = request.IDVariant,
                            IDReview = request.IDReview,
                            Path = imageUrl,
                            AltText = request.AltText,
                            Status = 1,
                            CreateBy = request.CreateBy,
                        };

                        await _dbcontext.MediaAssets.AddAsync(imageEntity);
                    }

                    await _dbcontext.SaveChangesAsync();
                    transaction.Commit();

                    return imageIds;

                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
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
        public async Task<bool> RemoveAsync(Guid ID, string IDUserdelete)
        {
            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    var obj = await _dbcontext.MediaAssets.FirstOrDefaultAsync(c => c.ID == ID);

                    if (obj != null)
                    {
                        obj.Status = 0;
                        obj.DeleteDate = DateTime.Now;
                        obj.DeleteBy = IDUserdelete;

                        _dbcontext.MediaAssets.Attach(obj);
                        await _dbcontext.SaveChangesAsync();


                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public Task<bool> UpdateAsync(Guid ID, MediaAssetsUpdateVM request)
        {
            throw new NotImplementedException();
        }
    }
}
