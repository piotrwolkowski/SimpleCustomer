using SimpleCustomer.Core;
using SimpleCustomer.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleCustomer.Model
{
    public static partial class ClientDataAccessManager
    {
        // Provides direct access to the data from the data service.

        /// <summary>
        /// Data acces class that provides access to the data service
        /// </summary>
        private class ClientDataAccess : DataProxy
        {
            /// <summary>
            /// Retries the customers data from the database.
            /// </summary>
            /// <returns>Collection of the customers.</returns>
            public new async Task<List<Customer>> GetCustomers()
            {
                return await base.GetCustomers();
            }

            /// <summary>
            /// Adds the customer to the database.
            /// </summary>
            /// <param name="customer">The customer to be added.</param>
            /// <param name="cancellationToken">Cancellation token to cancel save.</param>
            /// <returns>If successful returns datetime of the add operation, otherwise null.</returns>
            public new async Task<DateTime?> AddCustomer(Customer customer, CancellationToken cancellationToken)
            {
                return await base.AddCustomer(customer, cancellationToken);
            }

            /// <summary>
            /// Updates the customer in the database
            /// </summary>
            /// <param name="customer">The customer to be updated.</param>
            /// <param name="cancellationToken">Cancellation token to cancel update.</param>
            /// <returns>If successful returns datetime of the update operation, otherwise null.</returns>
            public new async Task<DateTime?> UpdateCustomer(Customer customer, CancellationToken cancellationToken)
            {
                return await base.UpdateCustomer(customer, cancellationToken);
            }

            /// <summary>
            /// Removes the customer from the database.
            /// </summary>
            /// <param name="customer">The customer to be removed.</param>
            /// <returns>If successful returns datetime of the remove operation, otherwise null.</returns>
            public new async Task<DateTime?> RemoveCustomer(Customer customer)
            {
                return await base.RemoveCustomer(customer);
            }

            /// <summary>
            /// Crates the modification history of all changes in the database.
            /// </summary>
            /// <returns>String containing description of all modifications in csv format.</returns>
            public new async Task<string> GetModificationHistory()
            {
                return await base.GetModificationHistory();
            }

            /// <summary>
            /// Request time of last access to the data.
            /// </summary>
            /// <returns>Date and time of last access to the data</returns>s
            public new DateTime RequestLastDataBaseModificationTime()
            {
                // The operation should not be asynchronious.
                // On the result of the request depends whether cached data
                // are accessed or are they requested from the server.
                // As long as there is no reply getting data won't be able to continue.
                return base.RequestLastDataBaseModificationTime();
            }
        }

    }
}
