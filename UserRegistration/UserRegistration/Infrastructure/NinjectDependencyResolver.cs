using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;
using Ninject;

namespace UserRegistration.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel _kernel; 

        public NinjectDependencyResolver(IKernel kernel) 
        {
            this._kernel = kernel;
            AddBindings();
        }

        private void AddBindings()
        {
            _kernel.Bind<IPersonsDataProvider>().To<PersonsDataProviderXml>().InSingletonScope().WithConstructorArgument(new Mutex());
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }
    }
}