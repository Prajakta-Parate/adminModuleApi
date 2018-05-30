using Api;
using Autofac;
using Autofac.Integration.WebApi;
using Entity;
using Repository;
using Services;
using System.Reflection;
using System.Web.Http;

namespace AutoFacDemo.Infrastructure
{
    internal class DependencyConfigure
    {
        //public static IContainer Container;
        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }

        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(WebApiApplication).Assembly).PropertiesAutowired();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<EmployeeContext>()
                   .As<IDbContext>()
                   .InstancePerRequest();
            builder.RegisterGeneric(typeof(RepositoryService<>))
                  .As(typeof(IRepository<>));
            builder.RegisterType<EmployeeService>().As<IEmployeeService>();



            //builder.RegisterAssemblyTypes(typeof(WebApiApplication).Assembly).PropertiesAutowired();

            ////deal with your dependencies here
            //builder.RegisterType<EmployeeContext>().As<IEmployeeContext>().InstancePerDependency(); ;

            //builder.RegisterGeneric(typeof(RepositoryService<>)).As(typeof(IRepository<>));

            //builder.RegisterType<EmployeeService>().As<IEmployeeService>();


            return builder.Build();
        }
    }
}