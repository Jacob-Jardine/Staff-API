using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Staff_Service.Context;
using Staff_Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staff_Service
{
    public class Startup
    {
        private IWebHostEnvironment _environment = null;
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<StagingContext>(options =>
            {
                var cs = Configuration.GetConnectionString("DbConnection");
                options.UseSqlServer(cs);
            });
            services.AddDbContext<ProductionContext>(options =>
            {
                var cs = Configuration.GetConnectionString("DbConnection");
                options.UseSqlServer(cs);
            });

            if (_environment.IsDevelopment()) 
            {
                services.AddSingleton<IStaffRepository, FakeStaffRepository>();
            }
            else if (_environment.IsStaging() || _environment.IsProduction()) 
            {
                services.AddScoped<IStaffRepository, SqlStaffRepository>();
            }
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
