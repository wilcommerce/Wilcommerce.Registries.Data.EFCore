using System;
using System.Threading.Tasks;
using Wilcommerce.Registries.Repository;

namespace Wilcommerce.Registries.Data.EFCore.Repository
{
    /// <summary>
    /// Implementation of <see cref="IRepository"/>
    /// </summary>
    public class Repository : IRepository
    {
        /// <summary>
        /// The DbContext instance
        /// </summary>
        protected RegistriesContext _context;

        /// <summary>
        /// Construct the repository the registries context
        /// </summary>
        /// <param name="context">The registries context instance</param>
        public Repository(RegistriesContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Dispose all the resources used
        /// </summary>
        public void Dispose() => _context?.Dispose();

        /// <summary>
        /// Saves all the changes made on the aggregate
        /// </summary>
        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Async method. Saves all the changes made on the aggregate
        /// </summary>
        /// <returns></returns>
        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// Add an aggregate to the repository
        /// </summary>
        /// <typeparam name="TModel">The aggregate's type</typeparam>
        /// <param name="model">The aggregate to add</param>
        public void Add<TModel>(TModel model) where TModel : class, Core.Infrastructure.IAggregateRoot
        {
            try
            {
                _context.Set<TModel>().Add(model);
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the aggregate based on the specified key
        /// </summary>
        /// <typeparam name="TModel">The aggregate's type</typeparam>
        /// <param name="key">The key of the aggregate to search</param>
        /// <returns>The aggregate found</returns>
        public TModel GetByKey<TModel>(Guid key) where TModel : class, Core.Infrastructure.IAggregateRoot
        {
            try
            {
                return _context.Find<TModel>(key);
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// Async method. Gets the aggregate based on the specified key
        /// </summary>
        /// <typeparam name="TModel">The aggregate's type</typeparam>
        /// <param name="key">The key of the aggregate to search</param>
        /// <returns>The aggregate found</returns>
        public async Task<TModel> GetByKeyAsync<TModel>(Guid key) where TModel : class, Core.Infrastructure.IAggregateRoot
        {
            try
            {
                var model = await _context.FindAsync<TModel>(key);
                return model;
            }
            catch 
            {
                throw;
            }
        }
    }
}
