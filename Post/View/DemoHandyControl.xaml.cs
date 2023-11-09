using HandyControl.Tools.Extension;
using Microsoft.EntityFrameworkCore;
using Post.Domain.Service.IService;
using Post.Model;
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

namespace Post.View
{
    /// <summary>
    /// Interaction logic for DemoHandyControl.xaml
    /// </summary>
    public partial class DemoHandyControl : Window
    {
        IProductService productService;
        ICustomerService customerService;
        Dictionary<Product, int> OrderedProducts = new Dictionary<Product, int>();
        private List<Product> products;
        private double totalPayable = 0;
        private Customer selectedCustomer;
        public DemoHandyControl(IProductService productService, ICustomerService customerService)
        {
            this.customerService = customerService;
            this.productService = productService;
            InitializeComponent();
            load();       
        }

        private void saleBtn_Click(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 0;
        }

        private void customerBtn_Click(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 1;
        }

        private void orderHistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 2;
        }

        private void profileBtn_Click(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 3;
        }

        private void load()
        {
            //products = new List<Product>();
            //products = _context.Products.Include(p => p.Category).Include(p => p.Unit).ToList();
            products = productService.getAllProducts();
        }

        private void searchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (searchResults.SelectedItem != null)
            {
                Product selectedProduct = searchResults.SelectedItem as Product;
                txtSearchProduct.Text = "";
                Order(selectedProduct);
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

        private void customerSearchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (customerSearchResults.SelectedItem != null)
            {
                selectedCustomer = customerSearchResults.SelectedItem as Customer;             
                txtCustomerSearch.Text = "";
                loadOrderedProduct();
            }
        }

        private void loadOrderedProduct()
        {
            CaculateTotalPayable();
            if (selectedCustomer != null)
            {
                txtCustomerNameCash.Text = $"{selectedCustomer.CustomerName} ({txtNumberOfVoucherCash.Maximum} vouchers)";
                txtCustomerNameTransfer.Text = $"{selectedCustomer.CustomerName} ({txtNumberOfVoucherTransfer.Maximum} vouchers)";
            }          
            lvSelectedProducts.ItemsSource = OrderedProducts.ToList();
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

        private void txtSearchProduct_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearchProduct.Text;

            // Thực hiện tìm kiếm dựa trên searchText và lấy danh sách các kết quả.
            List<Product> searchResultsList = PerformSearch(searchText);

            // Cập nhật danh sách kết quả trong ListBox.
            searchResults.ItemsSource = null;
            searchResults.ItemsSource = searchResultsList;

            // Hiển thị hoặc ẩn ListView tùy thuộc vào có kết quả tìm kiếm hay không.
            if (!txtSearchProduct.Text.Equals(""))
            {
                searchResults.Visibility = Visibility.Visible;
            }
            else
            {
                searchResults.Visibility = Visibility.Collapsed;
            }
        }

        private List<Product> PerformSearch(string searchText)
        {
            //List<Product> results = new List<Product>();
            return products.Where(p => p.ProductCode.Contains(searchText) || p.ProductName.Contains(searchText)).ToList();
        }

        private void txtCustomerSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = "";
            if (sender is TextBox textBox)
            {
                // Lấy giá trị văn bản từ TextBox
                searchText = textBox.Text;

                // Bây giờ bạn có thể sử dụng biến searchText như bạn muốn
                Console.WriteLine("Text changed: " + searchText);
            }


            // Thực hiện tìm kiếm dựa trên searchText và lấy danh sách các kết quả.
            List<Customer> customerSearchResultsList = PerformCustomerSearch(searchText);

            // Cập nhật danh sách kết quả trong ListBox.
            customerSearchResults.ItemsSource = null;
            customerSearchResults.ItemsSource = customerSearchResultsList;

            // Hiển thị hoặc ẩn ListView tùy thuộc vào có kết quả tìm kiếm hay không.
            if (!txtCustomerSearch.Text.Equals(""))
            {
                customerSearchResults.Visibility = Visibility.Visible;
            }
            else
            {
                customerSearchResults.Visibility = Visibility.Collapsed;
            }
        }

        private List<Customer> PerformCustomerSearch(string searchText)
        {
            return customerService.GetAllCustomer().Where(c => c.CustomerCode.Contains(searchText) || c.CustomerName.Contains(searchText)).ToList();
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
            txtTotalCash.Text = total.ToString();
            txtDiscountCash.Text = totalDiscount.ToString();
            totalPayable = total - totalDiscount;
            txtTotalPayableCash.Text = totalPayable.ToString();
            txtTotalTransfer.Text = total.ToString();
            txtDiscountTransfer.Text = totalDiscount.ToString();
            totalPayable = total - totalDiscount;

            txtNumberOfVoucherCash.Minimum = 0;
            txtNumberOfVoucherTransfer.Minimum = 0;
            if (selectedCustomer != null)
            {
                if (selectedCustomer.Point > 0)
                {
                    int? voucher = selectedCustomer.Point % 20;
                    txtNumberOfVoucherCash.Maximum = double.Parse(voucher.ToString());
                    txtNumberOfVoucherTransfer.Maximum = double.Parse(voucher.ToString());
                }
                else
                {
                    txtNumberOfVoucherCash.Maximum = 0;
                    txtNumberOfVoucherTransfer.Maximum = 0;
                }
               
            }

            txtTotalPayableTransfer.Text = totalPayable.ToString();
            return totalPayable;
        }

        private void transferMethodBtn_Click(object sender, RoutedEventArgs e)
        {
            PaymentTabControl.SelectedIndex = 1;
        }

        private void cashMethodBtn_Click(object sender, RoutedEventArgs e)
        {
            PaymentTabControl.SelectedIndex = 0;
        }

        private void HandleQRCode()
        {
            totalPayable -= (txtNumberOfVoucherTransfer.Value * 3);
            BitmapImage bitmapImage = new BitmapImage(new Uri("https://img.vietqr.io/image/TPB-04074651601-print.png?amount=" + totalPayable + "&accountName=Nguyen%20Van%20Son"));
            TransferInforImage.Source = bitmapImage;
            TransferInforImage.Visibility = Visibility.Visible;
        }

        private void exportQRBtn_Click(object sender, RoutedEventArgs e)
        {
            HandleQRCode();
        }

        private void confirmOrder()
        {

        }

        private void cancelOrder()
        {

        }

        private void confirmCashBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cancelCashBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void confirmTransferBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cancelTransferBtn_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
