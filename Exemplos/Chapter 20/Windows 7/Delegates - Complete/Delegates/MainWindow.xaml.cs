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
using DataTypes;
using AuditService;
using DeliveryService;
using CheckoutService;

namespace Delegates
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProductsDataSource data = null;
        private Order order = null;
        private Auditor auditor = null;
        private Shipper shipper = null;
        private CheckoutController checkoutController = null;

        public MainWindow()
        {
            InitializeComponent();

            this.auditor = new Auditor();
            this.shipper = new Shipper();
            this.checkoutController = new CheckoutController();
            this.checkoutController.CheckoutProcessing += this.auditor.AuditOrder;
            this.checkoutController.CheckoutProcessing += this.shipper.ShipOrder;

            this.auditor.AuditProcessingComplete += this.displayMessage;
            this.shipper.ShipProcessingComplete += this.displayMessage;
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            data = new ProductsDataSource();
            this.productList.DataContext = data.Products;

            this.order = new Order { Date = DateTime.Now, Items = new List<OrderItem>(), OrderID = Guid.NewGuid(), TotalValue = 0 };
        }

        private void AddProductToOrderButtonClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                // Find the product ID of the selected product (contained in the Tag property of the button)
                Button addButton = sender as Button;
                string productId = addButton.Tag as string;

                // Display the list view header if it is not already visible
                this.listViewHeader.Visibility = Visibility.Visible;

                // Enable the checkout button if it is not already enabled
                this.checkout.IsEnabled = true;

                // Check to see whether this product has already been added to the order
                OrderItem orderItem = order.Items.Find(o => o.Item.ProductID == productId);
                if (orderItem != null)
                {
                    // If the product is already included the order just increment the quantity
                    orderItem.Quantity++;

                    // Update the total value of the order
                    order.TotalValue += orderItem.Item.Price;
                }
                else
                {
                    // If the product has not previously been included in the order then add it

                    // First, find the details of the product
                    Product product = data.Products.Find(p => p.ProductID == productId);

                    // Create an OrderItem that references this product and set the Quatity to 1
                    orderItem = new OrderItem { Item = product, Quantity = 1 };

                    // Add the OrderItem to the Order
                    this.order.Items.Add(orderItem);

                    // Update the total value of the order
                    this.order.TotalValue += product.Price;
                }

                // Rebind the ListView to the order data to update the display
                this.orderDetails.DataContext = null;
                this.orderDetails.DataContext = order.Items;

                // Display the total order value
                this.orderValue.Text = String.Format("{0:C}", order.TotalValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception");
            }
        }

        private void CheckoutButtonClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                // Perform the checkout processing
                this.checkoutController.StartCheckoutProcessing(this.order);
                                 
                // Clear out the order details so the user can start again with a new order
                this.order = new Order { Date = DateTime.Now, Items = new List<OrderItem>(), OrderID = Guid.NewGuid(), TotalValue = 0 };
                this.orderDetails.DataContext = null;
                this.orderValue.Text = String.Format("{0:C}", order.TotalValue);
                this.listViewHeader.Visibility = Visibility.Collapsed;
                this.checkout.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception");
            }
        }

        private void displayMessage(string message)
        {
            this.messageBar.Text += message + "\n";
        }
    }
}
