using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntitiesFitness;
[ModelMetadataTypeAttribute(typeof(SeanceMetaData))]

public partial class Seance
{
}
public partial class SeanceMetaData 
{
    [Required(ErrorMessage = "Veullez choisir un exercice !")]
    public int ExerciceId { get; set; }
}
