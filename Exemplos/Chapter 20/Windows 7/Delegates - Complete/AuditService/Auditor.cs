using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTypes;
using System.IO;
using System.Xml;

namespace AuditService
{
    public class Auditor
    {
        public delegate void AuditingCompleteDelegate(string message);
        public event AuditingCompleteDelegate AuditProcessingComplete;

        public void AuditOrder(Order order)
        {
            this.doAuditing(order);
        }

        private async void doAuditing(Order order)
        {
            List<OrderItem> ageRestrictedItems = findAgeRestrictedItems(order);
            if (ageRestrictedItems.Count > 0)
            {
                try
                {
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    char seperator = Path.DirectorySeparatorChar;
                    FileStream file = await Task.Run<FileStream>(() => File.Create(path + seperator + "audit-" + order.OrderID.ToString() + ".xml"));

                    if (file != null)
                    {
                        XmlDocument doc = new XmlDocument();
                        XmlElement root = doc.CreateElement("Order");
                        root.SetAttribute("ID", order.OrderID.ToString());
                        root.SetAttribute("Date", order.Date.ToString());

                        foreach (OrderItem orderItem in ageRestrictedItems)
                        {
                            XmlElement itemElement = doc.CreateElement("Item");
                            itemElement.SetAttribute("Product", orderItem.Item.Name);
                            itemElement.SetAttribute("Description", orderItem.Item.Description);
                            root.AppendChild(itemElement);
                        }

                        doc.AppendChild(root);

                        await Task.Run(() => doc.Save(file));
                        await Task.Run(() => file.Close());
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
                    if (this.AuditProcessingComplete != null)
                    {
                        this.AuditProcessingComplete(String.Format(
                            "Audit record written for Order {0}", order.OrderID));
                    }
                }
            }
        }

        private List<OrderItem> findAgeRestrictedItems(Order order)
        {
            return order.Items.FindAll(o => o.Item.AgeRestricted == true);
        }
    }
}
