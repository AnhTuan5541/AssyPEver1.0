using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WaferMapViewer.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddDbContext<WaferMapViewerContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "eSign",
            ValidAudience = "https://localhost:44309/",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Amkor_Technology_Vietnam_is_the_best_company_in_Viet_Nam"))
        };
    });

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

app.MapControllers();

app.MapControllerRoute(
    name: "WaferMapViewer",
    pattern: "wafer-map-viewer",
    defaults: new { controller = "Home", action = "WaferMapViewer" });
app.MapControllerRoute(
    name: "AddUser",
    pattern: "add-user",
    defaults: new { controller = "Home", action = "AddUser" });

app.MapControllerRoute(
    name: "LCRCreate",
    pattern: "lcr-create",
    defaults: new { controller = "LCR", action = "Index" });

app.MapControllerRoute(
    name: "LCRDetail",
    pattern: "lcr-detail",
    defaults: new { controller = "LCR", action = "RequestDetail" });

app.MapControllerRoute(
    name: "LCRConfigMap",
    pattern: "lcr-configMap",
    defaults: new { controller = "LCR", action = "ConfigMap" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public class ConnectionStrings()
{
    public string DefaultConnection { get; set; }
}
