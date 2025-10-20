using EventResourceReservationApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.Services
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user, IList<string> roles);
    }
}
