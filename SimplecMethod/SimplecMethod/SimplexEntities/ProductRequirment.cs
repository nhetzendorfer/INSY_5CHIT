using System;
using System.Collections.Generic;

namespace SimplecMethod.SimplexEntities;

public partial class ProductRequirment
{
    public int ProductId { get; set; }

    public int ResourceId { get; set; }

    public decimal UnitsRequired { get; set; }

    public virtual ProductType Product { get; set; } = null!;

    public virtual Resource Resource { get; set; } = null!;
}
