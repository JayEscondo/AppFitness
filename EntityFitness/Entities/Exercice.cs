using System;
using System.Collections.Generic;

namespace EntitiesFitness;

public partial class Exercice
{
    public int Id { get; set; }

    public string? Date { get; set; }

    public int NomExerciceId { get; set; }

    public int NumSerie { get; set; }

    public int SerieId { get; set; }

    public virtual NomExercice NomExercice { get; set; } = null!;

    public virtual ICollection<Seance> Seances { get; set; } = new List<Seance>();

    public virtual Serie Serie { get; set; } = null!;
}
