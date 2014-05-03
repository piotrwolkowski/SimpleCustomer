using System;

namespace SimpleCustomer.ViewModel
{
    /// <summary>
    /// An interface implemented by any dialog that is going to support cancelling
    /// </summary>
    public interface ICancellableViewModel
    {
        /// <summary>
        /// An event that should be propagated when cancel is done.
        /// </summary>
        event EventHandler Cancelled;

        /// <summary>
        /// A method that should be invoked on cancel command.
        /// </summary>
        void Cancel();
    }
}
