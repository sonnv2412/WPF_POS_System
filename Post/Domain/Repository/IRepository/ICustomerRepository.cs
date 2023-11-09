using Post.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Domain.Repository.IRepository
{
    interface ICustomerRepository
    {
        public IEnumerable<Customer> GetAllCustomer();
        public Customer GetCustomer(int id);
        public void DeleteCustomer(int id);
        public void UpdateCustomer(Customer customer);
        public void AddCustomer(Customer customer);
    }
}
