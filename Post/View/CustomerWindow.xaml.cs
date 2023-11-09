using Post.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace Post.View
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        //CustomerRepository customerRepository;
        Customer cus;
        public CustomerWindow()
        {
            //customerRepository= new CustomerRepository();
            InitializeComponent();
            load();
        }

        public void load()
        {
            //lvCustomer.ItemsSource = customerRepository.GetCustomers();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //lvCustomer.ItemsSource = customerRepository.GetCustomers().Where(c => c.CustomerName.ToLower().Contains(txtSearch.Text.ToLower()));
        }

        private void lvCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvCustomer.SelectedItem != null)
            {
                // Ép kiểu mục đã chọn thành đối tượng Product
                cus = lvCustomer.SelectedItem as Customer;
                NewBtn.IsEnabled = false;
               

                // Bây giờ bạn có thể làm gì đó với đối tượng sản phẩm đã chọn
                // Ví dụ: hiển thị thông tin sản phẩm trong một cửa sổ hoặc thực hiện một tác vụ khác
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }

        private void reset()
        {
            NewBtn.IsEnabled = true;
            CancelBtn.IsEnabled = true;
            ApplyBtn.IsEnabled = true;
            txtCustomerCode.Text = string.Empty;
            txtCustomerName.Text = string.Empty;
            txtAddress.Text= string.Empty;
            txtContact.Text= string.Empty;
            txtPoint.Text= string.Empty;
        }

        private void NewBtn_Click(object sender, RoutedEventArgs e)
        {
            reset();
            txtCustomerCode.IsEnabled= false;
            txtPoint.IsEnabled= false;
            txtCustomerName.IsReadOnly = false;
            txtAddress.IsReadOnly = false;
            txtContact.IsReadOnly = false;
        }

        private void ApplyBtn_Click(object sender, RoutedEventArgs e)
        {
            if (txtCustomerCode.Text != "")
            {
                cus = new Customer();
                cus.CustomerCode = GenerateRandomString(txtCustomerName.Text, txtContact.Text);
                cus.CustomerName= txtCustomerName.Text;
                cus.Address= txtAddress.Text;
                cus.Contact= txtContact.Text;
                cus.Point = 0;

            }
            //customerRepository.setSelectedCustomer(cus);
            this.Close();
        }

        public void WaitForCloseAsync()
        {
            while (this.IsVisible)
            {
            }

        }

        public static string GenerateRandomString(string name, string contact)
        {
            // Kết hợp tên và thông tin liên hệ thành một chuỗi duy nhất
            string inputString = name + contact;

            using (MD5 md5 = MD5.Create())
            {
                // Chuyển đổi chuỗi thành mảng byte và tính giá trị băm MD5
                byte[] inputBytes = Encoding.UTF8.GetBytes(inputString);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Chuyển giá trị băm MD5 thành chuỗi hexa
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
