﻿using SimpleCustomer.Command;
using SimpleCustomer.Core;
using SimpleCustomer.Model;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleCustomer.ViewModel
{
    /// <summary>
    /// View model to add or modify a customer
    /// </summary>
    public class AddCustomerViewModel : ViewModelBase, ICancellableViewModel
    {
        #region fields and properties
        /// <summary>
        /// Event handler propagated when the save operation is completed.
        /// </summary>
        public event EventHandler SaveCompleted;

        /// <summary>
        /// A token that is used to cancel add/update operation.
        /// </summary>
        private CancellationTokenSource cancelSaveOperation;

        private Customer selectedCustomer;
        /// <summary>
        /// The selected customer displayed on the view.
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
        #endregion fields and properties

        #region commands
        /// <summary>
        /// Gets the SaveCommand
        /// </summary>
        public ICommand SaveCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the CancelCommand
        /// </summary>
        public ICommand CancelCommand
        {
            get;
            private set;
        }
        #endregion commands

        #region c-tor
        /// <summary>
        /// Creates an instance of the AddCustomerViewModel 
        /// that displays a customer
        /// </summary>
        /// <param name="customer">The customer that is to be edited.</param>
        public AddCustomerViewModel(Customer customer)
        {
            this.SelectedCustomer = customer;
            this.SaveCommand = new SaveCommand(this);
            this.CancelCommand = new CancelCommand(this);
            this.cancelSaveOperation = new CancellationTokenSource();
        }
        #endregion c-tor

        #region methods invoked from the commands
        /// <summary>
        /// Saves the customer selected on the view model.
        /// </summary>
        internal async void SaveCustomer()
        {
            try
            {
                string message;

                // There is an id so send update request.
                // New user has no Id. It will be autogenerated by the data base.
                this.BusyStart();
                if (this.selectedCustomer.Id > 0)
                {
                    // Update the customer.
                    DateTime? updateResult = await Task.Run(() =>
                        ClientDataAccessManager.DataAccessInstance.UpdateCustomer(this.selectedCustomer, cancelSaveOperation.Token));
                    this.BusyStop();
                    if (updateResult != null)
                    {
                        message = string.Format("Customer {0} has been updated", this.selectedCustomer.Name);
                        InfoDialogViewModel.ShowDialog(message, "Update successful");
                    }
                    else
                    {
                        message = string.Format("Customer {0} - update failed", this.selectedCustomer.Name);
                        InfoDialogViewModel.ShowDialog(message, "Update failed");
                    }
                }
                else
                {
                    // Inser a new customer.
                    DateTime? addResult = await Task.Run(() =>
                        ClientDataAccessManager.DataAccessInstance.AddCustomer(this.selectedCustomer, cancelSaveOperation.Token));
                    this.BusyStop();
                    if (addResult != null)
                    {
                        message = string.Format("Customer {0} has been added", this.selectedCustomer.Name);
                        InfoDialogViewModel.ShowDialog(message, "Save successful");
                    }
                    else
                    {
                        message = string.Format("Customer {0} - add operation failed", this.selectedCustomer.Name);
                        InfoDialogViewModel.ShowDialog(message, "Save failed");
                    }
                }

                this.OnSaveCompleted();
            }
            catch(OperationCanceledException e)
            {
                // Don't log the cancellation exception as it is requested by the user.
                InfoDialogViewModel.ShowDialog(e.Message, "Unhandled exception");
            }
            catch (Exception e)
            {
                InfoDialogViewModel.ShowDialog(e.Message, "Unhandled exception");
                Log.LogException(e, "AddCustomerViewModel.cs");
            }
        }
        #endregion methods invoked from the commands

        /// <summary>
        /// Invokes event handler for save operation
        /// </summary>
        private void OnSaveCompleted()
        {
            EventHandler handler = this.SaveCompleted;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        /// <summary>
        /// Invokes event handler for cancel operation
        /// </summary>
        private void OnCancelCompleted()
        {
            EventHandler handler = this.Cancelled;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        #region ICancellableViewModel
        /// <summary>
        /// Event handler propagated when the operation is cancelled
        /// </summary>
        public event EventHandler Cancelled;

        public void Cancel()
        {
            if (!this.cancelSaveOperation.IsCancellationRequested)
            {
                this.cancelSaveOperation.Cancel();
            }
            this.cancelSaveOperation.Dispose();
            this.OnCancelCompleted();
        }
        #endregion
    }
}
