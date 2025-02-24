using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;
using PromoCodeFactory.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Text.Json.Serialization;

namespace PromoCodeFactory.WebHost
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(o => {
                o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                o.JsonSerializerOptions.MaxDepth = 0;
            });
                // .AddNewtonsoftJson(options =>
                //     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            // services.AddScoped(typeof(IRepository<Employee>), (x) =>
            //     new InMemoryRepository<Employee>(FakeDataFactory.Employees));
            // services.AddScoped(typeof(IRepository<Role>), (x) =>
            //     new InMemoryRepository<Role>(FakeDataFactory.Roles));
            // services.AddScoped(typeof(IRepository<Preference>), (x) =>
            //     new InMemoryRepository<Preference>(FakeDataFactory.Preferences));
            // services.AddScoped(typeof(IRepository<Customer>), (x) =>
            //     new InMemoryRepository<Customer>(FakeDataFactory.Customers));

            services.AddDbContext<DatabaseContext>(option => option
                    .UseLazyLoadingProxies()
                    .UseSqlite(Configuration.GetConnectionString("Default")))
                .AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            
            services.AddOpenApiDocument(options =>
            {
                options.Title = "PromoCode Factory API Doc";
                options.Version = "1.0";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
            else
            {
                app.UseHsts();
            }

            app.UseOpenApi();
            app.UseSwaggerUi(x =>
            {
                x.DocExpansion = "list";
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}