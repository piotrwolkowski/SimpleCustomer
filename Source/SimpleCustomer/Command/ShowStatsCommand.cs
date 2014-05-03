using SimpleCustomer.ViewModel;
using System.Linq;

namespace SimpleCustomer.Command
{
    /// <summary>
    /// The command to invoke show stats on the mainViewModel.
    /// </summary>
    internal class ShowStatsCommand : CommandBase
    {
        private MainViewModel mainViewModel;

        /// <summary>
        /// Creates an instance of the ShowStatsCommand.
        /// </summary>
        /// <param name="mainViewModel">A view model on which command's method will be invoked.</param>
        public ShowStatsCommand(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
        }

        #region CommandBase
        public override bool CanExecute(object parameter)
        {
            // Only allow displaying stats for the customers
            // if there are any customers selected.
            return (this.mainViewModel != null &&
                this.mainViewModel.Customers != null &&
                this.mainViewModel.Customers.Any());
        }

        public override void Execute(object parameter)
        {
            this.mainViewModel.ShowStats();
        }
        #endregion
    }
}
