using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress.Test.Ioc
{
    using Microsoft.Practices.Unity;

    using NUnit.Framework;

    using SpecExpress.Ioc;
    using SpecExpress.Test.Domain.Entities;
    using SpecExpress.Test.Domain.Specifications;

    [TestFixture]

    public class IocTests
    {
        [Test]
        public void UseOnlyRootContainerValidationMustSucceed()
        {
            var rootContainer = new UnityContainer();
            rootContainer.RegisterType<ICustomersRepository, CustomersRepository>(new ContainerControlledLifetimeManager());
            var tenantOne = new Tenant(1);

            tenantOne.Customers.Add(1);
            tenantOne.Customers.Add(2);

            rootContainer.Resolve<ICustomersRepository>().GetCustomers().Add(1);
            rootContainer.Resolve<ICustomersRepository>().GetCustomers().Add(2);


            ValidationCatalog.Configuration.DependencyContainer = new DependencyContainer(t => rootContainer.Resolve(t));

            ValidationCatalog.Reset();
            ValidationCatalog.AddSpecification<TenantIocSpecification>();

            Assert.IsTrue(ValidationCatalog.Validate(tenantOne).IsValid);
        }

        [Test]
        public void UseOnlyRootContainerValidationMustHaveAnError()
        {
            var rootContainer = new UnityContainer();
            rootContainer.RegisterType<ICustomersRepository, CustomersRepository>(new ContainerControlledLifetimeManager());
            var tenantOne = new Tenant(1);

            tenantOne.Customers.Add(1);
            tenantOne.Customers.Add(2);

            rootContainer.Resolve<ICustomersRepository>().GetCustomers().Add(1);

            ValidationCatalog.Configuration.DependencyContainer = new DependencyContainer(t => rootContainer.Resolve(t));

            ValidationCatalog.Reset();
            ValidationCatalog.AddSpecification<TenantIocSpecification>();

            Assert.IsFalse(ValidationCatalog.Validate(tenantOne).IsValid);
        }

        [Test]
        public void UseChildContainerValidationMustSucceed()
        {
            var rootContainer = new UnityContainer();
            rootContainer.RegisterType<ICustomersRepository, CustomersRepository>(new HierarchicalLifetimeManager());

            var tenantOneContainer = rootContainer.CreateChildContainer();
            var tenantOne = new Tenant(1);
            tenantOne.Customers.Add(1);
            tenantOne.Customers.Add(2);
            tenantOneContainer.Resolve<ICustomersRepository>().GetCustomers().Add(1);
            tenantOneContainer.Resolve<ICustomersRepository>().GetCustomers().Add(2);

            var tenantTwoContainer = rootContainer.CreateChildContainer();
            var tenantTwo = new Tenant(2);
            tenantTwo.Customers.Add(7);
            tenantTwo.Customers.Add(8);
            tenantTwoContainer.Resolve<ICustomersRepository>().GetCustomers().Add(7);
            tenantTwoContainer.Resolve<ICustomersRepository>().GetCustomers().Add(8);

            var tenantOneContext = new TenantContext();
            tenantOneContext.DependencyContainer = new DependencyContainer(t => tenantOneContainer.Resolve(t));
            tenantOneContext.AddSpecification<TenantIocSpecification>();


            var tenantTwoContext = new TenantContext();
            tenantTwoContext.DependencyContainer = new DependencyContainer(t => tenantTwoContainer.Resolve(t));
            tenantTwoContext.AddSpecification(typeof(TenantIocSpecification));


            ValidationCatalog.Configuration.DependencyContainer = new DependencyContainer(t => rootContainer.Resolve(t));
            ValidationCatalog.Reset();


            Assert.IsTrue(ValidationCatalog.Validate(tenantOne,tenantOneContext).IsValid);
            Assert.IsTrue(ValidationCatalog.Validate(tenantTwo, tenantTwoContext).IsValid);

            tenantOne.Customers.Add(5);

            Assert.IsFalse(ValidationCatalog.Validate(tenantOne, tenantOneContext).IsValid);
            Assert.IsTrue(ValidationCatalog.Validate(tenantTwo, tenantTwoContext).IsValid);
        }

        [Test]
        public void ForSpecificationMustUseContainer()
        {
            var rootContainer = new UnityContainer();
            rootContainer.RegisterType<ICustomersRepository, CustomersRepository>(new HierarchicalLifetimeManager());

            var tenantOneContainer = rootContainer.CreateChildContainer();
            var tenantOne = new Tenant(1);
            tenantOne.Customers.Add(1);
            tenantOne.Customers.Add(2);
            tenantOneContainer.Resolve<ICustomersRepository>().GetCustomers().Add(1);
            tenantOneContainer.Resolve<ICustomersRepository>().GetCustomers().Add(2);

            var tenantOneContext = new TenantContext();
            tenantOneContext.DependencyContainer = new DependencyContainer(t => tenantOneContainer.Resolve(t));
            tenantOneContext.AddSpecification<TenantIocSpecification>();

            ValidationCatalog.Configuration.DependencyContainer = new DependencyContainer(t => rootContainer.Resolve(t));
            ValidationCatalog.Reset();

            Assert.IsTrue(ValidationCatalog.Validate(tenantOne, tenantOneContext).IsValid);
        }
    }
}
