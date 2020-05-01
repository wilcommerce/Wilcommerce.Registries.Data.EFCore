using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;

namespace Wilcommerce.Registries.Data.EFCore.Test.Fixtures
{
    public class RepositoryTestFixture : IDisposable
    {
        private DbContextOptions<RegistriesContext> _contextOptions;

        public RepositoryTestFixture()
        {
            BuildContextOptions();
        }

        public RegistriesContext BuildContext() => new RegistriesContext(this._contextOptions);

        public void PrepareData(RegistriesContext context, Action<RegistriesContext> prepare)
        {
            prepare.Invoke(context);
            context.SaveChanges();
        }

        public void CleanAllData(RegistriesContext context)
        {
            if (context.ShippingAddresses.Any())
            {
                context.ShippingAddresses.RemoveRange(context.ShippingAddresses);
            }

            if (context.BillingInfos.Any())
            {
                context.BillingInfos.RemoveRange(context.BillingInfos);
            }

            if (context.Customers.Any())
            {
                context.Customers.RemoveRange(context.Customers);
            }

            context.SaveChanges();
        }

        public void Dispose()
        {

        }

        protected virtual void BuildContextOptions()
        {
            var options = new DbContextOptionsBuilder<RegistriesContext>()
                .UseInMemoryDatabase(databaseName: "InMemory-Registries")
                .Options;

            this._contextOptions = options;
        }
    }
}
