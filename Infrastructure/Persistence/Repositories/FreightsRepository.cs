using Application.Interfaces.Repositories;
using Domain.Logistic;

namespace Infrastructure.Persistence.Repositories;

public class FreightsRepository : Repository<Freight>, IFreightRepository
{
    public FreightsRepository(MainDbContext dbContext) : base(dbContext)
    {
    }
}
