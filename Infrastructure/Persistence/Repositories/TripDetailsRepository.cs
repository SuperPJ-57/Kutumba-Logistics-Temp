using Application.Interfaces.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public class TripDetailsRepository : Repository<TripDetails>, ITripDetailsRepository
    {

        public TripDetailsRepository(MainDbContext context) : base(context)
        {

        }
    }
}
