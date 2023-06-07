using GetSit.Data;
using GetSit.Data.Services;
using GetSit.Data.enums;
using GetSit.Data.Security;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

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
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IUserManager, UserManager>();
/*Model Services*/
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IBookingHall_Service, BookingHall_Service>();
builder.Services.AddScoped<IBookingHallService_Service, BookingHallService_Service>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IFacultyService, FacultyService>();
builder.Services.AddScoped<IFavoriteHallService, FavoriteHallService>();
builder.Services.AddScoped<IHallFacilityService, HallFacilityService>();
builder.Services.AddScoped<IHallPhotoService, HallPhotoService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentCardService, PaymentCardService>();
builder.Services.AddScoped<IPaymentDetailService, PaymentDetailService>();
builder.Services.AddScoped<IServicePhotoService, ServicePhotoService>();
builder.Services.AddScoped<ISpaceService, Space_Service>();
builder.Services.AddScoped<ISpaceEmployeeService, SpaceEmployeeService>();
builder.Services.AddScoped<ISpaceHallService, SpaceHallService>();
builder.Services.AddScoped<ISpacePhoneService, SpacePhoneService>();
builder.Services.AddScoped<ISpacePhotoService, SpacePhotoService>();
builder.Services.AddScoped<ISpaceService_Service, SpaceService_Service>();
builder.Services.AddScoped<ISpaceWorkingDayService, SpaceWorkingDayService>();
builder.Services.AddScoped<ISystemAdminService, SystemAdminService>();
builder.Services.AddScoped<ITitleService, TitleService>();

var app = builder.Build();
AppDbInitializer.Seed(app);

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
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "notfound",
        pattern: "/Error/NotFound",
        defaults: new { controller = "Error", action = "NotFoundPage" });

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Explore}/{action=Index}/{id?}");
});

app.Run();
