using System;
using System.Collections.Generic;

namespace EntitiesFitness;

public partial class Serie
{
    public int Id { get; set; }

    public double? Poids { get; set; }

    public int? Rep { get; set; }

    public virtual ICollection<Exercice> Exercices { get; set; } = new List<Exercice>();
}
