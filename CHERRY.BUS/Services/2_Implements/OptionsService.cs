using AutoMapper;
using AutoMapper.QueryableExtensions;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.Brand;
using CHERRY.BUS.ViewModels.Options;
using CHERRY.BUS.ViewModels.Variants;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.DAL.Entities;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CHERRY.BUS.Services._2_Implements
{
    public class OptionsService : IOptionsService
    {
        private readonly CHERRY_DBCONTEXT _dbcontext;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly Cloudinary cloudinary;
        public OptionsService(CHERRY_DBCONTEXT dbcontext, Cloudinary cloudinary, IMapper mapper, IMemoryCache memoryCache)
        {
            this.cloudinary = cloudinary;
            _cache = memoryCache;
            _dbcontext = dbcontext;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<OptionsVM> FindIDOptionsAsync(Guid IDVariant, string size, string color)
        {
            var option = await _dbcontext.Options
                .Where(o => o.IDVariant == IDVariant && o.Sizes.Name == size && o.Color.Name == color)
                .Select(o => new OptionsVM
                {
                    ID = o.ID,
                    IDVariant = o.IDVariant.Value,
                    IDColor = o.IDColor,
                    IDSizes = o.IDSizes,
                    StockQuantity = o.StockQuantity,
                    CostPrice = o.CostPrice,
                    RetailPrice = o.RetailPrice,
                    DiscountedPrice = o.DiscountedPrice,
                    Name = o.Variants.VariantName,
                    SizeName = o.Sizes.Name,
                    ColorName = o.Color.Name,
                    ImageURL = o.ImageURL,
                    Status = o.Status
                })
                .FirstOrDefaultAsync();

            return option;
        }
        public async Task<Guid> EnsureSize(string sizeName)
        {
            var size = await _dbcontext.Sizes
                .FirstOrDefaultAsync(s => s.Name.ToLower() == sizeName.ToLower());

            if (size == null)
            {
                size = new Sizes { ID = Guid.NewGuid(), Name = sizeName, CreateBy = "A0278C2E-A23D-4B21-B018-D7C49B074E90" , Status = 1};
                await _dbcontext.Sizes.AddAsync(size);
                await _dbcontext.SaveChangesAsync();
            }

            return size.ID;
        }
        public async Task<Guid> EnsureColor(string colorName)
        {
            var color = await _dbcontext.Colors
                .FirstOrDefaultAsync(c => c.Name.ToLower() == colorName.ToLower());

            if (color == null)
            {
                color = new Colors { ID = Guid.NewGuid(), Name = colorName, CreateBy = "A0278C2E-A23D-4B21-B018-D7C49B074E90", Status = 1 };
                await _dbcontext.Colors.AddAsync(color);
                await _dbcontext.SaveChangesAsync();
            }

            return color.ID;
        }
        public async Task<bool> CreateAsync(OptionsCreateVM request)
        {
            var checkvariant = await _dbcontext.Variants.FirstOrDefaultAsync(c => c.ID == request.IDVariant);
            var checkColor = await _dbcontext.Colors.FirstOrDefaultAsync(c => c.ID == request.IDColor);
            var checkSizes = await _dbcontext.Sizes.FirstOrDefaultAsync(c => c.ID == request.IDSizes);

            if (checkvariant == null)
            {
                return false;
            }
            if (checkSizes == null && !string.IsNullOrEmpty(request.SizesName))
            {
                request.IDSizes = await EnsureSize(request.SizesName);
            }

            if (checkColor == null && !string.IsNullOrEmpty(request.ColorName))
            {
                request.IDColor = await EnsureColor(request.ColorName);
            }
            checkColor = await _dbcontext.Colors.FirstOrDefaultAsync(c => c.ID == request.IDColor);
            checkSizes = await _dbcontext.Sizes.FirstOrDefaultAsync(c => c.ID == request.IDSizes);
            var option = new Options
            {
                ID = Guid.NewGuid(),
                IDVariant = checkvariant.ID,
                IDColor = checkColor.ID,
                IDSizes = checkSizes.ID,
                CostPrice = request.CostPrice,
                RetailPrice = request.RetailPrice,
                StockQuantity = request.StockQuantity,
                DiscountedPrice = request.DiscountedPrice,
                Status = request.Status,
                CreateBy = request.CreateBy,
            };
            _dbcontext.Options.Add(option);

            // Tải ảnh lên Cloudinary
            var cloudinaryUrl = await UploadImageToCloudinary(request.ImagePaths);

            if (!string.IsNullOrEmpty(cloudinaryUrl))
            {
                option.ImageURL = cloudinaryUrl; // Lưu URL của ảnh vào Options

                await _dbcontext.SaveChangesAsync(); // Lưu vào cơ sở dữ liệu
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<string> UploadImageToCloudinary(IFormFile file)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
            };

            try
            {
                var uploadResult = await cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.AbsoluteUri;
            }
            catch (Exception)
            {
                return "Không thể upload hình ảnh";
            }
        }
        public async Task<List<OptionsVM>> GetAllActiveAsync()
        {
            var objList = await _dbcontext.Options
                 .AsNoTracking()
                 .Where(b => b.Status != 0)
                 .ProjectTo<OptionsVM>(_mapper.ConfigurationProvider)
                 .ToListAsync();

            return objList ?? new List<OptionsVM>();
        }
        public async Task<List<OptionsVM>> GetAllAsync()
        {
            var objList = await _dbcontext.Options
                 .AsNoTracking()
                 .ProjectTo<OptionsVM>(_mapper.ConfigurationProvider)
                 .ToListAsync();

            return objList ?? new List<OptionsVM>();
        }
        public async Task<OptionsVM> GetByIDAsync(Guid ID)
        {
            var obj = await _dbcontext.Options
                .Where(o => o.ID == ID)
                .ProjectTo<OptionsVM>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return obj;
        }
        public async Task<bool> RemoveAsync(Guid ID, string IDUserdelete)
        {
            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    var obj = await _dbcontext.Options.FirstOrDefaultAsync(c => c.ID == ID);

                    if (obj != null)
                    {
                        obj.Status = 0;
                        obj.DeleteDate = DateTime.Now;
                        obj.DeleteBy = IDUserdelete;

                        _dbcontext.Options.Attach(obj);
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
        public async Task<bool> UpdateAsync(Guid ID, OptionsUpdateVM request)
        {
            var checkVariant = await _dbcontext.Variants.FirstOrDefaultAsync(c => c.ID == request.IDVariant);
            var checkColor = await _dbcontext.Colors.FirstOrDefaultAsync(c => c.ID == request.IDColor);
            var checkSizes = await _dbcontext.Sizes.FirstOrDefaultAsync(c => c.ID == request.IDSizes);

            if (checkVariant == null)
            {
                return false;
            }

            if (checkSizes == null && !string.IsNullOrEmpty(request.SizesName))
            {
                request.IDSizes = await EnsureSize(request.SizesName);
            }

            if (checkColor == null && !string.IsNullOrEmpty(request.ColorName))
            {
                request.IDColor = await EnsureColor(request.ColorName);
            }

            checkColor = await _dbcontext.Colors.FirstOrDefaultAsync(c => c.ID == request.IDColor);
            checkSizes = await _dbcontext.Sizes.FirstOrDefaultAsync(c => c.ID == request.IDSizes);

            var option = await _dbcontext.Options.FindAsync(ID);

            if (option == null)
            {
                return false;
            }

            // Cập nhật các trường thông tin
            option.IDVariant = checkVariant.ID;
            option.IDColor = checkColor?.ID;
            option.IDSizes = checkSizes?.ID;
            option.CostPrice = request.CostPrice;
            option.RetailPrice = request.RetailPrice;
            option.StockQuantity = request.StockQuantity;
            option.DiscountedPrice = request.DiscountedPrice;
            option.Status = request.Status;

            if (request.ImageURL != null)
            {
                var cloudinaryUrl = await UploadImageToCloudinary(request.ImageURL);
                if (!string.IsNullOrEmpty(cloudinaryUrl))
                {
                    option.ImageURL = cloudinaryUrl;
                }
            }

            _dbcontext.Options.Update(option);
            await _dbcontext.SaveChangesAsync();

            return true;
        }
        public async Task<Guid> GetVariantByID(Guid IDOptions)
        {
            var variant = await _dbcontext.Variants
                .Where(v => v.Options.Any(o => o.ID == IDOptions)) // Điều chỉnh lại điều kiện truy vấn theo cấu trúc của dữ liệu
                .Include(v => v.Options) // Lấy thông tin các options của biến thể
                .Include(v => v.MediaAssets)
                .FirstOrDefaultAsync();

            if (variant == null)
            {
                return Guid.Empty;
            }

            return variant.ID;
        }
    }
}
