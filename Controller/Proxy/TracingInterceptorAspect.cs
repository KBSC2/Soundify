
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Controller.DbControllers;
using Model.Annotations;
using Model.Database.Contexts;

namespace Controller.Proxy
{
    [Serializable]
    public class TracingInterceptorAspect : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var x = invocation.Method.GetCustomAttributes(typeof(HasPermission), true).OfType<HasPermission>()
                .FirstOrDefault();

            var controller = UserController.Create(new DatabaseContext());

            bool allowed = x.HasMaxValue
                ? controller.HasPermission(UserController.CurrentUser, x.Permission)
                : controller.HasPermission(UserController.CurrentUser, x.Permission, x.MaxValue);

            if (x == null || UserController.CurrentUser == null || allowed)
            {
                invocation.Proceed();
            }
            else
            {
                // Open not allowed thingy
                Debug.WriteLine("NEE, MAG NIET");
            }
        }
    }
}
