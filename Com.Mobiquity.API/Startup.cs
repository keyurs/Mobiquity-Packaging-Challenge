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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Com.Mobiquity.Packer;

namespace Com.Mobiquity.API
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
            services.AddSingleton<IPackerService, PackerService>();
            services.AddControllers();
            // Register the Swagger services
            services.AddSwaggerDocument(settings =>
            {
                settings.Title = "Keyur Shah - Packaging Challenge API";
                settings.Description = "Please visit this link for code and documenations https://github.com/keyurs/Mobiquity-Packaging-Challenge" + Environment.NewLine
                                        + "To get the sample testing file, please download from github " + Environment.NewLine + Environment.NewLine
                                        + "Steps to Test API are as below" + Environment.NewLine
                                        + "Step 1: Download or Create Sample file. It should in format [MaxWeight] : (Index, Item Weight, Cost) ... As e.g. 81 : (1,53.38,�45) (2,88.62,�98) (3,78.48,�3) (4,72.30,�76) " + Environment.NewLine
                                        + "Step 2: Upload file into API https://keyurshahpackagingchallenge.azurewebsites.net/swagger/index.html#/Packer/Packer_UploadFile" + Environment.NewLine
                                        + "Step 3: Upload file return response with absolute path of server which will be input for other Pack API call. So copy response path" + Environment.NewLine
                                        + "Step 4: Call Pack API with absolute path of uploaded file https://keyurshahpackagingchallenge.azurewebsites.net/swagger/index.html#/Packer/Packer_pack" + Environment.NewLine
                                        + "Monitor response of pack API, it will result all lines output index for items best fit into package";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();
            app.UseSwaggerUi3();

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
