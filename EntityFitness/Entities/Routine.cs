using System;
using System.Collections.Generic;

namespace EntitiesFitness;

public partial class Routine
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int? SeanceId { get; set; }

    public virtual Seance? Seance { get; set; }

    public virtual User User { get; set; } = null!;
}
