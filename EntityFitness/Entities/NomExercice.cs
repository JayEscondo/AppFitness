using System;
using System.Collections.Generic;

namespace EntitiesFitness;

public partial class NomExercice
{
    public int Id { get; set; }

    public string Nom { get; set; } = null!;

    public int MuscleId { get; set; }

    public virtual ICollection<Exercice> Exercices { get; set; } = new List<Exercice>();

    public virtual Muscle Muscle { get; set; } = null!;
}
