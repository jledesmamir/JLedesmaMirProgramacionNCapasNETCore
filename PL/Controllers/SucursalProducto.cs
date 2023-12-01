using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.Data;

namespace PL.Controllers
{

    public class SucursalProducto : Controller
    {
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult SucursalDDLGetAll()
        {
            ML.Sucursal sucursal = new ML.Sucursal();

            sucursal.Sucurales = new List<object>();

            ML.Result result = BL.Sucursal.GetAllLINQ();
            sucursal.Sucurales = result.Objects.ToList();

            if (result.Correct)
            {
                return View(sucursal);
            }
            else
            {
                ViewBag.Message = "Ocurrió un error al traer los registros de materias" + result.ErrorMessage;
                return View(sucursal);
            }

        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult GetAllProductos(ML.Sucursal sucursal)
        {
            if (sucursal.IdSucursal == 0) //add
            {
                ViewBag.Mensaje = "Se ha ingresado correctamente el Producto";
            }
            else
            {
                //ML.Result result = BL.SucursalProducto.GetAllProductslLINQ(sucursal.IdSucursal);
                //if (result.Correct)
                //{
                //    ViewBag.Mensaje = "Se ha ingresado correctamente el Producto";
                //}
                //else
                //{
                //    ViewBag.Mensaje = "No se ha ingresado correctamente el Producto. Error: " + result.ErrorMessage;
                //}
            }
            return RedirectToAction("GetAll", new { sucursal.IdSucursal });
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]//GetAll_Productos_From_IdSucursal
        public IActionResult GetAll(int IdSucursal)
        {
            ML.SucursalProducto producto = new ML.SucursalProducto();
            producto.SucursalProductos = new List<object>();

            ML.Sucursal sucursal = new ML.Sucursal();

            sucursal.Sucurales = new List<object>();

            ML.Result result = BL.SucursalProducto.GetAllProductslLINQ(IdSucursal);


            producto.Sucursal = new ML.Sucursal();

            producto.Sucursal.Sucurales = new List<object>();
            //producto.Sucursal.Sucurales = resultDDLSucursal.Objects.ToList();

            if (result.Correct)
            {
                producto.SucursalProductos = result.Objects;
                return View(producto);
            }
            else
            {
                ViewBag.Message = "Ocurrió un error al traer los registros de materias" + result.ErrorMessage;
                return View(producto);
            }

        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult UpdateStock(ML.SucursalProducto scp)
        {
            ML.Result resultado = BL.SucursalProducto.UpdateLINQ(scp);
            int IdSucursal = (int)resultado.Object;
            return RedirectToAction("GetAll", new { IdSucursal });
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Form(int? IdProducto, string paramName)
        {
            ML.Producto producto = new ML.Producto();
            if (IdProducto == null) //add
            {
                return View(producto); //vacio
            }
            else
            {
                ML.Result result = BL.SucursalProducto.GetByIdLINQ(IdProducto.Value);

                //unboxing
                producto.IdProducto = ((ML.Producto)result.Object).IdProducto;
                producto.Nombre = paramName;
                producto.PrecioUnitario = ((ML.Producto)result.Object).PrecioUnitario;
                producto.Stock = ((ML.Producto)result.Object).Stock;
                producto.Sucursal = new ML.Sucursal();
                producto.Sucursal.IdSucursal = ((ML.Producto)result.Object).Sucursal.IdSucursal;
                return View(producto);
            }

        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Form(ML.Producto producto)
        {

            if (producto.IdProducto > 0) //add
            {
                ML.Result result = BL.SucursalProducto.UpdateLINQ(producto);
                if (result.Correct)
                {
                    ViewBag.Mensaje = "Se ha ingresado correctamente el Producto";
                }
                else
                {
                    ViewBag.Mensaje = "No se ha ingresado correctamente el Producto. Error: " + result.ErrorMessage;
                }
            }
            else
                ViewBag.Mensaje = "No se ha ingresado correctamente el Producto. Error: ";

            return RedirectToAction("GetAll", new { producto.Sucursal.IdSucursal });
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult DeleteProductFromSucursal(int IdSucursalProducto)
        {
            ML.Result result = BL.Producto.DeleteLINQ(IdSucursalProducto);
            if (result.Correct)
            {
                return RedirectToAction("GetAll");
            }
            else
                ModelState.AddModelError("", "No role found");
            return View();
        }
    }
}
