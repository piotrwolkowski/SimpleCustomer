using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleCustomer.Core;
using SimpleCustomer.Data;
using System;
using System.Linq;
using System.Threading;

namespace SimpleCustomer.Test
{
    [TestClass]
    public class SimpleCustomerDataTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            using (CustomersContext context = new CustomersContext())
            {
                if (context.Database.Exists())
                {
                    context.Database.Delete();
                }
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            using (CustomersContext context = new CustomersContext())
            {
                context.Database.CreateIfNotExists();
            }
        }

        [TestMethod]
        public void ValidCustomerIsSuccessfullyAddedToDb()
        {
            // Add customer to DB.
            Customer customer = TestHelpers.GetSampleCustomer();
            DataProxy dataProxy = new DataProxy();
            DateTime? result = dataProxy.AddCustomer(customer).Result;

            // Connect to CustomersContext to check if it exists in the DB.
            using (CustomersContext context = new CustomersContext())
            {
                bool exists = context.CustomerDbs.Any(c => c.Id == customer.Id);
                Assert.IsTrue(exists);
            }
        }

        [TestMethod]
        public void ValidCustomerIsSuccessfullyAddedToDb2()
        {
            // Add customer to DB.
            Customer customer = TestHelpers.GetSampleCustomer();
            DataProxy dataProxy = new DataProxy();
            DateTime? result = dataProxy.AddCustomer(customer).Result;

            // Connect to CustomersContext to check if it exists in the DB.
            using (CustomersContext context = new CustomersContext())
            {
                bool exists = context.CustomerDbs.Any(c => c.Id == customer.Id);
                Assert.IsTrue(exists);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddingNullCustomerThrowsException()
        {
            Customer customer = null;
            DataProxy dataProxy = new DataProxy();
            // Unwraps exception as await is ignored by the test framework
            DateTime? result = dataProxy.AddCustomer(customer).GetAwaiter().GetResult();
        }

        [TestMethod]
        [ExpectedException(typeof(OperationCanceledException))]
        public void CancellingAddingThrowsOperationCanceledException()
        {
            Customer customer = TestHelpers.GetSampleCustomer();
            DataProxy dataProxy = new DataProxy();
            CancellationTokenSource cancelSource = new CancellationTokenSource();
            cancelSource.Cancel();
            // Unwraps exception as await is ignored by the test framework
            DateTime? result = dataProxy.AddCustomer(customer, cancelSource.Token).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void InvalidCustomerIsNotAddedToDb()
        {
            // Add invalid customer to DB.
            Customer customer = TestHelpers.GetSampleCustomer();
            customer.Name = TestHelpers.STRING_LONGER_THAN_50_CHARS;
            DataProxy dataProxy = new DataProxy();
            DateTime? result = dataProxy.AddCustomer(customer).Result;

            // Connect to CustomersContext to check if it exists in the DB.
            using (CustomersContext context = new CustomersContext())
            {
                bool exists = context.CustomerDbs.Any(c => c.Id == customer.Id);
                Assert.IsFalse(exists);
            }
        }

        [TestMethod]
        public void UpdatingExistingUserWithValidDataIsSuccessful()
        {
            Customer customer = TestHelpers.GetSampleCustomer();
            DataProxy dataProxy = new DataProxy();
            DateTime? result = dataProxy.AddCustomer(customer).Result;
            customer.Name = "New Name";
            DateTime? updateResult = dataProxy.UpdateCustomer(customer).Result;

            Assert.IsNotNull(updateResult);

            // Check database if the user with id has specified name
            using (CustomersContext context = new CustomersContext())
            {
                bool existsWithUpdatedName = context.CustomerDbs
                    .Any(c => c.Id == customer.Id && c.Name == customer.Name);
                Assert.IsTrue(existsWithUpdatedName);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdatingNullCustomerThrowsException()
        {
            Customer customer = null;
            DataProxy dataProxy = new DataProxy();
            // Unwraps exception as await is ignored by the test framework
            DateTime? result = dataProxy.UpdateCustomer(customer).GetAwaiter().GetResult();
        }

        [TestMethod]
        [ExpectedException(typeof(OperationCanceledException))]
        public void CancellingUpdatingThrowsOperationCanceledException()
        {
            DataProxy dataProxy = new DataProxy();
            Customer customer = TestHelpers.GetSampleCustomer();
            DateTime? result = dataProxy.AddCustomer(customer).Result;
            customer.Name = "New Name";
            CancellationTokenSource cancelSource = new CancellationTokenSource();
            cancelSource.Cancel();
            // Unwraps exception as await is ignored by the test framework
            DateTime? result2 = dataProxy.UpdateCustomer(customer, cancelSource.Token).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void UpdatingExistingUserWithInvalidDataFails()
        {
            Customer customer = TestHelpers.GetSampleCustomer();
            DataProxy dataProxy = new DataProxy();
            DateTime? result = dataProxy.AddCustomer(customer).Result;
            customer.Name = TestHelpers.STRING_LONGER_THAN_50_CHARS;
            DateTime? updateResult = dataProxy.UpdateCustomer(customer).Result;

            Assert.IsNull(updateResult);

            // Check database if the user with id has specified name
            using (CustomersContext context = new CustomersContext())
            {
                bool existsWithUpdatedName = context.CustomerDbs
                    .Any(c => c.Id == customer.Id && c.Name == customer.Name);
                Assert.IsFalse(existsWithUpdatedName);
            }
        }

        [TestMethod]
        public void RemovingCustomerFromDbIsSuccessful()
        {
            Customer customer = TestHelpers.GetSampleCustomer();
            DataProxy dataProxy = new DataProxy();
            DateTime? result = dataProxy.AddCustomer(customer).Result;
            DateTime? removeResult = dataProxy.RemoveCustomer(customer).Result;

            Assert.IsNotNull(removeResult);
        }

        [TestMethod]
        public void RemovingCustomerThatIsNotInDbFails()
        {
            Customer customer = TestHelpers.GetSampleCustomer();
            DataProxy dataProxy = new DataProxy();
            DateTime? result = dataProxy.AddCustomer(customer).Result;

            // remove same customer twice so the second time it will fail.
            DateTime? removeResult = dataProxy.RemoveCustomer(customer).Result;
            Assert.IsNotNull(removeResult);
            DateTime? removeResult2 = dataProxy.RemoveCustomer(customer).Result;

            Assert.IsNull(removeResult2);
        }
    }
}
