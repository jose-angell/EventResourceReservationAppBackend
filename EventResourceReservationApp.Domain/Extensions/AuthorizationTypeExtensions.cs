using EventResourceReservationApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Domain.Extensions
{
    public static class AuthorizationTypeExtensions
    {
        public static string GetDescription(this ResourceAuthorizationType authorizationType) => authorizationType switch
        {
            ResourceAuthorizationType.Automatico => "Pendiente de aprobación",
            ResourceAuthorizationType.Manual => "Confirmada y reservada",
            _ => "Estado desconocido"
        };
    }
}
