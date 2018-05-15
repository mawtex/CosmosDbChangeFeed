using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDbChangeFeedSpike.Services
{
    public static class ServiceLocator
    {
        private static IServiceCollection _serviceCollection = new ServiceCollection();
        private static IServiceProvider _serviceProvider;

        private static IServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider == null)
                {
                    _serviceProvider = _serviceCollection.BuildServiceProvider(false);
                }

                return _serviceProvider;
            }
        }

        public static void AddSingleton(Type serviceType, object implementation)
        {
            _serviceCollection.Add(ServiceDescriptor.Singleton(serviceType, implementation));
            _serviceProvider = null;

        }

        public static void AddSingleton(Type serviceType, Type implementationType)
        {
            _serviceCollection.Add(ServiceDescriptor.Singleton(serviceType, implementationType));
            _serviceProvider = null;

        }

        public static void AddTransient(Type serviceType, Type implementationType)
        {
            _serviceCollection.AddTransient(serviceType, implementationType);
            _serviceProvider = null;

        }


        public static T GetService<T>()
        {
            return ServiceProvider.GetService<T>();
        }


    }
}
