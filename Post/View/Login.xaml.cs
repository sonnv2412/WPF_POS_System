using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Post.Domain.Service;
using Post.Domain.Service.IService;
using Post.Model;

namespace Post.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        IProductService productService;
        ICustomerService customerService;
        IInvoiceService invoiceService;
        IAccountService accountService;
        public Login(IAccountService accountService, IProductService productService, ICustomerService customerService, IInvoiceService invoiceService)
        {
            this.accountService = accountService;
            this.customerService = customerService;
            this.productService = productService;
            this.invoiceService = invoiceService;
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            accountService.Login(txtUsername.Text, txtPassword.Password);
            Account account = AccountService.account;
            if (account != null && account.Role == 2)
            {
                this.Hide();
                new DemoHandyControl(accountService ,productService, customerService, invoiceService).Show();
            }
            else
            {
                MessageBox.Show("Login failed. Username or Password is invalid!");
            }
        }

        public void close() => this.close();
    }
}
