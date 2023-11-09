using System;
using System.Collections.Generic;

namespace Post.Model;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public int? CustomerId { get; set; }

    public int? PaymentType { get; set; }

    public double? TotalAmount { get; set; }

    public double? AmountTendered { get; set; }

    public string? BankAccountName { get; set; }

    public string? BankAccountNumber { get; set; }

    public DateTime? DateRecorded { get; set; }

    public int? AccountId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<Sale>? Sales { get; set; } = new List<Sale>();
}
