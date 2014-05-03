using SimpleCustomer.ViewModel;

namespace SimpleCustomer.Command
{
    /// <summary>
    /// The command to invoke get data on the mainViewModel.
    /// </summary>
    internal class GetDataCommand : CommandBase
    {
        private MainViewModel customerViewModel;

        /// <summary>
        /// Creates an instance of the GetDataCommand.
        /// </summary>
        /// <param name="viewModel">A view model on which command's method will be invoked.</param>
        internal GetDataCommand(MainViewModel viewModel)
        {
            this.customerViewModel = viewModel;
        }

        #region CommandBase
        public override bool CanExecute(object parameter)
        {
            // Don't allow requesting data while
            // one request is processed.
            return !this.customerViewModel.IsRequestingData;
        }
        public override void Execute(object parameter)
        {
            this.customerViewModel.GetData();
        }
        #endregion
    }
}
