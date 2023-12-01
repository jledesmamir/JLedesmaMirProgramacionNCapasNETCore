using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Stripe;
using System.IdentityModel.Tokens.Jwt;

namespace PL.Controllers
{
    public class HistorialController : Controller
    {
       
        public HistorialController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            StripeConfiguration.ApiKey = "sk_test_51Nn3GbA6P4ZFiH4hQcnX1SUQzPSl4NlMpUEsbyDgmE9Vm8O6qd1DDIRFBVv2BSUSUF9sK8RwZttd5YcpZwzN7ZU700JYgvJtUp";
        }

        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet]
        public IActionResult GetAllProductosIdUsuario()
        {
            var sessionUser = HttpContext.Session.GetString("userSession");
            if (sessionUser != null)
            {
                ML.Usario usuario = new ML.Usario();
                usuario = JsonConvert.DeserializeObject<ML.Usario>(sessionUser);


                ML.VentaProducto ventaPorudctosInstance = new ML.VentaProducto();

                ventaPorudctosInstance.VentaProductos = new List<object>();

                ML.Result result = BL.Venta.GetAllVentaProducto_Usuario(usuario.Id);
                ventaPorudctosInstance.VentaProductos = result.Objects.ToList();

                if (result.Correct)
                {
                    return View(ventaPorudctosInstance);
                }
                else
                {
                    ViewBag.Message = "Ocurrió un error al traer los registros de materias" + result.ErrorMessage;
                    return View(ventaPorudctosInstance);
                }
            }
            else
            {
                //return RedirectToRoute("Identity/Account/Login");
                return RedirectToAction("Login", "Historial");
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View("/Areas/Identity/Pages/Account/Login.cshtml");
        }
    }
}
