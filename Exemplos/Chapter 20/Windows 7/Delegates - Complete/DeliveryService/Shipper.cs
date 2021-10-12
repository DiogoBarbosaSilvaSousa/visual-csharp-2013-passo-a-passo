using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTypes;
using System.IO;

namespace DeliveryService
{
    public class Shipper
    {
        public delegate void ShippingCompleteDelegate(string message);
        public event ShippingCompleteDelegate ShipProcessingComplete;

        public void ShipOrder(Order order)
        {
            this.doShipping(order);
        }

        private async void doShipping(Order order)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                char seperator = Path.DirectorySeparatorChar;
                FileStream file = await Task.Run<FileStream>(() => File.Create(path + seperator + "dispatch-" + order.OrderID.ToString() + ".txt"));

                if (file != null)
                {
                    string dispatchNote = "Order Summary: " +
                                          "\r\nOrder ID: " + order.OrderID +
                                          "\r\nOrder Total: " + String.Format("{0:C}", order.TotalValue);

                    StreamWriter writer = new StreamWriter(file);
                    await writer.WriteAsync(dispatchNote);
                    await Task.Run(() => writer.Close());
                }
                else
                {
                    MessageBox.Show(String.Format("Unable to save to file: {0}", file.Name), "Not saved");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception");
            }
            finally
            {
                if (this.ShipProcessingComplete != null)
                {
                    this.ShipProcessingComplete(String.Format(
                         "Dispatch note generated for Order {0}", order.OrderID));
                }
            }

        }
    }
}
