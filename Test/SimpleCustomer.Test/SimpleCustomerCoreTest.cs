using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleCustomer.Core;

namespace SimpleCustomer.Test
{
    [TestClass]
    public class SimpleCustomerCoreTest
    {
        [TestMethod]
        public void NameLongerThan50CharactersIsInvalid()
        {
            Customer customer = TestHelpers.GetSampleCustomer();
            Assert.IsTrue(customer.IsValid);

            customer.Name = TestHelpers.STRING_LONGER_THAN_50_CHARS;
            Assert.IsFalse(customer.IsValid);
        }

        [TestMethod]
        public void EmptyNameIsInvalid()
        {
            Customer customer = TestHelpers.GetSampleCustomer();
            Assert.IsTrue(customer.IsValid);

            // Neither empty...
            customer.Name = string.Empty;
            Assert.IsFalse(customer.IsValid);

            // nor white space is valid
            customer.Name = "    ";
            Assert.IsFalse(customer.IsValid);
        }

        [TestMethod]
        public void CountryLongerThan50CharactersIsInvalid()
        {
            Customer customer = TestHelpers.GetSampleCustomer();
            Assert.IsTrue(customer.IsValid);

            customer.Country = TestHelpers.STRING_LONGER_THAN_50_CHARS;
            Assert.IsFalse(customer.IsValid);
        }

        [TestMethod]
        public void EmptyCountryIsInvalid()
        {
            Customer customer = TestHelpers.GetSampleCustomer();
            Assert.IsTrue(customer.IsValid);

            // Neither empty...
            customer.Country = string.Empty;
            Assert.IsFalse(customer.IsValid);

            // nor white space is valid
            customer.Country = "    ";
            Assert.IsFalse(customer.IsValid);
        }

        [TestMethod]
        public void StateLongerThan50CharactersIsInvalid()
        {
            Customer customer = TestHelpers.GetSampleCustomer();
            Assert.IsTrue(customer.IsValid);

            customer.State = TestHelpers.STRING_LONGER_THAN_50_CHARS;
            Assert.IsFalse(customer.IsValid);
        }

        [TestMethod]
        public void EmptyStateIsInvalid()
        {
            Customer customer = TestHelpers.GetSampleCustomer();
            Assert.IsTrue(customer.IsValid);

            // Neither empty...
            customer.State = string.Empty;
            Assert.IsFalse(customer.IsValid);

            // nor white space is valid
            customer.State = "    ";
            Assert.IsFalse(customer.IsValid);
        }

        [TestMethod]
        public void AddressLongerThan255CharactersIsInvalid()
        {
            Customer customer = TestHelpers.GetSampleCustomer();
            Assert.IsTrue(customer.IsValid);

            customer.AddressLineOne = TestHelpers.STRING_LONGER_THAN_255_CHARS;
            Assert.IsFalse(customer.IsValid);
        }

        [TestMethod]
        public void EmptyAddressIsInvalid()
        {
            Customer customer = TestHelpers.GetSampleCustomer();
            Assert.IsTrue(customer.IsValid);

            // Neither empty...
            customer.AddressLineOne = string.Empty;
            Assert.IsFalse(customer.IsValid);

            // nor white space is valid
            customer.AddressLineOne = "    ";
            Assert.IsFalse(customer.IsValid);
        }

        [TestMethod]
        public void HouseNumberLongerThan50CharactersIsInvalid()
        {
            Customer customer = TestHelpers.GetSampleCustomer();
            Assert.IsTrue(customer.IsValid);

            customer.HouseNumber = TestHelpers.STRING_LONGER_THAN_50_CHARS;
            Assert.IsFalse(customer.IsValid);
        }

        [TestMethod]
        public void EmptyHouseNumberIsInvalid()
        {
            Customer customer = TestHelpers.GetSampleCustomer();
            Assert.IsTrue(customer.IsValid);

            // Neither empty...
            customer.HouseNumber = string.Empty;
            Assert.IsFalse(customer.IsValid);

            // nor white space is valid
            customer.HouseNumber = "    ";
            Assert.IsFalse(customer.IsValid);
        }

        [TestMethod]
        public void DateOfBirthFromFutureIsInvalid()
        {
            Customer customer = TestHelpers.GetSampleCustomer();
            Assert.IsTrue(customer.IsValid);

            customer.DateOfBirth = new DateTime(DateTime.Now.Year + 10, 3, 3);
            Assert.IsFalse(customer.IsValid);
        }

        [TestMethod]
        public void DateOfBirthFrom180YearAgoIsInvalid()
        {
            Customer customer = TestHelpers.GetSampleCustomer();
            Assert.IsTrue(customer.IsValid);

            customer.DateOfBirth = new DateTime(DateTime.Now.Year - 181, DateTime.Now.Month, DateTime.Now.Day);
            Assert.IsFalse(customer.IsValid);
        }

        [TestMethod]
        public void DateOfBirthFromLessThan180YearAgoIsValid()
        {
            Customer customer = TestHelpers.GetSampleCustomer();
            Assert.IsTrue(customer.IsValid);

            customer.DateOfBirth = new DateTime(DateTime.Now.Year - 179, DateTime.Now.Month, DateTime.Now.Day);
            Assert.IsTrue(customer.IsValid);
        }
    }
}
