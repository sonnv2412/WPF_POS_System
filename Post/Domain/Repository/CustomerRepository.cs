using Post.Domain.Repository.IRepository;
using Post.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Domain.Repository
{
    class CustomerRepository : ICustomerRepository
    {
        private PostContext _context;
        public CustomerRepository(PostContext context)
        {
            _context = context;
        }
        public void AddCustomer(Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public void DeleteCustomer(int id)
        {
            Customer c = _context.Customers.FirstOrDefault(c => c.CustomerId == id);
            if (c != null)
            {
                _context.Customers.Remove(c);
                _context.SaveChanges();
            }
            else
            {
                throw (new Exception("Customer with id " + id + " does not exist"));
            }
        }

        public IEnumerable<Customer> GetAllCustomer()
        {
            return _context.Customers;
        }

        public Customer GetCustomer(int id)
        {
            Customer c = _context.Customers.FirstOrDefault(c => c.CustomerId == id);
            if (c != null)
            {
                return c;
            }
            else
            {
                throw (new Exception("Customer with id " + id + " does not exist"));
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            Customer c = _context.Customers.FirstOrDefault(c => c.CustomerId == customer.CustomerId);
            if (c != null)
            {
                _context.Customers.Update(customer);
                _context.SaveChanges();
            }
            else
            {
                throw (new Exception("Customer with id " + customer.CustomerId + " does not exist"));
            }
        }
    }
}
