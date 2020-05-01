using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wilcommerce.Registries.Models;

namespace Wilcommerce.Registries.Data.EFCore.Test.Fixtures
{
    public class RegistriesDatabaseFixture : IDisposable
    {
        public RegistriesContext Context { get; private set; }

        public RegistriesDatabaseFixture()
        {
            BuildContext();
            PrepareData();
        }

        public void Dispose()
        {
            CleanData();
            Context?.Dispose();
        }

        private void BuildContext()
        {
            var options = new DbContextOptionsBuilder<RegistriesContext>()
                .UseInMemoryDatabase(databaseName: "InMemory-ReadModels-Registries")
                .Options;

            Context = new RegistriesContext(options);
        }

        private void PrepareData()
        {
            Context.Customers.AddRange(new Customer[]
            {
                Person.Register("First", "Last", Gender.Female, new DateTime(1980, 1, 1)),
                Person.RegisterWithAccount("firstname", "lastname", Gender.Male, new DateTime(1980, 1, 1), Guid.NewGuid(), "user"),
                Company.Register("company", "1234567890"),
                Company.RegisterWithAccount("company2", "7894561230", Guid.NewGuid(), "companyuser")
            });

            var customer = Person.Register("firstname", "lastname", Gender.Female, new DateTime(1980, 1, 1));
            customer.AddShippingAddress("address", "city", "12345", "province", "italy", true);

            Context.Customers.Add(customer);

            Context.SaveChanges();
        }

        private void CleanData()
        {
            if (Context.ShippingAddresses.Any())
            {
                Context.ShippingAddresses.RemoveRange(Context.ShippingAddresses);
            }

            if (Context.BillingInfos.Any())
            {
                Context.BillingInfos.RemoveRange(Context.BillingInfos);
            }

            if (Context.Customers.Any())
            {
                Context.Customers.RemoveRange(Context.Customers);
            }

            Context.SaveChanges();
        }
    }
}
