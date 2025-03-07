using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Data.Contracts;
using SanjeshP.RDC.Data.Repositories;
using SanjeshP.RDC.WebFramework.Configuration;
using WebFramework.Configuration;
using WebFramework.CustomMapping;
using WebFramework.Middlewares;

namespace SanjeshP.RDC.Web
{
    public class Startup
    {
        private readonly SiteSettings _siteSetting;
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _siteSetting = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.Configure<SiteSettings>(Configuration.GetSection(nameof(SiteSettings)));
            services.InitializeAutoMapper();
            services.AddDbContext(Configuration);
            services.AddCustomIdentity(_siteSetting.IdentitySettings);
            services.AddMinimalMvc();
            services.AddElmahCore(Configuration, _siteSetting);
            //services.AddJwtAuthentication(_siteSetting.JwtSettings);
            services.AddControllers();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddCustomAuthentication();

            //services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = "localhost:6379"; // آدرس Redis Server
            //    options.InstanceName = "MyAppCache_";
            //});

            #region Comment
            //services.AddCustomElmah(Configuration, _siteSetting);
            //services.AddCustomApiVersioning();
            //services.AddSwagger();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SanjeshP.RDC", Version = "v1" });
            //    c.SwaggerDoc("v2", new OpenApiInfo { Title = "SanjeshP.RDC", Version = "v2" });
            //});
            //services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(
            //        builder =>
            //        {
            //            builder.WithOrigins("http://localhost:10053", "http://localhost:4200")
            //                                .AllowAnyHeader()
            //                                .AllowAnyMethod();
            //        });
            //});
            #endregion
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //Register Services to Autofac ContainerBuilder
            builder.AddServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.InitializeDatabase();
            app.UseCustomExceptionHandler();
            app.UseHsts(env);
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication(); // اول احراز هویت
            app.UseAuthorization(); // سپس مجوزها

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "redirect",
                    pattern: "",
                    defaults: new { controller = "Account", action = "Login" });

                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                    name: "Test",
                    areaName: "Test",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });

        }
    }
}

