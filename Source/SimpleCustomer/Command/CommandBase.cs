using System;
using System.Windows.Input;

namespace SimpleCustomer.Command
{
    /// <summary>
    /// Base command class providing implementaion of the ICommand interface.
    /// </summary>
    internal abstract class CommandBase : ICommand
    {
        #region ICommand
        /// <summary>
        /// By default handle it by the CommandManager
        /// </summary>
        public virtual event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// Can execute returning true by defautl.
        /// </summary>
        /// <param name="parameter">Parameters accepted by the method.</param>
        /// <returns>Whether the command can be executed.</returns>
        public virtual bool CanExecute(object parameter)
        {
            // return true by default
            return true;
        }

        /// <summary>
        /// Execute method. Has to be overriden by every child
        /// </summary>
        /// <param name="parameter">Parameters accepted by the method.</param>
        public abstract void Execute(object parameter);

        #endregion
    }
}
