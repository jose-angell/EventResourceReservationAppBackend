using EventResourceReservationApp.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EventResourceReservationApp.Infrastructure
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            // Usamos un Diccionario para definir los roles y sus descripciones
            var rolesWithDescriptions = new Dictionary<string, string>
            {
                { "Admin", "Administrador con control total sobre el sistema." },
                { "Manager", "Gerente con permisos para gestionar reservas y recursos." },
                { "Client", "Usuario final con permisos para crear y ver sus propias reservas." }
            };

            foreach (var entry in rolesWithDescriptions)
            {
                string roleName = entry.Key;
                string roleDescription = entry.Value;

                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    // 1. Crear una nueva instancia de ApplicationRole
                    var role = new ApplicationRole
                    {
                        Name = roleName,
                        // 2. Asignar la descripción
                        Description = roleDescription,
                        // 3. Asignar el NormalizedName (necesario para Identity)
                        NormalizedName = roleName.ToUpper()
                    };

                    // 4. Crear el rol en la base de datos
                    var result = await roleManager.CreateAsync(role);

                    // Opcional: Manejar errores si la creación del rol falla
                    if (!result.Succeeded)
                    {
                        // Aquí podrías loggear un error.
                        // Generalmente esto no falla a menos que haya problemas de BD.
                    }
                }
            }
        }
    }
}
