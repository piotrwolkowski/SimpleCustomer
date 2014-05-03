using SimpleCustomer.Core;
using SimpleCustomer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleCustomer.Model
{
    public static partial class ClientDataAccessManager
    {
        // The client data cache invokes the operations on the client data access,
        // bBut also updates the client cached values.
        // It can be safely ommited and the operations and data can accessed directly from ClientDataAccess
        // In order to keep the cache transparent the ClientDataCache implements IDataProxyContract.

        /// <summary>
        /// Cached data access class that accesses the data service and caches the results
        /// </summary>
        private class ClientDataCache : IDataServiceContract
        {
            ClientDataAccess dataAccess;

            /// <summary>
            /// Requests the data and caches them. Uses the ClientDataAccess to request the data.
            /// </summary>
            /// <param name="dataAccess">Data access object that is used to request the data.</param>
            internal ClientDataCache(ClientDataAccess dataAccess)
            {
                this.dataAccess = dataAccess;
            }

            /// <summary>
            /// Cached customers collection to avoid constant file parsing.
            /// </summary>
            private static List<Customer> cachedCustomers;

            /// <summary>
            /// Keeps the time of the last modification  on the database requested from current client.
            /// In case of retrieving any data, if the time is the same as the last modification time
            /// on the data client, there is no need to request data as the current cache is up to date.
            /// </summary>
            private DateTime clientLastDbModification;

            /// <summary>
            /// Gets the customers from the database and caches the results.
            /// </summary>
            /// <returns>The task containing the results of the operation</returns>
            public async Task<List<Customer>> GetCustomers()
            {
                // Create cached collection of the customers.
                // Other operations keep the cache updated
                // when they change customers.
                // Check the last modification time the current client requested.
                // If it's lower than the last modification time on the data client
                // Request the data.
                if (cachedCustomers == null ||
                    this.clientLastDbModification < this.dataAccess.RequestLastDataBaseModificationTime())
                {
                    IEnumerable<Customer> customersLocal = await this.dataAccess.GetCustomers();

                    if (customersLocal != null &&
                        customersLocal.Any())
                    {
                        cachedCustomers = customersLocal.ToList();
                    }
                    else
                    {
                        cachedCustomers = null;
                    }
                }

                return cachedCustomers;
            }

            /// <summary>
            /// Sends the customer to the database and tries to update it with the newly created id.
            /// New customer is added to the local cache.
            /// </summary>
            /// <param name="customer">The customer to be saved in the database.</param>
            /// <param name="cancellationToken">Cancellation token to cancel save.</param>
            /// <returns>If successful returns datetime of the add operation, otherwise null.</returns>
            public async Task<Nullable<DateTime>> AddCustomer(Customer customer, CancellationToken cancellationToken)
            {
                // Successful if datetiem of the operation.
                // Failed if null.
                Nullable<DateTime> result = await this.dataAccess.AddCustomer(customer, cancellationToken);

                // Add customer to the cache
                if (result != null)
                {
                    // Update time only if successful.
                    // If failed cache will need an update.
                    this.clientLastDbModification = (DateTime)result;
                    if (cachedCustomers != null)
                    {
                        cachedCustomers.Add(customer);
                    }
                }
                return result;
            }

            /// <summary>
            /// Request customer update on the database.
            /// </summary>
            /// <param name="customer">The customer to be updated.</param>
            /// <param name="cancellationToken">Cancellation token to cancel save.</param>
            /// <returns>If successful returns datetime of the update operation, otherwise null.</returns>
            public async Task<Nullable<DateTime>> UpdateCustomer(Customer customer, CancellationToken cancellationToken)
            {
                // Successful if datetiem of the operation.
                // Failed if null.
                Nullable<DateTime> result = await this.dataAccess.UpdateCustomer(customer, cancellationToken);
                // Update local cache.
                if (this.clientLastDbModification != null)
                {
                    if (result != null)
                    {
                        // Update time only if successful.
                        // If failed cache will need an update.
                        this.clientLastDbModification = (DateTime)result;
                        Customer cachedCustomer = cachedCustomers.FirstOrDefault(c => c.Id == customer.Id);
                        if (cachedCustomer != null)
                        {
                            cachedCustomer.Copy(customer);
                        }
                    }
                }
                return result;
            }

            /// <summary>
            /// Removes the customer from the database.
            /// </summary>
            /// <param name="customer">The customer to be removed.</param>
            /// <returns>If successful returns datetime of the remove operation, otherwise null.</returns>
            public async Task<Nullable<DateTime>> RemoveCustomer(Customer customer)
            {
                // Successful if datetiem of the operation.
                // Failed if null.
                Nullable<DateTime> result = await this.dataAccess.RemoveCustomer(customer);

                // If db remove successful, remove the customer from the local cache.
                if (result != null)
                {
                    // Update time only if successful.
                    // If failed cache will need an update.
                    this.clientLastDbModification = (DateTime)result;
                    if (cachedCustomers != null &&
                        cachedCustomers.Contains(customer))
                    {
                        cachedCustomers.Remove(customer);
                    }
                }

                return result;
            }

            /// <summary>
            /// Request the modification history.
            /// </summary>
            /// <returns>Task containing the modification history.</returns>
            public async Task<string> GetModificationHistory()
            {
                return await this.dataAccess.GetModificationHistory();
            }

            /// <summary>
            /// Request the date and time of the last operation on the server.
            /// </summary>
            /// <returns>Date and time of the last operation on the server.</returns>
            public DateTime RequestLastDataBaseModificationTime()
            {
                return this.dataAccess.RequestLastDataBaseModificationTime();
            }
        }
    }
}
