using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class Producto : Controller
    {
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Producto producto = new ML.Producto();
            producto.Productos = new List<object>();

            ML.Result result = BL.Producto.GetAllLINQ();

            if (result.Correct)
            {
                producto.Productos = result.Objects;

                return View(producto);
            }
            else
            {
                ViewBag.Message = "Ocurrió un error al traer los registros de materias" + result.ErrorMessage;
                return View(producto);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Form(int? IdProducto)
        {
            ML.Producto producto = new ML.Producto();

            ML.Result getAreas = BL.Venta.GetAllAreas();
            ML.Result getProveedors = BL.Proveedor.GetAllLINQ();
            producto.Proveedor = new ML.Proveedor();
            producto.Proveedor.Proveedores = new List<object>();

            producto.Proveedor.Proveedores = getProveedors.Objects;


            if (IdProducto == null) //add
            {
                if (getAreas.Objects.Count > 0)
                {

                    producto.Departamento = new ML.Departamento();
                    producto.Departamento.Departamentos = new List<object>();
                    producto.Departamento.Area = new ML.Area();


                    producto.Departamento.Area.Areas = getAreas.Objects;
                }
                return View(producto);
            }
            else
            {
                ML.Result result = BL.Producto.GetByIdLINQ(IdProducto.Value);


                producto.Departamento = new ML.Departamento();
                producto.Departamento.Area = new ML.Area();

                producto.Departamento.Area.Areas = getAreas.Objects;
                //unboxing
                producto.IdProducto = ((ML.Producto)result.Object).IdProducto;
                producto.Nombre = ((ML.Producto)result.Object).Nombre;
                producto.PrecioUnitario = ((ML.Producto)result.Object).PrecioUnitario;
                producto.CodigoBarras = ((ML.Producto)result.Object).CodigoBarras;
                producto.Descripcion = ((ML.Producto)result.Object).Descripcion;
                producto.Imagen = ((ML.Producto)result.Object).Imagen;
                producto.Departamento.IdDepartamento = ((ML.Producto)result.Object).Departamento.IdDepartamento;
                producto.Departamento.Area.IdArea = ((ML.Producto)result.Object).Departamento.Area.IdArea;
                producto.Proveedor.IdProveedor = ((ML.Producto)result.Object).Proveedor.IdProveedor;
                getAreas = BL.Venta.GetAllDepartamento(producto.Departamento.Area.IdArea);

                producto.Departamento.Departamentos = getAreas.Objects;

                return View(producto);
            }

        }

        [Authorize(Roles = "Administrador")]
        public JsonResult GetDepartamento(int IdArea)
        {
            //BL- > Grupos de determinado plantel 
            ML.Result resulEstado = BL.Venta.GetAllDepartamento(IdArea);
            //crear un nuevo stored GrupoGetByIdPlantel -> DepartamentoGetByIdArea
            ML.Departamento departamento = new ML.Departamento();

            departamento.Departamentos = new List<object>();

            foreach (object objet in resulEstado.Objects)
            {
                ML.Departamento depto = new ML.Departamento();
                depto = (ML.Departamento)objet;
                departamento.Departamentos.Add(depto);
            }

            return Json(departamento.Departamentos);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Delete(int IdProducto)
        {
            ML.Result result = BL.Producto.DeleteLINQ(IdProducto);
            if (result.Correct)
            {
                return RedirectToAction("GetAll");
            }
            else
                ModelState.AddModelError("", "No role found");
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Form(ML.Producto producto, IFormFile imgProducto)
        {
            if (imgProducto != null)
                producto.Imagen = ConvertToBytes(imgProducto);
            if (producto.IdProducto == 0) //add
            {
                ML.Result result = BL.Producto.AddLINQ(producto);
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
            {
                ML.Result result = BL.Producto.UpdateLINQ(producto);
            }

            return RedirectToAction("GetAll");
        }

        public byte[] ConvertToBytes(IFormFile formFile)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(formFile.OpenReadStream());
            imageBytes = reader.ReadBytes((int)formFile.Length);
            return imageBytes;
        }


    }
}
