

using Domain.Entities;

namespace Domain.Configurations.MainConfigurations
{
    internal class TransportationOrderConfiguration : IEntityTypeConfiguration<TransportationOrder>, IMainDbContextConfig
    {
        public void Configure(EntityTypeBuilder<TransportationOrder> builder)
        {
            builder.Property(e => e.DriverAllowance).HasPrecision(18, 2);
            builder.Property(e => e.FreightRate).HasPrecision(18, 2);
            builder.Property(e => e.MaintenanceFee).HasPrecision(18, 2);
            builder.Property(e => e.TripAllowance).HasPrecision(18, 2);
        }
    }
}
