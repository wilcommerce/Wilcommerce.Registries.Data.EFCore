using Microsoft.EntityFrameworkCore;
using Wilcommerce.Registries.Models;

namespace Wilcommerce.Registries.Data.EFCore.Mapping
{
    /// <summary>
    /// Defines the modelBuilder's extension methods to map the <see cref="ShippingAddress"/> class
    /// </summary>
    public static class ShippingAddressMapping
    {
        /// <summary>
        /// Extension method. Map the shipping address class
        /// </summary>
        /// <param name="modelBuilder">The modelBuilder instance</param>
        /// <returns>The modelBuilder instance</returns>
        public static ModelBuilder MapShippingAddresses(this ModelBuilder modelBuilder)
        {
            var shippingAddressEntity = modelBuilder.Entity<ShippingAddress>();
            
            shippingAddressEntity
                .ToTable("Wilcommerce_ShippingAddresses")
                .HasKey(s => s.Id);

            shippingAddressEntity.OwnsOne(s => s.AddressInfo);

            return modelBuilder;
        }
    }
}
