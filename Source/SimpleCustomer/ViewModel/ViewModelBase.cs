using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
namespace SimpleCustomer.ViewModel
{
    /// <summary>
    /// Abstract ViewModel base that should be extended by any viewModel class.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        private bool disposed;

        #region Is Busy

        // Set of properties and methdos that allow any view model to easily provide
        // a view with the busy indicator.

        // The helper timer that increases a value of the progress bar in regular intervals.
        System.Timers.Timer timer = new System.Timers.Timer();

        private Visibility showBusy = Visibility.Collapsed;
        /// <summary>
        /// The visibility value for ShowBusy component.
        /// </summary>
        public Visibility ShowBusy
        {
            get
            {
                return this.showBusy;
            }
            set
            {
                this.showBusy = value;
                this.OnPropertyChanged();
            }
        }

        private int busyMaxValue = 10;
        /// <summary>
        /// The max value of the ShowBusy component.
        /// </summary>
        public int BusyMaxValue
        {
            get
            {
                return this.busyMaxValue;
            }
            set
            {
                this.busyMaxValue = value;
                this.OnPropertyChanged();
            }
        }

        private int busyMinValue;
        /// <summary>
        /// The min value of the ShowBusy component.
        /// </summary>
        public int BusyMinValue
        {
            get
            {
                return this.busyMinValue;
            }
            set
            {
                this.busyMinValue = value;
                this.OnPropertyChanged();
            }
        }

        private int busyCurrentValue;
        /// <summary>
        /// The current value of the ShowBusy component.
        /// </summary>
        public int BusyCurrentValue
        {
            get
            {
                return this.busyCurrentValue;
            }
            set
            {
                this.busyCurrentValue = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Starts updating the BusyCurrentValue
        /// </summary>
        protected void BusyStart()
        {
            this.ShowBusy = Visibility.Visible;
            timer.Interval = 250;
            timer.Elapsed += (s, e) =>
            {
                // Access UI thread using a dispatcher.
                Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    if (this.BusyCurrentValue == this.BusyMaxValue)
                    {
                        this.BusyCurrentValue = this.BusyMinValue;
                    }
                    ++this.BusyCurrentValue;
                });
            };
            timer.Start();
        }

        /// <summary>
        /// Stops updating the BusyCurrentValue
        /// </summary>
        protected void BusyStop()
        {
            timer.Stop();
            this.ShowBusy = Visibility.Collapsed;
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Create the OnPropertyChanged method to raise the event 
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // sealed class so not virtual
        private void Dispose(bool disposing)
        {
            if (disposed || !disposing)
            {
                return;
            }

            if (this.timer != null)
            {
                this.timer.Dispose();
            }

            this.disposed = true;

        }
        #endregion
    }
}
