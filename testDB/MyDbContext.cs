using Microsoft.EntityFrameworkCore;

namespace testDB
{
    public class MyDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public MyDbContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder options) => 
                                    options.UseSqlServer(Program.ConnectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
                         modelBuilder.Entity<Employee>().ToTable("Employees");

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) =>
                         configurationBuilder.Properties<DateTime>().HaveColumnType("datetime2");
        
    }
}

