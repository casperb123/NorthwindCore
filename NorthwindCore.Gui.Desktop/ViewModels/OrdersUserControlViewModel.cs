using NorthwindCore.DataAccess;
using NorthwindCore.Entities;
using NorthwindCore.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindCore.Gui.Desktop.ViewModels
{
    public class OrdersUserControlViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Order> orders;
        private Order selectedOrder;
        private OrderDetail selectedOrderDetail;
        private ExchangeRate selectedRate;
        private ObservableCollection<Invoice> invoices;
        private Invoice selectedInvoice;

        /// <summary>
        /// The selected invoice
        /// </summary>
        public Invoice SelectedInvoice
        {
            get { return selectedInvoice; }
            set
            {
                selectedInvoice = value;
                OnPropertyChanged(nameof(SelectedInvoice));
            }
        }

        /// <summary>
        /// The list of invoices for the selected order
        /// </summary>
        public ObservableCollection<Invoice> Invoices
        {
            get { return invoices; }
            set
            {
                invoices = value;
                OnPropertyChanged(nameof(Invoices));
            }
        }

        /// <summary>
        /// The list of exchange rates
        /// </summary>
        public List<ExchangeRate> ExchangeRates { get; set; }

        /// <summary>
        /// The selected exchange rate
        /// </summary>
        public ExchangeRate SelectedRate
        {
            get { return selectedRate; }
            set
            {
                if (value is null)
                {
                    throw new NullReferenceException("Valuta kan ikke være null");
                }

                selectedRate = value;
                OnPropertyChanged(nameof(SelectedRate));
            }
        }

        /// <summary>
        /// The selected order detail
        /// </summary>
        public OrderDetail SelectedOrderDetail
        {
            get { return selectedOrderDetail; }
            set
            {
                selectedOrderDetail = value;
                OnPropertyChanged(nameof(SelectedOrderDetail));
            }
        }

        /// <summary>
        /// The selected order
        /// </summary>
        public Order SelectedOrder
        {
            get { return selectedOrder; }
            set
            {
                selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
            }
        }

        /// <summary>
        /// The list of orders
        /// </summary>
        public ObservableCollection<Order> Orders
        {
            get { return orders; }
            set
            {
                if (value is null)
                {
                    throw new NullReferenceException("Listen med ordrer kan ikke være null");
                }

                orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }

        /// <summary>
        /// Returns the converted price
        /// </summary>
        /// <param name="exchangeRate">The exchange rate to convert to</param>
        /// <param name="price">The price to be converted</param>
        /// <returns>Converted price</returns>
        public double GetPrice(ExchangeRate exchangeRate, double price)
        {
            double converted = Math.Round(price * exchangeRate.Rate, 5);
            if (converted < 1)
            {
                return converted;
            }
            return Math.Round(converted, 2);
        }

        /// <summary>
        /// Gets a list of invoices and then adds them to the invoices list
        /// </summary>
        public async void GetInvoices()
        {
            Repository repository = new Repository();
            App.Current.Dispatcher.Invoke(() =>
            {
                Invoices.Clear();
            });
            await foreach (Invoice invoice in repository.GetInvoicesForOrderAsync(SelectedOrder.OrderId))
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    Invoices.Add(invoice);
                });
            }
        }

        public OrdersUserControlViewModel()
        {
            ValidationWebService validationWebService = new ValidationWebService();
            ExchangeRates = validationWebService.GetRates().ToList();
            orders = new ObservableCollection<Order>();
            invoices = new ObservableCollection<Invoice>();
            Task.Factory.StartNew(() => GetOrders());
            selectedRate = ExchangeRates.FirstOrDefault(e => e.Currency == "USD");
        }

        /// <summary>
        /// Gets a list of all order and then adds them to the orders list
        /// </summary>
        private async void GetOrders()
        {
            Repository repository = new Repository();
            int count = 0;
            await foreach (Order order in repository.GetOrdersAsync())
            {
                if (count < 25)
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        Orders.Add(order);
                    });
                    count++;
                }
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
