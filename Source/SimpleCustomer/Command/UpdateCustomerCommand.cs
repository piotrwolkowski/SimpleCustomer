using SimpleCustomer.ViewModel;

namespace SimpleCustomer.Command
{
    /// <summary>
    /// The command to invoke update customer on the mainViewModel.
    /// </summary>
    class UpdateCustomerCommand : CommandBase
    {
        private MainViewModel mainViewModel;

        /// <summary>
        /// Creates an instance of the UpdateCustomerCommand.
        /// </summary>
        /// <param name="mainViewModel">A view model on which command's method will be invoked.</param>
        public UpdateCustomerCommand(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
        }

        #region CommandBase
        public override bool CanExecute(object parameter)
        {
            // Allow to modify if any selected
            return (this.mainViewModel != null &&
                this.mainViewModel.SelectedCustomer != null);
        }

        public override void Execute(object parameter)
        {
            this.mainViewModel.UpdateCustomer();
        }
        #endregion
    }
}
