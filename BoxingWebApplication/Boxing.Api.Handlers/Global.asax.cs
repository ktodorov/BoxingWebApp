using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Core;
using Autofac.Integration.WebApi;
using AutoMapper;
using FluentValidation.WebApi;
using Boxing.Core.Sql;
using Boxing.Api.Handlers.Filters;
using Boxing.Core.Handlers;
using Boxing.Core.Handlers.CrossCutting;
using Boxing.Core.Handlers.Interfaces;
using Boxing.Contracts.Dto;
using Boxing.Core.Sql.Configurations;

namespace Boxing.Api.Handlers
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var config = GlobalConfiguration.Configuration;

            config.MapHttpAttributeRoutes();
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            config.Filters.Add(new BadRequestExceptionAttribute());
            config.Filters.Add(new NotFoundExceptionAttribute());
            config.Filters.Add(new AuthAttribute());
            config.Filters.Add(new ValidationFilterAttribute());

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterFilters(config);

            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);

            RegisterHandlers(builder);
            RegisterContext(builder);

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            FluentValidationModelValidatorProvider.Configure(config);

            config.EnsureInitialized();

            BoxingContext.SetInitializer();
            ConfigureMappings();
        }

        private static void RegisterHandlers(ContainerBuilder builder)
        {
            builder.RegisterType<Mediator>()
                .As<IMediator>();

            builder.RegisterAssemblyTypes(typeof(Mediator).Assembly)
                .As(type => type.GetInterfaces()
                    .Where(interfaceType => interfaceType.IsClosedTypeOf(typeof(IRequestHandler<,>)))
                    .Select(interfaceType => new KeyedService("handler", interfaceType)));

            builder.RegisterGenericDecorator(
                typeof(LoggingHandlerDecorator<,>),
                typeof(IRequestHandler<,>),
                fromKey: "handler");
        }

        private static void RegisterContext(ContainerBuilder builder)
        {
            builder.RegisterType<BoxingContext>()
                .InstancePerRequest()
                .AsSelf()
                .As<DbContext>();
        }

        private static void ConfigureMappings()
        {
            Mapper.CreateMap<BoxerDto, BoxerEntity>().ForAllMembers(opt => opt.Condition(e => !e.IsSourceValueNull));
            Mapper.CreateMap<BoxerEntity, BoxerDto>().ForAllMembers(opt => opt.Condition(e => !e.IsSourceValueNull));

            Mapper.CreateMap<MatchDto, MatchEntity>().ForAllMembers(opt => opt.Condition(e => !e.IsSourceValueNull));
            Mapper.CreateMap<MatchEntity, MatchDto>().ForAllMembers(opt => opt.Condition(e => !e.IsSourceValueNull));

            Mapper.CreateMap<UserDto, UserEntity>().ForAllMembers(opt => opt.Condition(e => !e.IsSourceValueNull));
            Mapper.CreateMap<UserEntity, UserDto>().ForAllMembers(opt => opt.Condition(e => !e.IsSourceValueNull));

            Mapper.CreateMap<LoginDto, LoginEntity>().ForAllMembers(opt => opt.Condition(e => !e.IsSourceValueNull));
            Mapper.CreateMap<LoginEntity, LoginDto>().ForAllMembers(opt => opt.Condition(e => !e.IsSourceValueNull));

            Mapper.CreateMap<PredictionDto, PredictionEntity>().ForAllMembers(opt => opt.Condition(e => !e.IsSourceValueNull));
            Mapper.CreateMap<PredictionEntity, PredictionDto>().ForAllMembers(opt => opt.Condition(e => !e.IsSourceValueNull));
        }
    }
}
