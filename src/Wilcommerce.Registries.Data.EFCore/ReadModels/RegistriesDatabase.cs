using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Wilcommerce.Registries.Models;
using Wilcommerce.Registries.ReadModels;

namespace Wilcommerce.Registries.Data.EFCore.ReadModels
{
    /// <summary>
    /// Implementation of <see cref="IRegistriesDatabase"/>
    /// </summary>
    public class RegistriesDatabase : IRegistriesDatabase
    {
        /// <summary>
        /// The DbContext instance
        /// </summary>
        protected RegistriesContext _context;

        /// <summary>
        /// Construct the registries database
        /// </summary>
        /// <param name="context">The instance of the registries context</param>
        public RegistriesDatabase(RegistriesContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Get the list of customers
        /// </summary>
        public IQueryable<Customer> Customers => _context.Customers.AsNoTracking();

        /// <summary>
        /// Get the list of shipping addresses
        /// </summary>
        public IQueryable<ShippingAddress> ShippingAddresses => _context.ShippingAddresses.AsNoTracking();

        /// <summary>
        /// Get the list of billing infos
        /// </summary>
        public IQueryable<BillingInfo> BillingInfos => _context.BillingInfos.AsNoTracking();
    }
}
