using Application.Interfaces.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public class TrackVehiclesRepository : Repository<TrackVehicles>, ITrackVehiclesRepository
    {
        public TrackVehiclesRepository(MainDbContext mainDbContext) : base(mainDbContext)
        {

        }
    }
}
