using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NLemos.Api.Framework.Data
{
    public abstract class SqlDbContext : DbContext
    {
        public SqlDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            MapModels(modelBuilder);

            //Do not change the dependent entities of the foreign keys
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        /// <summary>
        /// This is intended to do model mapping from/to db tables.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/> instance.</param>
        protected abstract void MapModels(ModelBuilder modelBuilder);

        public override int SaveChanges()
        {
            UpdateEntities();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateEntities()
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is IEntity entity)
                {
                    if (item.State == EntityState.Added)
                    {
                        //Ensure that the key is set
                        if (entity.Id == Guid.Empty)
                        {
                            entity.Id = Guid.NewGuid();
                        }

                        //Forces the added and modified dates to now
                        entity.Added = DateTimeOffset.Now;
                        entity.Modified = DateTimeOffset.Now;
                    }

                    if (item.State == EntityState.Modified)
                    {
                        //Always update the modified date
                        entity.Modified = DateTime.Now;
                    }
                }
            }
        }
    }
}
