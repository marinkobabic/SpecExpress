using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress
{
    public class SpecExpressNoSpecificationFoundForTypeException : SpecExpressConfigurationException
    {
        public Type Type { get; private set; }

        public SpecExpressNoSpecificationFoundForTypeException(Type type) : base("No Specification for type " + type + " was found.")
        {
            this.Type = type;
        }
    }
}
