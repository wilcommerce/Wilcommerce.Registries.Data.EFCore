using Microsoft.EntityFrameworkCore;
using Wilcommerce.Registries.Models;

namespace Wilcommerce.Registries.Data.EFCore.Mapping
{
    /// <summary>
    /// Defines the modelBuilder's extension methods to map the <see cref="BillingInfo"/> class
    /// </summary>
    public static class BillingInfoMapping
    {
        /// <summary>
        /// Extension method. Map the billing info class
        /// </summary>
        /// <param name="modelBuilder">The modelBuilder instance</param>
        /// <returns>The modelBuilder instance</returns>
        public static ModelBuilder MapBillingInfos(this ModelBuilder modelBuilder)
        {
            var billingInfoEntity = modelBuilder.Entity<BillingInfo>();

            billingInfoEntity
                .ToTable("Wilcommerce_BillingInfos")
                .HasKey(b => b.Id);

            billingInfoEntity
                .Property(b => b.NationalIdentificationNumber)
                .HasMaxLength(50);

            billingInfoEntity
                .Property(b => b.VatNumber)
                .HasMaxLength(50);

            billingInfoEntity.OwnsOne(b => b.BillingAddress);

            return modelBuilder;
        }
    }
}
