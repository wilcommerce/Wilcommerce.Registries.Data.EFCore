using System;
using System.Threading.Tasks;
using Wilcommerce.Registries.Data.EFCore.Test.Fixtures;
using Wilcommerce.Registries.Models;
using Xunit;

namespace Wilcommerce.Registries.Data.EFCore.Test.Repository
{
    public class RepositoryTest : IClassFixture<RepositoryTestFixture>
    {
        private RepositoryTestFixture _fixture;

        public RepositoryTest(RepositoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_If_Context_Is_Null()
        {
            RegistriesContext context = null;

            var ex = Assert.Throws<ArgumentNullException>(() => new EFCore.Repository.Repository(context));
            Assert.Equal(nameof(context), ex.ParamName);
        }

        [Fact]
        public async Task Add_Customer_Should_Add_The_New_Customer()
        {
            using (var context = _fixture.BuildContext())
            {
                var repository = new EFCore.Repository.Repository(context);

                var customer = Person.Register("Alberto", "Mori", Gender.Male, new DateTime(1987, 8, 24));
                repository.Add(customer);

                await repository.SaveChangesAsync();

                Assert.Contains(customer, context.Customers);

                _fixture.CleanAllData(context);
            }
        }

        [Fact]
        public async Task Add_Customer_With_Shipping_Address_Should_Add_The_New_Customer_And_Shipping_Address()
        {
            using (var context = _fixture.BuildContext())
            {
                var repository = new EFCore.Repository.Repository(context);

                string address = "address";
                string city = "city";
                string postalCode = "postalCode";
                string province = "province";
                string country = "country";

                var customer = Person.Register("Alberto", "Mori", Gender.Male, new DateTime(1987, 8, 24));
                customer.AddShippingAddress(address, city, postalCode, province, country, true);

                repository.Add(customer);

                await repository.SaveChangesAsync();

                Assert.Contains(customer, context.Customers);
                Assert.Collection(context.ShippingAddresses, s =>
                {
                    Assert.Equal(address, s.AddressInfo.Address);
                    Assert.Equal(city, s.AddressInfo.City);
                    Assert.Equal(postalCode, s.AddressInfo.PostalCode);
                    Assert.Equal(province, s.AddressInfo.Province);
                    Assert.Equal(country, s.AddressInfo.Country);
                    Assert.True(s.IsDefault);
                });

                _fixture.CleanAllData(context);
            }
        }

        [Fact]
        public async Task Add_Customer_With_Billing_Information_Should_Add_The_New_Customer_And_Billing_Information()
        {
            using (var context = _fixture.BuildContext())
            {
                var repository = new EFCore.Repository.Repository(context);

                string fullName = "full name";
                string address = "address";
                string city = "city";
                string postalCode = "postalCode";
                string province = "province";
                string country = "country";
                string nationalIdentificationNumber = "1234567890";
                string vatNumber = "1234567890";

                var customer = Company.Register("company", "1234567890");
                customer.AddBillingInformation(fullName, address, city, postalCode, province, country, nationalIdentificationNumber, vatNumber, true);

                repository.Add(customer);

                await repository.SaveChangesAsync();

                Assert.Contains(customer, context.Customers);
                Assert.Collection(context.BillingInfos, b =>
                {
                    Assert.Equal(fullName, b.FullName);
                    Assert.Equal(vatNumber, b.VatNumber);
                    Assert.Equal(nationalIdentificationNumber, b.NationalIdentificationNumber);
                    Assert.Equal(address, b.BillingAddress.Address);
                    Assert.Equal(city, b.BillingAddress.City);
                    Assert.Equal(postalCode, b.BillingAddress.PostalCode);
                    Assert.Equal(province, b.BillingAddress.Province);
                    Assert.Equal(country, b.BillingAddress.Country);
                    Assert.True(b.IsDefault);
                });

                _fixture.CleanAllData(context);
            }
        }

        [Fact]
        public async Task GetByKeyAsync_Should_Return_The_Customer_By_The_Specified_Id()
        {
            using (var context = _fixture.BuildContext())
            {
                var customer = Person.Register("firstName", "lastName", Gender.Female, new DateTime(1980, 1, 1));
                _fixture.PrepareData(context, ctx => ctx.Add(customer));

                var repository = new EFCore.Repository.Repository(context);

                var customerFound = await repository.GetByKeyAsync<Person>(customer.Id);

                Assert.NotNull(customerFound);
                Assert.Equal(customer, customerFound);

                _fixture.CleanAllData(context);
            }
        }

        [Fact]
        public async Task GetByKeyAsync_Should_Return_The_Customer_With_Shipping_Address_By_The_Specified_Id()
        {
            using (var context = _fixture.BuildContext())
            {
                var customer = Person.Register("firstName", "lastName", Gender.Female, new DateTime(1980, 1, 1));

                string address = "address";
                string city = "city";
                string postalCode = "12345";
                string province = "province";
                string country = "italy";
                customer.AddShippingAddress(address, city, postalCode, province, country, true);

                _fixture.PrepareData(context, ctx => ctx.Add(customer));

                var repository = new EFCore.Repository.Repository(context);

                var customerFound = await repository.GetByKeyAsync<Person>(customer.Id);

                Assert.NotNull(customerFound);
                Assert.Equal(customer, customerFound);
                Assert.Collection(customer.ShippingAddresses, s =>
                {
                    Assert.Equal(address, s.AddressInfo.Address);
                    Assert.Equal(city, s.AddressInfo.City);
                    Assert.Equal(postalCode, s.AddressInfo.PostalCode);
                    Assert.Equal(province, s.AddressInfo.Province);
                    Assert.Equal(country, s.AddressInfo.Country);
                    Assert.True(s.IsDefault);
                });

                _fixture.CleanAllData(context);
            }
        }

        [Fact]
        public async Task GetByKeyAsync_Should_Return_The_Customer_With_Billing_Information_By_The_Specified_Id()
        {
            using (var context = _fixture.BuildContext())
            {
                var customer = Company.Register("company", "1234567890");

                string fullName = "full name";
                string address = "address";
                string city = "city";
                string postalCode = "12345";
                string province = "province";
                string country = "italy";
                string nationalIdentificationNumber = "1234567890";
                string vatNumber = "1234567890";
                customer.AddBillingInformation(fullName, address, city, postalCode, province, country, nationalIdentificationNumber, vatNumber, true);

                _fixture.PrepareData(context, ctx => ctx.Add(customer));

                var repository = new EFCore.Repository.Repository(context);

                var customerFound = await repository.GetByKeyAsync<Company>(customer.Id);

                Assert.NotNull(customerFound);
                Assert.Equal(customer, customerFound);
                Assert.Collection(customer.BillingInformation, b =>
                {
                    Assert.Equal(fullName, b.FullName);
                    Assert.Equal(nationalIdentificationNumber, b.NationalIdentificationNumber);
                    Assert.Equal(vatNumber, b.VatNumber);
                    Assert.Equal(address, b.BillingAddress.Address);
                    Assert.Equal(city, b.BillingAddress.City);
                    Assert.Equal(postalCode, b.BillingAddress.PostalCode);
                    Assert.Equal(province, b.BillingAddress.Province);
                    Assert.Equal(country, b.BillingAddress.Country);
                    Assert.True(b.IsDefault);
                });

                _fixture.CleanAllData(context);
            }
        }

        [Fact]
        public void GetByKey_Should_Return_The_Customer_By_The_Specified_Id()
        {
            using (var context = _fixture.BuildContext())
            {
                var customer = Person.Register("firstName", "lastName", Gender.Female, new DateTime(1980, 1, 1));
                _fixture.PrepareData(context, ctx => ctx.Add(customer));

                var repository = new EFCore.Repository.Repository(context);

                var customerFound = repository.GetByKey<Person>(customer.Id);

                Assert.NotNull(customerFound);
                Assert.Equal(customer, customerFound);

                _fixture.CleanAllData(context);
            }
        }

        [Fact]
        public void GetByKey_Should_Return_The_Customer_With_Shipping_Address_By_The_Specified_Id()
        {
            using (var context = _fixture.BuildContext())
            {
                var customer = Person.Register("firstName", "lastName", Gender.Female, new DateTime(1980, 1, 1));

                string address = "address";
                string city = "city";
                string postalCode = "12345";
                string province = "province";
                string country = "italy";
                customer.AddShippingAddress(address, city, postalCode, province, country, true);

                _fixture.PrepareData(context, ctx => ctx.Add(customer));

                var repository = new EFCore.Repository.Repository(context);

                var customerFound = repository.GetByKey<Person>(customer.Id);

                Assert.NotNull(customerFound);
                Assert.Equal(customer, customerFound);
                Assert.Collection(customer.ShippingAddresses, s =>
                {
                    Assert.Equal(address, s.AddressInfo.Address);
                    Assert.Equal(city, s.AddressInfo.City);
                    Assert.Equal(postalCode, s.AddressInfo.PostalCode);
                    Assert.Equal(province, s.AddressInfo.Province);
                    Assert.Equal(country, s.AddressInfo.Country);
                    Assert.True(s.IsDefault);
                });

                _fixture.CleanAllData(context);
            }
        }

        [Fact]
        public void GetByKey_Should_Return_The_Customer_With_Billing_Information_By_The_Specified_Id()
        {
            using (var context = _fixture.BuildContext())
            {
                var customer = Company.Register("company", "1234567890");

                string fullName = "full name";
                string address = "address";
                string city = "city";
                string postalCode = "12345";
                string province = "province";
                string country = "italy";
                string nationalIdentificationNumber = "1234567890";
                string vatNumber = "1234567890";
                customer.AddBillingInformation(fullName, address, city, postalCode, province, country, nationalIdentificationNumber, vatNumber, true);

                _fixture.PrepareData(context, ctx => ctx.Add(customer));

                var repository = new EFCore.Repository.Repository(context);

                var customerFound = repository.GetByKey<Company>(customer.Id);

                Assert.NotNull(customerFound);
                Assert.Equal(customer, customerFound);
                Assert.Collection(customer.BillingInformation, b =>
                {
                    Assert.Equal(fullName, b.FullName);
                    Assert.Equal(nationalIdentificationNumber, b.NationalIdentificationNumber);
                    Assert.Equal(vatNumber, b.VatNumber);
                    Assert.Equal(address, b.BillingAddress.Address);
                    Assert.Equal(city, b.BillingAddress.City);
                    Assert.Equal(postalCode, b.BillingAddress.PostalCode);
                    Assert.Equal(province, b.BillingAddress.Province);
                    Assert.Equal(country, b.BillingAddress.Country);
                    Assert.True(b.IsDefault);
                });

                _fixture.CleanAllData(context);
            }
        }
    }
}
