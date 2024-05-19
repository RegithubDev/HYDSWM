using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using COMMON.SWMENTITY;
using HYDSWMAPI.Middleware;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Rewrite;
using DEMOSWMCKC;
using COMMON.ASSET;
using COMMON;
using DEMOSWMCKC.Middleware;

namespace HYDSWMAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            StaticConfig = configuration;
        }
        public static IConfiguration StaticConfig { get; private set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddCors();
            services.AddCors(option => option.AddPolicy("MyBlogPolicy", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

            }));
            //services.AddBrowserDetection();
            services.AddDistributedMemoryCache();
            services.AddOutputCaching();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);

            });
            services.AddScoped<TReport>();
            services.AddScoped<PdfReport>();
            // services.AddSingleton<RouteValueTransformer>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            //{
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.SecurePolicy = 1 == 1
            //      ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
            //    options.Cookie.SameSite = SameSiteMode.None;
            //    // options.Cookie.Path = "/abc";
            //});
            services.AddControllersWithViews().AddSessionStateTempDataProvider()
       .AddNewtonsoftJson(options =>
       {

           options.UseMemberCasing();
       }).AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
                app.UseRewriter(new RewriteOptions().AddRedirectToHttps(StatusCodes.Status301MovedPermanently, 443));
            }
            app.UseHttpsRedirection();
            //DefaultFilesOptions options = new DefaultFilesOptions();
            //options.DefaultFileNames.Clear();
            //options.DefaultFileNames.Add("Ward.kml");
            //app.UseDefaultFiles(options);
            // Set up custom content types - associating file extension to MIME type
            var provider = new FileExtensionContentTypeProvider();
            // Add new mappings
            provider.Mappings[".kml"] = "application/vnd.google-earth.kml+xml";
            app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = provider });
            app.UseOutputCaching();
            app.UseAntiXssMiddleware();
            app.UseSession();
            app.UseRouting();
            app.UseCors("MyBlogPolicy");
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<RateLimitMiddlware>();

            //app.UseHttpsRedirection();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=operation}/{action=OperationIndex}/{id?}");

                endpoints.MapControllerRoute(
                  name: "Unauthorized",
                  pattern: "{controller=Account}/{action=AccessDenied}/{id?}");
                //  endpoints.MapDynamicControllerRoute<RouteValueTransformer>("/");
                //endpoints.MapControllerRoute(
                //    name: "Employee",
                //    pattern: "{controller=Employee}/{action=Index}/{id?}");
            });

        }
    }
}
