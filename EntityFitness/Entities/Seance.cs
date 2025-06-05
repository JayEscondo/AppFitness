using System;
using System.Collections.Generic;

namespace EntitiesFitness;

public partial class Seance
{
    public int Id { get; set; }

    public int NomSeanceId { get; set; }

    public int ExerciceId { get; set; }

    public virtual Exercice Exercice { get; set; } = null!;

    public virtual NomSeance NomSeance { get; set; } = null!;

    public virtual ICollection<Routine> Routines { get; set; } = new List<Routine>();
}
