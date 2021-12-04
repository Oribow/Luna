using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Luna.Data
{
    public interface IPlatformBootstrapHelper
    {
        string LocPackageDir { get; }
        Stream OpenTestPackage();
    }
}
