using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress.Test.Domain.Specifications
{
    public interface ICustomersRepository
    {
        List<int> GetCustomers();
    }
}
