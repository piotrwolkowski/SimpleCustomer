using SimpleCustomer.Data;

namespace SimpleCustomer.Model
{

    /// The interface doesn't allow the async keyword. Therefore it abandons convention to suffix methods with Async.
    /// Hence the class implementing the interface, even though it has the liberty to use async keyword, 
    /// cannot follow the naming convention.

    // ClientDataAccessManager keeps the data access and data cache separated so it will be easy to add e.g. a settings entry
    // that will allow to skip caching if required. Keeps the singleton open to adding/removing caching behaviour.
    
    /// <summary>
    /// Client data access manager. Responsible for providing access to the data.
    /// </summary>
    public static partial class ClientDataAccessManager
    {
        // It's safe to use singleton on the client side.
        // The client construction won't allow threads
        // to interrupt each other while running similar
        // operations.
        private static IDataServiceContract dataAccessInstance;

        /// <summary>
        /// The current instance of the CustomersClientDataProxy that enables sending requests
        /// to the data service.
        /// </summary>
        public static IDataServiceContract DataAccessInstance
        {
            get
            {
                dataAccessInstance = dataAccessInstance ?? new ClientDataCache(new ClientDataAccess());
                return dataAccessInstance;
            }
        }
    }
}