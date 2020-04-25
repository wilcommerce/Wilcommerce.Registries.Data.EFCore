using Microsoft.EntityFrameworkCore;
using Wilcommerce.Registries.Models;

namespace Wilcommerce.Registries.Data.EFCore
{
    /// <summary>
    /// Defines the Entity Framework context for the registries package
    /// </summary>
    public class RegistriesContext : DbContext
    {
        /// <summary>
        /// Get or set the list of customers
        /// </summary>
        public virtual DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// Get or set the list of people
        /// </summary>
        public virtual DbSet<Person> People { get; set; }

        /// <summary>
        /// Get or set the list of companies
        /// </summary>
        public virtual DbSet<Company> Companies { get; set; }

        /// <summary>
        /// Get or set the list of shipping addresses
        /// </summary>
        public virtual DbSet<ShippingAddress> ShippingAddresses { get; set; }

        /// <summary>
        /// Get or set the list of billing information
        /// </summary>
        public virtual DbSet<BillingInfo> BillingInfos { get; set; }

        /// <summary>
        /// Construct the registries context
        /// </summary>
        /// <param name="options">The context options</param>
        public RegistriesContext(DbContextOptions<RegistriesContext> options)
            : base(options)
        {

        }

        /// <summary>
        /// Override the <see cref="DbContext.OnConfiguring(DbContextOptionsBuilder)"/>
        /// </summary>
        /// <param name="optionsBuilder">The options builder instance</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
