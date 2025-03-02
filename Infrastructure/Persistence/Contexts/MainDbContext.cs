
using Domain.Configurations.MainConfigurations;

namespace Infrastructure.Persistence.Contexts
{
    public class MainDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {

        }
        //public DbSet<Tenants> Tenants { get; set; }
        //public DbSet<Company> Companies { get; set; }
        public DbSet<TransportationOrder> TransportationOrders { get; set; }
        public DbSet<TrackVehicles> TrackVehicles { get; set; }
        public DbSet<TripLogging> TripLogging { get; set; }
        public DbSet<TripDetails> TripDetails { get; set; }
        public DbSet<TripRequest> TripRequest { get; set; }
        public DbSet<OngoingLogistics> OngoingLogistics { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            var configType = typeof(IMainDbContextConfig);
            var assembly = configType.Assembly;
            string namespaceName = configType.Namespace;

            var configurations = assembly.GetTypes()
                .Where(t => t.Namespace == namespaceName
                && typeof(IEntityTypeConfiguration<>).IsAssignableFrom(t)
                && !t.IsInterface
                && !t.IsAbstract);

            foreach (var config in configurations)
            {
                var configurationInstance = Activator.CreateInstance(config);
                builder.ApplyConfiguration((dynamic)configurationInstance);
            }
            base.OnModelCreating(builder);
        }
    }
}
