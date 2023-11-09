using HandyControl.Tools.Extension;
using Microsoft.EntityFrameworkCore;
using Post.Domain.Service.IService;
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
    /// Interaction logic for DemoHandyControl.xaml
    /// </summary>
    public partial class DemoHandyControl : Window
    {
        IProductService productService;
        ICustomerService customerService;
        IInvoiceService invoiceService;
        Dictionary<Product, int> OrderedProducts = new Dictionary<Product, int>();
        private int NumberOfAppliedVoucher = 0;
        private List<Product> products;
        private double totalPayable = 0;
        private Customer selectedCustomer;
        public DemoHandyControl(IProductService productService, ICustomerService customerService, IInvoiceService invoiceService)
        {
            this.customerService = customerService;
            this.productService = productService;
            this.invoiceService = invoiceService;
            InitializeComponent();
            load();
            loadCustomer();
            loadInvoice();
        }

        private void saleBtn_Click(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 0;
        }

        private void customerBtn_Click(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 1;
            loadCustomer();
        }

        private void orderHistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 2;
            loadInvoice();
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
                txtCustomerSearch1.Text = "";
                loadOrderedProduct();
            }
        }

        private void loadOrderedProduct()
        {     
            CaculateTotalPayable();
            if (selectedCustomer != null)
            {
                step.StepIndex = 2;
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
            if (PaymentTabControl.SelectedIndex == 0)
            {
                searchText = txtCustomerSearch.Text;
            }
            else
            {
                searchText = txtCustomerSearch1.Text;
                //MessageBox.Show(searchText);
            }


            // Thực hiện tìm kiếm dựa trên searchText và lấy danh sách các kết quả.
            List<Customer> customerSearchResultsList = PerformCustomerSearch(searchText);

            // Cập nhật danh sách kết quả trong ListBox.
            customerSearchResults.ItemsSource = null;
            customerSearchResults.ItemsSource = customerSearchResultsList;

            // Hiển thị hoặc ẩn ListView tùy thuộc vào có kết quả tìm kiếm hay không.
            if (!searchText.Equals(""))
            {
                step.StepIndex = 1;
                customerSearchResults.Visibility = Visibility.Visible;
            }
            else
            {
                step.StepIndex = 0;
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
                    int? voucher = selectedCustomer.Point / 20;
                    txtNumberOfVoucherCash.Maximum = Convert.ToInt32(voucher);
                    txtNumberOfVoucherTransfer.Maximum = Convert.ToInt32(voucher);
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
            TransferInforImage.Visibility = Visibility.Collapsed;
            PaymentTabControl.SelectedIndex = 0;
        }

        private void HandleQRCode()
        {
            totalPayable -= (txtNumberOfVoucherTransfer.Value * 30000);
            BitmapImage bitmapImage = new BitmapImage(new Uri("https://img.vietqr.io/image/TPB-04074651601-print.png?amount=" + totalPayable + "&accountName=Nguyen%20Van%20Son"));
            TransferInforImage.Source = bitmapImage;
            TransferInforImage.Visibility = Visibility.Visible;
        }

        private void exportQRBtn_Click(object sender, RoutedEventArgs e)
        {
            HandleQRCode();
        }

        private void confirmOrder(int PaymentType)
        {
            if (OrderedProducts.Count <= 0)
            {
                MessageBox.Show("The order is empty");
            }
            else
            {
                if (selectedCustomer != null)
                {
                    selectedCustomer.Point -= 20 * NumberOfAppliedVoucher;
                }
                Invoice invoice = new Invoice();
                invoice.PaymentType = PaymentType;
                invoice.TotalAmount = totalPayable - NumberOfAppliedVoucher * 30000;
                invoice.DateRecorded = DateTime.Now;
                invoice.AccountId = 1;

                invoiceService.AddInvoice(invoice, selectedCustomer, OrderedProducts);
                step.StepIndex = 3;
                MessageBox.Show("Invoice is successfully created!");
                refreshOrder();
            }
        }

        private void cancelOrder()
        {
            var confirmCancel = MessageBox.Show("Do you really want to cancel this order", "Confirm Cancel", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirmCancel == MessageBoxResult.Yes)
            {
                refreshOrder();
            }

        }

        private void confirmCashBtn_Click(object sender, RoutedEventArgs e)
        {
            confirmOrder(1);
        }

        private void cancelCashBtn_Click(object sender, RoutedEventArgs e)
        {

            cancelOrder();
        }

        private void confirmTransferBtn_Click(object sender, RoutedEventArgs e)
        {
            confirmOrder(2);
        }

        private void cancelTransferBtn_Click(object sender, RoutedEventArgs e)
        {
            cancelOrder();
        }

        private void txtNumberOfVoucherCash_ValueChanged(object sender, HandyControl.Data.FunctionEventArgs<double> e)
        {
            NumberOfAppliedVoucher = GetNumberOfAppliedVoucher();
        }

        private int GetNumberOfAppliedVoucher()
        {
            int point;
            double totalPayableTmp = totalPayable;
            if (PaymentTabControl.SelectedIndex == 0)
            {
                point = Convert.ToInt32(txtNumberOfVoucherCash.Value);
                totalPayableTmp -= point * 30000;
                txtTotalPayableCash.Text = totalPayableTmp.ToString();
            }
            else
            {
                point = Convert.ToInt32(txtNumberOfVoucherTransfer.Value);
                totalPayableTmp -= point * 30000;
                txtTotalPayableTransfer.Text = totalPayableTmp.ToString();
            }
            return point;
        }

        private void refreshOrder()
        {
            step.StepIndex = 0;
            txtCustomerNameTransfer.Text = "Customer";
            txtCustomerNameCash.Text = "Customer";
            TransferInforImage.Visibility = Visibility.Collapsed;
            selectedCustomer = null;
            OrderedProducts.Clear();
            foreach (var control in CashGrid.Children)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).Text = string.Empty;
                }
            }
            foreach (var control in TransferGrid.Children)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).Text = string.Empty;
                }
            }
            lvSelectedProducts.ItemsSource = null;
        }

        //Code xu ly Customer tab item///////////////////////////////////////////////////////////////////
        private void newCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            step.StepIndex = 2;
            TabControl.SelectedIndex = 1;
        }

        private void loadCustomer()
        {
            lvCustomer.ItemsSource = customerService.GetAllCustomer();
        }

        private void CustomerSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            String search = CustomerSearch.Text;

            List<Customer> customers = customerService.GetAllCustomer().Where(c => c.CustomerCode.Contains(search) || c.CustomerName.Contains(search)).ToList();
            lvCustomer.ItemsSource = customers;
        }

        private void RefreshCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var control in CustomerGrid.Children)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).Text = string.Empty;
                }
            }
            lvCustomer.SelectedIndex = -1;
        }

        private void ApplyCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!txtCustomerId.Text.Equals(""))
            {
                Customer c = GetCustomer();
                customerService.UpdateCustomer(c);
                var confirmRedirect = MessageBox.Show("Apply customer successfully, redirect to Orders screen ?", "Redirect to Order screen", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirmRedirect == MessageBoxResult.Yes)
                {
                    selectedCustomer = c;
                    loadOrderedProduct();
                    TabControl.SelectedIndex = 0;
                }

            }
        }

        private void NewCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            Customer c = GetCustomer();
            customerService.AddCustomer(c);
            var confirmRedirect = MessageBox.Show("Create new customer successfully, apply and redirect to Orders screen ?", "Redirect to Order screen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirmRedirect == MessageBoxResult.Yes)
            {
                selectedCustomer = c;
                loadOrderedProduct();
                TabControl.SelectedIndex = 0;
            }
        }

        private Customer GetCustomer()
        {
            Customer c;
            if (txtCustomerId.Text != "")
            {
                c = customerService.GetCustomerById(Convert.ToInt32(txtCustomerId.Text));
                c.CustomerName = txtCustomerName.Text;
                c.Contact = txtContact.Text;
                c.Address = txtAddress.Text;
            }
            else
            {
                c = new Customer();
                c.CustomerName = txtCustomerName.Text;
                c.Contact = txtContact.Text;
                c.Address = txtAddress.Text;
                c.Point = 0;
                c.CustomerCode = GenerateRandomString(c.CustomerName, c.Contact);
                MessageBox.Show(c.CustomerCode);
            }
            return c;
        }

        public static string GenerateRandomString(string name, string contact)
        {
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

                // Giới hạn độ dài của chuỗi hash MD5 thành 25 ký tự
                return builder.ToString().Substring(0, 20);
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            loadOrderedProduct();
            TabControl.SelectedIndex = 0;
        }


        // Code xu ly man hinh Invoice detail ///////////////////////////////////////////////////     
        private void loadInvoice()
        {
            cboPaymentTypeOrder.ItemsSource = new String[] {"All", "Cash", "Transfer" };          
            lvInvoices.ItemsSource = invoiceService.GetAllInvoices();
        }

        private void lvInvoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvInvoices.SelectedItem != null)
            {
                Invoice invoice = lvInvoices.SelectedItem as Invoice;
                lvProductsInInvoice.ItemsSource = invoice.Sales;
            }
        }

        private void filterInvoices()
        {
            lvInvoices.ItemsSource = invoiceService.filterInvoice(txtSearchCustomerOrderSearch.Text, txtRecordDateOrderSearch.Text);
        }

        private void txtSearchCustomerOrderSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
               filterInvoices();
        }
     
        private void txtRecordDateOrderSearch_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            filterInvoices();
        }

    }

}
