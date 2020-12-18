using Castle.DynamicProxy;
using Model.Database.Contexts;

namespace Controller.Proxy
{
    public static class ProxyController
    {
        private static ProxyGenerator generator = new ProxyGenerator();

        /**
         * Add a class to the generator proxy
         *
         * @param parameters Parameters to use for the class' constructor
         * @param context The IDatabaseContext to use for the TracingInterceptorAspect
         *
         * @return instance of class T
         */
        public static T AddToProxy<T>(object[] parameters, IDatabaseContext context) where T : class
        {
            return (T) generator.CreateClassProxy(typeof(T), parameters, new TracingInterceptorAspect(context));
        }

        /**
         * Add a class to the generator proxy
         *
         * @param context The IDatabaseContext to use for the TracingInterceptorAspect
         *
         * @return instance of class T
         */
        public static T AddToProxy<T>(IDatabaseContext context) where T : class
        {
            return generator.CreateClassProxy<T>(new TracingInterceptorAspect(context));
        }
    }
}
