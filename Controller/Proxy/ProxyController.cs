using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;
using Controller.DbControllers;
using Model.Database.Contexts;

namespace Controller.Proxy
{
    public static class ProxyController
    {
        public static ProxyGenerator generator = new ProxyGenerator();

        public static T AddToProxy<T>(object[] parameters, IDatabaseContext context) where T : class
        {
            return (T) generator.CreateClassProxy(typeof(T), parameters, new TracingInterceptorAspect(context));
        }
        public static T AddToProxy<T>(IDatabaseContext context) where T : class
        {
            return (T)generator.CreateClassProxy<T>(new TracingInterceptorAspect(context));
        }

    }
}
