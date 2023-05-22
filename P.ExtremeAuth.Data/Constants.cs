using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P.ExtremeAuth.Data
{
    public static class Constants
    {
        private static string _migrationAssembly;
        public static string MigrationAssembly { get { return _migrationAssembly ?? (_migrationAssembly = typeof(Constants).Assembly.FullName); } }
    }
}
