using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using TheCodeCamp.Data;

namespace TheCodeCamp
{
    public class AutofacConfig
    {
        public static void Register()
        {
            var bldr = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            bldr.RegisterApiControllers(Assembly.GetExecutingAssembly());
            RegisterServices(bldr);
            bldr.RegisterWebApiFilterProvider(config);
            bldr.RegisterWebApiModelBinderProvider();
            var container = bldr.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static void RegisterServices(ContainerBuilder bldr)
        {
            /* ***below I am adding the configuration in Autofac dependency injection for the mapping Entity to Model * * ***/
            MapperConfiguration configuration = new MapperConfiguration(cfg =>
              {
                  cfg.AddProfile(new CampMappingProfile());
              });

            bldr.RegisterInstance(configuration.CreateMapper()).As<IMapper>().SingleInstance();

            /* *** above I am adding the configuration in Autofac dependency injection for the mapping Entity to Model * * ***/

            bldr.RegisterType<CampContext>()
              .InstancePerRequest();

            bldr.RegisterType<CampRepository>()
              .As<ICampRepository>()
              .InstancePerRequest();
        }
    }
}
