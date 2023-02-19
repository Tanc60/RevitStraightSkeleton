using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AreaCalc.ModelObjects
{
    class GlobalConfig
    {
        
        public static readonly string assembleName = Assembly.GetExecutingAssembly().Location;
        public static readonly string ConfigPath = System.IO.Path.GetDirectoryName(assembleName) + "/AreaCalc_Config.ini";
        

    }
}
