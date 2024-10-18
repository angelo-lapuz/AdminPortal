using Microsoft.AspNetCore.Identity;
using System.Net.Http.Headers;
using System.Net.Mime;
using WebAPI.Data;
using AdminPortal.Models;
using AdminPortal.Utilities;
using Microsoft.EntityFrameworkCore;

namespace AdminPortal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Configure API client
            builder.Services.AddHttpClient("api", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5164");
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            });

            // Store session into Web-Server memory
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
            });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<Tools>();

            // Configure the database context
            builder.Services.AddDbContext<PeakHubContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Identity services for user authentication
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<PeakHubContext>()
            .AddDefaultTokenProviders();

            // Add distributed memory cache (to store sessions)
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddScoped<UserManager<User>>();
            builder.Services.AddScoped<SignInManager<User>>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                //pattern: "{controller=Home}/{action=Index}/{id?}");
                pattern: "{controller=Login}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
