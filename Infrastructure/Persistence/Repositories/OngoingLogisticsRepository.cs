using Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class OngoingLogisticsRepository : Repository<OngoingLogistics>, IOngoingLogisticsRepository
    {
        public OngoingLogisticsRepository(MainDbContext mainDbContext) : base(mainDbContext)
        {

        }
    }
}
