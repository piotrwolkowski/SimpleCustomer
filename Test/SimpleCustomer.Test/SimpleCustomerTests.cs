using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleCustomer.Core;
using SimpleCustomer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCustomer.Test
{
    [TestClass]
    public class SimpleCustomerTests
    {
        [TestMethod]
        public void SettingCustomerToMainViewModelTriggersPropertyChanged()
        {
            Customer customer = new Customer();
            MainViewModel viewModel = new MainViewModel();
            viewModel.PropertyChanged += (s, e) =>
            {
                Assert.AreEqual("SelectedCustomer", e.PropertyName);
                Assert.AreEqual(customer, viewModel.SelectedCustomer);
            };
            viewModel.SelectedCustomer = customer;
        }

        [TestMethod]
        public void SettingCustomersToMainViewModelTriggersPropertyChanged()
        {
            List<Customer> customers = new List<Customer>()
            {
                new Customer() {Name="A"},
                new Customer() {Name="B"},
                new Customer() {Name="C"}
            };
            MainViewModel viewModel = new MainViewModel();
            viewModel.PropertyChanged += (s, e) =>
            {
                Assert.AreEqual("Customers", e.PropertyName);
                Assert.AreEqual(customers, viewModel.Customers);
            };
            viewModel.Customers = customers;
        }

        [TestMethod]
        public void SettingCustomerToAddCustomerViewModelTriggersPropertyChanged()
        {
            Customer customer = new Customer();
            AddCustomerViewModel viewModel = new AddCustomerViewModel(customer);
            viewModel.PropertyChanged += (s, e) =>
            {
                Assert.AreEqual("SelectedCustomer", e.PropertyName);
                Assert.AreEqual(customer, viewModel.SelectedCustomer);
            };
            viewModel.SelectedCustomer = customer;
        }

        [TestMethod]
        public void SettingValueOnModificationViewModelTriggersPropertyChanged()
        {
            string modficationHistoryTest = "Modification history test";
            ModificationHistoryViewModel viewModel = new ModificationHistoryViewModel(modficationHistoryTest);
            viewModel.PropertyChanged += (s, e) =>
            {
                Assert.AreEqual("ModificationHistoryText", e.PropertyName);
                Assert.AreEqual(modficationHistoryTest, viewModel.ModificationHistoryText);
            };
            viewModel.ModificationHistoryText = modficationHistoryTest;
        }

        [TestMethod]
        public void IfNoUserIsSelectedUpdateCommandDoesNotExecute()
        {
            Customer customer = new Customer();
            MainViewModel viewModel = new MainViewModel();
            Assert.IsFalse(viewModel.UpdateCustomerCommand.CanExecute(null));
            viewModel.SelectedCustomer = customer;
            Assert.IsTrue(viewModel.UpdateCustomerCommand.CanExecute(null));
        }

        [TestMethod]
        public void IfNoUserIsSelectedDeleteCommandDoesNotExecute()
        {
            Customer customer = new Customer();
            MainViewModel viewModel = new MainViewModel();
            Assert.IsFalse(viewModel.DeleteCustomerCommand.CanExecute(null));
            viewModel.SelectedCustomer = customer;
            Assert.IsTrue(viewModel.DeleteCustomerCommand.CanExecute(null));
        }

        [TestMethod]
        public void IfUserListEmptyShowStatsCommandDoesNotExecute()
        {
            Customer customer = new Customer();
            MainViewModel viewModel = new MainViewModel();
            Assert.IsFalse(viewModel.ShowStatsCommand.CanExecute(null));
            viewModel.Customers = new List<Customer> { customer };
            Assert.IsTrue(viewModel.ShowStatsCommand.CanExecute(null));
        }

        [TestMethod]
        public void SaveCommandDoesNotExecuteIfCustomerIsInvalid()
        {
            Customer customer = TestHelpers.GetSampleCustomer();

            AddCustomerViewModel viewModel = new AddCustomerViewModel(customer);
            Assert.IsTrue(customer.IsValid);

            // Invalid address should disable save command.
            Assert.IsTrue(viewModel.SaveCommand.CanExecute(null));
            customer.AddressLineOne = TestHelpers.STRING_LONGER_THAN_255_CHARS;
            Assert.IsFalse(customer.IsValid);
            Assert.IsFalse(viewModel.SaveCommand.CanExecute(null));
            customer.AddressLineOne = "Sample Address";
            Assert.IsTrue(customer.IsValid);

            // Invalid date should disable save command.
            Assert.IsTrue(viewModel.SaveCommand.CanExecute(null));
            customer.DateOfBirth = new DateTime(1410, 07, 21);
            Assert.IsFalse(customer.IsValid);
            Assert.IsFalse(viewModel.SaveCommand.CanExecute(null));
            customer.DateOfBirth = new DateTime(1985, 7, 8);
            Assert.IsTrue(customer.IsValid);

            // Invalid name should disable save command.
            Assert.IsTrue(viewModel.SaveCommand.CanExecute(null));
            customer.Name = TestHelpers.STRING_LONGER_THAN_50_CHARS;
            Assert.IsFalse(customer.IsValid);
            Assert.IsFalse(viewModel.SaveCommand.CanExecute(null));
            customer.Name = "Test Name";
            Assert.IsTrue(customer.IsValid);
        }

        [TestMethod]
        public void FilteringByNameReturnsSubsetOfResults()
        {
            List<Customer> customers = TestHelpers.GetSampleCustomersList();
            MainViewModel viewModel = new MainViewModel();
            
            // CustomersBuckup is private field to keep data unchanged
            // and should not be exposed.
            // Use reflection for the purpose of the test.
            System.Reflection.FieldInfo fieldInfo = typeof(MainViewModel).GetField("customersBuckup",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            Assert.IsNotNull(fieldInfo);

            fieldInfo.SetValue(viewModel, customers);

            viewModel.Customers = customers;
            viewModel.Filter = "a";

            List<Customer> customersWithAInName = customers.Where(c => c.Name.Contains("a")).ToList();
            CollectionAssert.AreEquivalent(customersWithAInName, viewModel.Customers.ToList());
            CollectionAssert.IsSubsetOf(viewModel.Customers.ToList(), customers);

            // sanity check if the result is not always true
            CollectionAssert.IsNotSubsetOf(customers, viewModel.Customers.ToList());

            // empty fielter should bring back all the customers
            viewModel.Filter = string.Empty;
            CollectionAssert.AreEquivalent(viewModel.Customers.ToList(), customers);
        }

        [TestMethod]
        public void StatsByLocationReturnCorrectNumberOfUsersPerLocation()
        {
            List<Customer> customers = TestHelpers.GetSampleCustomersList();

            StatsViewModel viewModel = new StatsViewModel(customers);
            viewModel.SelectedStatsType = StatsType.Location;
            
            List<KeyValuePair<string, decimal>> expectedList = new List<KeyValuePair<string, decimal>>()
            {
                new KeyValuePair<string,decimal>("United States, Alaska", 2),
                new KeyValuePair<string,decimal>("United States, Florida", 1),
                new KeyValuePair<string,decimal>("United States, Washington", 1),
            };
            CollectionAssert.AreEquivalent(expectedList, viewModel.StatsData);

            List<KeyValuePair<string, decimal>> incorrectList = new List<KeyValuePair<string, decimal>>()
            {
                new KeyValuePair<string,decimal>("UK, Surrey", 1),
                new KeyValuePair<string,decimal>("United States, Florida", 1),
                new KeyValuePair<string,decimal>("United States, Washington", 1),
            };

            CollectionAssert.AreNotEquivalent(incorrectList, viewModel.StatsData);
        }

        [TestMethod]
        public void StatsByLocationReturnComaAndSpaceSeparateCountryAndStateDescription()
        {
            Customer customer = TestHelpers.GetSampleCustomer();
            List<Customer> customers = new List<Customer>() { customer };

            StatsViewModel viewModel = new StatsViewModel(customers);
            viewModel.SelectedStatsType = StatsType.Location;

            string expectedDescription = customer.Country + ", " + customer.State;
            decimal expectedValue = 1m;
            List<KeyValuePair<string, decimal>> expectedResult = new List<KeyValuePair<string, decimal>>()
            {
                new KeyValuePair<string,decimal>(expectedDescription, expectedValue)
            };
            CollectionAssert.AreEquivalent(expectedResult, viewModel.StatsData);

            List<KeyValuePair<string, decimal>> incorrectList = new List<KeyValuePair<string, decimal>>()
            {
                new KeyValuePair<string,decimal>("United States, Alaska ", 1)
            };
            CollectionAssert.AreNotEquivalent(incorrectList, viewModel.StatsData);

            List<KeyValuePair<string, decimal>> incorrectList2 = new List<KeyValuePair<string, decimal>>()
            {
                new KeyValuePair<string,decimal>("United States,Alaska",1)
            };
            CollectionAssert.AreNotEquivalent(incorrectList2, viewModel.StatsData);

            List<KeyValuePair<string, decimal>> incorrectList3 = new List<KeyValuePair<string, decimal>>()
            {
                new KeyValuePair<string,decimal>("Alaska, United States",1)
            };
            CollectionAssert.AreNotEquivalent(incorrectList3, viewModel.StatsData);
        }

        [TestMethod]
        public void StatsByCategoryReturnCorrectNumberOfUsersPerCategory()
        {
            List<Customer> customers = TestHelpers.GetSampleCustomersList();

            StatsViewModel viewModel = new StatsViewModel(customers);
            viewModel.SelectedStatsType = StatsType.Category;

            List<KeyValuePair<string, decimal>> expectedList = new List<KeyValuePair<string, decimal>>()
            {
                new KeyValuePair<string,decimal>("A", 2),
                new KeyValuePair<string,decimal>("B", 2),
            };
            CollectionAssert.AreEquivalent(expectedList, viewModel.StatsData);

            List<KeyValuePair<string, decimal>> incorrectList = new List<KeyValuePair<string, decimal>>()
            {
                new KeyValuePair<string,decimal>("A", 1),
                new KeyValuePair<string,decimal>("B", 1),
                new KeyValuePair<string,decimal>("C", 1),
            };

            CollectionAssert.AreNotEquivalent(incorrectList, viewModel.StatsData);
        }

        [TestMethod]
        public void StatsByLocationPercentShouldToupleWithDescriptionValuePercentage()
        {
            List<Customer> customers = TestHelpers.GetSampleCustomersList();

            StatsViewModel viewModel = new StatsViewModel(customers);
            viewModel.SelectedStatsType = StatsType.Location;

            List<Tuple<string, decimal, decimal>> expectedList = new List<Tuple<string, decimal, decimal>>()
            {
                new Tuple<string,decimal,decimal>("United States, Alaska", 2, 50),
                new Tuple<string,decimal,decimal>("United States, Florida", 1, 25),
                new Tuple<string,decimal,decimal>("United States, Washington", 1, 25)
            };
            CollectionAssert.AreEquivalent(expectedList, viewModel.StatsDataPercent);

            List<Tuple<string, decimal, decimal>> incorrectList = new List<Tuple<string, decimal, decimal>>()
            {
                new Tuple<string,decimal,decimal>("United States, Alaska", 2, 0),
                new Tuple<string,decimal,decimal>("United States, Florida", 1, 0),
                new Tuple<string,decimal,decimal>("United States, Washington", 1, 0)
            };

            CollectionAssert.AreNotEquivalent(incorrectList, viewModel.StatsData);
        }

        [TestMethod]
        public void StatsByCategoryPercentShouldToupleWithDescriptionValuePercentage()
        {
            List<Customer> customers = TestHelpers.GetSampleCustomersList();

            StatsViewModel viewModel = new StatsViewModel(customers);
            viewModel.SelectedStatsType = StatsType.Category;

            List<Tuple<string, decimal, decimal>> expectedList = new List<Tuple<string, decimal, decimal>>()
            {
                new Tuple<string,decimal,decimal>("A", 2, 50),
                new Tuple<string,decimal,decimal>("B", 2, 50),
            };
            CollectionAssert.AreEquivalent(expectedList, viewModel.StatsDataPercent);

            List<Tuple<string, decimal, decimal>> incorrectList = new List<Tuple<string, decimal, decimal>>()
            {
                new Tuple<string,decimal,decimal>("A", 2, 75),
                new Tuple<string,decimal,decimal>("B", 2, 25),
            };

            CollectionAssert.AreNotEquivalent(incorrectList, viewModel.StatsData);
        }

        [TestMethod]
        public void StatsByCategoryPercentRoundValueToTwoDecimalPlaces()
        {
            List<Customer> customers = new List<Customer>()
            {
                TestHelpers.GetSampleCustomer(),
                TestHelpers.GetSampleCustomer(),
                TestHelpers.GetSampleCustomer(),
            };

            customers[0].Category = Category.A;
            customers[1].Category = Category.B;
            customers[2].Category = Category.C;

            StatsViewModel viewModel = new StatsViewModel(customers);
            viewModel.SelectedStatsType = StatsType.Category;

            List<Tuple<string, decimal, decimal>> expectedList = new List<Tuple<string, decimal, decimal>>()
            {
                new Tuple<string,decimal,decimal>("A", 1, 33.33m),
                new Tuple<string,decimal,decimal>("B", 1, 33.33m),
                new Tuple<string,decimal,decimal>("C", 1, 33.33m),
            };
            CollectionAssert.AreEquivalent(expectedList, viewModel.StatsDataPercent);
        }

        [TestMethod]
        public void StatsByCategoryPercentRoundValueToTwoDecimalPlaces2()
        {
            List<Customer> customers = new List<Customer>()
            {
                TestHelpers.GetSampleCustomer(),
                TestHelpers.GetSampleCustomer(),
                TestHelpers.GetSampleCustomer(),
            };

            customers[0].Category = Category.A;
            customers[1].Category = Category.B;
            customers[2].Category = Category.B;

            StatsViewModel viewModel = new StatsViewModel(customers);
            viewModel.SelectedStatsType = StatsType.Category;

            List<Tuple<string, decimal, decimal>> expectedList = new List<Tuple<string, decimal, decimal>>()
            {
                new Tuple<string,decimal,decimal>("A", 1, 33.33m),
                new Tuple<string,decimal,decimal>("B", 2, 66.67m),
            };
            CollectionAssert.AreEquivalent(expectedList, viewModel.StatsDataPercent);
        }

        [TestMethod]
        public void StatsByLocationPercentRoundValueToTwoDecimalPlaces()
        {
            List<Customer> customers = new List<Customer>()
            {
                TestHelpers.GetSampleCustomer(),
                TestHelpers.GetSampleCustomer(),
                TestHelpers.GetSampleCustomer(),
            };

            customers[0].Country = "United States";
            customers[1].Country = "United States";
            customers[2].Country = "United States";

            customers[0].State = "Washington";
            customers[1].State = "Florida";
            customers[2].State = "Alaska";

            StatsViewModel viewModel = new StatsViewModel(customers);
            viewModel.SelectedStatsType = StatsType.Location;

            List<Tuple<string, decimal, decimal>> expectedList = new List<Tuple<string, decimal, decimal>>()
            {
                new Tuple<string,decimal,decimal>("United States, Alaska", 1, 33.33m),
                new Tuple<string,decimal,decimal>("United States, Florida", 1, 33.33m),
                new Tuple<string,decimal,decimal>("United States, Washington", 1, 33.33m),
            };
            CollectionAssert.AreEquivalent(expectedList, viewModel.StatsDataPercent);
        }

        [TestMethod]
        public void StatsByLocationPercentRoundValueToTwoDecimalPlaces2()
        {
            List<Customer> customers = new List<Customer>()
            {
                TestHelpers.GetSampleCustomer(),
                TestHelpers.GetSampleCustomer(),
                TestHelpers.GetSampleCustomer(),
            };

            customers[0].Country = "United States";
            customers[1].Country = "United States";
            customers[2].Country = "United States";

            customers[0].State = "Washington";
            customers[1].State = "Alaska";
            customers[2].State = "Alaska";

            StatsViewModel viewModel = new StatsViewModel(customers);
            viewModel.SelectedStatsType = StatsType.Location;

            List<Tuple<string, decimal, decimal>> expectedList = new List<Tuple<string, decimal, decimal>>()
            {
                new Tuple<string,decimal,decimal>("United States, Alaska", 2, 66.67m),
                new Tuple<string,decimal,decimal>("United States, Washington", 1, 33.33m),
            };
            CollectionAssert.AreEquivalent(expectedList, viewModel.StatsDataPercent);
        }
    }
}
