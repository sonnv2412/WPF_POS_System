using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Post.Model;

public partial class PostContext : DbContext
{
    public PostContext()
    {
    }

    public PostContext(DbContextOptions<PostContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductUnit> ProductUnits { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<ReceiveProduct> ReceiveProducts { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server =(local); database = POST; uid=sa;pwd=123;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__46A222CDD2B66342");

            entity.ToTable("Account");

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Contact)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contact");
            entity.Property(e => e.Designation).HasColumnName("designation");
            entity.Property(e => e.Fullname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("fullname");
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.Username)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__CD65CB85BB79833E");

            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.Contact)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("contact");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("customer_code");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("customer_name");
            entity.Property(e => e.Point)
                .HasDefaultValueSql("((0))")
                .HasColumnName("point");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__Invoice__F58DFD49A4A1ACAB");

            entity.ToTable("Invoice");

            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.AmountTendered).HasColumnName("amount_tendered");
            entity.Property(e => e.BankAccountName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("bank_account_name");
            entity.Property(e => e.BankAccountNumber)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("bank_account_number");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DateRecorded)
                .HasColumnType("date")
                .HasColumnName("date_recorded");
            entity.Property(e => e.PaymentType).HasColumnName("payment_type");
            entity.Property(e => e.TotalAmount).HasColumnName("total_amount");

            entity.HasOne(d => d.Account).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Invoice_Account");

            entity.HasOne(d => d.Customer).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Invoice_Customer");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__47027DF577CFBAE0");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.DiscountPercentage).HasColumnName("discount_percentage");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("product_code");
            entity.Property(e => e.ProductName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("product_name");
            entity.Property(e => e.ReorderLevel).HasColumnName("reorder_level");
            entity.Property(e => e.UnitId).HasColumnName("unit_id");
            entity.Property(e => e.UnitInStock).HasColumnName("unit_in_stock");
            entity.Property(e => e.UnitPrice).HasColumnName("unit_price");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Product_Category");

            entity.HasOne(d => d.Unit).WithMany(p => p.Products)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("FK_Product_Unit");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__ProductC__D54EE9B46DDE0905");

            entity.ToTable("ProductCategory");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<ProductUnit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("PK__ProductU__D3AF5BD7A95B2C21");

            entity.ToTable("ProductUnit");

            entity.Property(e => e.UnitId).HasColumnName("unit_id");
            entity.Property(e => e.UnitName)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("unit_name");
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.PurchaseOrderId).HasName("PK__Purchase__AFCA88E647BBF773");

            entity.ToTable("PurchaseOrder");

            entity.Property(e => e.PurchaseOrderId).HasColumnName("purchase_order_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.OrderDate)
                .HasColumnType("date")
                .HasColumnName("order_date");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SubTotal).HasColumnName("sub_total");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.UnitPrice).HasColumnName("unit_price");

            entity.HasOne(d => d.Account).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_PurchaseOrder_Account");

            entity.HasOne(d => d.Product).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_PurchaseOrder_Product");

            entity.HasOne(d => d.Supplier).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK_PurchaseOrder_Supplier");
        });

        modelBuilder.Entity<ReceiveProduct>(entity =>
        {
            entity.HasKey(e => e.ReceiveProductId).HasName("PK__ReceiveP__91B6C32B75E20243");

            entity.ToTable("ReceiveProduct");

            entity.Property(e => e.ReceiveProductId).HasColumnName("receive_product_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.ReceivedDate)
                .HasColumnType("date")
                .HasColumnName("received_date");
            entity.Property(e => e.SubTotal).HasColumnName("sub_total");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.UnitPrice).HasColumnName("unit_price");

            entity.HasOne(d => d.Account).WithMany(p => p.ReceiveProducts)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_ReceiveProduct_Account");

            entity.HasOne(d => d.Product).WithMany(p => p.ReceiveProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ReceiveProduct_Product");

            entity.HasOne(d => d.Supplier).WithMany(p => p.ReceiveProducts)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK_ReceiveProduct_Supplier");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SalesId).HasName("PK__Sales__995B85855065AFA7");

            entity.Property(e => e.SalesId).HasColumnName("sales_id");
            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SubTotal).HasColumnName("sub_total");
            entity.Property(e => e.UnitPrice).HasColumnName("unit_price");

            entity.HasOne(d => d.Invoice).WithMany(p => p.Sales)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK_Sales_Invoice");

            entity.HasOne(d => d.Product).WithMany(p => p.Sales)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Sales_Product");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__6EE594E81196DBD0");

            entity.ToTable("Supplier");

            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.SupplierAddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("supplier_address");
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("supplier_code");
            entity.Property(e => e.SupplierContact)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("supplier_contact");
            entity.Property(e => e.SupplierEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("supplier_email");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("supplier_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
