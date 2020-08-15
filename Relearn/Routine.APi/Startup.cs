using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Routine.APi.Data;
using Routine.APi.Services;
using AutoMapper;
using Newtonsoft.Json;

namespace Routine.APi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Accept Header for output formatters
            // content-type header for input formatters
            //services.AddControllers(options =>
            //{
            //    // set 406 code
            //    options.ReturnHttpNotAcceptable = true;
            //    //options.OutputFormatters.Add(
            //        //new XmlDataContractSerializerOutputFormatter());
            //    // options.OutputFormatters.Insert(0, new XmlDataContractSerializerOutputFormatter());
            //}).AddXmlDataContractSerializerFormatters();
            // new way to write AddXmlDataContractSerializerFormatters()
            services.AddControllers(options =>
            {
                // 406 state code
                options.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson(
            {

            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddDbContext<RoutineDbContext>(options =>
            {
                var connectionString =
                    "Data Source = (localdb)\\MSSQLLocalDB; DataBase = routine; Integrated Security = SSPI";
                options.UseSqlServer(connectionString);
              });
        }
        // router middle wares 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // the sequences of middle wares
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //500 
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Unexpected Error!");
                    });
                });
            }

            app.UseRouting();
            app.UseAuthentication();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
