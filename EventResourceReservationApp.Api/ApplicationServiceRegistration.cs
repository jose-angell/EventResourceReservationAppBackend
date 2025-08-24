using EventResourceReservationApp.Application.UseCases.Categories;
using EventResourceReservationApp.Application.UseCases.Locations;

namespace EventResourceReservationApp.Api
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register Use Cases for Categories
            services.AddTransient<CreateCategoryUseCase>();
            services.AddTransient<UpdateCategoryUseCase>();
            services.AddTransient<DeleteCategoryUseCase>();
            services.AddTransient<ListAllCategoryUseCase>();
            services.AddTransient<ReadAllCategoryUseCase>();
            services.AddTransient<ReadByIdCategoryUseCase>();

            // Register Use Cases for Locations
            services.AddTransient<CreateLocationUseCase>();
            services.AddTransient<UpdateLocationUseCase>();
            services.AddTransient<DeleteLocationUseCase>();
            services.AddTransient<ReadAllLocationUseCase>();
            services.AddTransient<ReadByIdLocationUseCase>();

            return services;
        }
    }
}
