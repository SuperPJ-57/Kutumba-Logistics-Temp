
using Domain.Configurations.MainConfigurations;
using Domain.Logistic;




namespace Infrastructure.Persistence.Contexts
{
    public class MainDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public MainDbContext()
        {
        }

        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {

        }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<Driver> drivers { get; set; }
        public DbSet<Vehicle> vehicles { get; set; }
        public DbSet<Trip> trips { get; set; }
        public DbSet<Consignment> consignments { get; set; }
        public DbSet<Freight> freights { get; set; }
        public DbSet<TripRequest> tripRequests { get; set; }

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

            builder.Entity<Driver>()
                .HasMany(d => d.TripRequest)
                .WithOne(d => d.Driver)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Vehicle>()
                .HasMany(v => v.TripRequest)
                .WithOne(v => v.Vehicle)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Trip>()
                .HasMany(t => t.TripRequest)
                .WithOne(t => t.Trip)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Consignment>()
                .HasMany(c => c.TripRequest)
                .WithOne(c => c.Consignment)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Freight>()
                .HasMany(f => f.TripRequest)
                .WithOne(f => f.Freight)
                .OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(builder);
            //SeedRoles(builder);
        }
        //private void SeedRoles(ModelBuilder builder)
        //{
        //    builder.Entity<ApplicationRole>().HasData
        //        (
        //            new ApplicationRole() { Name= "Super Admin", ConcurrencyStamp = "1", NormalizedName = "Master"},
        //            new ApplicationRole() { Name= "Admin", ConcurrencyStamp = "2", NormalizedName = "Admin"},
        //            new ApplicationRole() { Name= "Driver", ConcurrencyStamp = "3", NormalizedName = "User"}
        //        );
        //}
    }
}
