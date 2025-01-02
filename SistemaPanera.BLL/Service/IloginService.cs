using SistemaPanera.Models;
using System.Net.Http;

namespace SistemaPanera.BLL.Service
{
    public interface ILoginService
    {
        Task<User> Login(string username, string password);

        Task<bool> Logout();
    }
}
