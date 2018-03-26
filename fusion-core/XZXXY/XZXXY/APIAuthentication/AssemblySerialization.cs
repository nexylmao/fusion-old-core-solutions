using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;

namespace XZXXY.APIAuthentication
{
    internal static class AssemblySerialization
    {
        public static string AssemblyJson()
        {
            return new AssemblyInfo().ToString();
        }

        internal class AssemblyInfo
        {
            public Assembly XZXXYAssembly;

            public AssemblyInfo()
            {
                XZXXYAssembly = typeof(AssemblySerialization).Assembly;
            }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this, Formatting.Indented);
            }
        }
    }
}
