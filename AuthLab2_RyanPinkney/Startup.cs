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
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;
using System.IO;

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
                options.MaxAge = TimeSpan.FromDays(90);
                options.IncludeSubDomains = true;
                options.Preload = true;
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status301MovedPermanently;
                options.HttpsPort = 443;
            });


        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                // HSTS should only be enabled on production, not on localhost
                app.UseHsts();
            }

            // Add other security headers
            app.UseMiddleware<SecurityHeadersMiddleware>();

            // Redirect http to https
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

    public sealed class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            context.Response.Headers.Add("strict-transport-security", new StringValues("max-age=31536000"));

            context.Response.Headers.Add("referrer-policy", new StringValues("strict-origin-when-cross-origin"));

            context.Response.Headers.Add("x-content-type-options", new StringValues("nosniff"));

            context.Response.Headers.Add("x-frame-options", new StringValues("DENY"));

            context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", new StringValues("none"));

            context.Response.Headers.Add("x-xss-protection", new StringValues("1; mode=block"));

            context.Response.Headers.Add("Expect-CT", new StringValues("max-age=0, enforce, report-uri=\"https://example.report-uri.com/r/d/ct/enforce\""));

            context.Response.Headers.Add("Feature-Policy", new StringValues(
                "accelerometer 'none';" +
                "ambient-light-sensor 'none';" +
                "autoplay 'none';" +
                "battery 'none';" +
                "camera 'none';" +
                "display-capture 'none';" +
                "document-domain 'none';" +
                "encrypted-media 'none';" +
                "execution-while-not-rendered 'none';" +
                "execution-while-out-of-viewport 'none';" +
                "gyroscope 'none';" +
                "magnetometer 'none';" +
                "microphone 'none';" +
                "midi 'none';" +
                "navigation-override 'none';" +
                "payment 'none';" +
                "picture-in-picture 'none';" +
                "publickey-credentials-get 'none';" +
                "sync-xhr 'none';" +
                "usb 'none';" +
                "wake-lock 'none';" +
                "xr-spatial-tracking 'none';"
                ));

            return _next(context);
        }

    }
}
