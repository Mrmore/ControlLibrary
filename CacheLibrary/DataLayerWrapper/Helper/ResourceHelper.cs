using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace DataLayerWrapper.Helper
{
    internal static class ResourceHelper
    {
        private static ResourceLoader errorResourceLoader;

        public static string GetErrorResource(string resourceName)
        {
            if (errorResourceLoader == null)
                errorResourceLoader = new ResourceLoader();

            return errorResourceLoader.GetString(resourceName);
        }
    }
}
