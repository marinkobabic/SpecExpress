using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress
{
    using SpecExpress.Ioc;

    public abstract class ValidationContext
    {
        private SpecificationContainer _specContainer = new SpecificationContainer();

        private IDependencyContainer dependencyContainer;

        public ValidationContext()
        {
            
        }
        
        public void AddSpecifications(Func<IEnumerable<SpecificationBase>,IEnumerable<SpecificationBase>>  selectedSpecs)
        {  
            //Add Specification returned by the function to the Context
            _specContainer.Add(selectedSpecs(ValidationCatalog.SpecificationContainer.GetAllSpecifications()));
        }

        public void AddSpecification(Type specType)
        {
            _specContainer.Add(this.DependencyContainer.Resolve(specType) as SpecificationBase);
        }

        public void AddSpecification<TSpecType>() where TSpecType : SpecificationBase, new()
        {
            _specContainer.Add(this.DependencyContainer.Resolve<TSpecType>());
        }

        public IDependencyContainer DependencyContainer
        {
            get
            {
                return this.dependencyContainer ?? ValidationCatalog.Configuration.DependencyContainer;
            }
            set
            {
                this.dependencyContainer = value;
            }
        }

        public SpecificationContainer SpecificationContainer
        {
            get { return _specContainer; }
        }
    }
}
