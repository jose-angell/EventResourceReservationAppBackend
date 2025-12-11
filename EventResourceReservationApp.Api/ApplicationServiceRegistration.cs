using EventResourceReservationApp.Application.UseCases.Categories;
using EventResourceReservationApp.Application.UseCases.Locations;
using EventResourceReservationApp.Application.UseCases.ReservationCarItems;
using EventResourceReservationApp.Application.UseCases.Resources;
using EventResourceReservationApp.Application.UseCases.Reviews;

/*
 * El proposito de la clase y su metodo de extencion AddApplicationServicies es registrar todas las clases de la capa
 * de Aplicacion (los use cases) con el contenedor de inversion de Control (IoC) de ASP.NET.
 * 
 * Esta clase actua como un modulo de configuracion, que maneja todas las peticiones de creacion (Transient) 
 * de las clases registradas aqui.
 * 
 * En la implementacion parece que se inyecta la clase directamente poruqe en el proyecto no se esta usando interfaces
 * para los use cases. Pero sigue funcionando porque el contenedor sabe como crear la clase concreta que se le pidio en el
 * contrller, gracias a que se registro previamente con AddApplicationServices
 */

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

            // Resource
            services.AddTransient<CreateResourceUseCase>();
            services.AddTransient<UpdateResourceUseCase>();
            services.AddTransient<DeleteResourceUseCase>();
            services.AddTransient<ReadAllResourceUseCase>();
            services.AddTransient<ReadByIdResourceUseCase>();
            services.AddTransient<ReadByIdAndDateRangeResourceUseCase>();

            return services;
        }
    }
}
