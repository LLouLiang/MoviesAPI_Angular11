using AngularMoviesAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AngularMoviesAPI", Version = "v1" });
            });
            // Denpendency injection when IRepository is created and only one instance of InMemoryRepository will be created
            services.AddSingleton<IRepository, InMemoryRepository>(); // The IRepository will be served to only instance of InMemoryRepository
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
