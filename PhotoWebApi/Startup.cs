using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using PhotoWebApi.Helpers;

namespace PhotoWebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // services.Configure<ApiBehaviorOptions>(options =>
            // {
            //     options.SuppressConsumesConstraintForFormFileParameters = true;
            //     options.SuppressInferBindingSourcesForParameters = true;
            //     options.SuppressModelStateInvalidFilter = true;
            // });

            // services.Configure<ApiBehaviorOptions>(options =>
            // {
            //     options.InvalidModelStateResponseFactory = ctx => new ValidationProblemDetailsResult();
            // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }
            // else
            // {
            //     app.UseHsts(); // ???
            // }

            app.UseExceptionHandler("/api/Errors");

            // app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials());

            // TODO: How to correctly treat HTTPS redirect?
            // app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
