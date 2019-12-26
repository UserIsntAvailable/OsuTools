using System.IO;
using System.Text;
using System.Reflection;

namespace OsuTools.Tests.Resources {
    internal static class ResourcesHelper {

        /// <summary>
        /// Get a resource file path from Resources folder
        /// </summary>
        /// <param name="filename">The filename</param>
        /// <returns>The full path of the file</returns>
        internal static string GetResourcePathFile(string filename) {

            /* I don't know if this is the best way to do this method
             * I will try to improve it later : / */

            var resourcesPath = new StringBuilder();

            string assemblypath = Assembly.GetExecutingAssembly().Location;

            foreach (var item in assemblypath.Split("\\")) {

                if (item != "OsuTools") {

                    resourcesPath.Append(item);
                    resourcesPath.Append("\\");
                }

                else {
                    resourcesPath.Append("OsuTools\\OsuTools.Tests\\Resources\\");
                    break;
                }
            }

            return Path.Join(resourcesPath.ToString(), filename);
        }
    }
}
