using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress.Test.Domain.Specifications
{
    public class CustomersRepository : ICustomersRepository
    {
        private readonly List<int> allCustomers;

        public CustomersRepository()
        {
            this.allCustomers = new List<int>();
        }

        public List<int> GetCustomers()
        {
            return this.allCustomers;
        }
    }
}
