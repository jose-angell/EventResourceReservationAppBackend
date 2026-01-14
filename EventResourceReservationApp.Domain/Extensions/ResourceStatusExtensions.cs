using EventResourceReservationApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Domain.Extensions
{
    public static class ResourceStatusExtensions
    {
        public static string GetDescription(this ResourceStatus resource) => resource switch
        {
            ResourceStatus.Disponible => "Disponible para usar.",
            ResourceStatus.Bloqueado => "Bloqueado para reservar",
            ResourceStatus.FueraDeServicio => "No se encuentra disponible",
            ResourceStatus.Eliminado => "Recurso eliminado para su uso",
            _ => "Estado desconocido"
        };
    }
}
