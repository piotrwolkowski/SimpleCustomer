using SimpleCustomer.ViewModel;

namespace SimpleCustomer.Command
{
    /// <summary>
    /// The command to invoke save customer on the addCustomerViewModel.
    /// </summary>
    class SaveCommand : CommandBase
    {
        private AddCustomerViewModel addCustomerViewModel;

        /// <summary>
        /// Creates an instance of the SaveCommand.
        /// </summary>
        /// <param name="addCustomerViewModel">A view model on which command's method will be invoked.</param>
        public SaveCommand(AddCustomerViewModel addCustomerViewModel)
        {
            this.addCustomerViewModel = addCustomerViewModel;
        }

        #region CommandBase
        public override bool CanExecute(object parameter)
        {
            // Only allow saving the customer if all entries are valid.
            return this.addCustomerViewModel.SelectedCustomer.IsValid &&
                this.addCustomerViewModel.ShowBusy != System.Windows.Visibility.Visible;
        }

        public override void Execute(object parameter)
        {
            this.addCustomerViewModel.SaveCustomer();
        }
        #endregion
    }
}
