using SimpleCustomer.Command;
using SimpleCustomer.Core;
using SimpleCustomer.Model;
using SimpleCustomer.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleCustomer.ViewModel
{
    /// <summary>
    /// Main view model to which main window bounds.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region properties and fields
        /// <summary>
        /// The collection of the users that will remain unchanged after the data are loaded.
        /// </summary>
        private IEnumerable<Customer> customersBuckup;

        private bool isRequestingData;
        /// <summary>
        /// A flag indicating whether the data has been requested.
        /// </summary>
        internal bool IsRequestingData
        {
            get
            {
                return this.isRequestingData;
            }
            private set
            {
                this.isRequestingData = value;
                // Show is busy indicator if requesting data.
                if (isRequestingData)
                {
                    this.BusyStart();
                }
                else
                {
                    this.BusyStop();
                }
                this.OnPropertyChanged();
            }
        }

        private string filter;
        /// <summary>
        /// Gets or sets the filter string to filter the customers collection.
        /// </summary>
        public string Filter
        {
            get
            {
                return this.filter;
            }
            set
            {
                this.filter = value;
                this.Customers = FilterCustomers(this.filter);
                this.OnPropertyChanged();
            }
        }

        private IEnumerable<Customer> customers;
        /// <summary>
        /// The customers collection.
        /// </summary>
        public IEnumerable<Customer> Customers
        {
            get
            {
                return this.customers;
            }
            set
            {
                this.customers = value;
                this.OnPropertyChanged();
            }
        }

        private Customer selectedCustomer;
        /// <summary>
        /// The selected customer.
        /// </summary>
        public Customer SelectedCustomer
        {
            get
            {
                return this.selectedCustomer;
            }
            set
            {
                this.selectedCustomer = value;
                this.OnPropertyChanged();
            }
        }
        #endregion properties and fields

        #region commands
        /// <summary>
        /// Gets the UpdateCommand.
        /// </summary>
        public ICommand GetDataCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the ShowStatsCommand.
        /// </summary>
        public ICommand ShowStatsCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the AddCustomerCommand.
        /// </summary>
        public ICommand AddCustomerCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the UpdateCustomerCommand.
        /// </summary>
        public ICommand UpdateCustomerCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the DeleteCustomerCommand.
        /// </summary>
        public ICommand DeleteCustomerCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the DisplayModificationHistoryCommand.
        /// </summary>
        public ICommand DisplayModificationHistoryCommand
        {
            get;
            private set;
        }
        #endregion commands

        #region c-tor
        /// <summary>
        /// Cratea a new instance of the CastomerViewModel.
        /// </summary>
        public MainViewModel()
        {
            // Initialise commands.
            this.GetDataCommand = new GetDataCommand(this);
            this.ShowStatsCommand = new ShowStatsCommand(this);
            this.AddCustomerCommand = new AddCustomerCommand(this);
            this.UpdateCustomerCommand = new UpdateCustomerCommand(this);
            this.DeleteCustomerCommand = new DeleteCustomerCommand(this);
            this.DisplayModificationHistoryCommand = new DisplayModificationHistoryCommand(this);
        }
        #endregion c-tor

        #region methods invoked from commands
        /// <summary>
        /// Requests data from the data base and refreshes data grid.
        /// </summary>
        internal async void GetData()
        {
            try
            {
                this.IsRequestingData = true;
                this.customersBuckup = await Task.Run(() => ClientDataAccessManager.DataAccessInstance.GetCustomers());

                // Assignment to Customers is redundant as the filter will do it
                // But if filter is already empty it won't change the state of the
                // DataGrid therefore refresh the data grid here.
                this.Customers = null;
                this.Customers = this.customersBuckup;
                // Clear filter in case a user typed anything in.
                this.Filter = string.Empty;
                this.IsRequestingData = false;
                // Forcing the CommandManager to raise the RequerySuggested event
                CommandManager.InvalidateRequerySuggested();
            }
            catch (Exception ex)
            {
                InfoDialogViewModel.ShowDialog(ex.Message, "Unhandled Exception");
                Log.LogException(ex, "MainViewModel.cs");
            }
        }

        /// <summary>
        /// Invokes the window with statistical data for the users.
        /// </summary>
        internal void ShowStats()
        {
            try
            {
                StatsView statsView = new StatsView();
                StatsViewModel statsViewModel = new StatsViewModel(this.customers);
                statsView.DataContext = statsViewModel;
                statsView.ShowDialog();
            }
            catch (Exception ex)
            {
                InfoDialogViewModel.ShowDialog(ex.Message, "Unhandled Exception");
                Log.LogException(ex, "MainViewModel.cs");
            }
        }

        /// <summary>
        /// Invokes the add new customer window .
        /// </summary>
        internal void AddCustomer()
        {
            try
            {
                AddCustomerView addCustomerView = new AddCustomerView();
                // Set the date of birth to today, so it's not set to default 01/01/0001
                Customer customer = new Customer() { DateOfBirth = DateTime.Now };
                AddCustomerViewModel addCustomerViewModel = new AddCustomerViewModel(customer);
                addCustomerViewModel.SaveCompleted += (s, e) =>
                    {
                        // Get latest data after save.
                        this.SelectedCustomer = null;
                        addCustomerView.Close();
                        this.GetData();
                    };
                addCustomerViewModel.Cancelled += (s, e) =>
                    {
                        this.SelectedCustomer = null;
                        addCustomerView.Close();
                    };
                addCustomerView.DataContext = addCustomerViewModel;
                addCustomerView.ShowDialog();
            }
            catch (Exception ex)
            {
                InfoDialogViewModel.ShowDialog(ex.Message, "Unhandled Exception");
                Log.LogException(ex, "MainViewModel.cs");
            }
        }

        /// <summary>
        /// Invokes the update customer window.
        /// </summary>
        internal void UpdateCustomer()
        {
            try
            {
                // Make a copy of the current customer to avoid directly editing the customer in the data grid.
                // The customer will be modified in the cache and in the database
                // and the grid view will be updated from the cache or the database.
                Customer c = new Customer();
                c.Copy(this.SelectedCustomer);

                AddCustomerView addCustomerView = new AddCustomerView();
                AddCustomerViewModel addCustomerViewModel = new AddCustomerViewModel(c);
                addCustomerViewModel.SaveCompleted += (s, e) =>
                {
                    // Get latest data after save.
                    this.SelectedCustomer = null;
                    addCustomerView.Close();
                    this.GetData();
                };
                addCustomerViewModel.Cancelled += (s, e) =>
                {
                    this.SelectedCustomer = null;
                    addCustomerView.Close();
                };
                addCustomerView.DataContext = addCustomerViewModel;
                addCustomerView.ShowDialog();
                this.SelectedCustomer = null;
            }
            catch (Exception ex)
            {
                InfoDialogViewModel.ShowDialog(ex.Message, "Unhandled Exception");
                Log.LogException(ex, "MainViewModel.cs");
            }
        }

        /// <summary>
        /// Deletes the customer.
        /// </summary>
        internal async void DeleteCustomer()
        {
            try
            {
                DateTime? result = await Task.Run(() => ClientDataAccessManager.DataAccessInstance.RemoveCustomer(this.SelectedCustomer));
                if (result != null)
                {
                    string message = string.Format("Customer {0} has been removed", this.SelectedCustomer.Name);
                    InfoDialogViewModel.ShowDialog(message, "Remove successful");
                }
                else
                {
                    string message = "Remove customer failed";
                    InfoDialogViewModel.ShowDialog(message, "Remove failed");
                }
                this.SelectedCustomer = null;
                this.GetData();
            }
            catch (Exception ex)
            {
                InfoDialogViewModel.ShowDialog(ex.Message, "Unhandled Exception");
                Log.LogException(ex, "MainViewModel.cs");
            }

        }

        /// <summary>
        /// Invokes the window displaying the modification history in the csv format.
        /// </summary>
        internal async void DisplayModificationHistory()
        {
            try
            {
                string modificationHistoryText = await Task.Run(() => ClientDataAccessManager.DataAccessInstance.GetModificationHistory());
                ModificationHistoryView modificationHistoryView = new ModificationHistoryView();
                ModificationHistoryViewModel modificationHistoryViewModel = new ModificationHistoryViewModel(modificationHistoryText);
                modificationHistoryView.DataContext = modificationHistoryViewModel;
                modificationHistoryView.ShowDialog();
            }
            catch (Exception ex)
            {
                InfoDialogViewModel.ShowDialog(ex.Message, "Unhandled Exception");
                Log.LogException(ex, "MainViewModel.cs");
            }
        }
        #endregion methods invoked from commands

        /// <summary>
        /// Applies a filter to the collection of user. Filters by name.
        /// </summary>
        /// <param name="filter">The filter to apply to the users collection.</param>
        /// <returns>The filtered users collection.</returns>
        private IEnumerable<Customer> FilterCustomers(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return this.customersBuckup;
            }

            return this.customersBuckup.Where(c => c.Name.ToLower().Contains(filter.ToLower())).Take(5);
        }
    }
}
