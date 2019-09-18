using NorthwindCore.DataAccess;
using NorthwindCore.Entities;
using NorthwindCore.Gui.Desktop.ViewModels;
using NorthwindCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        /// <summary>
        /// Converts all the prices in the textboxes to the selected currency
        /// </summary>
        private void SetNewPrices()
        {
            ExchangeRate selectedRate = ordersUserControlViewModel.SelectedRate;

            if (ordersUserControlViewModel.SelectedOrder != null)
            {
                textBoxTotalPrice.Text = ordersUserControlViewModel.GetPrice(selectedRate, (double)ordersUserControlViewModel.SelectedOrder.TotalPrice).ToString();
                textBoxFreight.Text = ordersUserControlViewModel.GetPrice(selectedRate, (double)ordersUserControlViewModel.SelectedOrder.Freight).ToString();
                if (ordersUserControlViewModel.SelectedOrderDetail != null)
                {
                    textBoxUnitPrice.Text = ordersUserControlViewModel.GetPrice(selectedRate, (double)ordersUserControlViewModel.SelectedOrderDetail.UnitPrice).ToString();
                }
                if (ordersUserControlViewModel.SelectedInvoice != null)
                {
                    textBoxInvoiceUnitPrice.Text = ordersUserControlViewModel.GetPrice(selectedRate, (double)ordersUserControlViewModel.SelectedInvoice.UnitPrice).ToString();
                    textBoxInvoiceExtendedPrice.Text = ordersUserControlViewModel.GetPrice(selectedRate, (double)ordersUserControlViewModel.SelectedInvoice.ExtendedPrice).ToString();
                    textBoxInvoiceFreight.Text = ordersUserControlViewModel.GetPrice(selectedRate, (double)ordersUserControlViewModel.SelectedInvoice.Freight).ToString();
                }
            }
        }

        private void DataGridOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Task.Factory.StartNew(() => ordersUserControlViewModel.GetInvoices());
            SetNewPrices();
        }

        private void DataGridOrderDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetNewPrices();
        }

        private void ComboBoxCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetNewPrices();
        }

        private void DataGridInvoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetNewPrices();
        }
    }
}
