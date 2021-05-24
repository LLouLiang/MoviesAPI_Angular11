using AngularMoviesAPI.Filters;
using AngularMoviesAPI.helpers;
using AngularMoviesAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI
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
            // Add data model autoMapper to autocomplete DTO (data transfer object) to our model
            // DTO will hide our data model to user but expose itself to user, need to complete field mapping when user
            // communcating the data between server and frontend
            services.AddAutoMapper(typeof(Startup));
            
            // Add Database entity framework core service
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.UseNetTopologySuite())); // Added NetTopology
            // Add Dependency injection of geometryFactory services into Class AutoMapperProfilers
            services.AddSingleton(provider => new MapperConfiguration(config =>
            {
                var geometryFactory = provider.GetRequiredService<GeometryFactory>();
                config.AddProfile(new AutoMapperProfilers(geometryFactory));
            }).CreateMapper());
            // Add geomtry factory singleton 
            services.AddSingleton<GeometryFactory>(NtsGeometryServices
                .Instance.CreateGeometryFactory(srid: 4326));

            // Add service addCors to enable CROS communication between angular and web api
            // after added the addcors service, add this into app configure app.useCors() in configuration method
            services.AddCors(options =>
            {
                // THIS FUNC is going to allow the client to recongize the output header
                options.AddDefaultPolicy(builder =>
                {
                    // added physical url address in appsetting.json file as "frontend_url"
                    var angularURL = Configuration.GetValue<string>("frontend_url");
                    // allow any method sent from this website, and any header of requests
                    // *** remove the backslash of this url ****
                    builder.WithOrigins(angularURL).AllowAnyMethod().AllowAnyHeader() //-- Appsetting.json -- add angular server url
                    .WithExposedHeaders(new string[] { "totalAmountOfRecords" }); // HttpContextExtensions - add httpcontext -- "totalAmountOfRecords"
                });
            });
            // Add custom exception filter to application controller
            services.AddControllers(options =>
            {
                // Not sure why need to use typeof()
                options.Filters.Add(typeof(CustomExceptionFilter));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AngularMoviesAPI", Version = "v1" });
            });
            // Denpendency injection when IRepository is created and only one instance of InMemoryRepository will be created
            services.AddSingleton<IRepository, InMemoryRepository>(); // The IRepository will be served to only instance of InMemoryRepository
            /*
             * Filters type
             * Authorization Filter
             * Resource Filter
             * Action Filter: 
             * Exception Filter
             * Result Filter
             */
            // Add responseCaching 
            services.AddResponseCaching();

            // Add authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            // Add custom action filter
            services.AddTransient<CustomActionFilter>();

            services.AddScoped<IFileStorageService, AzureStorageService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> log)
        {
            /*
             * Updated because of here is a middleware setting 
             * Updated to capture all incoming request before it goes to the controller
             */
           
            // here real example: we are going to log all incoming request and log all requests body
            // middlewares
            app.Use(async (context, next) => {
                using (var swapStream = new MemoryStream())
                {
                    var originalResponseBody = context.Response.Body;
                    context.Response.Body = swapStream;
                    await next.Invoke();

                    swapStream.Seek(0, SeekOrigin.Begin);
                    string responseBody = new StreamReader(swapStream).ReadToEnd();
                    swapStream.Seek(0, SeekOrigin.Begin);

                    await swapStream.CopyToAsync(originalResponseBody);
                    context.Response.Body = originalResponseBody;

                    log.LogInformation(responseBody);
                }
            });

            /*
             * app.Run() will short-circuiting the pipeline(the line 52 till 69)
             */
            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AngularMoviesAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // have to put after UseRouting
            // add cors 
            app.UseCors();
            // response caching middleware
            app.UseResponseCaching();
            // authentication middleware
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
