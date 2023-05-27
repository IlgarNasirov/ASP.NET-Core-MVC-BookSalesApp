using BookSalesApp.IRepositories;
using BookSalesApp.IServices;
using BookSalesApp.Models;
using BookSalesApp.Repositories;
using BookSalesApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IGetUserService, GetUserService>();
builder.Services.AddDbContext<BookSalesDbContext>();
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    x =>
    {
        x.LoginPath = "/User/Login";
        x.AccessDeniedPath = "/User/Login";
    }
);
builder.Services.AddMvc(config =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});
var app = builder.Build();
app.UseStatusCodePagesWithReExecute("/Error/Error404", "?code={0}");
app.UseExceptionHandler("/Error/Error500");
app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{Controller=User}/{Action=Index}");
app.UseAuthentication();
app.UseAuthorization();
app.Run();