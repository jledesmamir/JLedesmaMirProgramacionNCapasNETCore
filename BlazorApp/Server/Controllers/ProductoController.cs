using BlazorApp.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        public static List<Producto> productos = new List<Producto> {

        new Producto{IdProducto=1,Nombre="Marvel" },
        new Producto{IdProducto=2,Nombre="DC" }
        };

        public static List<Proveedor> proveedores = new List<Proveedor> {

        new Proveedor{IdProveedor=1,Nombre="Peter" },
        new Proveedor{IdProveedor=2,Nombre="Bruce" },
        };

        [HttpGet]
        public async Task<ActionResult<List<Producto>>> GetProductos()
        {
            //var superHeroes = await _dataContext.superHeroes.Include(sh => sh.Comic).ToListAsync();
            return Ok(productos);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Producto>>> GetProducto(int id)
        {
            //var superHeroes = await _dataContext.superHeroes.Include(sh => sh.Comic).ToListAsync();
            var hero = productos.FirstOrDefault(h => h.IdProducto == id);
            if (hero == null)
                return NotFound("Sorry, no heroes found.");
            else
                return Ok(hero);
        }
    }
}
