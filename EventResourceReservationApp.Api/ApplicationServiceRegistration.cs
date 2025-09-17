using EventResourceReservationApp.Application.UseCases.Categories;
using EventResourceReservationApp.Application.UseCases.Locations;
using EventResourceReservationApp.Application.UseCases.ReservationCarItems;
using EventResourceReservationApp.Application.UseCases.Reviews;

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

            //Rservation Car Items
            services.AddTransient<CreateReservationCarItemUseCase>();
            services.AddTransient<ReadAllReservationCarItemUseCase>();
            services.AddTransient<UpdateReservationCarItemUseCase>();
            services.AddTransient<DeleteReservationCarItemUseCase>();

            // Review 
            services.AddTransient<CreateReviewUseCase>();
            services.AddTransient<ReadAllReviewUseCase>();
            services.AddTransient<UpdateReviewUseCase>();
            services.AddTransient<DeleteReviewUseCase>();
            services.AddTransient<ReadByIdReviewUseCase>();

            return services;
        }
    }
}
