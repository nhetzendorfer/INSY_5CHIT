using System;
using System.Collections.Generic;

namespace SimplecMethod.SimplexEntities;

public partial class ProductType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal ProfitPreHl { get; set; }

    public virtual ICollection<ProductRequirment> ProductRequirments { get; set; } = new List<ProductRequirment>();
}
