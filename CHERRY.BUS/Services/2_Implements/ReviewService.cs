using AutoMapper;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.Review;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.DAL.Entities;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using CHERRY.BUS.ViewModels.Variants;
using AutoMapper.QueryableExtensions;
using CHERRY.BUS.ViewModels.User;
using Microsoft.AspNetCore.Identity;

namespace CHERRY.BUS.Services._2_Implements
{
    public class ReviewService : IReviewService
    {
        private readonly Cloudinary cloudinary;
        private readonly UserManager<User> _userManager;
        private readonly CHERRY_DBCONTEXT _dbcontext;
        private readonly IMapper _mapper;
        public ReviewService(CHERRY_DBCONTEXT CHERRY_DBCONTEXT, IMapper mapper, Cloudinary cloudinary, UserManager<User> userManager)
        {
            _dbcontext = CHERRY_DBCONTEXT;
            _mapper = mapper;
            this.cloudinary = cloudinary;
            _userManager = userManager;

        }
        public async Task<bool> CreateAsync(ReviewCreateVM request)
        {
            if (request == null || request.IDOrderVariant == null)
            {
                return false;
            }

            var orderVariant = await _dbcontext.OrderVariant
                                               .Include(ov => ov.Options)
                                               .ThenInclude(o => o.Variants)
                                               .FirstOrDefaultAsync(ov => ov.ID == request.IDOrderVariant.Value);

            if (orderVariant == null || !orderVariant.HasPurchased)
            {
                return false;
            }

            var variantId = orderVariant.Options.Variants.ID;

            var hasReviewed = _dbcontext.Review
                       .Any(r => r.IDUser == request.IDUser && r.IDOrderVariant == request.IDOrderVariant.Value);

            if (hasReviewed)
            {
                return false;
            }
            var pbj = new Review()
            {
                ID = Guid.NewGuid(),
                IDVariant = variantId,
                IDUser = request.IDUser,
                IDOrderVariant = orderVariant.ID,
                Content = request.Content,
                Rating = request.Rating,
                Status = 1,
                CreateBy = request.CreateBy
            };

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
                        IDVariant = null,
                        IDReview = pbj.ID,
                        Path = imageUrl,
                        Status = 1,
                        CreateBy  = request.CreateBy
                    };

                    await _dbcontext.MediaAssets.AddAsync(imageEntity);
                }
                catch (Exception)
                {
                }
            }
            await _dbcontext.Review.AddRangeAsync(pbj);
            orderVariant.HasReviewed = true;
            await _dbcontext.SaveChangesAsync();
            return true;
        }
        public async Task<List<ReviewVM>> GetAllActiveAsync()
        {
            var reviews = await _dbcontext.Review
                               .Where(u => u.Status == 1)
                               .ProjectTo<ReviewVM>(_mapper.ConfigurationProvider)
                               .ToListAsync();

            var userIds = reviews.Select(r => r.IDUser).Distinct().ToList();

            var userNames = new Dictionary<string, string>();
            foreach (var userId in userIds)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    userNames[userId] = user.UserName;
                }
            }

            var reviewVMs = reviews.Select(review =>
            {
                var reviewVM = _mapper.Map<ReviewVM>(review);
                if (userNames.ContainsKey(review.IDUser))
                {
                    reviewVM.Username = userNames[review.IDUser];
                }
                return reviewVM;
            }).ToList();

            return reviewVMs;
        }
        public async Task<List<ReviewVM>> GetAllAsync()
        {
            var reviews = await _dbcontext.Review
                               .ProjectTo<ReviewVM>(_mapper.ConfigurationProvider)
                               .ToListAsync();

            var userIds = reviews.Select(r => r.IDUser).Distinct().ToList();

            var userNames = new Dictionary<string, string>();
            foreach (var userId in userIds)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    userNames[userId] = user.UserName;
                }
            }

            var reviewVMs = reviews.Select(review =>
            {
                var reviewVM = _mapper.Map<ReviewVM>(review);
                if (userNames.ContainsKey(review.IDUser))
                {
                    reviewVM.Username = userNames[review.IDUser];
                }
                return reviewVM;
            }).ToList();

            return reviewVMs;
        }
        public async Task<ReviewVM> GetByIDAsync(Guid ID)
        {
            var review = await _dbcontext.Review.FindAsync(ID);
            var reviewVM = _mapper.Map<ReviewVM>(review);
            if (review != null)
            {
                var user = await _userManager.FindByIdAsync(review.IDUser.ToString());
                if (user != null)
                {
                    reviewVM.Username = user.UserName;
                }
            }
            return reviewVM;

        }
        public async Task<List<ReviewVM>> GetByVariant(Guid IDVariant)
        {
            var reviews = await _dbcontext.Review
                                  .Where(r => r.IDVariant == IDVariant)
                                  .ProjectTo<ReviewVM>(_mapper.ConfigurationProvider)
                                  .ToListAsync();

            var reviewVMs = _mapper.Map<List<ReviewVM>>(reviews);
            foreach (var reviewVM in reviewVMs)
            {
                var user = await _userManager.FindByIdAsync(reviewVM.IDUser);
                if (user != null)
                {
                    reviewVM.Username = user.UserName;
                }
            }

            return reviewVMs;
        }
        public async Task<ReviewVM> GetByUser(string IDUser)
        {
            var review = await _dbcontext.Review.FirstOrDefaultAsync(r => r.IDUser == IDUser);
            var reviewVM = _mapper.Map<ReviewVM>(review);
            if (review != null)
            {
                var user = await _userManager.FindByIdAsync(IDUser);
                if (user != null)
                {
                    reviewVM.Username = user.UserName;
                }
            }
            return reviewVM;

        }

        public async Task<bool> RemoveAsync(Guid ID, string IDUserdelete)
        {
            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    var obj = await _dbcontext.Review.FirstOrDefaultAsync(c => c.ID == ID);

                    if (obj != null)
                    {
                        obj.Status = 0;
                        obj.DeleteDate = DateTime.Now;
                        obj.DeleteBy = IDUserdelete;

                        _dbcontext.Review.Attach(obj);
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
        public async Task<bool> UpdateAsync(Guid ID, ReviewUpdateVM request)
        {
            var pbj = await _dbcontext.Review.FindAsync(ID);
            if (pbj == null)
            {
                return false;
            }

            pbj.IDUser = request.IDUser;
            // pbj.IDOptions = request.IDOptions;
            pbj.Content = request.Content;
            pbj.Rating = request.Rating;
            pbj.Status = request.Status;

            _dbcontext.Review.Update(pbj);
            await _dbcontext.SaveChangesAsync();
            return true;
        }
    }
}
