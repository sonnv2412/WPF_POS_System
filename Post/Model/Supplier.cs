using System;
using System.Collections.Generic;

namespace Post.Model;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string SupplierCode { get; set; }

    public string SupplierName { get; set; }

    public string SupplierContact { get; set; }

    public string SupplierAddress { get; set; }

    public string SupplierEmail { get; set; }

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

    public virtual ICollection<ReceiveProduct> ReceiveProducts { get; set; } = new List<ReceiveProduct>();
}
