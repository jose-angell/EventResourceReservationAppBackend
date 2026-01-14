using EventResourceReservationApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Domain.Extensions
{
    public static class ReservationStatusExtensions
    {
        public static string GetDescription(this ReservationStatus statusId) => statusId switch
        {
            ReservationStatus.Pending => "Pendiente de aprobación",
            ReservationStatus.Confirmed => "Confirmada y reservada",
            ReservationStatus.Completed => "Finalizada exitosamente",
            ReservationStatus.Canceled => "Cancelada por el cliente",
            ReservationStatus.Rejected => "Rechazada por el administrador",
            _ => "Estado desconocido"
        };
    }
}
