using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace NorthwindCore.Entities
{
    public partial class Order : INotifyPropertyChanged
    {
        private ICollection<OrderDetail> orderDetails;

        public Order()
        {
            orderDetails = new ObservableCollection<OrderDetail>();
        }

        /// <summary>
        /// Gets and sets the order id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Gets and sets the customer id
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets and sets the employee id
        /// </summary>
        public int? EmployeeId { get; set; }

        /// <summary>
        /// Gets and sets the order date
        /// </summary>
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// Gets and sets the required date
        /// </summary>
        public DateTime? RequiredDate { get; set; }

        /// <summary>
        /// Gets and sets the shipped date
        /// </summary>
        public DateTime? ShippedDate { get; set; }

        /// <summary>
        /// Gets and sets the ship via
        /// </summary>
        public int? ShipVia { get; set; }

        /// <summary>
        /// Gets and sets the freight
        /// </summary>
        public decimal? Freight { get; set; }

        /// <summary>
        /// Gets and sets the ship name
        /// </summary>
        public string ShipName { get; set; }

        /// <summary>
        /// Gets and sets the ship address
        /// </summary>
        public string ShipAddress { get; set; }

        /// <summary>
        /// Gets and sets the ship city
        /// </summary>
        public string ShipCity { get; set; }

        /// <summary>
        /// Gets and sets the ship region
        /// </summary>
        public string ShipRegion { get; set; }

        /// <summary>
        /// Gets and sets the ship postal code
        /// </summary>
        public string ShipPostalCode { get; set; }

        /// <summary>
        /// Gets and sets the ship country
        /// </summary>
        public string ShipCountry { get; set; }

        /// <summary>
        /// Returns the total number of order details
        /// </summary>
        public int TotalOrderDetails
        {
            get
            {
                return OrderDetails.Count;
            }
        }

        /// <summary>
        /// Returns the total price for all order details
        /// </summary>
        public decimal TotalPrice
        {
            get
            {
                decimal price = 0;
                OrderDetails.ToList().ForEach(o => price += o.UnitPrice);
                return price;
            }
        }

        /// <summary>
        /// Gets and sets the customer navigation property
        /// </summary>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Gets and sets the employee navigation property
        /// </summary>
        public virtual Employee Employee { get; set; }

        /// <summary>
        /// Gets and sets the ship via navigation property
        /// </summary>
        public virtual Shipper ShipViaNavigation { get; set; }

        /// <summary>
        /// Gets and sets the order details list
        /// </summary>
        public virtual ICollection<OrderDetail> OrderDetails
        {
            get
            {
                return orderDetails;
            }
            set
            {
                if (value is null)
                {
                    throw new NullReferenceException("Listen med ordrer detaljer kan ikke være null");
                }

                orderDetails = value;
                OnPropertyChanged(nameof(OrderDetails));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string prop)
        {
            if (prop != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
