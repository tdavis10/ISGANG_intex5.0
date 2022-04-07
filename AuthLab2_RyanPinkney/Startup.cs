using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using AuthLab2_RyanPinkney.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AuthLab2_RyanPinkney.Models;
using Microsoft.JSInterop;
using Microsoft.ML.OnnxRuntime;

namespace AuthLab2_RyanPinkney
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();


            // connect to database
            services.AddDbContext<AccidentDbContext>(options =>
            {
                options.UseMySql(Configuration["connectionString:RDSConnection"]); // Change to RDSConnection for RDS
                // Change to CrashDbConnection for localhost
            });

            // initialize repository method
            services.AddScoped<ICrashRepository, EFCrashRepository>();

            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 12;
                options.Password.RequiredUniqueChars = 1;
            });

            //services.AddSingleton<InferenceSession>(
            //      new InferenceSession("wwwroot/crash_final2.onnx")
            //    );



            //services.AddAuthentication().AddGoogle(options =>
            //     {
            //         IConfigurationSection googleAuthNSection =
            //         Configuration.GetSection("Authentication:Google");
            //         options.ClientId = googleAuthNSection["ClientId"];
            //         options.ClientSecret = googleAuthNSection["ClientSecret"];
            //     });


            // Add this to add razor pages
            services.AddRazorPages();


            // Blazor Services
            services.AddServerSideBlazor();

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

            }

            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Use(async (ctx, next) =>
            {
                ctx.Response.Headers.Add("Content-Security-Policy",
                //"default-src * 'unsafe-inline' 'unsafe-eval'; script-src * 'unsafe-inline' 'unsafe-eval'; connect-src * 'unsafe-inline'; img-src * data: blob: 'unsafe-inline'; frame-src *; style-src * 'unsafe-inline';");
                "default-src 'self' https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js http://www.w3.org/2000/svg https://cdn.jsdelivr.net/npm/cookieconsent@3/build/cookieconsent.min.js https://cdn.jsdelivr.net/npm/cookieconsent@3/build/cookieconsent.min.css https://cdn.jsdelivr.net/npm/bootstrap-icons@1.5.0/font/bootstrap-icons.css https://cdn.jsdelivr.net/npm/bootstrap-icons@1.5.0/font/fonts/bootstrap-icons.woff?856008caa5eb66df68595e734e59580d https://cdn.jsdelivr.net/npm/bootstrap-icons@1.5.0/font/fonts/bootstrap-icons.woff2?856008caa5eb66df68595e734e59580d");
                await next();
            });

            app.UseEndpoints(endpoints =>
            {


                // This one first
                endpoints.MapControllerRoute(
                    name: "typePage",
                    pattern: "{cityNames}/{iPageNum}",
                    defaults: new { Controller = "Home", action = "Summary" });

                // This one first
                endpoints.MapControllerRoute(
                    name: "Paging",
                    pattern: "Page{iPageNum}",
                    defaults: new { Controller = "Home", action = "Summary", iPageNum = 1 });


                // This one first
                endpoints.MapControllerRoute(
                    name: "type",
                    pattern: "{cityNames}",
                    defaults: new { Controller = "Home", action = "Summary", iPageNum = 1 });


                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

                // Blazor endpoints
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/admin/{*catchall}", "/Admin/Index");

                // Add this for Razor pages
                endpoints.MapRazorPages();
            });

        }
    }
}
