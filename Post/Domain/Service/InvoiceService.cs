using Microsoft.EntityFrameworkCore;
using Post.Domain.Repository.IRepository;
using Post.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Post.Domain.Service.IService
{

    class InvoiceService : IInvoiceService
    {
        private IInvoiceRepository invoiceRepository;
        private ICustomerService customerService;
        public InvoiceService(IInvoiceRepository invoiceRepository, ICustomerService customerService)
        {
            this.invoiceRepository = invoiceRepository;
            this.customerService = customerService;
        }
        public void AddInvoice(Invoice invoice, Customer customer, Dictionary<Product, int> orders)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    String addForeignKeyContraint = "ALTER TABLE Invoice\r\nADD CONSTRAINT FK_Invoice_Customer FOREIGN KEY (customer_id) REFERENCES Customer(customer_id);";
                    String dropForeignKeyContraint = "ALTER TABLE Invoice\r\nDrop CONSTRAINT FK_Invoice_Customer;";

                    if (customer == null)
                    {
                        PostContext context = new PostContext();
                        context.Database.ExecuteSqlRaw(dropForeignKeyContraint);
                        invoiceRepository.AddInvoice(invoice);
                        AddSales(invoice, orders);
                        context.Database.ExecuteSqlRaw(addForeignKeyContraint);
                        context.SaveChanges();
                    }
                    else
                    {
                        if (invoice.TotalAmount >= 100000)
                        {
                            double? point = invoice.TotalAmount / 100000;
                            customer.Point += Convert.ToInt32(point);
                        }
                        invoice.Customer = customer;
                        invoiceRepository.AddInvoice(invoice);
                        AddSales(invoice, orders);
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error occured");
                }

            }

        }

        public List<Invoice> GetAllInvoices()
        {
            return invoiceRepository.GetInvoices().ToList();
        }

        public void AddSales(Invoice invoice, Dictionary<Product, int> orders)
        {
            List<Sale> sales = new List<Sale>();
            foreach (var order in orders)
            {
                Sale s = new Sale();
                s.InvoiceId = invoice.InvoiceId;
                s.ProductId = order.Key.ProductId;
                s.Quantity = order.Value;
                s.UnitPrice = order.Key.UnitPrice;
                s.SubTotal = order.Key.UnitPrice * (order.Key.DiscountPercentage / 100);
                sales.Add(s);
            }
            invoiceRepository.AddSales(invoice, sales);
        }

        public List<Invoice> filterInvoice(string customerName, string RecordedDate)
        {
            List<Invoice> result = new List<Invoice>();
            if (!customerName.Equals(""))
            {
                if (RecordedDate.Equals(""))
                {
                    result = GetAllInvoices().Where(i => i.CustomerId != null && i.Customer.CustomerName.ToLower().Contains(customerName.ToLower())).ToList();
                }
                else
                {
                    result = GetAllInvoices().Where(i => i.CustomerId != null && i.Customer.CustomerName.ToLower().Contains(customerName.ToLower()) && i.DateRecorded.Equals(DateTime.Parse(RecordedDate))).ToList();

                }
            }
            else
            {
                if (RecordedDate.Equals(""))
                {
                    result = GetAllInvoices().ToList();
                }
                else
                {
                    result = GetAllInvoices().Where(i => i.DateRecorded.Equals(DateTime.Parse(RecordedDate))).ToList();

                }
            }
            return result;
        }
    }
}
