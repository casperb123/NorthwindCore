using NorthwindCore.Entities;
using NorthwindCore.Gui.Desktop.ViewModels;
using NorthwindCore.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NorthwindCore.Gui.Desktop.CustomControls
{
    /// <summary>
    /// Interaction logic for OrdersUserControl.xaml
    /// </summary>
    public partial class OrdersUserControl : UserControl
    {
        private readonly OrdersUserControlViewModel ordersUserControlViewModel;
        private Dictionary<string, double> rates;

        public OrdersUserControl()
        {
            InitializeComponent();
            ordersUserControlViewModel = new OrdersUserControlViewModel();
            DataContext = ordersUserControlViewModel;
            ValidationWebService validationWebService = new ValidationWebService();
            rates = validationWebService.GetRates().rates;
            foreach (KeyValuePair<string, double> rate in rates)
            {
                comboBoxCurrency.Items.Add(rate.Key);
            }
        }

        private void DataGridOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidationWebService validationWebService = new ValidationWebService();
            ordersUserControlViewModel.SelectedOrder = dataGridOrders.SelectedItem as Order;
            textBoxTotalPrice.Text = validationWebService.GetPrice(ordersUserControlViewModel.SelectedCurrency, ordersUserControlViewModel.SelectedOrder.TotalPrice).ToString();
        }

        private void DataGridOrderDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ordersUserControlViewModel.SelectedOrderDetail = dataGridOrderDetails.SelectedItem as OrderDetail;
        }

        private void ComboBoxCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidationWebService validationWebService = new ValidationWebService();
            ordersUserControlViewModel.SelectedCurrency = comboBoxCurrency.SelectedItem.ToString();
            textBoxTotalPrice.Text = validationWebService.GetPrice(ordersUserControlViewModel.SelectedCurrency, ordersUserControlViewModel.SelectedOrder.TotalPrice).ToString();
        }
    }
}
