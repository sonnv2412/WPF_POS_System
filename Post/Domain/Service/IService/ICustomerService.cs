using Post.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Domain.Service.IService
{
    public interface ICustomerService
    {
        public List<Customer> GetAllCustomer();
        public Customer GetCustomerById(int id);
        public void AddCustomer(Customer c);
        public void UpdateCustomer(Customer c); 
    }
}
