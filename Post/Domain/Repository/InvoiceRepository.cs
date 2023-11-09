using Microsoft.EntityFrameworkCore;
using Post.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Domain.Repository.IRepository
{
    class InvoiceRepository : IInvoiceRepository
    {
        private PostContext _context;
        public InvoiceRepository(PostContext context)
        {
            _context = context;
        }
        public void AddInvoice(Invoice invoice)
        {
            try
            {
                _context.Invoices.Add(invoice);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw (new Exception("Error occured!"));
            }
        }

        public IEnumerable<Invoice> GetInvoices()
        {
            return _context.Invoices.Include(i=>i.Account).Include(i=>i.Sales).Include(i=>i.Customer);
        }

        public void AddSales(Invoice invoice, List<Sale> sales)
        {
            invoice.Sales = sales;
            _context.SaveChanges();
        }
    }
}
