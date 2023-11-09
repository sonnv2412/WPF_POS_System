using Post.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Domain.Repository.IRepository
{
    interface IInvoiceRepository
    {
        public void AddInvoice(Invoice invoice);
        public IEnumerable<Invoice> GetInvoices();
        public void AddSales(Invoice invoice, List<Sale> sales);
    }
}
