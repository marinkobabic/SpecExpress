using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress.Ioc
{
    public class DependencyContainer : IDependencyContainer
    {
        private readonly Func<Type,object> resolveAction;

        public DependencyContainer()
        {
            this.resolveAction = t => Activator.CreateInstance(t);
        }
        public DependencyContainer(Func<Type,object> resolveAction)
        {
            if (resolveAction == null) throw new ArgumentNullException(nameof(resolveAction));
            this.resolveAction = resolveAction;
        }

        public void Register(Type type)
        {
        }

        public T Resolve<T>()
        {
            return (T)this.Resolve(typeof(T));
        }

        public object Resolve(Type spec)
        {
            return this.resolveAction(spec);
        }
    }
}
