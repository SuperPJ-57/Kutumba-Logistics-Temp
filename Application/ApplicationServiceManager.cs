using Application.Common.Behaviours;
using Application.Exceptions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Gurung.ServiceRegister;

namespace Application
{
    public class ApplicationServiceManager : IServicesRegistration
    {
        public void AddServices(IServiceCollection services)
        {
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), ServiceLifetime.Scoped);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddControllersWithViews(options => options.Filters.Add(new ApiExceptionFilter()));
            services.AddSingleton<ApiExceptionFilter>();
            
        }
    }
}
