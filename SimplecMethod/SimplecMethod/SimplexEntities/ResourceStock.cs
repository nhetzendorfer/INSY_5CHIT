using System;
using System.Collections.Generic;

namespace SimplecMethod.SimplexEntities;

public partial class ResourceStock
{
    public int ResourceId { get; set; }

    public decimal AvailibleUnits { get; set; }

    public virtual Resource Resource { get; set; } = null!;
}
