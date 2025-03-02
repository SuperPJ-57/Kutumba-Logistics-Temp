

using Application.Interfaces.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public class TripLoggingRepository : Repository<TripLogging>, ITripLoggingRepository
    {
        public TripLoggingRepository(MainDbContext mainDbContext) : base(mainDbContext)
        {

        }
    }
}
