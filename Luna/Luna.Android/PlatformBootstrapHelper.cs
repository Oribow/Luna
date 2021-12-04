using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Luna.Data;
using Luna.Droid.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Luna.Droid
{
    class PlatformBootstrapHelper : IPlatformBootstrapHelper
    {
        public string LocPackageDir => SceneRepository.BasePath;

        AssetManager assetMan;

        public PlatformBootstrapHelper(AssetManager assetMan)
        {
            this.assetMan = assetMan;
        }

        public Stream OpenTestPackage()
        {
            return assetMan.Open("testdata.zip");
        }
    }
}