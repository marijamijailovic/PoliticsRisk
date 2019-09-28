using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoliticsRisk.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSignalR();

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

            services.AddMemoryCache();
            services.AddSession();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseAuthentication();
            
            app.UseSignalR(config => {
                config.MapHub<MessageHub>("/messages");
            });

            app.UseSession();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
