﻿using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;

public partial class UnidadesNegocio
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Insumo> Insumos { get; set; } = new List<Insumo>();

    public virtual ICollection<Local> Locales { get; set; } = new List<Local>();

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
