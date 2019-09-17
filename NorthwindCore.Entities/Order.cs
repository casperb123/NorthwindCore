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

        public int OrderId { get; set; }
        public string CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        public decimal? Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }

        public int TotalOrderDetails
        {
            get
            {
                return OrderDetails.Count;
            }
        }

        public decimal TotalPrice
        {
            get
            {
                decimal price = 0;
                OrderDetails.ToList().ForEach(o => price += o.UnitPrice);
                return price;
            }
        }

        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Shipper ShipViaNavigation { get; set; }
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
