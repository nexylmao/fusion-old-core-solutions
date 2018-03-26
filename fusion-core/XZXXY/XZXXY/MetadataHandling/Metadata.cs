using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XZXXY.MetadataHandling
{
    internal sealed class Metadata
    {
        #region Fields
        [JsonProperty] // Default Database Path that Database class uses
        string defaultPath;
        [JsonProperty] // List of strings where each one contains a path to an assembly
        List<string> assemlbyPaths;
        [JsonProperty] // Boolean value that should enable debug mode
        // safe mode with internal methods, additional logging to the console
        bool debugMode;
        #endregion
    }
}
