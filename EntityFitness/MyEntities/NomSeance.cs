using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntitiesFitness;
[ModelMetadataTypeAttribute(typeof(NomSeanceMetaData))]

public partial class NomSeance
{
}
public partial class NomSeanceMetaData 
{
    [Required(ErrorMessage = "Veuillez entrer un nom")]
    public String Nom { get; set; }
}