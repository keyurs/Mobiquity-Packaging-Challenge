using System;
using System.Collections.Generic;

namespace Com.Mobiquity.Packer.EntitiesModel
{
    public class Package
    {
        public double MaxWeight { get; set; }

        public List<PackageItem> PackageItems { get; set; }

        public string PackageResult { get; set; }
    }
}
