using Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class TransportationOrderRepository : Repository<TransportationOrder>, ITransportationOrderRepository
    {

        public TransportationOrderRepository(MainDbContext context) : base(context) 
        {
        
        }
    }
}
