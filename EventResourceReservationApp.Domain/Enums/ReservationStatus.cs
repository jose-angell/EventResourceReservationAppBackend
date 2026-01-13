using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Domain.Enums
{
    public enum ReservationStatus
    {
        Pending = 1,
        Confirmed = 2,
        Completed = 3,
        Canceled = 4,
        Rejected = 5
    }
}
