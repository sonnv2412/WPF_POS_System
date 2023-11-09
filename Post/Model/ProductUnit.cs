using System;
using System.Collections.Generic;

namespace Post.Model;

public partial class ProductUnit
{
    public int UnitId { get; set; }

    public string UnitName { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
