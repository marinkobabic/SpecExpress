using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress
{
    public class SpecExpressNoSpecificationsFoundException : SpecExpressConfigurationException
    {
        public SpecExpressNoSpecificationsFoundException() : base("No specifications are registered with ValidationCatalog. Check if Scan has been run.")
        {
        }
    }
}
