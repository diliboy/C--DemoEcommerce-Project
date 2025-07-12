using eCommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;


namespace ProductApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services,IConfiguration config)
        {
            //Add database connectivity
            //Add authentication scheme
            SharedServiceContainer.AddSharedService<ProductDbContext>(services, config, config["MySeriLog:FineName"]!);

            //Create dependency injection 
            services.AddScoped<IProduct, ProductRepository>();

            return services;
        }

        public static IApplicationBuilder UserInfrastructurePolicy(this IApplicationBuilder app)
        {
            //Register middlewre such as :
            //Global Exception: handles external errors
            //Listen only to APIGateway : blocks all outsider calls

            SharedServiceContainer.UserSharedPolicies(app);

            return app;
        }
    }
}
