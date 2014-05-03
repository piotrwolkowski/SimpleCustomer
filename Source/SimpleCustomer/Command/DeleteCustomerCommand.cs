using SimpleCustomer.ViewModel;

namespace SimpleCustomer.Command
{
    /// <summary>
    /// The command to invoke delete customer on the mainViewModel.
    /// </summary>
    class DeleteCustomerCommand : CommandBase
    {
        private MainViewModel mainViewModel;

        /// <summary>
        /// Creates an instance of the DeleteCustomerCommand.
        /// </summary>
        /// <param name="mainViewModel">A view model on which command's method will be invoked.</param>
        public DeleteCustomerCommand(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
        }

        #region CommandBase
        public override bool CanExecute(object parameter)
        {
            // Allow to delete only if any customer is selected.
            return (this.mainViewModel != null &&
                this.mainViewModel.SelectedCustomer != null);
        }

        public override void Execute(object parameter)
        {
            this.mainViewModel.DeleteCustomer();
        }
        #endregion
    }
}
