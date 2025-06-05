using System;
using System.Collections.Generic;

namespace EntitiesFitness;

public partial class Muscle
{
    public int Id { get; set; }

    public string? Nom { get; set; }

    public virtual ICollection<NomExercice> NomExercices { get; set; } = new List<NomExercice>();
}
