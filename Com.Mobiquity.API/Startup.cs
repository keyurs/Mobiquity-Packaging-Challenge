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
                                        + "To get the sample testing file, please download from github https://github.com/keyurs/Mobiquity-Packaging-Challenge/blob/main/Com.Mobiquity.API/Upload/SamplePackagingInputFile.txt" + Environment.NewLine + Environment.NewLine + Environment.NewLine
                                        + "Steps to Test API are as below" + Environment.NewLine
                                        + "Step 1: Download or Create input file. Sample file is given here : https://github.com/keyurs/Mobiquity-Packaging-Challenge/blob/main/Com.Mobiquity.API/Upload/SamplePackagingInputFile.txt " + Environment.NewLine
                                        + "        Make sure file should have .txt as extension" + Environment.NewLine + Environment.NewLine
                                        + "Step 2: Upload file into API /api/Packer/uploadInputFile URL: https://keyurshahpackagingchallenge.azurewebsites.net/swagger/index.html#/Packer/Packer_UploadFile" + Environment.NewLine + Environment.NewLine
                                        + "        Upload file return response with absolute path of server which will be input for other Pack API call. So copy response path" + Environment.NewLine + Environment.NewLine
                                        + "Step 3: Call pack API /api/Packer/pack with absolute path of uploaded file https://keyurshahpackagingchallenge.azurewebsites.net/swagger/index.html#/Packer/Packer_pack" + Environment.NewLine + Environment.NewLine
                                        + "        Monitor response of pack API, it will result all lines output index for items best fit into package" + Environment.NewLine + Environment.NewLine
                                        + "Demo available at : https://github.com/keyurs/Mobiquity-Packaging-Challenge/blob/main/Com.Mobiquity.API/Upload/KeyurShahPackagingChallenge.gif";
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
