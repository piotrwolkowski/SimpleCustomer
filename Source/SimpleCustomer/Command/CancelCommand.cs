using SimpleCustomer.ViewModel;

namespace SimpleCustomer.Command
{
    class CancelCommand : CommandBase
    {
        private ICancellableViewModel cancellableViewModel;

        public CancelCommand(ICancellableViewModel cancellableViewModel)
        {
            this.cancellableViewModel = cancellableViewModel;
        }

        #region CommandBase
        public override void Execute(object parameter)
        {
            this.cancellableViewModel.Cancel();
        }
        #endregion
    }
}
