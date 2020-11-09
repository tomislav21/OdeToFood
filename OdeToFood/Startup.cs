using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OdeToFood.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace OdeToFood
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
            services.AddRazorPages();
            services.AddControllers();

            services.AddDbContext<OdeToFoodDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("OdeToFoodDb"), opts =>
                {
                    opts.EnableRetryOnFailure();
                })); ;

            services.AddScoped<IRestaurantData, SqlRestaurantData>();
            
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // In development, this middleware display exception page with detail information 
                // about exception that comes from other middlewares in pipelane 
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // In production, this middleware display exception page with error page without details
                // that was caused by other middlewares in pipelane 
                app.UseExceptionHandler("/Error");
                // Instructs the browser to access only this information over a secure connection 
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // This is going to send HTTP redirect instruction to any browser that tries to access the application using plain HTTP
            app.UseHttpsRedirection();
            // Attempts to serve a request by responding with a file that's in the wwwroot folder
            app.UseStaticFiles();
            // Serve static files from the node_modules folder
            app.UseNodeModules();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
