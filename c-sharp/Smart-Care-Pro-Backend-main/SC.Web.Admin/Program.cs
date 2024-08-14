using Infrastructure;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Set the golbal date format dd/mm/yyyy.
CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-GB");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.CurrentCulture = cultureInfo;

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBLocation")));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(240); });
builder.Services.AddAuthentication("Cookies")
                   .AddCookie("Cookies", config =>
                   {
                       config.Cookie.Name = "__SCinfo__";
                       config.LoginPath = "/";
                       config.SlidingExpiration = true;
                   });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();