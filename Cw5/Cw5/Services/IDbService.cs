using Cw5.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cw5.Models
{
    public interface IDbService
    {
        Task<IEnumerable<TripGetter>> GetTripList();
        Task<int> RemoveClient(int idClient);
        Task<int> AddClienttoTrip(ClientTrips clientTrip);

    }
}
