using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Domain.Enums
{
    public enum ResourceStatus
    {
        Disponible = 1,
        Bloqueado = 2,
        FueraDeServicio = 3,
        Eliminado = 4,
    }
}
