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
        private Invoice selectecInvoice;

        public Invoice SelectedInvoice
        {
            get { return selectecInvoice; }
            set
            {
                selectecInvoice = value;
                OnPropertyChanged(nameof(SelectedInvoice));
            }
        }

        public ObservableCollection<Invoice> Invoices
        {
            get { return invoices; }
            set
            {
                invoices = value;
                OnPropertyChanged(nameof(Invoices));
            }
        }

        public List<ExchangeRate> ExchangeRates { get; set; }

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

        public OrderDetail SelectedOrderDetail
        {
            get { return selectedOrderDetail; }
            set
            {
                selectedOrderDetail = value;
                OnPropertyChanged(nameof(SelectedOrderDetail));
            }
        }

        public Order SelectedOrder
        {
            get { return selectedOrder; }
            set
            {
                selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
            }
        }

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

        public double GetPrice(ExchangeRate exchangeRate, double amount)
        {
            double converted = Math.Round(amount * exchangeRate.Rate, 5);
            if (converted < 1)
            {
                return converted;
            }
            return Math.Round(converted, 2);
        }

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
