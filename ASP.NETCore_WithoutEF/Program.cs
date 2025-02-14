using ASP.NETCore_WithoutEF.Models;
using ASP.NETCore_WithoutEF.Repository.Contract;
using ASP.NETCore_WithoutEF.Repository.Implementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add references of repositories
builder.Services.AddScoped<IGenericRepository<Category>, CategoryRepository>();
builder.Services.AddScoped<IGenericRepository<Product>, ProductRepository>();
builder.Services.AddScoped<IGenericRepository<Company>, CompanyRepository>();
builder.Services.AddScoped<IGenericRepository<ShoppingCart>, ShoppingCartRepository>();
builder.Services.AddScoped<IGenericRepository<OrderDetail>, OrderDetailRepository>();
builder.Services.AddScoped<IGenericRepository<OrderHeader>, OrderHeaderRepository>();
builder.Services.AddScoped<IGenericRepository<ApplicationUser>, ApplicationUserRepository>();

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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
