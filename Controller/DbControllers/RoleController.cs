using Controller.Proxy;
using Model.Database.Contexts;
using Model.DbModels;

namespace Controller.DbControllers
{
    public class RoleController : DbController<Role>
    {
        /**
         * This function creates a instance of this controller
         * It adds the controller to the proxy
         * @returns the proxy with a instance of this controller included
         */
        public static RoleController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<RoleController>(new object[] { context }, context);
        }

        protected RoleController(IDatabaseContext context) : base(context, context.Roles)
        {
        }
    }
}
