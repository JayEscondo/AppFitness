using System;
using System.Collections.Generic;

namespace EntitiesFitness;

public partial class User
{
    public int Id { get; set; }

    public string Nom { get; set; } = null!;

    public int LoginId { get; set; }

    public double? Poids { get; set; }

    public int? Tailles { get; set; }

    public virtual Login Login { get; set; } = null!;

    public virtual ICollection<Routine> Routines { get; set; } = new List<Routine>();
}
