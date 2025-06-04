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
    public class ProveedoresInsumosRepository : IProveedoresInsumosRepository<ProveedoresInsumos>
    {

        private readonly SistemaPaneraContext _dbcontext;

        public ProveedoresInsumosRepository(SistemaPaneraContext context)
        {
            _dbcontext = context;
        }
       

        public async Task<bool> Insertar(Models.ProveedoresInsumos model)
        {
            using var transaction = await _dbcontext.Database.BeginTransactionAsync();
            try
            {
                _dbcontext.ProveedoresInsumos.Add(model);
                await _dbcontext.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> Actualizar(ProveedoresInsumos model)
        {
            using var transaction = await _dbcontext.Database.BeginTransactionAsync();
            try
            {
                _dbcontext.ProveedoresInsumos.Update(model);

                await _dbcontext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }


        public async Task<bool> Eliminar(int id)
        {
            Models.ProveedoresInsumos model = _dbcontext.ProveedoresInsumos.First(c => c.Id == id);
            _dbcontext.ProveedoresInsumos.Remove(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }


        public async Task<Models.ProveedoresInsumos> Obtener(int id)
        {
            return await _dbcontext.ProveedoresInsumos
                .FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<IQueryable<Models.ProveedoresInsumos>> ObtenerTodos()
        {

            IQueryable<Models.ProveedoresInsumos> query = _dbcontext.ProveedoresInsumos;
            return await Task.FromResult(query);
        }

        public async Task<IQueryable<Models.ProveedoresInsumos>> ObtenerPorProveedor(int idProveedor)
        {

            IQueryable<Models.ProveedoresInsumos> query = _dbcontext.ProveedoresInsumos.Where(x => x.IdProveedor == idProveedor);
            return await Task.FromResult(query);
        }
        public async Task<bool> ImportarDesdeLista(int idProveedor, List<ProveedoresInsumos> lista)
        {
            using var transaction = await _dbcontext.Database.BeginTransactionAsync();

            try
            {
                var codigos = lista
                    .Where(x => !string.IsNullOrWhiteSpace(x.Codigo))
                    .Select(x => x.Codigo.Trim().ToUpper())
                    .Distinct()
                    .ToList();

                var descripciones = lista
                    .Where(x => string.IsNullOrWhiteSpace(x.Codigo))
                    .Select(x => x.Descripcion?.Trim().ToUpper())
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Distinct()
                    .ToList();

                var existentes = _dbcontext.ProveedoresInsumos
                    .Where(x => x.IdProveedor == idProveedor &&
                                (
                                    (!string.IsNullOrWhiteSpace(x.Codigo) && codigos.Contains(x.Codigo.Trim().ToUpper())) ||
                                    (string.IsNullOrWhiteSpace(x.Codigo) && descripciones.Contains(x.Descripcion.Trim().ToUpper()))
                                ))
                    .ToList();

                foreach (var item in lista)
                {
                    var codigo = item.Codigo?.Trim().ToUpper();
                    var descripcion = item.Descripcion?.Trim().ToUpper();

                    ProveedoresInsumos existente;

                    if (!string.IsNullOrWhiteSpace(codigo))
                    {
                        existente = existentes.FirstOrDefault(x => x.Codigo?.Trim().ToUpper() == codigo);
                    }
                    else
                    {
                        existente = existentes.FirstOrDefault(x =>
                            string.IsNullOrWhiteSpace(x.Codigo) &&
                            x.Descripcion?.Trim().ToUpper() == descripcion);
                    }

                    if (existente != null)
                    {
                        existente.Descripcion = item.Descripcion?.Trim() ?? "";
                        existente.CostoUnitario = item.CostoUnitario;
                        existente.FechaActualizacion = DateTime.Now;
                    }
                    else
                    {
                        item.IdProveedor = idProveedor;
                        item.FechaActualizacion = DateTime.Now;
                        _dbcontext.ProveedoresInsumos.Add(item);
                    }
                }

                await _dbcontext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }




    }
}
