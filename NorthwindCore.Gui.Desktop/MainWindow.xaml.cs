﻿using NorthwindCore.Gui.Desktop.CustomControls;
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

namespace NorthwindCore.Gui.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public UserControl DetailsUserControl
        {
            get => detailsUserControl;
            set
            {
                if (value != null)
                {
                    detailsUserControl = value;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            masterUserControl.Content = new StackedButtonNavigationUserControl(this);
        }
    }
}
