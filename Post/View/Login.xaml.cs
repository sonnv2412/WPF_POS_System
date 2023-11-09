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
using Post.Model;

namespace Post.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        ////AccountService accountService;
        ////PostContext context;
        //AccountRepository accountRepository;
        //public Login()
        //{
        //    accountRepository= new AccountRepository();
        //    InitializeComponent();
        //}

        //private void LoginButton_Click(object sender, RoutedEventArgs e)
        //{
        //    //Account account = accountService.login(txtUsername.Text, txtPassword.Password);
        //    Account account = accountRepository.getAccount(txtUsername.Text, txtPassword.Password);
        //    if (account != null)
        //    {
        //        accountRepository.setCurrentAccount(account);
        //        this.Hide();
        //        if (account.Role == 2)
        //        {
        //            new EmployeeWindow().Show();
        //        }
        //        else
        //        {
        //            new OwnerWindow().Show();
        //        }          
        //    }
        //    else
        //    {
        //        MessageBox.Show("Login failed. Username or Password is invalid!");
        //    }
        //}

        //public void close() => this.close();
    }
}
