using Application.Interfaces.Repositories;
using Domain.Logistic;

namespace Infrastructure.Persistence.Repositories;

public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
{
    public VehicleRepository(MainDbContext dbContext) : base(dbContext)
    {
    }
}
