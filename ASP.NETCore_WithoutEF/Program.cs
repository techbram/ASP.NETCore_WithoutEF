using Microsoft.AspNetCore.Http;
using ASP.NETCore_WithoutEF.App.Models;
using ASP.NETCore_WithoutEF.App.DataAccess.Repository.Contract;
using ASP.NETCore_WithoutEF.App.DataAccess.Repository.Implementation;
using ASP.NETCore_WithoutEF.App.Utility;
using Stripe;
using App.DataAccess.Repository.Contract;
using App.Models;
using App.DataAccess.Repository.Implementation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

//Add Session service
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
//})
//.AddCookie()
//.AddFacebook(options =>
//{
//    options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
//    options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];    
//    options.CallbackPath = "/signin-facebook"; // default callback
//    options.Fields.Add("email");
//    options.Fields.Add("name");
//    options.SaveTokens = true;
//});

// Add references of repositories
builder.Services.AddScoped<ICategoryRepository<Category>, CategoryRepository>();
builder.Services.AddScoped<IProductRepository<ASP.NETCore_WithoutEF.App.Models.Product>, ProductRepository>();
builder.Services.AddScoped<ICompanyRepository<Company>, CompanyRepository>();
builder.Services.AddScoped<IShoppingCartRepository<ShoppingCart>, ShoppingCartRepository>();
builder.Services.AddScoped<IOrderDetailRepository<OrderDetail>, OrderDetailRepository>();
builder.Services.AddScoped<IOrderHeaderRepository<OrderHeader>, OrderHeaderRepository>();
builder.Services.AddScoped<IApplicationUserRepository<ApplicationUser>, ApplicationUserRepository>();
builder.Services.AddScoped<IApplicationUserRepository<ApplicationUser>, ApplicationUserRepository>();
builder.Services.AddScoped<IShoppingCartRepository<ShoppingCart>, ShoppingCartRepository>();
builder.Services.AddScoped<IOrderHeaderRepository<OrderHeader>, OrderHeaderRepository>();
builder.Services.AddScoped<IRoleRepository<Role>, RoleRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
