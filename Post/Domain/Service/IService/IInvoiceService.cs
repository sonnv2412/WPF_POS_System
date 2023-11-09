using Post.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Domain.Service.IService
{
    public interface IInvoiceService
    {
        public void AddInvoice(Invoice invoice, Customer customer, Dictionary<Product, int> orders);
        public List<Invoice> GetAllInvoices();
        public void AddSales (Invoice invoice, Dictionary<Product, int> orders);
        public List<Invoice> filterInvoice(string customerName, string RecordedDate);
    }
}
