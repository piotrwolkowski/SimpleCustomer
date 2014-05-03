using SimpleCustomer.View;
namespace SimpleCustomer.ViewModel
{
    /// <summary>
    /// A simpe message box view model.
    /// </summary>
    internal class InfoDialogViewModel : ViewModelBase
    {
        #region fields and properties
        private string text;
        /// <summary>
        /// The text of the message box.
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
                this.OnPropertyChanged();
            }
        }

        private string caption;
        /// <summary>
        /// The caption of the message box.
        /// </summary>
        public string Caption
        {
            get
            {
                return this.caption;
            }
            set
            {
                this.caption = value;
                this.OnPropertyChanged();
            }
        }
        #endregion fields and properties

        #region c-tor
        /// <summary>
        /// Creates an instance of a simple message box
        /// </summary>
        /// <param name="text">The text displayed in the message box.</param>
        /// <param name="caption">The caption of the message box.</param>
        private InfoDialogViewModel(string text, string caption)
        {
            this.Text = text;
            this.Caption = caption;
        }
        #endregion c-tor

        /// <summary>
        /// Invokes the message box.
        /// </summary>
        /// <param name="text">The text displayed in the message box.</param>
        /// <param name="caption">The caption of the message box.</param>
        internal static void ShowDialog(string text, string caption)
        {
            InfoDialogView view = new InfoDialogView();
            InfoDialogViewModel viewModel = new InfoDialogViewModel(text, caption);
            view.DataContext = viewModel;
            view.ShowDialog();
        }
    }
}
