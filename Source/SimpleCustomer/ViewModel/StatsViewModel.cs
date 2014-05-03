using SimpleCustomer.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCustomer.ViewModel
{
    /// <summary>
    /// View model with statistical data
    /// </summary>
    public class StatsViewModel : ViewModelBase
    {
        #region fields and properties
        private IEnumerable<Customer> customers;
        private StatsType selectedStatsType;
        /// <summary>
        /// Enforces calculation of proper statistical data dependinc on the StatsType enum.
        /// </summary>
        public StatsType SelectedStatsType
        {
            get
            {
                return this.selectedStatsType;
            }

            set
            {
                this.selectedStatsType = value;
                if (this.selectedStatsType == StatsType.Category)
                {
                    this.StatsData = this.StatsByCategoryByNumber();
                    this.StatsDataPercent = this.StatsByCategoryByPercent();
                }
                else if (this.selectedStatsType == StatsType.Location)
                {
                    this.StatsData = this.StatsByLocationByNumber();
                    this.StatsDataPercent = this.StatsByLocationByPercent();
                }
                this.OnPropertyChanged();
            }
        }

        private List<KeyValuePair<string, decimal>> statsData;
        /// <summary>
        /// Statistical data that are a source for the StatsView chart.
        /// </summary>
        public List<KeyValuePair<string, decimal>> StatsData
        {
            get
            {
                return this.statsData;
            }
            set
            {
                this.statsData = value;
                this.OnPropertyChanged();
            }
        }

        private List<Tuple<string, decimal, decimal>> statsDataPercent;
        /// <summary>
        /// Statistical data containing in numbers and percentage that
        /// are a source for the DataGrid.
        /// </summary>
        public List<Tuple<string, decimal, decimal>> StatsDataPercent
        {
            get
            {
                return this.statsDataPercent;
            }
            set
            {
                this.statsDataPercent = value;
                this.OnPropertyChanged();
            }
        }
        #endregion fields and properties

        #region c-tors
        /// <summary>
        /// Create a new instance of the StatsViewModel.
        /// </summary>
        /// <param name="customers">The set of customers for which the statistical data are created</param>
        public StatsViewModel(IEnumerable<Customer> customers)
        {
            this.customers = customers;
            this.SelectedStatsType = StatsType.Location;
        }
        #endregion c-tors

        #region calculate statistical data
        /// <summary>
        /// Calculate statistical data for customers by location.
        /// </summary>
        /// <returns>Returns statistical data in a format that the chart can understand.</returns>
        private List<KeyValuePair<string, decimal>> StatsByLocationByNumber()
        {
            Dictionary<Tuple<string, string>, List<Customer>> customersByLocation =
                this.customers.Select(c => new { Key = new Tuple<string, string>(c.Country, c.State), Value = c })
                              .GroupBy(item => item.Key)
                              .ToDictionary(
                                group => group.Key,
                                group => group.Select(item => item.Value).ToList()
                              );

            List<KeyValuePair<string, decimal>> customersByLocationByNumber =
                customersByLocation.ToDictionary(
                    entry => entry.Key.Item1 + ", " + entry.Key.Item2,
                    entry => (decimal)entry.Value.Count
                ).ToList();

            return customersByLocationByNumber;
        }

        /// <summary>
        /// Calculate statistical data for customers by location in percents.
        /// </summary>
        /// <returns>Returns statistical data in a format that the chart can understand.</returns>
        private List<Tuple<string, decimal, decimal>> StatsByLocationByPercent()
        {
            Dictionary<Tuple<string, string>, List<Customer>> customersByLocation =
                this.customers.Select(c => new { Key = new Tuple<string, string>(c.Country, c.State), Value = c })
                              .GroupBy(item => item.Key)
                              .ToDictionary(
                                group => group.Key,
                                group => group.Select(item => item.Value).ToList()
                              );

            decimal customersTotal = this.customers.Count();
            List<Tuple<string, decimal, decimal>> customersByLocationByPercent =
                customersByLocation.Select(entry =>
                    {
                        return new Tuple<string, decimal, decimal>(
                        entry.Key.Item1 + ", " + entry.Key.Item2,
                        entry.Value.Count,
                        Math.Round(entry.Value.Count / customersTotal * 100m, 2));
                    }
                ).ToList();

            return customersByLocationByPercent;
        }

        /// <summary>
        /// Calculate statistical data for customers by category.
        /// </summary>
        /// <returns>Returns statistical data in a format that the chart can understand.</returns>
        private List<KeyValuePair<string, decimal>> StatsByCategoryByNumber()
        {
            Dictionary<Category, List<Customer>> customersByCategory =
                this.customers.GroupBy(c => c.Category)
                              .ToDictionary(
                                group => group.Key,
                                group => group.Select(item => item).ToList()
                              );

            List<KeyValuePair<string, decimal>> customersByCategoryByNumber =
                customersByCategory.ToDictionary(
                    entry => entry.Key.ToString(),
                    entry => (decimal)entry.Value.Count
                ).ToList();

            return customersByCategoryByNumber;
        }

        /// <summary>
        /// Calculate statistical data for customers by category.
        /// </summary>
        /// <returns>Returns statistical data in a format that the chart can understand.</returns>
        private List<Tuple<string, decimal, decimal>> StatsByCategoryByPercent()
        {
            Dictionary<Category, List<Customer>> customersByCategory =
            this.customers.GroupBy(c => c.Category)
                          .ToDictionary(
                            group => group.Key,
                            group => group.Select(item => item).ToList()
                          );

            decimal usersTotal = this.customers.Count();
            List<Tuple<string, decimal, decimal>> customersByCategoryByPercent =
                customersByCategory.Select(entry =>
                    {
                        return new Tuple<string, decimal, decimal>(
                        entry.Key.ToString(),
                        entry.Value.Count,
                        Math.Round(entry.Value.Count / usersTotal * 100m, 2));
                    }
                ).ToList();

            return customersByCategoryByPercent;
        }
        #endregion calculate statistical data
    }
    
    // Used locally to fill in combobox. Do not move it to the Core library
    /// <summary>
    /// Enum to which the View binds to provide combobox values.
    /// </summary>
    public enum StatsType
    {
        Location,
        Category,
    }

}
