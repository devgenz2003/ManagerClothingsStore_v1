using CHERRY.BUS.ViewModels.User;
using CHERRY.BUS.ViewModels;
using CHERRY.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using CHERRY.BUS.Services._1_Interfaces;
using Microsoft.EntityFrameworkCore;
using CHERRY.DAL.ApplicationDBContext;

namespace CHERRY.BUS.Services._2_Implements
{
    public class RegisterServices : IRegisterServices
    {
        private readonly CHERRY_DBCONTEXT _dbContext;

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RegisterServices(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, CHERRY_DBCONTEXT CHERRY_DBCONTEXT)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = CHERRY_DBCONTEXT;

        }

        public async Task<Response> RegisterAsync(RegisterUser registerUser, string role)
        {
            if (await _userManager.FindByEmailAsync(registerUser.Email) != null)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "This email is already exists!"
                };
            }
            else if (await _userManager.FindByNameAsync(registerUser.Username) != null)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "This username is already exists!"
                };
            }

            if (registerUser.Password != registerUser.ConfirmPassword)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "This password doesn't match with confirm password!"
                };
            }

            User NewUser = new()
            {
                UserName = registerUser.Username,
                Gender = registerUser.Gender,
                Email = registerUser.Email,
                FirstName = registerUser.FirstName,
                MiddleName = registerUser.MiddleName,
                SurName = registerUser.SurName,
                PhoneNumber = registerUser.PhoneNumber,
                Status = 1,
            };
            var memberRank = await _dbContext.MemberRank.FirstOrDefaultAsync(r => r.MemberName == "Base");
            if (memberRank == null)
            {
                memberRank = new MemberRank
                {
                    ID = Guid.NewGuid(),
                    MemberName = "Base",
                    Description = "Base",
                    Rank = Member_Rank.Base,
                    Status = 1,
                    CreateBy = "",
                    CreateDate = DateTime.Now,
                };
                _dbContext.MemberRank.Add(memberRank);
                await _dbContext.SaveChangesAsync();
            }

            NewUser.ID_MemberRank = memberRank.ID;
            if (await _roleManager.RoleExistsAsync(role))
            {
                var result = await _userManager.CreateAsync(NewUser, registerUser.Password);

                if (!result.Succeeded)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Message = "Register failed, something went wrong!"
                    };
                }
                var cart = new Cart
                {
                    ID = Guid.NewGuid(),
                    ID_User = NewUser.Id,
                    Status = 1,
                    CreateDate = DateTime.Now,
                    CreateBy = ""
                };
                _dbContext.Cart.Add(cart);
                await _dbContext.SaveChangesAsync();
                await _userManager.AddToRoleAsync(NewUser, role);
                return new Response
                {
                    IsSuccess = true,
                    StatusCode = 201,
                    Message = "Register successfully!"
                };
            }
            else
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "This role doesn't exists!"
                };
            }
        }
    }
}
