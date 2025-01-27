using System;
using System.Collections.Generic;


namespace SistemaPanera.Models;

public partial class PrefabricadosCategoria
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Prefabricado> Prefabricados { get; set; } = new List<Prefabricado>();
}
