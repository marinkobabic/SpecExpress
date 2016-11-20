using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress
{
    public class SpecExpressMultipleSpecificationsFoundException : SpecExpressConfigurationException
    {
        public Type Type { get; private set; }

        public SpecExpressMultipleSpecificationsFoundException(Type type) : base($"Multiple Specifications found and none are defined as default for {type.FullName}.")
        {
            this.Type = type;
        }
    }
}
