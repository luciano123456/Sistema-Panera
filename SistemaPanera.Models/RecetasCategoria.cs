using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;
public partial class RecetasCategoria
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Receta> Receta { get; set; } = new List<Receta>();
}
