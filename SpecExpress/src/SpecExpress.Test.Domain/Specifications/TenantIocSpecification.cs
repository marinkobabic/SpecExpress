using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress.Test.Domain.Specifications
{
    using SpecExpress.Test.Domain.Entities;
    public class TenantIocSpecification : Validates<Tenant>
    {
        public TenantIocSpecification()
        {
            
        }

        public TenantIocSpecification(ICustomersRepository customersRepository)
        {
            Check(t => t.Id).Required();
            Check(t => t.Customers).Required().ForEach(c => customersRepository.GetCustomers().Contains((int)c), "Customer is not in the list");
            Check(t => t.Address).Required().Specification<AddressSpecification>();

        }
    }
}
