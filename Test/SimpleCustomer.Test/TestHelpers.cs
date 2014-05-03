using SimpleCustomer;
using SimpleCustomer.Core;
using System;
using System.Collections.Generic;
namespace SimpleCustomer.Test
{
    internal static class TestHelpers
    {
        /// <summary>
        /// String exceeding 50 characters.
        /// </summary>
        internal const string STRING_LONGER_THAN_50_CHARS = @"A string exceeding 50 characters to test 
            any value that has this limit.";

        /// <summary>
        /// String exceeding 255 characters.
        /// </summary>
        internal const string STRING_LONGER_THAN_255_CHARS = @"A string exceeding 255 characters to test 
            any value that has this limit. A string exceeding 255 characters to test any value that has 
            this limit. A string exceeding 255 characters to test any value that has this limit. A string 
            exceeding 255 characters to test any value that has this limit.";

        /// <summary>
        /// Helper method to get a sample customer
        /// </summary>
        /// <returns>A sample customer</returns>
        internal static Customer GetSampleCustomer()
        {
            Customer customer = new Customer()
            {
                Category = Category.A,
                DateOfBirth = new DateTime(1985, 7, 8),
                Gender = Gender.Unknown,
                HouseNumber = "323A",
                Name = "Tom",
                AddressLineOne = "Test address",
                State = "Alaska",
                Country = "United States",
            };

            return customer;
        }


        /// <summary>
        /// Helper method to create a list of customers.
        /// </summary>
        /// <returns>A list of customers.</returns>
        internal static List<Customer> GetSampleCustomersList()
        {
            List<Customer> customers = new List<Customer>()
            {
                new Customer()
                {
                    AddressLineOne = "Test address",
                    Category = Category.A,
                    DateOfBirth = new DateTime(1985, 7, 8),
                    Gender = Gender.Unknown,
                    HouseNumber = "323A",
                    Name = "Piotr Wolkowski",
                    State = "Alaska",
                    Country = "United States",
                },

                new Customer()
                {
                    AddressLineOne = "Test address",
                    Category = Category.A,
                    DateOfBirth = new DateTime(1985, 7, 8),
                    Gender = Gender.Unknown,
                    HouseNumber = "323A",
                    Name = "Adam Scott",
                    State = "Florida",
                    Country = "United States",
                },

                new Customer()
                {
                    AddressLineOne = "Test address",
                    Category = Category.B,
                    DateOfBirth = new DateTime(1985, 7, 8),
                    Gender = Gender.Unknown,
                    HouseNumber = "323A",
                    Name = "Aaron",
                    State = "Washington",
                    Country = "United States",
                },

                new Customer()
                {
                    AddressLineOne = "Test address",
                    Category = Category.B,
                    DateOfBirth = new DateTime(1985, 7, 8),
                    Gender = Gender.Unknown,
                    HouseNumber = "323A",
                    Name = "Marie Berenerd",
                    State = "Alaska",
                    Country = "United States",
                },
            };

            return customers;
        }
    }
}
