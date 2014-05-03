using SimpleCustomer.ViewModel;

namespace SimpleCustomer.Command
{
    /// <summary>
    /// The command to invoke display modification history on the mainViewModel.
    /// </summary>
    class DisplayModificationHistoryCommand : CommandBase
    {
        private MainViewModel mainViewModel;

        /// <summary>
        /// Creates an instance of the DisplayModificationHistoryCommand.
        /// </summary>
        /// <param name="mainViewModel">A view model on which command's method will be invoked.</param>
        public DisplayModificationHistoryCommand(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
        }

        #region CommandBase
        public override void Execute(object parameter)
        {
            this.mainViewModel.DisplayModificationHistory();
        }
        #endregion
    }
}
