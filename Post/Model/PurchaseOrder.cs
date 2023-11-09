using System;
using System.Collections.Generic;

namespace Post.Model;

public partial class PurchaseOrder
{
    public int PurchaseOrderId { get; set; }

    public int? ProductId { get; set; }

    public double? Quantity { get; set; }

    public double? UnitPrice { get; set; }

    public double? SubTotal { get; set; }

    public int? SupplierId { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? AccountId { get; set; }

    public virtual Account Account { get; set; }

    public virtual Product Product { get; set; }

    public virtual Supplier Supplier { get; set; }
}
