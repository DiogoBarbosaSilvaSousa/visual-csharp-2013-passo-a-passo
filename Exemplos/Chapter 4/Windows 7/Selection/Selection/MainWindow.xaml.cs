using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Selection
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void compareClick(object sender, RoutedEventArgs e)
        {
            int diff = dateCompare(first, second);
            info.Text = "";
            show("firstDate == secondDate", diff == 0);
            show("firstDate != secondDate", diff != 0);
            show("firstDate <  secondDate", diff < 0);
            show("firstDate <= secondDate", diff <= 0);
            show("firstDate >  secondDate", diff > 0);
            show("firstDate >= secondDate", diff >= 0);
        }

        private void show(string exp, bool result)
        {
            info.Text += exp;
            info.Text += " : " + result.ToString();
            info.Text += "\n";
        }

        private int dateCompare(DateTime leftHandSide, DateTime rightHandSide)
        {
            // TO DO
            return 42;
        }

        #region Conversion properties to cater for the deifferences betwwen the DatePicker control in Windows 7 and Windows 8.1
        private DateTime first
        {
            get { return this.firstDate.SelectedDate.GetValueOrDefault(this.firstDate.DisplayDate); }
        }

        private DateTime second
        {
            get { return this.secondDate.SelectedDate.GetValueOrDefault(this.secondDate.DisplayDate); }
        }
        #endregion
    }
}
