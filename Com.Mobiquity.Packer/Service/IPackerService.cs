using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Com.Mobiquity.Packer.EntitiesModel;

namespace Com.Mobiquity.Packer
{
    public interface IPackerService
    {
        /// <summary>
        /// Pack API help to process Raw data from input file and provide output with best matched index of items can fit
        /// Pack logic process as per highest cost and weight ration match to fit in box which consist hire value in box with lower weight as possible
        /// </summary>
        /// <param name="filePath">Absolute path reference of input file uploaded via upload API</param>
        /// <returns>Result with multiple Line of matching index</returns>
        Task<List<Package>> pack(String filePath);

        /// <summary>
        /// packSingle API help to process Raw data with single package process and provide output of best match index
        /// This API use to just test individual line instead upload file
        /// </summary>
        /// <param name="lineData">Line data with package "maxWeight : (index, weight, cost)" format</param>
        /// <returns>Result matching indexs fit as per max weight</returns>
        Task<Package> packSingle(String lineData);
    }
}
