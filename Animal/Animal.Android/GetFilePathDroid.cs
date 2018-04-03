
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(Animal.Droid.GetFilePathDroid))]

namespace Animal.Droid
{
    class GetFilePathDroid : IGetFilePath
    {
            public string GetFullPath(string relativeFilePath)
        {
            return Path.Combine(Android.App.Application.Context.ExternalCacheDir.Path, relativeFilePath);
        }
    }
}