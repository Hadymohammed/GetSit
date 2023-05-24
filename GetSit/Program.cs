using GetSit.Data;
using GetSit.Data.enums;
using GetSit.Data.Security;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
var DBconnection= builder.Configuration["DBconnection"];
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDBcontext>(options => options.UseSqlServer(
       DBconnection
        ));
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies",config =>
    {
        config.Cookie.Name = "GetSit_Auth";
        config.LoginPath = "/Account/Login";
        config.AccessDeniedPath = "/Account/AccessDenied";

    });
builder.Services.AddAuthorization(config =>
{
    config.AddPolicy("CustomerPolicy", policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser();
        policyBuilder.RequireRole(new string[] { UserRole.Customer.ToString()});
        policyBuilder.RequireClaim(ClaimTypes.Role);
        policyBuilder.RequireClaim(ClaimTypes.NameIdentifier);
    });
});
  builder.Services.AddScoped<IUserManager, UserManager>();

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
app.UseAuthentication();
app.UseAuthorization();
AppDbInitializer.Seed(app);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
