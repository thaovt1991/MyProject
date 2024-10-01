using HomemadeCakes.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomemadeCakes.Model;
using MongoDB.Driver;
using Amazon.Runtime.Internal.Transform;
using EasyCaching.InMemory;

namespace HomemadeCakes
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

            // MongoDB- cach 1
            //services.AddScoped<IMongoClient, MongoClient>(provider =>
            //{
            //    var connectionString = Configuration.GetConnectionString("MongoDB");
            //    return new MongoClient(connectionString);
            //});

            // ...

            //services.AddMvc();

            // MongoDB- cach 2
            //services.Configure<ConnectDatabaseSettings>(Configuration.GetSection("HomemadeCakesDatabase"));
            //Enable CORS
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod()
                 .AllowAnyHeader());
            });

            services.AddSingleton<UserService>();

            services.AddControllers().AddJsonOptions(
            options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
            //doi json va doc dât json..
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HomemadeCakes", Version = "v1" });

                c.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id ="Bearer"
                            }, Scheme ="auth2" ,
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },  new List<string>()
                } }); 
            });

            //Set cookie - Xem lai cho này đang loi
            // services.AddSession(options =>
            //{
            //options.IdleTimeout = TimeSpan.FromSeconds(3600);
            //options.Cookie.HttpOnly = true;
            //options.Cookie.IsEssential = true;
            // });


            //Important step for In-Memory Caching
            //Cach 1
            services.AddEasyCaching(options =>
            {
                // use memory cache with a simple way
                options.UseInMemory("default");

                // use memory cache with your own configuration
                options.UseInMemory(config =>
                {
                    config.DBConfig = new InMemoryCachingOptions
                    {
                        // scan time, default value is 60s
                        ExpirationScanFrequency = 60,
                        // total count of cache items, default value is 10000
                        SizeLimit = 100,

                        // below two settings are added in v0.8.0
                        // enable deep clone when reading object from cache or not, default value is true.
                        EnableReadDeepClone = true,
                        // enable deep clone when writing object to cache or not, default value is false.
                        EnableWriteDeepClone = false,
                    };
                    // the max random second will be added to cache's expiration, default value is 120
                    config.MaxRdSecond = 120;
                    // whether enable logging, default is false
                    config.EnableLogging = false;
                    // mutex key's alive time(ms), default is 5000
                    config.LockMs = 5000;
                    // when mutex key alive, it will sleep some time, default is 300
                    config.SleepMs = 300;
                }, "default1");
            });

            //Important step for In-Memory Caching
            //Cach 2 - thiet lap trong application
            services.AddEasyCaching(options =>
            {
                //use memory cache
                options.UseInMemory(Configuration, "default", "easycaching:inmemory");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());//Cho phé các request http gọi vào
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomemadeCakes v1"));
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();
            //swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomemadeCakes V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
