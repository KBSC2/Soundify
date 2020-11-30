using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;
using Controller.DbControllers;

namespace Controller.Proxy
{
    public static class ProxyController
    {
        public static ProxyGenerator generator = new ProxyGenerator();

        public static T AddToProxy<T>(object[] parameters) where T : class
        {
            return (T) generator.CreateClassProxy(typeof(T), parameters, new TracingInterceptorAspect());
        }
        public static T AddToProxy<T>() where T : class
        {
            return (T)generator.CreateClassProxy<T>(new TracingInterceptorAspect());
        }

    }
}
