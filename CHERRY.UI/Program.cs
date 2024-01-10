using AutoMapper.Extensions.ExpressionMapping;
using CHERRY.BUS.Services._1_Interface;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.UI.Repositorys._1_Interface;
using CHERRY.UI.Repositorys._2_Implement;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Rotativa.AspNetCore;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();
builder.Services.AddTransient<CHERRY_DBCONTEXT>();
builder.Services.AddTransient<IVnPayService, VnPayService>();
builder.Services.AddTransient<IPromotionVariantRepository, PromotionVariantRepository>();
builder.Services.AddTransient<IColorsRepository, ColorsRepository>();
builder.Services.AddTransient<ISizesRepository, SizesRepository>();
builder.Services.AddTransient<IPromotionRepository, PromotionRepository>();
builder.Services.AddTransient<IUserResponse, UserResponse>();
builder.Services.AddTransient<IVoucherRepository, VoucherRepository>();
builder.Services.AddTransient<IOptionsRepository, OptionsRepository>();
builder.Services.AddTransient<IReviewRepository, ReviewRepository>();
builder.Services.AddTransient<ICategoriesVariantsRepository, CategoriesVariantsRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IMaterialRepository, MaterialRepository>();
builder.Services.AddTransient<IBrandRepository, BrandRepository>();
builder.Services.AddTransient<IOrderVariantRepository, OrderVariantRepository>();
builder.Services.AddTransient<IVariantRepository, VariantRepository>();
builder.Services.AddTransient<ICategoryRespository, CategoryRespository>();
builder.Services.AddTransient<IMediaAssetsRepository, MediaAssetsRepository>();
builder.Services.AddTransient<ICartRepository, CartRepository>();
builder.Services.AddTransient<ICartProductVariantsRepository, CartProductVariantsRepository>();
builder.Services.AddMemoryCache();
builder.Services.AddSignalR();

var cloudinaryAccount = new Account("dqzks8gjg", "462712979292774", "DMkKRmICNqaz_5TFD0e1VupM7mA");
var cloudinary = new Cloudinary(cloudinaryAccount);
builder.Services.AddSingleton(cloudinary);
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddHttpClient("examapi",
    client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["BackEndAPIURL"]);
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", Role => Role.RequireRole("Admin"));
    options.AddPolicy("User", Role => Role.RequireRole("User"));
});
builder.Services.AddScoped(api => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration["BackEndAPIURL"])
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
        };
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
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();
RotativaConfiguration.Setup(app.Environment.ContentRootPath, @"C:\Program Files\wkhtmltopdf\bin");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

// Trong phương thức Configure
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});
app.Run();
