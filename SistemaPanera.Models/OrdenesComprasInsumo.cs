using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaPanera.Models;

public partial class OrdenesComprasInsumo
{
    public int Id { get; set; }

    public int IdOrdenCompra { get; set; }

    public int IdInsumo { get; set; }

    public int? IdProveedorLista { get; set; }

    public decimal CantidadPedida { get; set; }

    public decimal CantidadEntregada { get; set; }

    public decimal CantidadRestante { get; set; }

    public decimal PrecioLista { get; set; }

    public decimal Subtotal { get; set; }

    public int IdEstado { get; set; }

    public string? NotaInterna { get; set; }

    public virtual OrdenesComprasInsumosEstado IdEstadoNavigation { get; set; } = null!;

    public virtual Insumo IdInsumoNavigation { get; set; } = null!;

    public virtual OrdenesCompra IdOrdenCompraNavigation { get; set; } = null!;

    [ForeignKey(nameof(IdProveedorLista))]
    public virtual ProveedoresInsumos IdProveedorListaNavigation { get; set; } = null!;
}
