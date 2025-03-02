using Application.Interfaces.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public class TripRequestRepository : Repository<TripRequest>, ITripRequestRepository
    {

        public TripRequestRepository(MainDbContext context) : base(context)
        {

        }
    }
}
