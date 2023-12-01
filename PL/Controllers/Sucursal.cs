using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class Sucursal : Controller
    {
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Sucursal sucursal = new ML.Sucursal();

            sucursal.Sucurales = new List<object>();

            ML.Result result = BL.Sucursal.GetAllLINQ();


            if (result.Correct)
            {
                sucursal.Sucurales = result.Objects;
                return View(sucursal);
            }
            else
            {
                ViewBag.Message = "Ocurrió un error al traer los registros de materias" + result.ErrorMessage;
                return View(sucursal);
            }

        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Form(int? IdSucursal)
        {
            ML.Sucursal producto = new ML.Sucursal();
            if (IdSucursal == null) //add
            {
                return View(producto); //vacio
            }
            else
            {
                ML.Result result = BL.Sucursal.GetByIdLINQ(IdSucursal.Value);

                //unboxing
                producto.IdSucursal = ((ML.Sucursal)result.Object).IdSucursal;
                producto.Nombre = ((ML.Sucursal)result.Object).Nombre;
                producto.Direccion = ((ML.Sucursal)result.Object).Direccion;
                producto.PaginaWeb = ((ML.Sucursal)result.Object).PaginaWeb;
                return View(producto);
            }

        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Form(ML.Sucursal sucursalNueva)
        {
            if (sucursalNueva.IdSucursal == 0) //add
            {
                ML.Result result = BL.Sucursal.AddLINQ(sucursalNueva);
                if (result.Correct)
                {
                    ViewBag.Mensaje = "Se ha ingresado correctamente el Sucursal";
                }
                else
                {
                    ViewBag.Mensaje = "No se ha ingresado correctamente el Sucursal. Error: " + result.ErrorMessage;
                }
            }
            else
            {
                ML.Result result = BL.Sucursal.UpdateLINQ(sucursalNueva);
            }

            return RedirectToAction("GetAll");
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Delete(int IdSucursal)
        {
            ML.Result result = BL.Sucursal.DeleteLINQ(IdSucursal);
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
