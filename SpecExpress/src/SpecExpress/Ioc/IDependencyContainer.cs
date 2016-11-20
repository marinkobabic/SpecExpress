using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress.Ioc
{
    public interface IDependencyContainer
    {
        void Register(Type type);

        object Resolve(Type type);

        T Resolve<T>();
    }
}
