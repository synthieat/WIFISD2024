using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SD.Application.Movies;
using SD.Persistence.Repositories.DBContext;
using SD.Web.Data;
using SD.Persistence.Extensions;
using SD.Application.Extensions;
using System.Configuration;
using System.Reflection;
using SD.Rescources.Attributes;
using SD.Rescources;
using System.Globalization;
using Microsoft.Extensions.Options;

namespace SD.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());

            // Add services to the container.
            var identityDbConnectionString = builder.Configuration.GetConnectionString("IdentityDbConnection") ?? throw new InvalidOperationException("Connection string 'IdentityDbConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(identityDbConnectionString));

            var movieDbConnectionString = builder.Configuration.GetConnectionString("MovieDbContext");
            builder.Services.AddDbContext<MovieDbContext>(options => options.UseSqlServer(movieDbConnectionString));
            
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                            .AddEntityFrameworkStores<ApplicationDbContext>();
            
            /* Bootstrapping von Handler und Repositories */
            builder.Services.RegisterRepositories();
            builder.Services.RegisterApplicationService();

            /* MediatR registrieren */
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(MovieQueryHandler).GetTypeInfo().Assembly));


            /* Browser-Sprach-Erkennung implementieren */
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultuers = new List<CultureInfo>
                {
                    new CultureInfo("de"),
                    new CultureInfo("de-AT"),
                    new CultureInfo("de-DE"),
                    new CultureInfo("de-CH"),
                    new CultureInfo("en")
                };

                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("de");
                options.SupportedCultures = supportedCultuers;
                options.SupportedUICultures = supportedCultuers;
            });



            builder.Services.AddRazorPages()
                            .AddRazorRuntimeCompilation();

            /* Verwendung von Sessions konfigurieren */
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

            });

                        
            builder.Services.AddControllersWithViews();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            /* Browser-Sprach-Erkennung aktivieren */
            var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            /* Damit Sessions genutzt werden k�nnen */
            app.UseSession();

            app.UseRouting();
                        
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Movies}/{action=Index}/{id?}");

            app.MapRazorPages();

            LocalizedDescriptionAttribute.Setup(new System.Resources.ResourceManager(typeof(BasicRes)));

            app.Run();
        }
    }
}
