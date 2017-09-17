using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CoreIdentity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CoreIdentity.Insfrastructure;
using Microsoft.Extensions.Logging;

namespace CoreIdentity
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc();

            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DataConnection")));

            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();


            
            //Implementating CustomeValidator
            services.AddTransient<IPasswordValidator<AppUser>, CustomerValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseStatusCodePages();
            app.UseIdentity();
            app.UseMvcWithDefaultRoute();

            //AppDbContext.CreateAdminAccount(app.ApplicationServices, Configuration).Wait();

           

            
                //DbInitializer.CreateAdmin(app.ApplicationServices).Wait();
            


        }
    }
}
