using SimpleCustomer.ViewModel;

namespace SimpleCustomer.Command
{
    /// <summary>
    /// The command to invoke add customer on the mainViewModel.
    /// </summary>
    class AddCustomerCommand : CommandBase
    {
        private MainViewModel mainViewModel;

        /// <summary>
        /// Creates an instance of the AddCustomerCommand.
        /// </summary>
        /// <param name="mainViewModel">A view model on which command's method will be invoked.</param>
        public AddCustomerCommand(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
        }

        #region CommandBase
        public override void Execute(object parameter)
        {
            this.mainViewModel.AddCustomer();
        }
        #endregion
    }
}
