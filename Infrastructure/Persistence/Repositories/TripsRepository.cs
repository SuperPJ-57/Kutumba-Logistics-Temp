using Application.Interfaces.Repositories;
using Domain.Logistic;

namespace Infrastructure.Persistence.Repositories;

public class TripsRepository : Repository<Trip>, ITripRepository
{
    public TripsRepository(MainDbContext dbContext) : base(dbContext)
    {
    }
}
