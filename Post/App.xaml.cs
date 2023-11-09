using Microsoft.Extensions.DependencyInjection;
using Post.Domain.Repository;
using Post.Domain.Repository.IRepository;
using Post.Domain.Service;
using Post.Domain.Service.IService;
using Post.Model;
using Post.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Post
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceColection = new ServiceCollection(); 
          
            serviceColection.AddTransient<IProductService, ProductService>();
            serviceColection.AddTransient<IProductRepository, ProductRepository>();
            
            serviceColection.AddTransient<ICustomerService, CustomerService>();
            serviceColection.AddTransient<ICustomerRepository, CustomerRepository>();
            
            serviceColection.AddTransient<IInvoiceService, InvoiceService>();
            serviceColection.AddTransient<IInvoiceRepository, InvoiceRepository>();

            serviceColection.AddTransient<IAccountService, AccountService>();
            serviceColection.AddTransient<IAccountRepository, AccountRepository>();

            serviceColection.AddTransient<Login>();
            serviceColection.AddScoped<PostContext>();
            var ServiceProvider = serviceColection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<Login>().Show();
        }
    }
}
