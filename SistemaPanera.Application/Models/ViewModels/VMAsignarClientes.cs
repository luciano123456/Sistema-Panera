using SistemaPanera.DAL.DataContext;
using SistemaPanera.Models;

namespace SistemaPanera.Application.Models.ViewModels
{
    public class VMAsignarClientes
    {
        public string productos { get; set; }
        public int idCliente { get; set; }
        public int idProveedor { get; set; }

    }
}
