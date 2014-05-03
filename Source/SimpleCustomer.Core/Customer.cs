using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace SimpleCustomer.Core
{
    /// <summary>
    /// The customer class. Provides basic fields and validation of the customer. 
    /// Allow the object to be flexible and easy to modfiy its properties as the object
    /// is used only to carry data to the database entity.
    /// </summary>
    public class Customer : INotifyPropertyChanged, IDataErrorInfo
    {
        private int id;
        /// <summary>
        /// The Id of the customer.
        /// </summary>
        public int Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
                this.OnPropertyChanged();
            }
        }

        private string name;
        /// <summary>
        /// The name of the customer.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                this.OnPropertyChanged();
            }
        }

        private Gender gender;
        /// <summary>
        /// The gender of the customer.
        /// </summary>
        public Gender Gender
        {
            get
            {
                return this.gender;
            }
            set
            {
                this.gender = value;
                this.OnPropertyChanged();
            }
        }

        private Category category;
        /// <summary>
        /// The category of the customer.
        /// </summary>
        public Category Category
        {
            get
            {
                return this.category;
            }
            set
            {
                this.category = value;
                this.OnPropertyChanged();
            }
        }

        private DateTime dateOfBirth;
        /// <summary>
        /// The date of birth of the customer.
        /// </summary>
        public DateTime DateOfBirth
        {
            get
            {
                return this.dateOfBirth;
            }
            set
            {
                this.dateOfBirth = value;
                this.OnPropertyChanged();
            }
        }

        private string houseNumber;
        /// <summary>
        /// The house number of the customer.
        /// </summary>
        public string HouseNumber
        {
            get
            {
                return this.houseNumber;
            }
            set
            {
                this.houseNumber = value;
                this.OnPropertyChanged();
            }
        }

        private string addressLineOne;
        /// <summary>
        /// The first line of the address of the customer.
        /// </summary>
        public string AddressLineOne
        {
            get
            {
                return this.addressLineOne;
            }
            set
            {
                this.addressLineOne = value;
                this.OnPropertyChanged();
            }
        }

        private string state;
        /// <summary>
        /// The address state of the customer.
        /// </summary>
        public string State
        {
            get
            {
                return this.state;
            }
            set
            {
                this.state = value;
                this.OnPropertyChanged();
            }
        }

        private string country;
        /// <summary>
        /// The country of the customer.
        /// </summary>
        public string Country
        {
            get
            {
                return this.country;
            }
            set
            {
                this.country = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Makes a copy of all properties from the source customer to current customer
        /// </summary>
        /// <param name="sourceCustomer">The source customer to copy properties from</param>
        public void Copy(Customer sourceCustomer)
        {
            this.Id = sourceCustomer.Id;
            this.AddressLineOne = sourceCustomer.AddressLineOne;
            this.Category = sourceCustomer.Category;
            this.Country = sourceCustomer.Country;
            this.DateOfBirth = sourceCustomer.DateOfBirth;
            this.Gender = sourceCustomer.Gender;
            this.HouseNumber = sourceCustomer.HouseNumber;
            this.Name = sourceCustomer.Name;
            this.State = sourceCustomer.State;
        }

        #region Validation

        static readonly string[] ValidatedProperties =
        {
            "Name", "DateOfBirth", "HouseNumber",  "AddressLineOne", "State", "Country"
        };

        /// <summary>
        /// Whether all required properties are valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                foreach (string property in ValidatedProperties)
                {
                    if (GetValidationError(property) != null)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private string GetValidationError(string propertyName)
        {
            this.Error = null;

            switch (propertyName)
            {
                case "Name":
                    this.Error = this.ValidateCustomerName();
                    break;
                case "DateOfBirth":
                    this.Error = this.ValidateCustomerDateOfBirth();
                    break;
                case "HouseNumber":
                    this.Error = this.ValidateCustomerHouseNumber();
                    break;
                case "AddressLineOne":
                    this.Error = this.ValidateCustomerAddressLineOne();
                    break;
                case "State":
                    this.Error = this.ValidateCustomerState();
                    break;
                case "Country":
                    this.Error = this.ValidateCustomerCountry();
                    break;
            }

            return this.Error;
        }

        private string ValidateCustomerCountry()
        {
            if (String.IsNullOrWhiteSpace(this.Country))
            {
                return "The country cannot be empty.";
            }
            if (this.Country.Length > 50)
            {
                return "The country name cannot be longer than 50 characters.";
            }

            return null;
        }

        private string ValidateCustomerState()
        {
            if (String.IsNullOrWhiteSpace(this.State))
            {
                return "Customer state cannot be empty.";
            }
            if (this.State.Length > 50)
            {
                return "The state name cannot be longer than 50 characters.";
            }
            return null;
        }

        private string ValidateCustomerAddressLineOne()
        {
            if (String.IsNullOrWhiteSpace(this.AddressLineOne))
            {
                return "Customer address cannot be empty.";
            }
            if (this.AddressLineOne.Length > 255)
            {
                return "The address cannot be longer than 255 characters.";
            }
            return null;
        }

        private string ValidateCustomerHouseNumber()
        {
            if (String.IsNullOrWhiteSpace(this.HouseNumber))
            {
                return "Customer house number cannot be empty.";
            }
            if (this.HouseNumber.Length > 50)
            {
                return "The house number cannot be longer than 50 characters.";
            }
            return null;
        }

        private string ValidateCustomerDateOfBirth()
        {
            if (this.DateOfBirth > DateTime.Now)
            {
                return "Customer date of birth cannot be in the future.";
            }

            int ageInYears = DateTime.Now.Year - this.DateOfBirth.Year;
            if (ageInYears > 180)
            {
                return "Customer must be alive.";
            }

            return null;
        }

        private string ValidateCustomerName()
        {
            if (String.IsNullOrWhiteSpace(this.Name))
            {
                return "Customer name cannot be empty";
            }
            if (this.Name.Length > 50)
            {
                return "The customer's name cannot be longer than 50 characters.";
            }
            return null;
        }
        #endregion

        #region IDataErrorInfo
        /// <summary>
        /// String with error message if client is invalid.
        /// </summary>
        public string Error
        {
            get;
            private set;
        }

        /// <summary>
        /// Indexer accessing propety name.
        /// </summary>
        public string this[string propertyName]
        {
            get
            {
                return GetValidationError(propertyName);
            }
        }

        #endregion

        #region INotifyPropertyChanged
        /// <summary>
        /// Event triggered when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Create the OnPropertyChanged method to raise the event 
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
