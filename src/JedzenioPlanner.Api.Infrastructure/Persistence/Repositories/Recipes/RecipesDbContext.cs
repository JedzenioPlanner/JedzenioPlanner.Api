using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Recipes;
using Microsoft.EntityFrameworkCore;

namespace JedzenioPlanner.Api.Infrastructure.Persistence.Repositories.Recipes
{
    public class RecipesDbContext : DbContext, IEventProjectorTarget<Recipe>
    {
        public RecipesDbContext(DbContextOptions<RecipesDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecipeDatabaseEntity>(x =>
            {
                x.Property(y => y.Name).HasMaxLength(256);
                x.Property(y => y.Description).HasMaxLength(3072);
                x.Property(y => y.PictureUrl).HasMaxLength(2048);
            });
            
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<RecipeDatabaseEntity> Recipes { get; set; }
        
        public async Task<Recipe> GetById(Guid id)
        {
            return (await Recipes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id))?.ToRecipe();
        }

        public Task<IEnumerable<Recipe>> GetAll()
        {
            return Task.FromResult<IEnumerable<Recipe>>(Recipes.AsNoTracking().Select(x => x.ToRecipe()).ToArray());
        }

        public async Task Add(Recipe entity)
        {
            await Recipes.AddAsync(RecipeDatabaseEntity.FromRecipe(entity));
        }

        public Task Update(Recipe entity)
        {
            Recipes.Update(RecipeDatabaseEntity.FromRecipe(entity));
            return Task.CompletedTask;
        }

        public Task Delete(Recipe entity)
        {
            Recipes.Remove(RecipeDatabaseEntity.FromRecipe(entity));
            return Task.CompletedTask;
        }

        public new async Task SaveChanges()
        {
            await SaveChangesAsync();
        }
    }
}