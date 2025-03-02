using Application.Interfaces.Repositories;
using Domain.Logistic;

namespace Infrastructure.Persistence.Repositories;

public class DriversRepository : Repository<Driver>, IDriversRepository
{
    public DriversRepository(MainDbContext dbContext) : base(dbContext)
    {
    }
}
