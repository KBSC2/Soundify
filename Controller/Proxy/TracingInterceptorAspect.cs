
using System;
using System.Diagnostics;
using System.Linq;
using Castle.DynamicProxy;
using Controller.DbControllers;
using Model.Annotations;
using Model.Database.Contexts;
using Model.DbModels;
using Model.EventArgs;

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
            


            if (x.HasMaxValue)
            {
                controller.HasPermission(UserController.CurrentUser, x.Permission, x.MaxValue).ContinueWith(res =>
                {
                    var z = res.Result;
                    RunTask(invocation, z, x);
                });
                /*var z = await controller.HasPermission(UserController.CurrentUser, x.Permission, x.MaxValue);
                RunTask(invocation, z, x);*/
            }
            else
            {
                var allowed = controller.HasPermission(UserController.CurrentUser, x.Permission);
                RunTask(invocation, allowed, x);
            }


        }

        private void RunTask(IInvocation invocation, bool allowed, HasPermission x)
        {
            if (allowed)
                invocation.Proceed();
            else
                PermissionController.NoRightsEvent?.Invoke(this, new NoRightsEventArgs() { Permission = x.HasMaxValue ? x.MaxValue : x.Permission });
        }
    }
}
