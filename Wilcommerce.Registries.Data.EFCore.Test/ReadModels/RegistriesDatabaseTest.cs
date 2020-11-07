using System;
using System.Linq;
using Wilcommerce.Registries.Data.EFCore.ReadModels;
using Wilcommerce.Registries.Data.EFCore.Test.Fixtures;
using Xunit;

namespace Wilcommerce.Registries.Data.EFCore.Test.ReadModels
{
    public class RegistriesDatabaseTest : IClassFixture<RegistriesDatabaseFixture>
    {
        private RegistriesDatabaseFixture _fixture;

        public RegistriesDatabaseTest(RegistriesDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_If_RegistriesContext_Is_Null()
        {
            RegistriesContext context = null;

            var ex = Assert.Throws<ArgumentNullException>(() => new RegistriesDatabase(context));
            Assert.Equal(nameof(context), ex.ParamName);
        }

        [Fact]
        public void Customers_Should_Return_The_List_Of_Customers()
        {
            var database = new RegistriesDatabase(_fixture.Context);
            var customers = database.Customers;

            Assert.Equal(_fixture.Context.Customers.Count(), customers.Count());
        }

        [Fact]
        public void ShippingAddresses_Should_Return_The_List_Of_Shipping_Addresses()
        {
            var database = new RegistriesDatabase(_fixture.Context);
            var shippingAddresses = database.ShippingAddresses;

            Assert.Equal(_fixture.Context.ShippingAddresses.Count(), shippingAddresses.Count());
        }

        [Fact]
        public void BillingInfos_Should_Return_The_List_Of_Billing_Information()
        {
            var database = new RegistriesDatabase(_fixture.Context);
            var billingInfos = database.BillingInfos;

            Assert.Equal(_fixture.Context.BillingInfos, billingInfos);
        }
    }
}
