using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpecExpress.MessageStore;

namespace SpecExpress
{
    using SpecExpress.Ioc;

    public class ValidationCatalogConfiguration
    {
        public  IDictionary<string, IMessageStore> MessageStores { private set; get;}

        private readonly IDependencyContainer _defaultDependencyContainer = new DependencyContainer();

        private IDependencyContainer _dependencyContainer;

        private NoSpecificationFoundBehavior _noSpecificationFoundBehavior = NoSpecificationFoundBehavior.RaiseExceptionIfNoSpecificationsForTypeFound | NoSpecificationFoundBehavior.RaiseExceptionIfNoSpecificationsFound;

        public bool ValidateObjectGraph { get; set; }

        public void AddMessageStore(IMessageStore store, string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (MessageStores == null)
            {
                MessageStores = new Dictionary<string, IMessageStore>();
            }
            MessageStores.Add(key, store);
        }

        public NoSpecificationFoundBehavior NoSpecificationFoundBehavior
        {
            get
            {
                return this._noSpecificationFoundBehavior;
            }
            set
            {
                this._noSpecificationFoundBehavior = value;
            }
        }

        public IMessageStore DefaultMessageStore { get; set; }

        public IDependencyContainer DependencyContainer
        {
            get
            {
                return this._dependencyContainer ?? this._defaultDependencyContainer;
            }
            set
            {
                this._dependencyContainer = value;
            }
        }
    }
}
