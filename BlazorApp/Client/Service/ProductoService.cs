using System.Net.Http;
using System.Net.Http.Json;

namespace BlazorApp.Client.Service
{
    public class ProductoService : IProductoService
    {
        private readonly HttpClient _httpClient;
        public ProductoService(HttpClient http)
        {
            _httpClient=http;
        }
        public List<Producto> productos { get; set ; }
        public List<Proveedor> proveedors { get ; set; }

        public async Task GetProductos()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Producto>>("api/Producto");
            if (result != null)
                productos = result;
        }

        public async Task GetProveedorss()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Proveedor>>("api/Producto");
            if (result != null)
                proveedors = result;
        }

        public Task<Producto> GetSingleProducto(int id)
        {
            throw new NotImplementedException();
        }
    }
}
