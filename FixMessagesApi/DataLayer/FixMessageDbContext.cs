using FixMessagesApi.DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace FixMessagesApi.DataLayer
{
    public class FixMessageDbContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<FixMessageDataModel> Messages { get; set; }
        public DbSet<FieldNameMappingDataModel> FieldNameMappings { get; set; }

        public FixMessageDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_connectionString}");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FixMessageDataModel>().ToTable("Messages");
            modelBuilder.Entity<FixMessageDataModel>(entity => { entity.HasKey(_ => _.Id); });
            modelBuilder.Entity<FixMessageDataModel>(entity => { entity.HasIndex(_ => _.Description); });
            modelBuilder.Entity<FixMessageDataModel>(entity => { entity.HasIndex(_ => _.SendingTime); });

            modelBuilder.Entity<FieldNameMappingDataModel>().ToTable("FieldNameMappings");
            modelBuilder.Entity<FieldNameMappingDataModel>(entity => { entity.HasKey(_ => _.Key); });

            base.OnModelCreating(modelBuilder);
        }
    }
}