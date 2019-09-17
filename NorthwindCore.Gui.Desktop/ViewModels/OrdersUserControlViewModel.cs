using NorthwindCore.DataAccess;
using NorthwindCore.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindCore.Gui.Desktop.ViewModels
{
    public class OrdersUserControlViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Order> orders;
        private Order selectedOrder;

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

        public OrdersUserControlViewModel()
        {
            orders = new ObservableCollection<Order>();
            Task.Factory.StartNew(() => GetOrders());
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
