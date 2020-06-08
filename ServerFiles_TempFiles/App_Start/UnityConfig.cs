using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Web.Http;
using ThirdApp.DataBase;
using ThirdApp.Helpers;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace ThirdApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IDbManager, DbManager>(TypeLifetime.Singleton);
            container.RegisterType<IImageFilesManager, ImageFilesManager>(TypeLifetime.Singleton);
            container.RegisterType<IGameManager, GameManager>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}