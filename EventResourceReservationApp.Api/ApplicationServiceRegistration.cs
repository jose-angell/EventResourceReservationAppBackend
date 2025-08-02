using EventResourceReservationApp.Application.UseCases.Categories;

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

            return services;
        }
    }
}
