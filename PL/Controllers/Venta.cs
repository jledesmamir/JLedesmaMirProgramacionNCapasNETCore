using DL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ML;
using Newtonsoft.Json;
using NuGet.Protocol;
using Stripe;
using Stripe.Checkout;
using System.Net.Mail;

namespace PL.Controllers
{
    public class Venta : Controller
    {
        IConfiguration configuration_;
        IWebHostEnvironment webHost_;
        private UserManager<IdentityUser> _userManager;

        //App Name:  NCapasNetCore, pasword:jpei mfpq sizm mufn, email:jledesmamir@gmail.com

        //testing@gmail.com
        //Welcome01$$$@

        //admninistrador@gmail.com
        //Welcome01$$$@

        public Venta(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            webHost_ = webHostEnvironment;
            configuration_ = configuration;
            _userManager = userManager;
            StripeConfiguration.ApiKey = "sk_test_51Nn3GbA6P4ZFiH4hQcnX1SUQzPSl4NlMpUEsbyDgmE9Vm8O6qd1DDIRFBVv2BSUSUF9sK8RwZttd5YcpZwzN7ZU700JYgvJtUp";
        }

        [Authorize(Roles = "Administrador, Cliente")]
        [HttpGet]
        public IActionResult SucursalDDL()
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

        [Authorize(Roles = "Administrador, Cliente")]
        [HttpPost]
        public IActionResult GetAllProductos(ML.Sucursal sucursal)
        {
            if (sucursal.IdSucursal == 0) //add
            {
                ViewBag.Mensaje = "Se ha ingresado correctamente el Producto";
            }
            else
            {

            }
            //return RedirectToAction("GetAll", new { sucursal.IdSucursal });
            return RedirectToAction("DeptoAreaDDL", new { sucursal.IdSucursal });
        }

        [Authorize(Roles = "Administrador, Cliente")]
        [HttpGet]
        public IActionResult DeptoAreaDDL()
        {
            ML.SucursalProducto sucProd = new ML.SucursalProducto();
            var sessionUsuario = HttpContext.Session.GetString("userSession");
            if (sessionUsuario != null)
            {
                ML.Usario usuario = new ML.Usario();
                usuario = JsonConvert.DeserializeObject<ML.Usario>(sessionUsuario);
                if (usuario.VentaProducto != null)
                {
                    sucProd = usuario.VentaProducto.SucursalProductos;
                    sucProd.Sucursal.Sucurales = BL.Sucursal.GetAllLINQ().Objects;

                    sucProd.Producto.Departamento.Area.Areas = BL.Venta.GetAllAreas().Objects;

                    ML.Result resultGetDeptos = new ML.Result();

                    resultGetDeptos = BL.Venta.GetAllDepartamento(usuario.VentaProducto.SucursalProductos.Producto.Departamento.Area.IdArea);

                    sucProd.Producto.Departamento.Departamentos = resultGetDeptos.Objects;

                    SessionHelper.SetObjectAsJson(HttpContext.Session, "userSession", usuario);

                    return View(sucProd);
                }
                else
                {
                    ML.Result resultGetSucursales = BL.Sucursal.GetAllLINQ();
                    sucProd.Sucursal = new ML.Sucursal();
                    sucProd.Sucursal.Sucurales = resultGetSucursales.Objects;

                    sucProd.Producto = new ML.Producto();
                    sucProd.Producto.Departamento = new ML.Departamento();

                    sucProd.Producto.Departamento.Area = new ML.Area();
                    sucProd.Producto.Departamento.Area.Areas = new List<object>();

                    ML.Result result = BL.Venta.GetAllAreas();

                    if (result.Correct)
                    {
                        sucProd.Producto.Departamento.Area.Areas = result.Objects.ToList();

                        return View(sucProd);
                    }

                    return View(sucProd);
                }

            }
            else
            {
                //ML.SucursalProducto sucProd = new ML.SucursalProducto();
                ML.Result resultGetSucursales = BL.Sucursal.GetAllLINQ();
                sucProd.Sucursal = new ML.Sucursal();
                sucProd.Sucursal.Sucurales = resultGetSucursales.Objects;

                sucProd.Producto = new ML.Producto();
                sucProd.Producto.Departamento = new ML.Departamento();

                sucProd.Producto.Departamento.Area = new ML.Area();
                sucProd.Producto.Departamento.Area.Areas = new List<object>();

                ML.Result result = BL.Venta.GetAllAreas();

                if (result.Correct)
                {
                    sucProd.Producto.Departamento.Area.Areas = result.Objects.ToList();

                    return View(sucProd);
                }
                else
                {
                    ViewBag.Message = "Ocurrió un error al traer los registros de materias" + result.ErrorMessage;
                    return View(sucProd);
                }

            }


        }

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

        public JsonResult GetSucursal()
        {
            ML.Result resulEstado = BL.Sucursal.GetAllLINQ();

            ML.Sucursal departamento = new ML.Sucursal();

            departamento.Sucurales = new List<object>();

            foreach (object objet in resulEstado.Objects)
            {
                ML.Sucursal depto = new ML.Sucursal();
                depto = (ML.Sucursal)objet;
                departamento.Sucurales.Add(depto);
            }

            return Json(departamento.Sucurales);
        }

        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet]//GetAll_Productos_From_IdSucursal
        public IActionResult GetAllProductosVenta(ML.SucursalProducto sucProd)
        {

            var sessionUsuario = HttpContext.Session.GetString("userSession");
            if (sessionUsuario != null)
            {
                ML.Usario usuario = new ML.Usario();
                usuario = JsonConvert.DeserializeObject<ML.Usario>(sessionUsuario);

                usuario.VentaProducto = new ML.VentaProducto();
                usuario.VentaProducto.SucursalProductos = new ML.SucursalProducto();
                usuario.VentaProducto.SucursalProductos = sucProd;
                SessionHelper.SetObjectAsJson(HttpContext.Session, "userSession", usuario);
            }

            ML.SucursalProducto producto = new ML.SucursalProducto();
            producto.Producto = new ML.Producto();
            producto.Producto.Productos = new List<object>();
            producto.Sucursal = new ML.Sucursal();
            producto.SucursalProductos = new List<object>();

            ML.Result result = BL.Venta.GetAllProductos(sucProd.Producto.Departamento.IdDepartamento, sucProd.Sucursal.IdSucursal);
            ML.Result resultGetSucName = BL.Sucursal.GetByIdLINQ(sucProd.Sucursal.IdSucursal);

            bool validar = WipeProductosCarrito(sucProd.Sucursal.IdSucursal);

            producto.Sucursal = (ML.Sucursal)resultGetSucName.Object;

            if (result.Objects.Count > 0)
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

        public bool WipeProductosCarrito(int IdSucursal)
        {
            var sessionSucursalNavg = HttpContext.Session.GetString("SucursalNavg");
            if (sessionSucursalNavg == null)
            {
                //Asigna IdSucursal si es null
                SessionHelper.SetObjectAsJson(HttpContext.Session, "SucursalNavg", IdSucursal);
                return true;

            }
            else
            {
                if (sessionSucursalNavg != IdSucursal.ToString())
                {
                    //Vacia la lista de productos
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "SucursalNavg", IdSucursal);
                    //var sessionCarrito = HttpContext.Session.GetString("productLista");
                    List<ML.VentaProducto> lista = new List<ML.VentaProducto>();
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "productLista", lista);
                }
                return false;
            }
        }

        public JsonResult GetAllProductSucursal(int IdSucursal)
        {
            ML.Result resultGetProductos = BL.SucursalProducto.GetAllProductslLINQ(IdSucursal);
            return Json(resultGetProductos.Objects);
        }

        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet]
        public IActionResult Form(int? IdProducto)
        {
            ML.Producto producto = new ML.Producto();
            if (IdProducto == null) //add
            {
                return View(producto); //vacio
            }
            else
            {
                ML.Result result = BL.Producto.GetByIdLINQ(IdProducto.Value);

                //unboxing
                producto.IdProducto = ((ML.Producto)result.Object).IdProducto;
                producto.Nombre = ((ML.Producto)result.Object).Nombre;
                producto.PrecioUnitario = ((ML.Producto)result.Object).PrecioUnitario;
                producto.Stock = ((ML.Producto)result.Object).Stock;
                producto.Descripcion = ((ML.Producto)result.Object).Descripcion;
                producto.Imagen = ((ML.Producto)result.Object).Imagen;
                producto.Proveedor.IdProveedor = ((ML.Producto)result.Object).Proveedor.IdProveedor;
                producto.Departamento.IdDepartamento = ((ML.Producto)result.Object).Departamento.IdDepartamento;
                return View(producto);
            }

        }

        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet]
        public IActionResult AddCarrito(int IdSucursalProduccto)
        {
            List<ML.VentaProducto> lista = new List<ML.VentaProducto>();
            var sessionCarrito = HttpContext.Session.GetString("productLista");

            ML.Result obj = BL.SucursalProducto.GetByIdSucLINQ(IdSucursalProduccto);

            if (sessionCarrito == null)
            {
                ML.Result resultadoBusqueda = BL.SucursalProducto.GetByIdSucLINQ(IdSucursalProduccto);

                ML.VentaProducto ventaProducto = new ML.VentaProducto();
                ventaProducto.SucursalProductos = (ML.SucursalProducto)resultadoBusqueda.Object;
                ventaProducto.CantidadProductoVenta = 1;
                lista.Add(ventaProducto);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "productLista", lista);
            }
            else
            {
                ML.VentaProducto repetido = new ML.VentaProducto();

                lista = JsonConvert.DeserializeObject<List<ML.VentaProducto>>(sessionCarrito);
                bool flag = false;
                foreach (ML.VentaProducto object_ in lista)
                {
                    if (object_.SucursalProductos.IdSucursalProducto == IdSucursalProduccto)
                    {
                        flag = true;
                        repetido = object_;
                    }

                }
                if (flag == false)
                {
                    ML.Result resultadoBusqueda = BL.SucursalProducto.GetByIdSucLINQ(IdSucursalProduccto);

                    ML.VentaProducto ventaProducto = new ML.VentaProducto();
                    ventaProducto.SucursalProductos = (ML.SucursalProducto)resultadoBusqueda.Object;

                    ventaProducto.CantidadProductoVenta = 1;
                    lista.Add(ventaProducto);
                }
                else
                {
                    ML.Result resultadoBusqueda = BL.SucursalProducto.GetByIdSucLINQ(IdSucursalProduccto);

                    if (repetido.CantidadProductoVenta <= ((ML.SucursalProducto)resultadoBusqueda.Object).Cantidad)
                    {
                        lista.FirstOrDefault(repetido).CantidadProductoVenta = lista.FirstOrDefault(repetido).CantidadProductoVenta + 1;

                    }
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "productLista", lista);
            }
            return RedirectToAction("GetAllProductSucursal", new { ((ML.SucursalProducto)obj.Object).Sucursal.IdSucursal });

        }

        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet]
        public IActionResult GetProductoSucursalStock(int Cantidad, int IdProductoSucursal)
        {

            List<ML.VentaProducto> lista = new List<ML.VentaProducto>();
            var sessionCarrito = HttpContext.Session.GetString("productLista");

            lista = JsonConvert.DeserializeObject<List<ML.VentaProducto>>(sessionCarrito);

            ML.VentaProducto productoactualizar = new ML.VentaProducto();

            bool flag = false;
            foreach (ML.VentaProducto object_ in lista)
            {


                if (object_.SucursalProductos.IdSucursalProducto == IdProductoSucursal)
                {
                    flag = true;
                    productoactualizar = object_;
                    object_.CantidadProductoVenta = Cantidad;
                }

            }
            if (flag)
            {
                #region Intento Fallido
                //productoactualizar.CantidadProductoVenta = Cantidad;
                //lista.Add(productoactualizar);
                //ML.Result resultadoBusqueda = BL.SucursalProducto.GetByIdSucLINQ(IdSucursalProduccto);

                //ML.VentaProducto ventaProducto = new ML.VentaProducto();
                //ventaProducto.SucursalProductos = (ML.SucursalProducto)resultadoBusqueda.Object;
                //ventaProducto.CantidadProductoVenta = 1;
                //lista.Add(ventaProducto);
                //// getbyid
                //lista.Add(ventaProducto); 
                #endregion
                SessionHelper.SetObjectAsJson(HttpContext.Session, "productLista", lista);
            }

            return RedirectToAction("ProductosCarrito");
        }

        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet]
        public IActionResult EliminarProducto(int IdProductoVenta)
        {
            List<ML.VentaProducto> lista = new List<ML.VentaProducto>();
            var sessionCarrito = HttpContext.Session.GetString("productLista");

            lista = JsonConvert.DeserializeObject<List<ML.VentaProducto>>(sessionCarrito);

            ML.VentaProducto productoactualizar = new ML.VentaProducto();

            int index = 0;

            bool flag = false;
            foreach (ML.VentaProducto object_ in lista)
            {
                if (object_.SucursalProductos.IdSucursalProducto == IdProductoVenta)
                {
                    flag = true;
                    productoactualizar = object_;
                    index = lista.IndexOf(object_);
                }

            }
            if (flag)
            {
                //productoactualizar.CantidadProductoVenta = Cantidad;
                //lista.Add(productoactualizar);
                //ML.Result resultadoBusqueda = BL.SucursalProducto.GetByIdSucLINQ(IdSucursalProduccto);

                //ML.VentaProducto ventaProducto = new ML.VentaProducto();
                //ventaProducto.SucursalProductos = (ML.SucursalProducto)resultadoBusqueda.Object;
                //ventaProducto.CantidadProductoVenta = 1;
                //lista.Add(ventaProducto);
                //// getbyid
                //lista.Add(ventaProducto);
                lista.RemoveAt(index);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "productLista", lista);
            }
            else
            {

            }
            return RedirectToAction("ProductosCarrito");
        }

        [Authorize(Roles = "Administrador,Cliente")]
        [HttpDelete]
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

        [Authorize(Roles = "Administrador,Cliente")]
        [HttpPost]
        public IActionResult Form(ML.Producto producto, IFormFile Imagen)
        {
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

            return RedirectToAction("GetAll");
        }

        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet]//Controlador vista productos Carrito
        public IActionResult ProductosCarrito()
        {
            ML.VentaProducto instance = new ML.VentaProducto();
            instance.VentaProductos = new List<object>();
            AspNetUser user = new AspNetUser();

            List<ML.VentaProducto> lista = new List<ML.VentaProducto>();
            var sessionCarrito = HttpContext.Session.GetString("productLista");



            if (sessionCarrito != null)
            {
                lista = JsonConvert.DeserializeObject<List<ML.VentaProducto>>(sessionCarrito);

                instance.VentaProductos.AddRange(lista);
            }
            else
            {
                //No existe el carrito
            }
            return View(instance);

        }

        //Seccion de controles para Venta con STRIPE
        //-----------------------------------
        //---------------------------------

        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet]
        public ActionResult GenerarPagoJesus()
        {

            string contentRoot = webHost_.WebRootPath;
            string webHosting = webHost_.ContentRootPath;

            string path = "";

            path = Path.Combine(contentRoot, "html", "Mail.html");
            //path = Path.Combine(webHosting, "html");

            var htmlFileContent = System.IO.File.ReadAllText(path);


            Email loginUser = new Email();
            loginUser.From = "NcapasCore@gmail.com";
            loginUser.To = "jledesmamir@gmail.com";
            loginUser.Subject = "prueba";
            loginUser.Body = htmlFileContent;

            List<ML.VentaProducto> ventaProductos = new List<ML.VentaProducto>();

            var sessionCarrito = HttpContext.Session.GetString("productLista");

            //ventaProductos = JsonConvert.DeserializeObject<List<ML.VentaProducto>>(sessionCarrito);


            var sessionUsuario = HttpContext.Session.GetString("userSession");
            ML.Usario usuario = new ML.Usario();
            usuario = JsonConvert.DeserializeObject<ML.Usario>(sessionUsuario);
            usuario.VentaProducto.VentaProductos = new List<object>();
            usuario.VentaProducto.VentaProductos.AddRange(JsonConvert.DeserializeObject<List<ML.VentaProducto>>(sessionCarrito));



            decimal amount = 0;

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = "http://localhost:5012/Venta/SuccessDir",
                CancelUrl = "http://localhost:5012/Venta/FailDir",
            };
            ML.Result pruebaGeneracionProductos = new ML.Result();
            try
            {
                foreach (ML.VentaProducto item in usuario.VentaProducto.VentaProductos)
                {
                    var sessionListItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.CantidadProductoVenta * item.SucursalProductos.Producto.PrecioUnitario) * 100,//Se multiplica por 100, porque, por deffecto, la unidad que se plazma son centavos (100 cent = 1 peso)

                            Currency = "mxn",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.SucursalProductos.Producto.Nombre.ToString(),
                            },

                        },
                        Quantity = item.CantidadProductoVenta
                    };
                    options.LineItems.Add(sessionListItem);

                    amount = amount + (item.CantidadProductoVenta * item.SucursalProductos.Producto.PrecioUnitario);
                }
                pruebaGeneracionProductos.Correct = true;
            }
            catch (Exception ex)
            {
                pruebaGeneracionProductos.Correct = false;
                pruebaGeneracionProductos.ErrorMessage = ex.Message;
                pruebaGeneracionProductos.Ex = ex;

            }
            if (pruebaGeneracionProductos.Correct && usuario.VentaProducto.VentaProductos.Count > 0)
            {
                foreach (ML.VentaProducto articuloVendido in usuario.VentaProducto.VentaProductos)
                {
                    articuloVendido.IdUsuario = usuario.Id;//Obtiene IdUsuario de Session
                    ML.Result restarCantidadComprada = BL.Venta.UpdateLINQ(articuloVendido);
                    ML.Result resultadoAddVentaProducto = BL.Venta.AddLINQ(articuloVendido);
                }

                var options2 = new PaymentIntentCreateOptions
                {
                    Amount = (long)amount * 100,
                    Currency = "mxn",
                    PaymentMethodTypes = new List<string> { "card" },
                    Description = "Thanks for your purchase!",
                    ReceiptEmail = "jledesmamir@gmail.com",

                };
                var options3 = new PaymentIntentCreateOptions
                {
                    Amount = (long)amount * 100,
                    Currency = "mxn",
                    AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                    {
                        Enabled = true,
                    },
                };
                var service3 = new PaymentIntentService();
                service3.Create(options3);

                var service2 = new PaymentIntentService();
                service2.Create(options2);

                var service = new SessionService();
                Session session = service.Create(options);

                Response.Headers.Add("Location", session.Url);

                service.Create
                (options);

                //var sessionUsuario = HttpContext.Session.GetString("userSession");
                //ML.Usario usuario = new ML.Usario();
                //usuario = JsonConvert.DeserializeObject<ML.Usario>(sessionUsuario);

                usuario.email = "jledesmamir@gmail.com";
                GetParametersEmail(usuario);


                AspNetUser aspNetUser = new AspNetUser();
                aspNetUser.Id = "legolas";
                aspNetUser.UserName = "Jonathan";
                aspNetUser.Email = "jledesmamir@gmail.com";
                aspNetUser.Roles = new List<AspNetRole>();
                AspNetRole role = new AspNetRole();
                role.Id = "hola";
                role.Name = "Administrador";
                aspNetUser.Roles.Add(role);

                //Aqui me quede
                //GetParametersEmail(aspNetUser);
            }
            return new StatusCodeResult(303);
        }

        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet()]
        public ActionResult SuccessDir()
        {
            List<ML.VentaProducto> lista = new List<ML.VentaProducto>();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "productLista", lista);
            return View();
        }

        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet()]
        public ActionResult FailDir()
        {
            return View();
        }

        //Seccion de controles para Enviar correo
        //---------------------------------

        private void GetParametersEmail(AspNetUser netUser)
        {
            string Nombre;

            string contentRoot = webHost_.WebRootPath;

            string pathToHTML = Path.Combine(contentRoot, "html", "Mail.html");
            // InitializeControls();
            EnviarEmail(pathToHTML, netUser.UserName, netUser.Email);


        }
        private void EnviarEmail(string pathHTML, string UserName, string emailTo)
        {

            ML.Result result = new ML.Result();

            try
            {
                result = BL.Email.PopulateBody(pathHTML, UserName, "Welcome01$$$@");

                ML.Email emailModel = new ML.Email();

                emailModel.FromDisplayName = UserName;

                emailModel.Host = configuration_.GetSection("EmailSenderOptions").GetSection("Host").Value;

                emailModel.User = configuration_.GetSection("EmailSenderOptions").GetSection("UserName").Value;
                emailModel.Password = configuration_.GetSection("EmailSenderOptions").GetSection("Password").Value;
                emailModel.Port = int.Parse(configuration_.GetSection("EmailSenderOptions").GetSection("Port").Value);
                emailModel.Body = result.Object.ToString();
                emailModel.From = configuration_.GetSection("EmailSenderOptions").GetSection("Email").Value;
                emailModel.Subject = "Recuperación de contraseña";
                emailModel.To = emailTo;

                result = BL.Email.SendEmail(emailModel);

                if (result.Correct)
                {
                    //Response.Redirect("~/Account/RecuperarPasswordSuccessEmail.aspx");
                }
                else
                {
                    //lblTextRegistro.InnerText = "Ocurrió un error al generar la contraseña " + result.ErrorMessage;
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "showModal()", true);
                }

                //Console.WriteLine("Correo Enviado");

                //Console.ReadLine();

            }
            catch (Exception ex)
            {
                result.Ex = ex;
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
        }


        private void GetParametersEmail(ML.Usario netUser)
        {
            string contentRoot = webHost_.WebRootPath;

            string pathToHTML = Path.Combine(contentRoot, "html", "Mail.html");
            // InitializeControls();
            EnviarEmail(pathToHTML, netUser);


        }
        private void EnviarEmail(string pathHTML, ML.Usario usuario)
        {

            ML.Result result = new ML.Result();

            try
            {

                result = BL.Email.PopulateBody(pathHTML, usuario.Username, "Welcome01$$$@", usuario);

                ML.Email emailModel = new ML.Email();

                emailModel.FromDisplayName = usuario.Username;

                emailModel.Host = configuration_.GetSection("EmailSenderOptions").GetSection("Host").Value;

                emailModel.User = configuration_.GetSection("EmailSenderOptions").GetSection("UserName").Value;
                emailModel.Password = configuration_.GetSection("EmailSenderOptions").GetSection("Password").Value;
                emailModel.Port = int.Parse(configuration_.GetSection("EmailSenderOptions").GetSection("Port").Value);
                emailModel.Body = result.Object.ToString();
                emailModel.From = configuration_.GetSection("EmailSenderOptions").GetSection("Email").Value;
                emailModel.Subject = "Recuperación de contraseña";
                emailModel.To = usuario.email;

                result = BL.Email.SendEmail(emailModel);

                if (result.Correct)
                {
                    //Response.Redirect("~/Account/RecuperarPasswordSuccessEmail.aspx");
                }
                else
                {
                    //lblTextRegistro.InnerText = "Ocurrió un error al generar la contraseña " + result.ErrorMessage;
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "showModal()", true);
                }


            }
            catch (Exception ex)
            {
                result.Ex = ex;
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
        }
        //-----------------------------------
        //Seccion de Historial
        //---------------------------------

    }
}
