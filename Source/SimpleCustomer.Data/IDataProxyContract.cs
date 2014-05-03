using SimpleCustomer.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleCustomer.Data
{
    /// <summary>
    /// The interface that has a role of a data base operation contract.
    /// Provides the operations that the data access service guarantees
    /// </summary>
    public interface IDataServiceContract
    {
        /// <summary>
        /// Retries the customers data from the database.
        /// </summary>
        /// <returns>Collection of the customers.</returns>
        Task<List<Customer>> GetCustomers();

        /// <summary>
        /// Adds the customer to the database.
        /// </summary>
        /// <param name="customer">The customer to be added.</param>
        /// <param name="cancellationToken">Cancellation token to cancel save.</param>
        /// <returns>If successful returns datetime of the add operation, otherwise null.</returns>
        Task<Nullable<DateTime>> AddCustomer(Customer customer, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the customer in the database
        /// </summary>
        /// <param name="customer">The customer to be updated.</param>
        /// <param name="cancellationToken">Cancellation token to cancel update.</param>
        /// <returns>If successful returns datetime of the update operation, otherwise null.</returns>
        Task<Nullable<DateTime>> UpdateCustomer(Customer customer, CancellationToken cancellationToken);

        /// <summary>
        /// Removes the customer from the database.
        /// </summary>
        /// <param name="customer">The customer to be removed.</param>
        /// <returns>If successful returns datetime of the remove operation, otherwise null.</returns>
        Task<Nullable<DateTime>> RemoveCustomer(Customer customer);

        /// <summary>
        /// Crates the modification history of all changes in the database.
        /// </summary>
        /// <returns>String containing description of all modifications in csv format.</returns>
        Task<string> GetModificationHistory();

        /// <summary>
        /// Request time of last access to the data.
        /// </summary>
        /// <returns>Date and time of last access to the data</returns>
        DateTime RequestLastDataBaseModificationTime();
    }
}
