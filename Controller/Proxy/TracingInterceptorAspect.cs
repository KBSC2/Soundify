
using System;
using System.Diagnostics;
using System.Linq;
using Castle.DynamicProxy;
using Controller.DbControllers;
using Model.Annotations;
using Model.Database.Contexts;

namespace Controller.Proxy
{
    [Serializable]
    public class TracingInterceptorAspect : IInterceptor
    {
        private IDatabaseContext context;
        public TracingInterceptorAspect(IDatabaseContext context)
        {
            this.context = context;
        }

        /**
         * Every time a method within a routed class gets called, it goes through this fuction.
         * invocation.Proceed() is what calls the actual method.
         *
         * If the function has an HasPermission annotation, check for permissions.
         * If not allowed display the Not Allowed popup
         */
        public void Intercept(IInvocation invocation)
        {
            var x = invocation.Method.GetCustomAttributes(typeof(HasPermission), true).OfType<HasPermission>()
                .FirstOrDefault();

            var controller = UserController.Create(context);

            if (x == null || UserController.CurrentUser == null)
            {
                invocation.Proceed();
                return;
            }

            var allowed = x.HasMaxValue
                ? controller.HasPermission(UserController.CurrentUser, x.Permission, x.MaxValue)
                : controller.HasPermission(UserController.CurrentUser, x.Permission);

            if (allowed)
            {
                invocation.Proceed();
            }
            else
            {
                // Open not allowed thingy : Ben
                Debug.WriteLine("NEE, MAG NIET");
            }
        }
    }
}
