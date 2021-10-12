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

namespace Indexers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PhoneBook phoneBook = new PhoneBook();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void findByNameClick(object sender, RoutedEventArgs e)
        {
            // TODO:
        }

        private void findByPhoneNumberClick(object sender, RoutedEventArgs e)
        {
            // TODO:
        }

        private void addClick(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(name.Text) && !String.IsNullOrEmpty(phoneNumber.Text))
            {
                phoneBook.Add(new Name(name.Text),
                              new PhoneNumber(phoneNumber.Text));
                name.Text = "";
                phoneNumber.Text = "";
            }
        }
    }
}
