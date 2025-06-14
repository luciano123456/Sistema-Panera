using Microsoft.EntityFrameworkCore;
using SistemaPanera.BLL.Service;
using SistemaPanera.DAL.DataContext;
using SistemaPanera.DAL.Repository;
using SistemaPanera.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<SistemaPaneraContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SistemaDB")));


// Agregar Razor Pages
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// Registrar repositorios y servicios
builder.Services.AddScoped<IProvinciaRepository<Provincia>, ProvinciaRepository>();
builder.Services.AddScoped<IProvinciaService, ProvinciaService>();
builder.Services.AddScoped<IGenericRepository<Proveedor>, ProveedorRepository>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();

builder.Services.AddScoped<IUsuariosRepository<User>, UsuariosRepository>();
builder.Services.AddScoped<IUsuariosService, UsuariosService>();

builder.Services.AddScoped<IRolesRepository<Rol>, RolesRepository>();
builder.Services.AddScoped<IRolesService, RolesService>();

builder.Services.AddScoped<IEstadosUsuariosRepository<EstadosUsuario>, EstadosUsuariosRepository>();
builder.Services.AddScoped<IEstadosUsuariosService, EstadosUsuariosService>();

builder.Services.AddScoped<ILoginRepository<User>, LoginRepository>();
builder.Services.AddScoped<ILoginService, LoginService>();


builder.Services.AddScoped<IUnidadesNegocioRepository<UnidadesNegocio>, UnidadesNegocioRepository>();
builder.Services.AddScoped<IUnidadesNegocioService, UnidadesNegocioService>();

builder.Services.AddScoped<IUnidadesMedidaRepository<UnidadesMedida>, UnidadesMedidaRepository>();
builder.Services.AddScoped<IUnidadesMedidaService, UnidadesMedidaService>();

builder.Services.AddScoped<IInsumosCategoriaRepository<InsumosCategoria>, InsumosCategoriaRepository>();
builder.Services.AddScoped<IInsumosCategoriaService, InsumosCategoriaService>();

builder.Services.AddScoped<IInsumoRepository<Insumo>, InsumoRepository>();
builder.Services.AddScoped<IInsumoService, InsumoService>();

builder.Services.AddScoped<IGenericRepository<Local>, LocalRepository>();
builder.Services.AddScoped<ILocalService, LocalService>();

builder.Services.AddScoped<IRecetasCategoriaRepository<RecetasCategoria>, RecetasCategoriaRepository>();
builder.Services.AddScoped<IRecetasCategoriaService, RecetasCategoriaService>();

builder.Services.AddScoped<IOrdenesComprasEstadoRepository<OrdenesComprasEstado>, OrdenesComprasEstadoRepository>();
builder.Services.AddScoped<IOrdenesComprasEstadoservice, OrdenesComprasEstadoService>();

builder.Services.AddScoped<IOrdenesComprasInsumoEstadoRepository<OrdenesComprasInsumosEstado>, OrdenesComprasInsumosEstadoRepository>();
builder.Services.AddScoped<IOrdenesComprasInsumosEstadoservice, OrdenesComprasInsumosEstadoService>();

builder.Services.AddScoped<IRecetaRepository<Receta>, RecetaRepository>();
builder.Services.AddScoped<IRecetaService, RecetaService>();

builder.Services.AddScoped<IProveedoresInsumosRepository<ProveedoresInsumos>, ProveedoresInsumosRepository>();
builder.Services.AddScoped<IProveedoresInsumoservice, ProveedoresInsumoservice>();

builder.Services.AddScoped<ISubRecetasCategoriaRepository<SubrecetasCategoria>, SubrecetasCategoriaRepository>();
builder.Services.AddScoped<ISubRecetasCategoriaService, SubrecetasCategoriaService>();

builder.Services.AddScoped<ISubrecetaRepository<Subreceta>, SubrecetaRepository>();
builder.Services.AddScoped<ISubrecetaService, SubrecetaService>();

builder.Services.AddScoped<ICompraRepository<Compra>, CompraRepository>();
builder.Services.AddScoped<ICompraService, CompraService>();

builder.Services.AddScoped<IOrdenesCompraRepository, OrdenesCompraRepository>();
builder.Services.AddScoped<IOrdenesCompraService, OrdenesCompraService>();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        o.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Configurar autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";  // Ruta para redirigir al login si no está autenticado
        options.LogoutPath = "/Login/Logout"; // Ruta para cerrar sesión
    });


var app = builder.Build();

// Configurar el pipeline de middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Usuarios/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Habilitar la autenticación con cookies
app.UseAuthorization();  // Habilitar la autorización

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuarios}/{action=Index}/{id?}");

// Asegúrate de que las rutas de login estén excluidas del middleware de autenticación
app.MapControllerRoute(
    name: "login",
    pattern: "Login/{action=Index}",
    defaults: new { controller = "Login", action = "Index" });
app.Run();
