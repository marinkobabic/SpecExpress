using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress.Test.Domain.Entities
{
    using SpecExpress.Test.Domain.Values;

    public class Tenant
    {
        public Tenant(int id)
        {
            this.Id = id;
            this.Customers = new List<int>();
            this.Address = new Address() {City = "New York", Province = "US", Country = "US", Street = "Walking Street"};
        }

        public int Id { get; private set; }

        public List<int> Customers { get; private set; }

        public Address Address { get; private set; }
    }
}
