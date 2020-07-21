using AdventureWorksMVCCore.Web.Models;
using AdventureWorksMVCCore.Web.Service.Implementation;
using AdventureWorksMVCCore.Web.Service.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace AdventureWorksMVCCore.Web
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
            services.AddControllersWithViews();

            Dictionary<string, string> secrets = services.GetSqlCredential("CycleStoreCredentials").Result;

            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            connectionString = connectionString.Replace("<UserId>", secrets["username"]);
            connectionString = connectionString.Replace("<Password>", secrets["password"]);

            services.AddDbContext<CYCLE_STOREContext>(options =>
            options.UseSqlServer(connectionString));

            services.TryAddScoped<ICategoryService, CategoryService>();
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
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
