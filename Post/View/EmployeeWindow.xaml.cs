using Post.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using System.Windows.Threading;

namespace Post.View
{
    /// <summary>
    /// Interaction logic for EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        Dictionary<Product, int> OrderedProducts = new Dictionary<Product, int>();
        String productSearch = "";
        private DispatcherTimer timer;
        double totalPayable = 0;
        public EmployeeWindow()
        {
            InitializeComponent();
            load();

            // Tạo một DispatcherTimer với khoảng thời gian cập nhật là 1 giây
            timer = new DispatcherTimer();
            TimeUpdate();
        }

        private void TimeUpdate()
        {
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void load()
        {
            //txtEmployeeName.Content = AccountRepository.CurrentAccount.Fullname;
            //lvProduct.ItemsSource = productRepository.getProducts(productSearch);
        }

        private void reset()
        {
            ResetOrderedProduct();
            ResetButton();
        }

        private void loadOrderedProduct()
        {
            CaculateTotalPayable();
            lvSelectedProduct.ItemsSource = OrderedProducts.ToList();
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            new Login().Show();
            this.Close();
        }

        private void lvProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvProduct.SelectedItem != null)
            {
                // Ép kiểu mục đã chọn thành đối tượng Product
                Product selectedProduct = lvProduct.SelectedItem as Product;
                Order(selectedProduct);

                // Bây giờ bạn có thể làm gì đó với đối tượng sản phẩm đã chọn
                // Ví dụ: hiển thị thông tin sản phẩm trong một cửa sổ hoặc thực hiện một tác vụ khác
            }
        }

        private void Order(Product selectedProduct)
        {
            if (!OrderedProducts.ContainsKey(selectedProduct))
            {
                OrderedProducts.Add(selectedProduct, 1);
            }
            else
            {
                OrderedProducts[selectedProduct] += 1;
            }
            loadOrderedProduct();
        }

        private void txtBarcode_TextChanged(object sender, TextChangedEventArgs e)
        {
            productSearch = txtBarcode.Text;
            load();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            productSearch = txtSearch.Text;
            load();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Cập nhật TextBlock với thời gian thực
            txtTime.Content = DateTime.Now.ToString("HH:mm:ss tt, MMMM dd, yyyy");
        }

        private void SubtractButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            KeyValuePair<Product, int> item = (KeyValuePair<Product, int>)button.DataContext;
            OrderedProducts[item.Key]--;
            if (OrderedProducts[item.Key] == 0)
            {
                OrderedProducts.Remove(item.Key);

            }
            loadOrderedProduct();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            KeyValuePair<Product, int> item = (KeyValuePair<Product, int>)button.DataContext;
            OrderedProducts[item.Key]++;
            loadOrderedProduct();
        }

        private double CaculateTotalPayable()
        {
            double totalDiscount = 0;
            double total = 0;
            foreach (KeyValuePair<Product, int> item in OrderedProducts)
            {
                total += (double)(item.Key.UnitPrice * item.Value);
                totalDiscount += (double)(total * (item.Key.DiscountPercentage / 100));
            }
            txtTotal.Text = total.ToString();
            txtDiscount.Text = totalDiscount.ToString();
            totalPayable = total - totalDiscount;
            txtTotalPayable.Content = totalPayable.ToString();
            return totalPayable;
        }

        private void TransferBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OrderedProducts.Count > 0)
            {
                if (TransferBtn.Content.Equals("Transfer"))
                {
                    CustomerWindow customerWindow = new CustomerWindow();
                    customerWindow.Show();
                    customerWindow.Closed += async (s, args) =>
                    {
                        // Chờ cho đến khi cửa sổ mới được đóng (không làm đơ ứng dụng)
                        while (customerWindow.IsVisible)
                        {
                            await Task.Delay(100); // Chờ 100ms trước khi kiểm tra lại
                        }
                        // Sau khi cửa sổ mới đã được đóng, tiếp tục thực hiện các tác vụ khác
                        HandleQRCode();
                       
                    };
                   
                }
                else
                {
                    TransferInforImage.Visibility = Visibility.Collapsed;
                    TransferInforImage.Width = 0;
                    TransferInforImage.Height = 0;
                    reset();
                }


            }
            else
            {
                MessageBox.Show("Order at least 1 product, please!");
            }


        }

        private void HandleQRCode()
        {
                TransferInforImage.Source = new BitmapImage(new Uri("https://img.vietqr.io/image/TPB-04074651601-print.png?amount=" + totalPayable + "&accountName=Nguyen%20Van%20Son", UriKind.RelativeOrAbsolute));
                TransferInforImage.Visibility = Visibility.Visible;
                TransferInforImage.Width = 400;
                TransferInforImage.Height = 700;

                CashSaleBtn.IsEnabled = false;
                TransferBtn.Content = "Confirm";
        }

        private void ResetButton()
        {
            CashSaleBtn.IsEnabled = true;
            CashSaleBtn.Content = "Cash Sale";
            TransferBtn.IsEnabled = true;
            TransferBtn.Content = "Transfer";
            CancelBtn.IsEnabled = true;
            //CustomerRepository.selectedCustomer = null;
        }

        private void ResetOrderedProduct()
        {
            txtTotal.Text = "0";
            txtDiscount.Text = "0";
            txtTotalPayable.Content = "0";
            OrderedProducts.Clear();
            lvSelectedProduct.ItemsSource = OrderedProducts;
        }
    }
}
