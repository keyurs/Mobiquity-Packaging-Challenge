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
    public class PackerService : IPackerService
    {
        private static String PACKAGE_REGEX = "\\((?<index>\\d+)\\,(?<weight>\\d+(\\.\\d{1,2})?)\\,€(?<cost>\\d+(\\.\\d{1,2})?)\\)";
        private const int MAX_ITEMS_IN_LINE = 15;
        private const int MAX_WEIGHT = 100;
        private const int MAX_COST = 100;

        /// <summary>
        /// pack function help to parse file and calculate best package items match
        /// </summary>
        /// <param name="filePath">File absolute path to process</param>
        /// <returns>Result return complete package and items as objects along with process result</returns>
        public async Task<List<Package>> pack(String filePath)
        {
            return await parseInputFile(filePath);
        }

        /// <summary>
        /// packSingle function help to parse single Line and calculate best indexs
        /// </summary>
        /// <param name="lineData">Line data with package "maxWeight : (index, weight, cost)" format</param>
        /// <returns>Result best match indexs in lineData</returns>
        public async Task<Package> packSingle(String lineData)
        {
            return await parseItemsLine(1, lineData);
        }

        /// <summary>
        /// parseItemsLine function help to parse line data of file. 
        /// </summary>
        /// <param name="lineNumber">File line number in file, help to provide reference during error or exception</param>
        /// <param name="lineData">Line data with package "maxWeight : (index, weight, cost)" format</param>
        /// <returns>Result best match indexs in lineData</returns>
        private async Task<Package> parseItemsLine(int lineNumber, String lineData)
        {
            List<PackageItem> packageItems = new List<PackageItem>();

            String[] splited = lineData.Split(":");

            if (splited.Length != 2)
            {
                throw new APIException("Line must contain exactly one `:`", lineData, lineNumber);
            }

            double maxWeight;

            try
            {
                maxWeight = double.Parse(splited[0].Trim());
            }
            catch
            {
                throw new APIException("Left side of `:` must be a number", lineData, lineNumber);
            }

            MatchCollection allMatch = Regex.Matches(lineData, PACKAGE_REGEX, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            foreach(Match matchPattern in allMatch)
            {
                try
                {
                    int index = int.Parse(matchPattern.Groups["index"].Value);
                    double weight = double.Parse(matchPattern.Groups["weight"].Value);
                    double cost = double.Parse(matchPattern.Groups["cost"].Value);

                    if (index > MAX_ITEMS_IN_LINE || index < 0)
                    {
                        throw new APIException(String.Format("Index must be in range (1, {0})", MAX_ITEMS_IN_LINE), lineData, lineNumber);
                    }

                    if (weight > MAX_WEIGHT || weight < 0)
                    {
                        throw new APIException(String.Format("weight mas be in range (0, {0})", MAX_WEIGHT), lineData, lineNumber);
                    }

                    if (cost > MAX_COST || cost < 0)
                    {
                        throw new APIException(String.Format("cost mas be in range (0, {0})", MAX_COST), lineData, lineNumber);
                    }

                    packageItems.Add(new PackageItem() { ItemIndex = index, Weight = weight, Cost = cost });
                }
                catch (Exception e) {
                    throw new APIException(e, lineData, lineNumber);
                }
            }

            int n = packageItems.Count + 1;
            int w = (int)(maxWeight * 100) + 1;
            double[,] a = new double[n,w];

            for (int i = 1; i < n; i++)
            {
                PackageItem pack = packageItems[i - 1];
                int packWeight = (int)(pack.Weight * 100);
                int packCost = (int)(pack.Cost * 100);

                for (int j = 1; j < w; j++)
                {
                    if (packWeight > j)
                    {
                        a[i,j] = a[i - 1,j];
                    }
                    else
                    {
                        a[i,j] = Math.Max(a[i - 1,j], a[i - 1,j - packWeight] + packCost);
                    }
                }
            }

            List<int> indexes = new List<int>();
            int k = (int)(maxWeight * 100);
            double totalcost = a[n - 1,w - 1];
            for (; k > 0 && a[n - 1,k - 1] == totalcost; k--);

            for (int i = n - 1; i > 0; i--)
            {
                if (a[i,k] != a[i - 1,k])
                {
                    indexes.Add(packageItems[i - 1].ItemIndex);
                    k -= (int)(packageItems[i - 1].Weight * 100);
                }
            }
            indexes.Sort();

            String result = string.Join(",", indexes.Select(n => n.ToString()).ToArray());
            result = result == string.Empty ? "-" : result;

            return new Package { MaxWeight = maxWeight, PackageItems = packageItems, PackageResult = result };
        }

        /// <summary>
        /// parseInputFile function help to parse whole file. 
        /// </summary>
        /// <param name="filePath">File absolute path to process</param>
        /// <returns>Result return complete package and items as objects along with process result</returns>
        private async Task<List<Package>> parseInputFile(String filePath) 
        {
            List<Package> allPackageItems = new List<Package>();
            string lineData = string.Empty;
            int lineNumber = 0;

            using (var fileReader = new StreamReader(filePath, Encoding.UTF8))
            {
                while ((lineData = fileReader.ReadLine()) != null)
                {
                    lineNumber++;
                    var package = await parseItemsLine(lineNumber, lineData);
                    allPackageItems.Add(package);
                }
            }

            return allPackageItems;
        }
    }
}
