using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServerHost.Quickstart.UI;
using Microsoft.EntityFrameworkCore;
using DoctorHouse.DAL;
using Microsoft.Extensions.Configuration;
using DoctorHouse.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using IdentityServer4.Services;
using System.Reflection;

namespace DoctorHouse.IdentityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // uncomment, if you want to add an MVC-based UI
            services.AddControllersWithViews();

            services.AddDbContext<EFContext>(options =>
              options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<DbUser, DbRole>(options =>
                    options.Stores.MaxLengthForKeys = 128)
                .AddEntityFrameworkStores<EFContext>()
                .AddDefaultTokenProviders();


            //JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            //services.Configure<SecurityStampValidatorOptions>(options =>
            //{
            //    options.ValidationInterval = TimeSpan.Zero;
            //});
            var migrationAssembly = typeof(EFContext).GetTypeInfo().Assembly.GetName().Name;
            // configure identity server with in-memory stores, keys, clients and resources
            var builder = services.AddIdentityServer()
                 //.AddConfigurationStore(options =>
                 //{
                 //    options.ConfigureDbContext = b => b.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
                 //    sql => sql.MigrationsAssembly(migrationAssembly));
                 //})
                 //.AddOperationalStore(options => 
                 //{
                 //    options.ConfigureDbContext = b => b.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
                 //    sql => sql.MigrationsAssembly(migrationAssembly));
                 //})

                 .AddInMemoryClients(Config.Clients)
                .AddInMemoryApiResources(Config.ApiResources)
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes) // IdentityServer4 version 4.x.x changes

                //.AddInMemoryClients(ConfigGlobal.Clients)
                //.AddTestUsers(TestUsers.Users);
                .AddAspNetIdentity<DbUser>()
                .AddProfileService<ProfileService>();

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()));
            //builder.Services.AddTransient<IProfileService, ProfileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // uncomment if you want to add MVC
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors("AllowAll");
            app.UseIdentityServer();

            // uncomment, if you want to add MVC
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
            DatabaseInitializer.Init(app.ApplicationServices);
        }
    }
}


