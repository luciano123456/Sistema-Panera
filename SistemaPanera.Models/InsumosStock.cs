using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;

public partial class InsumosStock
{
    public int Id { get; set; }

    public int IdUnidadNegocio { get; set; }

    public int IdLocal { get; set; }

    public int? IdMovimiento { get; set; }

    public string TipoMovimiento { get; set; } = null!;

    public int IdInsumo { get; set; }

    public DateTime Fecha { get; set; }

    public string Concepto { get; set; } = null!;

    public decimal Entrada { get; set; }

    public decimal Salida { get; set; }

    public virtual ICollection<FacturasProveedoresInsumo> FacturasProveedoresInsumos { get; set; } = new List<FacturasProveedoresInsumo>();

    public virtual Insumo IdInsumoNavigation { get; set; } = null!;

    public virtual Local IdLocalNavigation { get; set; } = null!;

    public virtual UnidadesNegocio IdUnidadNegocioNavigation { get; set; } = null!;
}
