using GameWebAPI.DataBase;
using GameWebAPI.Helpers;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace GameWebAPI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<IDataBaseManager, DataBaseManager>(TypeLifetime.Singleton);
            container.RegisterType<IGameManager, GameManager>(TypeLifetime.Singleton);

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}