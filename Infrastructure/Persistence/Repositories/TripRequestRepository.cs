
using Application.Interfaces.Repositories;
using Domain.Logistic;

namespace Infrastructure.Persistence.Repositories;

public class TripRequestRepository : Repository<TripRequest>, ITripRequestRepository
{
    public TripRequestRepository(MainDbContext dbContext) : base(dbContext)
    {
    }
}
