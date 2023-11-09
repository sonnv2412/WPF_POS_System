using System;
using System.Collections.Generic;

namespace Post.Model;

public partial class ProductCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
