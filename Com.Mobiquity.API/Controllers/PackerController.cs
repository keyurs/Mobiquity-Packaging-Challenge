using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Com.Mobiquity.Packer;
using Com.Mobiquity.Packer.EntitiesModel;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.IO;

namespace Com.Mobiquity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackerController : ControllerBase
    {
        private readonly ILogger<PackerController> _logger;
        private Packer.IPackerService _packerService;

        /// <summary>
        /// Packer Controller using Depedancy injection for packer service and logger
        /// </summary>
        public PackerController(ILogger<PackerController> logger, IPackerService packerService)
        {
            _logger = logger;
            _packerService = packerService;
        }

        /// <summary>
        /// Upload file API help to first setup/input file with data for further user with "pack" api call
        /// Function store files into Upload\Files folder of API
        /// </summary>
        /// <param name="file">Input file as IFormFile post</param>
        /// <returns>Absolute path of file</returns>
        [HttpPost("uploadInputFile", Name = "uploadInputFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<string> UploadFile(
         IFormFile file,
         CancellationToken cancellationToken)
        {
            string fileSavedRefPath = string.Empty;
            if (file != null && file.Length > 0 && Path.GetExtension(file.FileName) == ".txt")
            {
                string fileName;
                try
                {
                    //Create a new Name for the file due to security reasons.
                    fileName = String.Format("{0}_{1}{2}", 
                                            Path.GetFileNameWithoutExtension(file.FileName),
                                            DateTime.Now.Ticks,
                                            Path.GetExtension(file.FileName)); 

                    var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files");
                    if (!Directory.Exists(pathBuilt))
                    {
                        Directory.CreateDirectory(pathBuilt);
                    }

                    fileSavedRefPath = Path.Combine("Upload\\files", fileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), fileSavedRefPath);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                catch (Exception e)
                {
                    fileSavedRefPath = string.Empty;
                    throw e;
                }
            }
            else
            {
                throw new APIException("Invalid file extension, please upload .txt file with correct inputs", string.Empty, 0);
            }

            return fileSavedRefPath;
        }

        /// <summary>
        /// Pack API help to process Raw data from input file and provide output with best matched index of items can fit
        /// Pack logic process as per highest cost and weight ration match to fit in box which consist hire value in box with lower weight as possible
        /// </summary>
        /// <param name="filePath">Absolute path reference of input file uploaded via upload API</param>
        /// <returns>Result with multiple Line of matching index</returns>
        [HttpPost("pack")]
        public async Task<string> pack(string filePath)
        {
            var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), filePath);

            if(!System.IO.File.Exists(pathBuilt))
            {
                throw new APIException("Invalid file path, file does not exists, please try upload and use the same path as reference input", string.Empty, 0);
            } 
            else
            {
                List<Package> packages = await CalculateAllPackers(pathBuilt);
                return string.Join("\n", packages.Select(x => x.PackageResult).ToArray());
            }
        }

        /// <summary>
        /// packReturnJson API help to process Raw data from input file and provide output with complete objects and its result with each associated package items
        /// This API use to provide complete data in case needed to list everythign individually
        /// </summary>
        /// <param name="filePath">Absolute path reference of input file uploaded via upload API</param>
        /// <returns>Result complete Package object with output result of each object inside package items</returns>
        [HttpPost("packReturnJson")]
        public async Task<IEnumerable<Package>> packReturnJson(string filePath)
        {
            var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), filePath);

            if (!System.IO.File.Exists(pathBuilt))
            {
                throw new APIException("Invalid file path, file does not exists, please try upload and use the same path as reference input", string.Empty, 0);
            }
            else
            {                
                return await CalculateAllPackers(pathBuilt);
            }
        }

        /// <summary>
        /// packSingle API help to process Raw data with single package process and provide output of best match index
        /// This API use to just test individual line instead upload file
        /// </summary>
        /// <param name="lineData">Line data with package "maxWeight : (index, weight, cost)" format</param>
        /// <returns>Result matching indexs fit as per max weight</returns>
        [HttpPost("packSingle")]
        public async Task<string> packSingle(string lineData)
        {
            return (await _packerService.packSingle(lineData)).PackageResult;
        }

        /// <summary>
        /// CalculateAllPackers function help to parse file and calculate best packages match
        /// </summary>
        /// <param name="filePath">File absolute path to process</param>
        /// <returns>Result return complete package and items as objects along with process result</returns>
        private async Task<List<Package>> CalculateAllPackers(string filePath)
        {
            List<Package> packages = new List<Package>();
            packages = await _packerService.pack(filePath);
            return packages;
        }
    }
}
