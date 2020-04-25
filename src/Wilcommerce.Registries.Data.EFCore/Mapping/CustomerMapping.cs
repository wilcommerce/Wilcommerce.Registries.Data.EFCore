using Microsoft.EntityFrameworkCore;
using System;
using Wilcommerce.Registries.Models;

namespace Wilcommerce.Registries.Data.EFCore.Mapping
{
    /// <summary>
    /// Defines the modelBuilder's extension methods to map the <see cref="Customer"/> classes
    /// </summary>
    public static class CustomerMapping
    {
        /// <summary>
        /// Extension method. Map the customer class and its inherited classes (Person and Company)
        /// </summary>
        /// <param name="modelBuilder">The modelBuilder instance</param>
        /// <returns>The modelBuilder instance</returns>
        public static ModelBuilder MapCustomers(this ModelBuilder modelBuilder)
        {
            var customerEntity = modelBuilder.Entity<Customer>();

            customerEntity
                .ToTable("Wilcommerce_Customers")
                .HasKey(c => c.Id);

            customerEntity
                .Property(c => c.NationalIdentificationNumber)
                .HasMaxLength(50);

            customerEntity
                .HasDiscriminator<string>("CustomerType")
                .HasValue<Person>(nameof(Person))
                .HasValue<Company>(nameof(Company));
            customerEntity.Property("CustomerType").HasMaxLength(255);

            customerEntity
                .HasMany(c => c.ShippingAddresses)
                .WithOne(s => s.Customer);

            customerEntity
                .HasMany(c => c.BillingInformation)
                .WithOne(b => b.Customer);

            customerEntity.OwnsOne(c => c.Account);

            customerEntity.Ignore(c => c.HasAccount);

            MapPeople(modelBuilder);
            MapCompanies(modelBuilder);

            return modelBuilder;
        }

        private static void MapPeople(ModelBuilder modelBuilder)
        {
            var personEntity = modelBuilder.Entity<Person>();

            personEntity
                .Property(p => p.FirstName)
                .HasMaxLength(255);

            personEntity
                .Property(p => p.LastName)
                .HasMaxLength(255);

            personEntity
                .Property(p => p.Gender)
                .HasConversion(g => g.ToString(), g => (Gender)Enum.Parse(typeof(Gender), g));
        }

        private static void MapCompanies(ModelBuilder modelBuilder)
        {
            var companyEntity = modelBuilder.Entity<Company>();

            companyEntity
                .Property(c => c.CompanyName)
                .HasMaxLength(255);

            companyEntity
                .Property(c => c.VatNumber)
                .HasMaxLength(50);

            companyEntity.OwnsOne(c => c.LegalAddress);
        }
    }
}
