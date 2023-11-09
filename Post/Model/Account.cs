using System;
using System.Collections.Generic;

namespace Post.Model;

public partial class Account
{
    public int AccountId { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string Fullname { get; set; }

    public int? Designation { get; set; }

    public string Contact { get; set; }

    public int? Role { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

    public virtual ICollection<ReceiveProduct> ReceiveProducts { get; set; } = new List<ReceiveProduct>();
}
