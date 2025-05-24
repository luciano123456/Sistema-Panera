using System;
using System.Collections.Generic;
namespace SistemaPanera.Models;
public partial class InsumosStock
{
    public int Id { get; set; }

    public int IdLocal { get; set; }

    public int IdInsumo { get; set; }

    public string? TipoMovimiento { get; set; }

    public decimal? Ingreso { get; set; }

    public decimal? Egreso { get; set; }
}
