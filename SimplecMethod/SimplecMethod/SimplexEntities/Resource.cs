using System;
using System.Collections.Generic;

namespace SimplecMethod.SimplexEntities;

public partial class Resource
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ProductRequirment> ProductRequirments { get; set; } = new List<ProductRequirment>();

    public virtual ResourceStock? ResourceStock { get; set; }
}
