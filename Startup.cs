using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using PoliticsRisk.Data;
using static PoliticsRisk.MessageHub;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PoliticsRisk
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
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            
            services.AddAuthentication()
                // .AddMicrosoftAccount(microsoftOptions => { 
                //         IConfigurationSection microsoftAuthNSection = Configuration.GetSection("Authentication:Microsoft");
                //         microsoftOptions.ClientId = microsoftAuthNSection["Authentication:Microsoft:ClientId"];
                //         microsoftOptions.ClientSecret = microsoftAuthNSection["Authentication:MicrosoftClientSecret"];
                //     })
                .AddGoogle(googleOptions => { 
                        IConfigurationSection googleAuthNSection = Configuration.GetSection("Authentication:Google");
                        googleOptions.ClientId = googleAuthNSection["ClientId"];
                        googleOptions.ClientSecret = googleAuthNSection["ClientSecret"]; 
                    });
                // .AddFacebook(facebookOptions => { 
                //         IConfigurationSection facebookAuthNSection = Configuration.GetSection("Authentication:Facebook");
                //         facebookOptions.ClientId = facebookAuthNSection["Authentication:Facebook:ClientId"];
                //         facebookOptions.ClientSecret = facebookAuthNSection["Authentication:Facebook:ClientSecret"];
                //     });
            services.AddAuthorization();
            services.AddMemoryCache();
            services.AddSession();

            
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSignalR();
        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            //     app.UseDatabaseErrorPage();
            // }
            // else
            // {
            //     app.UseExceptionHandler("/Home/Error");
            // }
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseStatusCodePages();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors();
            
            // app.UseSignalR(config => {
            //     config.MapHub<MessageHub>("/messageHub");
            // });

            app.UseEndpoints(endpoints => 
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHub<GameHub>("gameHub");
                endpoints.MapHub<MessageHub>("/messageHub");
            });

            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapRazorPages();
            //     endpoints.MapHub<MessageHub>("/messageHub");
            // });

            app.UseSession();

            // app.UseMvc(routes => {
            //     routes.MapRoute(
            //         name: "default",
            //         template: "{controller=Home}/{action=Index}/{id?}");
            // });
        }
    }
}
