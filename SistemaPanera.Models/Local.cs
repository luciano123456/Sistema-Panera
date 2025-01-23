using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;

public partial class Local
{
    public int Id { get; set; }

    public int IdUnidadNegocio { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual UnidadesNegocio IdUnidadNegocioNavigation { get; set; } = null!;
}
