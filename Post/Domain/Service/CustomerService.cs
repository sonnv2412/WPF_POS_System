using Post.Domain.Repository.IRepository;
using Post.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Domain.Service.IService
{
    class CustomerService : ICustomerService
    {
        ICustomerRepository customerRepository;
        public CustomerService(ICustomerRepository customerRepository) {
            this.customerRepository = customerRepository;
        }
        public void AddCustomer(Customer c)
        {
            customerRepository.AddCustomer(c);
        }

        public List<Customer> GetAllCustomer()
        {
            return customerRepository.GetAllCustomer().ToList();
        }

        public Customer GetCustomerById(int id)
        {
            return customerRepository.GetCustomer(id);
        }

        public void UpdateCustomer(Customer c)
        {
            customerRepository.UpdateCustomer(c);
        }
    }
}
