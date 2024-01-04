using AutoMapper;
using AutoMapper.QueryableExtensions;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.User;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CHERRY.BUS.Services._2_Implements
{
    public partial class UserService : IUserService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly CHERRY_DBCONTEXT _dbcontext;
        private readonly IMapper _mapper;
        public UserService(CHERRY_DBCONTEXT dbcontext, IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbcontext = dbcontext;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> ChangePasswordAsync(string IDUser, ChangePasswordViewModel model)
        {
            var user = await _userManager.FindByIdAsync(IDUser);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            return result.Succeeded;
        }
        public async Task<bool> ChangeUserRoleAsync(string IDUser, string newRole)
        {
            var user = await _userManager.FindByIdAsync(IDUser);
            if (user == null)
            {
                return false; 
            }

            var existingRole = await _roleManager.FindByNameAsync(newRole);
            if (existingRole == null)
            {
                return false; 
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, userRoles.ToArray());
            if (!result.Succeeded)
            {
                return false; 
            }

            result = await _userManager.AddToRoleAsync(user, newRole);
            if (!result.Succeeded)
            {
                return false; 
            }

            return true;
        }
        public async Task<bool> CreateAsync(UserCreateVM request)
        {
            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    if (await _userManager.FindByNameAsync(request.UserName) != null ||
                        await _userManager.FindByEmailAsync(request.Gmail) != null)
                    {
                        transaction.Rollback();
                        return false;
                    }

                    var memberrank = await _dbcontext.MemberRank.FirstOrDefaultAsync(r => r.MemberName == "Base");
                    if (memberrank == null)
                    {
                        memberrank = new MemberRank
                        {
                            ID = Guid.NewGuid(),
                            MemberName = "Base",
                            Description = "Base",
                            Rank = Member_Rank.Base,
                            Status = 1,
                            CreateBy = request.CreateBy,
                            CreateDate = DateTime.Now,
                        };

                        _dbcontext.MemberRank.Add(memberrank);
                    }

                    string fileName = null;
                    if (request.ImagePath != null)
                    {
                        var imagePath = Path.Combine("wwwroot", "UserImages");
                        Directory.CreateDirectory(imagePath);
                        fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.ImagePath.FileName)}";
                        var fullPath = Path.Combine(imagePath, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await request.ImagePath.CopyToAsync(stream);
                        }
                    }
                    var user = new User
                    {
                        ID_MemberRank = memberrank.ID,
                        UserName = request.UserName,
                        Email = request.Gmail,
                        PhoneNumber = request.PhoneNumber,
                        FirstName = request.FirstName,
                        MiddleName = request.MiddleName,
                        SurName = request.SurName,
                        DateOfBirth = request.DateOfBirth,
                        Gender = request.Gender,
                        Status = 1,
                        CreateDate = DateTime.Now,
                    };

                    var result = await _userManager.CreateAsync(user, request.Password);

                    if (!result.Succeeded)
                    {
                        transaction.Rollback();
                        return false;
                    }

                    var cart = new Cart
                    {
                        ID = Guid.NewGuid(),
                        ID_User = user.Id,
                        Status = 1,
                        CreateDate = DateTime.Now,
                        CreateBy = ""
                    };
                    _dbcontext.Cart.Add(cart);
                    await _dbcontext.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public async Task<List<UserVM>> GetAllActiveAsync()
        {
            var list = await _userManager.Users
                .Where(u => u.Status == 1) 
                .ProjectTo<UserVM>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return list;
        }
        public async Task<List<UserVM>> GetAllAsync()
        {
            var users = await _userManager.Users
             .ProjectTo<UserVM>(_mapper.ConfigurationProvider)
             .ToListAsync();
            return users;
        }
        public async Task<UserVM> GetByGmailAsync(string Gmail)
        {
            var user = await _userManager.FindByEmailAsync(Gmail);
            return _mapper.Map<UserVM>(user);
        }
        public async Task<UserVM> GetByIDAsync(string ID)
        {
            var user = await _userManager.FindByIdAsync(ID);

            if (user != null)
            {
                var userVM = _mapper.Map<UserVM>(user);
                return userVM;
            }
            return null;
        }
        public async Task<bool> RemoveAsync(string ID, Guid IDUserdelete)
        {
            var user = await _userManager.FindByIdAsync(ID);
            if (user != null)
            {
                user.Status = 0;
                await _userManager.UpdateAsync(user);
                return true;
            }
            return false;
        }
        public async Task<bool> UpdateAsync(string ID, UserUpdateVM request)
        {
            var user = await _userManager.FindByIdAsync(ID);
            if (user != null)
            {
                user.FirstName = request.FirstName;
                user.MiddleName = request.MiddleName;
                user.SurName = request.SurName;
                user.Email = request.Gmail;
                user.DateOfBirth = request.DateOfBirth;
                user.PhoneNumber = request.PhoneNumber;
                user.Status = request.Status;

                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            return false;
        }
    }
}
