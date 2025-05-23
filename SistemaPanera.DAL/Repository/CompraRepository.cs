using Microsoft.EntityFrameworkCore;
using SistemaPanera.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using SistemaPanera.DAL.DataContext;

namespace SistemaPanera.DAL.Repository
{
    public class CompraRepository : ICompraRepository<Compra>
    {

        private readonly SistemaPaneraContext _dbcontext;

        public CompraRepository(SistemaPaneraContext context)
        {
            _dbcontext = context;
        }
        public async Task<bool> Actualizar(Models.Compra model)
        {
            _dbcontext.Compras.Update(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            Models.Compra model = _dbcontext.Compras.First(c => c.Id == id);
            _dbcontext.Compras.Remove(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Insertar(Models.Compra model)
        {
            _dbcontext.Compras.Add(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<Models.Compra> Obtener(int id)
        {
            //Models.Compra model = await _dbcontext.Compras
            //    .Include(p => p.ComprasDetalle)
            //    .FirstOrDefaultAsync(p => p.Id == id);
            return null;
        }

        public async Task<IQueryable<Models.Compra>> ObtenerTodos()
        {
            IQueryable<Models.Compra> query = _dbcontext.Compras
            .Include(c => c.IdLocalNavigation)
            .Include(c => c.IdUnidadNegocioNavigation);

            return await Task.FromResult(query);
        }

        public async Task<bool> InsertarInsumos(List<ComprasDetalle> insumos)
        {
            foreach (ComprasDetalle p in insumos)
            {
                // Verificar si el insumo ya existe, por ejemplo, por Idinsumo y IdPedido
                var insumoExistente = await _dbcontext.ComprasDetalles
                                                         .FirstOrDefaultAsync(x => x.IdCompra == p.IdCompra && x.IdInsumo == p.IdInsumo);

                if (insumoExistente != null)
                {
                    // Si el insumo existe, actualizamos sus propiedades
                    insumoExistente.CostoUnitario = p.CostoUnitario;
                    insumoExistente.SubTotal = p.SubTotal;
                    insumoExistente.Cantidad = p.Cantidad;
                }
                else
                {
                    // Si el insumo no existe, lo agregamos a la base de datos
                    _dbcontext.ComprasDetalles.Add(p);
                }
            }


            var insumosIdsModelo = insumos.Select(p => p.IdCompra).Distinct().ToList();
            var insumosAEliminar = await _dbcontext.ComprasDetalles
                                                      .Where(x => insumosIdsModelo.Contains(x.IdCompra)
                                                              && !insumos.Select(p => p.IdInsumo).Contains(x.IdInsumo)
                                                              && x.Id != 0)
                                                      .ToListAsync();


            foreach (var insumo in insumosAEliminar)
            {
                _dbcontext.ComprasDetalles.Remove(insumo);
            }

            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarInsumos(List<ComprasDetalle> insumos)
        {
            foreach (ComprasDetalle p in insumos)
            {
                _dbcontext.ComprasDetalles.Update(p);
            }

            await _dbcontext.SaveChangesAsync();
            return true;

        }


        public async Task<List<ComprasDetalle>> ObtenerInsumos(int idCompra)
        {
            try
            {

                List<ComprasDetalle> productos = _dbcontext.ComprasDetalles
                    //.Include(c => c.IdCompraNavigation)
                    .Include(c => c.IdInsumoNavigation)
                    .Where(c => c.IdCompra == idCompra).ToList();
                return productos;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }



    }
}
