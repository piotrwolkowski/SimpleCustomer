using SimpleCustomer.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleCustomer.Data
{
    /// <summary>
    /// A data access service that directly performs any data base operations
    /// </summary>
    public class DataProxy : IDataServiceContract
    {
        #region IDataServiceContract
        /// <summary>
        /// The field containing last update time. Instead of requesting new data collection
        /// the client application can request the LastDataBaseModificationTime and compare it with its own
        /// LastDataBaseModificationTime.
        /// Instead of performing time and resource consuming data request check the last modification time.
        /// If the time is the same as on the client there has been no update from other client applications.
        /// </summary>
        private DateTime lastDataBaseModificationTime;

        /// <summary>
        /// Retries the customers data from the database.
        /// </summary>
        /// <returns>Collection of the customers.</returns>
        public async Task<List<Customer>> GetCustomers()
        {
            try
            {
                using (CustomersContext context = new CustomersContext())
                {
                    List<CustomerDb> customersDb = await context.CustomerDbs.ToListAsync();
                    List<Customer> customers = customersDb
                        .Select(cdb =>
                        {
                            Customer c = new Customer()
                            {
                                Id = cdb.Id,
                                AddressLineOne = cdb.AddressLineOne,
                                Category = cdb.Category,
                                Country = cdb.Country,
                                DateOfBirth = cdb.DateOfBirth,
                                Gender = cdb.Gender,
                                HouseNumber = cdb.HouseNumber,
                                Name = cdb.Name,
                                State = cdb.State,
                            };
                            return c;
                        }
                    ).ToList();

                    return customers;
                }

            }
            catch (InvalidCastException e)
            {
                DbLog.LogException(e, "DataProxy.cs");
                throw;
            }
            catch (InvalidOperationException e)
            {
                DbLog.LogException(e, "DataProxy.cs");
                throw;
            }
        }

        /// <summary>
        /// Adds the customer to the database.
        /// </summary>
        /// <param name="customer">The customer to be added.</param>
        /// <param name="cancellationToken">Cancellation token to cancel save.</param>
        /// <returns>If successful returns datetime of the add operation, otherwise null.</returns>
        public async Task<Nullable<DateTime>> AddCustomer(Customer customer, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                if (customer == null)
                {
                    throw new ArgumentNullException("Null Customer cannot be added");
                }

                if (!customer.IsValid)
                {
                    return null;
                }

                using (CustomersContext context = new CustomersContext())
                {
                    CustomerDb customerDb = new CustomerDb()
                    {
                        AddressLineOne = customer.AddressLineOne,
                        Category = customer.Category,
                        Country = customer.Country,
                        DateOfBirth = customer.DateOfBirth,
                        Gender = customer.Gender,
                        HouseNumber = customer.HouseNumber,
                        Name = customer.Name,
                        State = customer.State
                    };

                    context.CustomerDbs.Add(customerDb);

                    await context.SaveChangesAsync(cancellationToken);
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        // The Id is created by the database so before
                        // logging modification history entry update the customer with the new Id.
                        customer.Id = customerDb.Id;
                        await EnterModificationHistory(context, customer, ModificationType.Add);
                        this.lastDataBaseModificationTime = DateTime.Now;
                        return this.lastDataBaseModificationTime;
                    }
                    return null;
                }
            }
            catch (InvalidOperationException e)
            {
                DbLog.LogException(e, "DataProxy.cs");
                throw;
            }
            catch (NullReferenceException e)
            {
                DbLog.LogException(e, "DataProxy.cs");
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                // Cancelletion exception can be wrapped in several other exceptions.
                // Instead of unwrapping it check if cancellation requested.
                if (cancellationToken.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }
                throw;
            }
        }

        /// <summary>
        /// Updates the customer in the database
        /// </summary>
        /// <param name="customer">The customer to be updated.</param>
        /// <param name="cancellationToken">Cancellation token to cancel save.</param>
        /// <returns>If successful returns datetime of the update operation, otherwise null.</returns>
        public async Task<Nullable<DateTime>> UpdateCustomer(Customer customer, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                if (customer == null)
                {
                    throw new ArgumentNullException("Null Customer cannot be updated.");
                }
                if (!customer.IsValid)
                {
                    return null;
                }
                using (CustomersContext context = new CustomersContext())
                {
                    CustomerDb customerDb = context.CustomerDbs.Find(customer.Id);

                    if (customerDb == null)
                    {
                        return null;
                    }

                    customerDb.AddressLineOne = customer.AddressLineOne;
                    customerDb.Category = customer.Category;
                    customerDb.Country = customer.Country;
                    customerDb.DateOfBirth = customer.DateOfBirth;
                    customerDb.Gender = customer.Gender;
                    customerDb.HouseNumber = customer.HouseNumber;
                    customerDb.Name = customer.Name;
                    customerDb.State = customer.State;

                    await context.SaveChangesAsync(cancellationToken);
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await EnterModificationHistory(context, customer, ModificationType.Update);
                        this.lastDataBaseModificationTime = DateTime.Now;
                        return this.lastDataBaseModificationTime;
                    }
                    return null;
                }
            }
            catch (InvalidOperationException e)
            {
                DbLog.LogException(e, "DataProxy.cs");
                throw;
            }
            catch (NullReferenceException e)
            {
                DbLog.LogException(e, "DataProxy.cs");
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                // Cancelletion exception can be wrapped in several other exceptions.
                // Instead of unwrapping it check if cancellation requested.
                if (cancellationToken.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }
                throw;
            }
        }

        /// <summary>
        /// Removes the customer from the database.
        /// </summary>
        /// <param name="customer">The customer to be removed.</param>
        /// <returns>If successful returns datetime of the remove operation, otherwise null.</returns>
        public async Task<Nullable<DateTime>> RemoveCustomer(Customer customer)
        {
            try
            {
                if (customer == null)
                {
                    throw new ArgumentNullException("Null Customer cannot be updated.");
                }
                using (CustomersContext context = new CustomersContext())
                {
                    CustomerDb customerDb = context.CustomerDbs.FirstOrDefault(c => c.Id == customer.Id);

                    if (customerDb == null)
                    {
                        return null;
                    }

                    context.CustomerDbs.Remove(customerDb);

                    await context.SaveChangesAsync();
                    await EnterModificationHistory(context, customer, ModificationType.Remove);

                    this.lastDataBaseModificationTime = DateTime.Now;

                    return this.lastDataBaseModificationTime;
                }
            }
            catch (InvalidOperationException e)
            {
                DbLog.LogException(e, "DataProxy.cs");
                throw;
            }
            catch (NullReferenceException e)
            {
                DbLog.LogException(e, "DataProxy.cs");
                throw;
            }
        }

        /// <summary>
        /// Crates the modification history of all changes in the database.
        /// Order by modification time, in descending order.
        /// </summary>
        /// <returns>String containing description of all modifications in csv format.</returns>
        public async Task<string> GetModificationHistory()
        {
            try
            {
                using (CustomersContext context = new CustomersContext())
                {
                    StringBuilder modificationHistory = new StringBuilder();
                    // Pass empty item to the method to get headers.
                    modificationHistory.AppendLine(CustomerHistoryToCsvHeaders(new CustomerDbHistory()));

                    List<CustomerDbHistory> customers = await (context.CustomerDbHistories.OrderByDescending(ch => ch.ChangedOn)).ToListAsync();
                    foreach (CustomerDbHistory entry in customers)
                    {
                        modificationHistory.AppendLine(CustomerHistoryToCsv(entry));
                    }

                    return modificationHistory.ToString();
                }
            }
            catch (InvalidOperationException e)
            {
                DbLog.LogException(e, "DataProxy.cs");
                throw;
            }
            catch (ArgumentNullException e)
            {
                DbLog.LogException(e, "DataProxy.cs");
                throw;
            }
        }

        /// <summary>
        /// Instead of requesting new data collection the client application can request the LastDataBaseModificationTime 
        /// and compare it with its own LastDataBaseModificationTime. Then it can request new data only when the data
        /// cached on the client are older.
        /// </summary>
        /// <returns>Time of the last operation on the database.</returns>
        public DateTime RequestLastDataBaseModificationTime()
        {
            return this.lastDataBaseModificationTime;
        }
        #endregion IDataServiceContract

        /// <summary>
        /// Enteres a modification history entry based on the submitted customer and the modification type.
        /// </summary>
        /// <param name="context">The context in which the modification entry will be performed.</param>
        /// <param name="customer">The customer object that has been modified. Original details are saved in the history table.</param>
        /// <param name="modificationType">The modofication type: Add, Remove, Update, Other</param>
        /// <returns></returns>
        private async Task EnterModificationHistory(CustomersContext context, Customer customer, ModificationType modificationType)
        {
            CustomerDbHistory customerHistory = new CustomerDbHistory()
            {
                // log modification type
                ModificationType = modificationType,
                // and customer details so it's easier to reproduce changes
                Id = customer.Id,
                AddressLineOne = customer.AddressLineOne,
                Category = customer.Category,
                Country = customer.Country,
                DateOfBirth = customer.DateOfBirth,
                Gender = customer.Gender,
                HouseNumber = customer.HouseNumber,
                Name = customer.Name,
                State = customer.State,
                ChangedOn = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt")
            };

            try
            {
                context.CustomerDbHistories.Add(customerHistory);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                DbLog.LogException(e, "DataProxy.cs");
            }
        }
        
        /// <summary>
        /// Takes the CustomerDbHistory and retrieves all the properties values in the form of csv entry.
        /// </summary>
        /// <param name="history">CustomerDbHistory entry to parse into csv entry.</param>
        /// <returns>Values of the CustomerDbHistory object translated into csv entry.</returns>
        private string CustomerHistoryToCsv(CustomerDbHistory history)
        {
            if (history == null)
            {
                throw new ArgumentNullException("history", "Value can not be null or Nothing!");
            }

            StringBuilder historyEntry = new StringBuilder();
            Type type = history.GetType();
            System.Reflection.PropertyInfo[] propertyInfo = type.GetProperties();

            for (int index = 0; index < propertyInfo.Length; index++)
            {
                historyEntry.Append(propertyInfo[index].GetValue(history, null));

                if (index < propertyInfo.Length - 1)
                {
                    historyEntry.Append(",");
                }
            }

            return historyEntry.ToString();
        }

        /// <summary>
        /// Takes the CustomerDbHistory and retrieves all the properties names in the form of csv headers.
        /// </summary>
        /// <param name="history">CustomerDbHistory object to parse into csv headers.</param>
        /// <returns>Names of the CustomerDbHistory properties translated into csv headers.</returns>
        private string CustomerHistoryToCsvHeaders(CustomerDbHistory history)
        {
            if (history == null)
            {
                throw new ArgumentNullException("history", "Value can not be null or Nothing!");
            }

            StringBuilder historyHeader = new StringBuilder();
            Type type = history.GetType();
            System.Reflection.PropertyInfo[] propertyInfo = type.GetProperties();

            for (int index = 0; index < propertyInfo.Length; index++)
            {
                historyHeader.Append(propertyInfo[index].Name);

                if (index < propertyInfo.Length - 1)
                {
                    historyHeader.Append(",");
                }
            }

            return historyHeader.ToString();
        }
    }
}
