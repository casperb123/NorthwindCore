using NorthwindCore.Entities;
using NorthwindCore.Gui.Desktop.ViewModels;
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

        public OrdersUserControl()
        {
            InitializeComponent();
            ordersUserControlViewModel = new OrdersUserControlViewModel();
            DataContext = ordersUserControlViewModel;
        }

        private void DataGridOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ordersUserControlViewModel.SelectedOrder = dataGridOrders.SelectedItem as Order;
        }
    }
}
