using System;
using System.Collections.Generic;

namespace EntitiesFitness;

public partial class NomSeance
{
    public int Id { get; set; }

    public string Nom { get; set; } = null!;

    public virtual ICollection<Seance> Seances { get; set; } = new List<Seance>();
}
