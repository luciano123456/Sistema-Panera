﻿using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;

public partial class InsumosProveedor
{
    public int Id { get; set; }

    public int IdInsumo { get; set; }

    public int IdProveedor { get; set; }

    public int IdListaProveedor { get; set; }

    public virtual Insumo IdInsumoNavigation { get; set; } = null!;

    public virtual ProveedoresInsumos IdListaProveedorNavigation { get; set; } = null!;
}
