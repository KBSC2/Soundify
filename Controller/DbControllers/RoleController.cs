using Controller.Proxy;
using Model.Database.Contexts;
using Model.DbModels;

namespace Controller.DbControllers
{
    public class RoleController : DbController<Role>
    {
        /**
         * Creates a instance of this controller
         * It adds the controller to the proxy
         *
         * @param IDatabaseContext instance of a database session
         *
         * @returns RoleController : the proxy with a instance of this controller included
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
