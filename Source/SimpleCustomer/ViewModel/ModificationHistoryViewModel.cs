namespace SimpleCustomer.ViewModel
{
    /// <summary>
    /// Simple view model that allows displaying a modification history in form of csv file.
    /// </summary>
    internal class ModificationHistoryViewModel : ViewModelBase
    {
        private string modificationHistoryText;
        public string ModificationHistoryText
        {
            get
            {
                return this.modificationHistoryText;
            }
            set
            {
                this.modificationHistoryText = value;
                this.OnPropertyChanged();
            }
        }

        internal ModificationHistoryViewModel(string modificationHistoryText)
        {
            this.ModificationHistoryText = modificationHistoryText;
        }
    }
}
