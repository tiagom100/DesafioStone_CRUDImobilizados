using DesafioStone.Infra.Data.Interface;
using DesafioStone.Infra.Data.Repositories;
using DesafioStone.WebServiceRest.SimpleInjector;
using SimpleInjector;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DesafioStone.WebServiceRest
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var container = new Container();
            container.Register<IEdificioRepository, EdificioRepository>();
            container.Register<INivelRepository, NivelRepository>();
            container.Register<IItemRepository, ItemRepository>();

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorDependencyResolver(container);
        }
    }
}
