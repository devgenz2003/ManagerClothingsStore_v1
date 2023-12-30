using AutoMapper;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.User;
using CHERRY.BUS.ViewModels;
using CHERRY.DAL.ApplicationDBContext;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using CHERRY.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace CHERRY.BUS.Services._2_Implements
{
    public class Login_Service : ILogin_Service
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly CHERRY_DBCONTEXT _dbcontext;
        private readonly IMapper _mapper;
        public Login_Service(CHERRY_DBCONTEXT CHERRY_DBCONTEXT,IConfiguration configuration, IMapper mapper, UserManager<User> userManager)
        {
            _dbcontext = CHERRY_DBCONTEXT;
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Response> Login(UserLoginModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.UserName) || string.IsNullOrWhiteSpace(model.PassWord))
            {
                return new Response { IsSuccess = false, StatusCode = 400, Message = "Username and password must be provided." };
            }

            try
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null || !(await _userManager.CheckPasswordAsync(user, model.PassWord)))
                {
                    return new Response { IsSuccess = false, StatusCode = 401, Message = "Invalid credentials." };
                }

                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

                authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    expires: DateTime.Now.AddMinutes(double.Parse(_configuration["JWT:DurationInMinutes"])),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha512Signature)
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                var roles = await _userManager.GetRolesAsync(user);

                return new Response
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "Authentication successful.",
                    Token = tokenString,
                    Roles = roles.ToList() 
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in Login: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return new Response { IsSuccess = false, StatusCode = 500, Message = "Internal server error." };
            }
        }

    }
}