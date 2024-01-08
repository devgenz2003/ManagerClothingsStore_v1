using AutoMapper;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.Variants;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.DAL.Entities;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using AutoMapper.QueryableExtensions;
using CHERRY.BUS.ViewModels.Options;
using static System.Net.Mime.MediaTypeNames;
namespace CHERRY.BUS.Services._2_Implements
{
    public class VariantsService : IVariantsService
    {
        private readonly CHERRY_DBCONTEXT _dbContext;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly Cloudinary cloudinary;
        public VariantsService(CHERRY_DBCONTEXT dbcontext, Cloudinary cloudinary, IMapper mapper, IMemoryCache memoryCache)
        {
            this.cloudinary = cloudinary;
            _cache = memoryCache;
            _dbContext = dbcontext;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<Guid> EnsureMaterial(string MaterialName)
        {
            var Material = await _dbContext.Material
                .FirstOrDefaultAsync(s => s.Name.ToLower() == MaterialName.ToLower());

            if (Material == null)
            {
                Material = new Material { ID = Guid.NewGuid(), Name = MaterialName, Status = 1, CreateBy = "4fc47db8-be19-4bac-804f-ed4fd1007c76" };
                await _dbContext.Material.AddAsync(Material);
                await _dbContext.SaveChangesAsync();
            }

            return Material.ID;
        }
        public async Task<Guid> EnsureBrand(string BrandName)
        {
            var Brand = await _dbContext.Brand
                .FirstOrDefaultAsync(c => c.Name.ToLower() == BrandName.ToLower());

            if (Brand == null)
            {
                Brand = new Brand { ID = Guid.NewGuid(), Name = BrandName, Status = 1, CreateBy = "4fc47db8-be19-4bac-804f-ed4fd1007c76" };
                await _dbContext.Brand.AddAsync(Brand);
                await _dbContext.SaveChangesAsync();
            }

            return Brand.ID;
        }
        public async Task<Guid> EnsureCategory(string CategoryName)
        {
            var Category = await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == CategoryName.ToLower());

            if (Category == null)
            {
                Category = new Categories { ID = Guid.NewGuid(), Name = CategoryName, Status = 1, CreateBy = "4fc47db8-be19-4bac-804f-ed4fd1007c76" };
                await _dbContext.Categories.AddAsync(Category);
                await _dbContext.SaveChangesAsync();
            }

            return Category.ID;
        }
        public async Task<bool> CreateAsync(VariantsCreateVM request)
        {
            var checkbrand = await _dbContext.Brand.FirstOrDefaultAsync(c => c.ID == request.IDBrand);
            var checkmaterial = await _dbContext.Material.FirstOrDefaultAsync(c => c.ID == request.IDMaterial);
            var checkcategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.ID == request.IDCategory);

            if (checkbrand == null && !string.IsNullOrEmpty(request.BrandName))
            {
                request.IDBrand = await EnsureBrand(request.BrandName);
            }

            if (checkcategory == null && !string.IsNullOrEmpty(request.CategoryName))
            {
                request.IDCategory = await EnsureCategory(request.CategoryName);
            }

            if (checkmaterial == null && !string.IsNullOrEmpty(request.MaterialName))
            {
                request.IDMaterial = await EnsureMaterial(request.MaterialName);
            }

            checkbrand = await _dbContext.Brand.FirstOrDefaultAsync(c => c.ID == request.IDBrand);
            checkmaterial = await _dbContext.Material.FirstOrDefaultAsync(c => c.ID == request.IDMaterial);
            checkcategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.ID == request.IDCategory);

            var ProductVariants = new Variants
            {
                ID = Guid.NewGuid(),
                SKU_v2 = request.SKU_v2,
                Style = request.Style,
                VariantName = request.VariantName,
                Description = request.Description,
                Origin = request.Origin,
                IDMaterial = checkmaterial.ID,
                IDBrand = checkbrand.ID,
                CreateBy = request.CreateBy,
                Status = 1,
            };
            var productVariantCategory = new CategoriesVariants
            {
                IDVariants = ProductVariants.ID,
                IDCategories = checkcategory.ID,
                CreateBy = request.CreateBy,
                Status = 1
            };

            _dbContext.CategoriesVariants.Add(productVariantCategory);

            _dbContext.Variants.Add(ProductVariants);
            List<Guid> imageIds = new List<Guid>();

            foreach (var file in request.ImagePaths)
            {
                var imageId = Guid.NewGuid();
                var fileName = imageId.ToString() + Path.GetExtension(file.FileName);
                using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(fileName, stream),
                    PublicId = imageId.ToString()
                };

                try
                {
                    var uploadResult = await cloudinary.UploadAsync(uploadParams);
                    var imageUrl = uploadResult.Url.ToString();

                    var imageEntity = new MediaAssets
                    {
                        ID = imageId,
                        IDVariant = ProductVariants.ID,
                        IDReview = null,
                        Path = imageUrl,
                        Status = 1,
                        CreateDate = DateTime.Now,
                        CreateBy = request.CreateBy
                    };

                    await _dbContext.MediaAssets.AddAsync(imageEntity);
                }
                catch (Exception)
                {

                }
            }
            await _dbContext.SaveChangesAsync();

            return true;
        }
        public async Task<List<VariantsVM>> GetAllActiveAsync()
        {
            var activeVariants = await _dbContext.Variants
                .AsNoTracking()
                .Where(v => v.Status == 1)
                .Select(v => new
                {
                    Variant = v,
                    Brand = v.Brand,
                    Material = v.Material,
                    Options = v.Options,
                    MediaAssets = v.MediaAssets.Where(m => m.Status == 1)
                })
                .ToListAsync();

            var variantsVMList = activeVariants.Select(v => new VariantsVM
            {
                ID = v.Variant.ID,
                VariantName = v.Variant.VariantName,
                BrandName = v.Brand?.Name,
                CreateDate = v.Variant.CreateDate,
                MaterialName = v.Material?.Name,
                Description = v.Variant.Description,
                Minprice = v.Options.Any() ? v.Options.Min(opt => opt.RetailPrice) : 0,
                Maxprice = v.Options.Any() ? v.Options.Max(opt => opt.RetailPrice) : 0,
                TotalOptions = v.Options.Count(),
                TotalQuantity = v.Options.Sum(opt => opt.StockQuantity),
                SizeName = v.Options.FirstOrDefault(o => o.Sizes != null)?.Sizes.Name,
                ColorName = v.Options.FirstOrDefault(o => o.Color != null)?.Color.Name,
                ImagePaths = v.MediaAssets.Select(m => m.Path).ToList(),
                Status = v.Variant.Status
            }).ToList();

            return variantsVMList;
        }
        public async Task<List<VariantsVM>> GetAllAsync()
        {

            var activeVariants = await _dbContext.Variants
                .AsNoTracking()
                .Select(v => new
                {
                    Variant = v,
                    Brand = v.Brand,
                    Material = v.Material,
                    Options = v.Options,
                    MediaAssets = v.MediaAssets.Where(m => m.Status == 1)
                })
                .ToListAsync();

            var variantsVMList = activeVariants.Select(v => new VariantsVM
            {
                ID = v.Variant.ID,
                VariantName = v.Variant.VariantName,
                BrandName = v.Brand?.Name,
                CreateDate = v.Variant.CreateDate,
                Description = v.Variant.Description,
                MaterialName = v.Material?.Name,
                Minprice = v.Options.Any() ? v.Options.Min(opt => opt.RetailPrice) : 0,
                Maxprice = v.Options.Any() ? v.Options.Max(opt => opt.RetailPrice) : 0,
                TotalOptions = v.Options.Count(),
                TotalQuantity = v.Options.Sum(opt => opt.StockQuantity),
                SizeName = v.Options.FirstOrDefault(o => o.Sizes != null)?.Sizes.Name,
                ColorName = v.Options.FirstOrDefault(o => o.Color != null)?.Color.Name,
                ImagePaths = v.MediaAssets.Select(m => m.Path).ToList(),
                Status = v.Variant.Status
            }).ToList();

            return variantsVMList;
        }
        public async Task<VariantsVM> GetByIDAsync(Guid ID)
        {
            var cacheKey = $"Product_{ID}";

            if (_cache.TryGetValue(cacheKey, out VariantsVM cachedData))
            {
                return cachedData;
            }

            var obj = await _dbContext.Variants.AsQueryable()
                .ProjectTo<VariantsVM>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(c => c.ID == ID);

            if (obj == null)
            {
                return null;
            }

            var mediaAssets = await _dbContext.MediaAssets.AsNoTracking()
                .Where(m => m.IDVariant == obj.ID && m.Status == 1)
                .OrderByDescending(m => m.CreateDate)
                .Select(m => m.Path)
                .ToListAsync();

            var options = await _dbContext.Options.AsNoTracking()
                .Where(opt => opt.IDVariant == obj.ID)
                .Include(opt => opt.Color)
                .Include(opt => opt.Sizes)
                .ToListAsync();

            var sizes = options.Where(opt => opt.Sizes != null).Select(opt => opt.Sizes.Name).Distinct().ToList();
            var colors = options.Where(opt => opt.Color != null).Select(opt => opt.Color.Name).Distinct().ToList();

            obj.SizeName = sizes.FirstOrDefault();
            obj.ColorName = colors.FirstOrDefault();

            obj.ImagePaths = mediaAssets;

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };

            _cache.Set(cacheKey, obj, cacheEntryOptions);

            return obj;
        }
        public async Task<bool> RemoveAsync(Guid ID, string IDUserdelete)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var obj = await _dbContext.Variants.FirstOrDefaultAsync(c => c.ID == ID);

                    if (obj != null)
                    {
                        obj.Status = 0;
                        obj.DeleteDate = DateTime.Now;
                        obj.DeleteBy = IDUserdelete;

                        _dbContext.Variants.Attach(obj);
                        await _dbContext.SaveChangesAsync();


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
        public async Task<bool> UpdateAsync(Guid ID, VariantsUpdateVM request)
        {
            if (ID == Guid.Empty || request == null)
            {
                return false;
            }

            var variantToUpdate = await _dbContext.Variants
                .Include(v => v.MediaAssets)
                .FirstOrDefaultAsync(v => v.ID == ID);

            if (variantToUpdate == null)
            {
                return false;
            }

            using var transaction = _dbContext.Database.BeginTransaction(); 

            try
            {
                var checkbrand = await _dbContext.Brand.FirstOrDefaultAsync(c => c.ID == request.IDBrand);
                var checkmaterial = await _dbContext.Material.FirstOrDefaultAsync(c => c.ID == request.IDMaterial);
                var checkcategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.ID == request.IDCategory);

                if (checkbrand == null && !string.IsNullOrEmpty(request.BrandName))
                {
                    request.IDBrand = await EnsureBrand(request.BrandName);
                }

                if (checkcategory == null && !string.IsNullOrEmpty(request.CategoryName))
                {
                    request.IDCategory = await EnsureCategory(request.CategoryName);
                }

                if (checkmaterial == null && !string.IsNullOrEmpty(request.MaterialName))
                {
                    request.IDMaterial = await EnsureMaterial(request.MaterialName);
                }

                checkbrand = await _dbContext.Brand.FirstOrDefaultAsync(c => c.ID == request.IDBrand);
                checkmaterial = await _dbContext.Material.FirstOrDefaultAsync(c => c.ID == request.IDMaterial);
                checkcategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.ID == request.IDCategory);

                variantToUpdate.VariantName = request.VariantName;
                variantToUpdate.Description = request.Description;
                variantToUpdate.Style = request.Style;
                variantToUpdate.Origin = request.Origin;
                variantToUpdate.SKU_v2 = request.SKU_v2;
                variantToUpdate.Status = request.Status;
                variantToUpdate.ModifieBy = "";
                variantToUpdate.ModifieDate = DateTime.UtcNow;


                _dbContext.MediaAssets.RemoveRange(variantToUpdate.MediaAssets);

                foreach (var file in request.ImagePaths)
                {
                    var imageId = Guid.NewGuid();
                    var fileName = imageId.ToString() + Path.GetExtension(file.FileName);
                    using var stream = file.OpenReadStream();

                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(fileName, stream),
                        PublicId = imageId.ToString()
                    };

                    var uploadResult = await cloudinary.UploadAsync(uploadParams);
                    var imageUrl = uploadResult.Url.ToString();

                    var imageEntity = new MediaAssets
                    {
                        ID = imageId,
                        IDVariant = variantToUpdate.ID,
                        Path = imageUrl,
                        Status = 1,
                        CreateDate = DateTime.Now,
                        ModifieBy = request.ModifieBy,
                        ModifieDate = DateTime.Now,
                        CreateBy = imageId.ToString(),
                    };

                    _dbContext.MediaAssets.Add(imageEntity);
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync(); // Kết thúc giao dịch
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); 
                return false;
            }
        }
        public async Task<List<OptionsVM>> GetOptionVariantByIDAsync(Guid IDVariant)
        {
            var optionsVMs = await _dbContext.Options
                .Include(o => o.Variants)
                .Include(o => o.Color)
                .Include(o => o.Sizes)
                .Where(o => o.IDVariant == IDVariant)
                .ProjectTo<OptionsVM>(_mapper.ConfigurationProvider)
                .Select(optionVariant => new OptionsVM
                {
                    ID = optionVariant.ID,
                    IDColor = optionVariant.IDColor,
                    IDSizes = optionVariant.IDSizes,
                    IDVariant = IDVariant,
                    CostPrice = optionVariant.CostPrice,
                    RetailPrice = optionVariant.RetailPrice,
                    DiscountedPrice = optionVariant.DiscountedPrice,
                    StockQuantity = optionVariant.StockQuantity,
                    Name = optionVariant.Name,
                    ColorName = optionVariant.ColorName,
                    SizeName = optionVariant.SizeName,
                    ImageURL = optionVariant.ImageURL,
                    Status = optionVariant.Status,
                })
                .ToListAsync();

            return optionsVMs;
        }
    }
}
