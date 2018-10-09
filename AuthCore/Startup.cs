using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using AuthCore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;
using System.IO;

namespace AuthCore
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
            string conn = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<Core.DB>(opts => opts.UseSqlServer(conn));

            services.AddAuthorization(_ =>
            {
                _.AddPolicy("OnlyForMicrosoft_Name", p => p.RequireClaim("company", "Microsoft"));
                _.AddPolicy("pname", p => p.RequireClaim("ppp", "qqq", "ууу"));
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opts => {
                    opts.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login")
                    ;// opts.Cookie.Name = "MyCoolie";
                    // opts.DataProtectionProvider = DataProtectionProvider.Create(new DirectoryInfo(@"C:\temp-keys\"));

                });
            //services.AddDataProtection().PersistKeysToFileSystem(
            //new DirectoryInfo(@"C:\temp-keys\"));
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // app.Run(async (_) =>{ _.Response.Cookies.Append("mykey", "1111111szvzsdvszdvzs111111111"); return; });
            //app.Run(async hc => { hc.Response.; });
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");//
            });
        }
    }
}
