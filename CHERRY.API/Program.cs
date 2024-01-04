using AutoMapper.Extensions.ExpressionMapping;
using CHERRY.BUS.Services._1_Interface;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.Services._2_Implements;
using CHERRY.BUS.ViewModels;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.DAL.Entities;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplication();
//Đăng ký DI
builder.Services.AddTransient<CHERRY_DBCONTEXT>();
builder.Services.AddTransient<CHERRY_IdentityDbContext>();
builder.Services.AddTransient<ISizesService, SizesService>();
builder.Services.AddTransient<IColorsService, ColorsService>();
builder.Services.AddTransient<IEmailSender, SendMailService>();
builder.Services.AddTransient<IPromotionVariantService, PromotionVariantService>();
builder.Services.AddTransient<IOrderVariantService, OrderVariantService>();
builder.Services.AddTransient<IPromotionService, PromotionService>();
builder.Services.AddTransient<IBrandServices, BrandServices>();
builder.Services.AddTransient<IMaterialServices, MaterialServices>();
builder.Services.AddTransient<IReviewService, ReviewService>();
builder.Services.AddTransient<IVoucherHistoryService, VoucherHistoryService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IVariantsService, VariantsService>();
builder.Services.AddTransient<IVoucherUserService, VoucherUserService>();
builder.Services.AddTransient<IVoucherService, VoucherService>();
builder.Services.AddTransient<ILogin_Service, Login_Service>();
builder.Services.AddTransient<IOptionsService, OptionsService>();
builder.Services.AddTransient<IMediaAssetsService, MediaAssetsService>();
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ICategoriesVariantsService, CategoriesVariantsService>();
builder.Services.AddTransient<IRegisterServices, RegisterServices>();
builder.Services.AddTransient<ICartProductVariantsService, CartProductVariantsService>();
builder.Services.AddMemoryCache();
var cloudinaryAccount = new Account("dqzks8gjg", "462712979292774", "DMkKRmICNqaz_5TFD0e1VupM7mA");
var cloudinary = new Cloudinary(cloudinaryAccount);
builder.Services.AddSingleton(cloudinary);

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<CHERRY_IdentityDbContext>()
    .AddDefaultTokenProviders();
var mailsetting = builder.Configuration.GetSection("MailSettings");
builder.Services.Configure<MailSettings>(mailsetting);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
        };
    });
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddAutoMapper(config => { config.AddExpressionMapping(); }, Assembly.GetExecutingAssembly(),
    Assembly.GetEntryAssembly());
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();