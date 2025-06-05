using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntitiesFitness;
[ModelMetadataTypeAttribute(typeof(NomExerciceMetaData))]

public partial class NomExercice { }

public partial class NomExerciceMetaData
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Le nom est obligatoire")]
    public string Nom { get; set; }

    [Required(ErrorMessage = "Veuillez choisir un muscle")]
    public int MuscleId { get; set; }

}
