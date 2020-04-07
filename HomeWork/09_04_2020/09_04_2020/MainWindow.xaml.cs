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

namespace _09_04_2020
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "_00":
                    break;
                case "_01":
                    break;
                case "_02":
                    break;
                ///////////////
                case "_10":
                    break;
                case "_11":
                    break;
                case "_12":
                    break;
                ///////////////
                case "_20":
                    break;
                case "_21":
                    break;
                case "_22":
                    break;
                default:
                    MessageBox.Show("Eror Invalit press button☺");
                    break;
            }
        }
    }
}
