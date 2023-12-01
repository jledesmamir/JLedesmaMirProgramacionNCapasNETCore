using BlazorApp.Client.Pages;
using BlazorApp.Shared;

namespace BlazorApp.Client.Service
{
    public interface IProductoService
    {
        List<BlazorApp.Shared.Producto> productos { get; set; }
        List<Proveedor> proveedors { get; set; }

        Task GetProveedorss();
        Task GetProductos();
        Task<BlazorApp.Shared.Producto> GetSingleProducto(int id);
    }
}
