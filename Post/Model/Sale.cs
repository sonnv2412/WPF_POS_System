using System;
using System.Collections.Generic;

namespace Post.Model;

public partial class Sale
{
    public int SalesId { get; set; }

    public int? InvoiceId { get; set; }

    public int? ProductId { get; set; }

    public double? Quantity { get; set; }

    public double? UnitPrice { get; set; }

    public double? SubTotal { get; set; }

    public virtual Invoice Invoice { get; set; }

    public virtual Product Product { get; set; }
}
