using Application.Interfaces.Repositories;
using Domain.Logistic;

namespace Infrastructure.Persistence.Repositories;

public class ConsignmentDetailsRepository : Repository<Consignment>, IConsignmentRepository
{
    public ConsignmentDetailsRepository(MainDbContext dbContext) : base(dbContext)
    {
    }
}
