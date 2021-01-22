using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Database.Models;
using Timothy.Models.Inquiry;
using Timothy.Models.System;
using Timothy.Models.GuestType;
using Timothy.Models.User;
using Timothy.Models.Classification;
using Timothy.Models.ContactMethod;
using Timothy.Models.CallRegister;
using Timothy.Models.Summary;

namespace Timothy
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

            services.AddDbContext<DatabaseContext>(options => 
                options.UseNpgsql(
                    Configuration.GetConnectionString("DatabaseContext")
                )
            );

            // Session Setting
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = ".AdventureWorks.Session";
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.IsEssential = true;
            });

            // Insert dependencies
            services.AddScoped<IInquiry, InquiryModel>();
            services.AddScoped<ISystem, SystemModel>();
            services.AddScoped<IContactMethod, ContactMethodModel>();
            services.AddScoped<IGuestType, GuestTypeModel>();
            services.AddScoped<IUser, UserModel>();
            services.AddScoped<IClassification, ClassificationModel>();
            services.AddScoped<ICallRegister, CallRegisterModel>();
            services.AddScoped<ISummary, SummaryModel>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Summary}/{action=Index}/{id?}");
            });
        }
    }
}
