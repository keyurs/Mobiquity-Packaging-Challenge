using NUnit.Framework;
using Com.Mobiquity.Packer;
using Com.Mobiquity.Packer.EntitiesModel;
using System.IO;
using System;

namespace Com.Mobiquity.NUnitTests
{
    public class PackerServiceTest
    {        
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Inputfile_CheckAllRowsResults()
        {
            var packerService = new PackerService();

            var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "UploadTest");
            if (!Directory.Exists(pathBuilt))
            {
                Directory.CreateDirectory(pathBuilt);
            }

            var fileFullPath = Path.Combine(pathBuilt, "packaging inputs.txt");

            var result = packerService.pack(fileFullPath).Result;

            //assert
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("4", result[0].PackageResult);
            Assert.AreEqual("-", result[1].PackageResult);
            Assert.AreEqual("2,7", result[2].PackageResult);
            Assert.AreEqual("8,9", result[3].PackageResult);
        }

        [Test]
        public void SingleLine_CheckCorrectMatch()
        {
            var packerService = new PackerService();

            var defineTestValue = "81 : (1,53.38,€45) (2,88.62,€98) (3,78.48,€3) (4,72.30,€76) (5,30.18,€9) (6,46.34,€48)";

            var result = packerService.packSingle(defineTestValue).Result;

            //assert
            Assert.AreEqual("4", result.PackageResult);
            Assert.Pass();
        }

        [Test]
        public void SingleLine_NO_or_EmptyMatch()
        {
            var packerService = new PackerService();

            var defineTestValue = "8 : (1,15.3,€34)";

            var result = packerService.packSingle(defineTestValue).Result;

            //assert
            Assert.AreEqual("-", result.PackageResult);
            Assert.Pass();
        }
    }
}