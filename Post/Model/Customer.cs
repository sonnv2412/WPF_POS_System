using System;
using System.Collections.Generic;

namespace Post.Model;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string CustomerCode { get; set; }

    public string CustomerName { get; set; }

    public string Contact { get; set; }

    public string Address { get; set; }

    public int? Point { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
