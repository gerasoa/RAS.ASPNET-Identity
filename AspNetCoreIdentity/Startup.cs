using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreIdentity.Areas.Identity.Data;
using AspNetCoreIdentity.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreIdentity
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //Add EF to Asp.Net Context  
            services.AddDbContext<AspNetCoreIdentityContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("AspNetCoreIdentityContextConnection")));

            //Configuring the Identity on the application            
            services.AddDefaultIdentity<IdentityUser>() 
                .AddRoles<IdentityRole>() //This key suport is apply when we implement Authorization
                .AddDefaultUI(Microsoft.AspNetCore.Identity.UI.UIFramework.Bootstrap4)    
                .AddEntityFrameworkStores<AspNetCoreIdentityContext>();

            //Configuring the Claims
            services.AddAuthorization(options =>
            {
                //Handler by ASP.Net
                options.AddPolicy("AuthorizedDelete", policy => policy.RequireClaim("AuthorizedDelete"));

                //When I create a Requiriment (NecessessaryPermission) this is my responsability create a Handle
                options.AddPolicy("Write", policy => policy.Requirements.Add(new NecessessaryPermission("Write"))); //Claims 
                options.AddPolicy("Read", policy => policy.Requirements.Add(new NecessessaryPermission("Read"))); //Claims
            });


            //Register the class by Interface
            services.AddSingleton<IAuthorizationHandler, NecessessaryPermissionHandler>(); //Claims

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            

            //Identity configured in the application
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
