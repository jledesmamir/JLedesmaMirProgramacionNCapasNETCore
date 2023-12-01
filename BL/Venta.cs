using DL;
using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Venta
    {
        public static ML.Result GetAllDepartamento(int IdArea)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JledesmaProgramacionNcapasNetcoreContext context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {
                    var productoBuscado =
                        (
                                from Departamentos in context.Departamentos
                                join Areas in context.Areas
                                on Departamentos.IdArea equals Areas.IdArea
                                where Departamentos.IdArea == IdArea
                                select new
                                {
                                    IdProducto = Departamentos.IdDepartamento,
                                    Nombre = Departamentos.Nombre,

                                }
                             );
                    result.Objects = new List<object>();

                    if (productoBuscado != null)
                    {
                        if (productoBuscado.ToList().Count > 0)
                        {
                            foreach (var obj in productoBuscado)
                            {
                                ML.Departamento ventaProducto = new ML.Departamento();

                                ventaProducto.IdDepartamento = obj.IdProducto;
                                ventaProducto.Nombre = obj.Nombre;
                                result.Object = ventaProducto;

                                result.Correct = true;

                                result.Objects.Add(ventaProducto);
                            }
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No se encontraron registros";
                        }
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontraron registros";
                    }

                }
            }
            catch (Exception ex)
            {

                result.Correct = false;
                result.ErrorMessage = ex.Message;

            }

            return result;
        }
        public static ML.Result GetAllAreas()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JledesmaProgramacionNcapasNetcoreContext context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {
                    var productoBuscado =
                        (
                                from Areas in context.Areas
                                select new
                                {
                                    IdProducto = Areas.IdArea,
                                    Nombre = Areas.Nombre,

                                }
                             );
                    result.Objects = new List<object>();

                    if (productoBuscado != null)
                    {
                        if (productoBuscado.ToList().Count > 0)
                        {
                            foreach (var obj in productoBuscado)
                            {
                                ML.Area ventaProducto = new ML.Area();

                                ventaProducto.IdArea = obj.IdProducto;
                                ventaProducto.Nombre = obj.Nombre;
                                result.Object = ventaProducto;

                                result.Correct = true;

                                result.Objects.Add(ventaProducto);
                            }
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No se encontraron registros";
                        }
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontraron registros";
                    }

                }
            }
            catch (Exception ex)
            {

                result.Correct = false;
                result.ErrorMessage = ex.Message;

            }

            return result;
        }

        public static ML.Result GetAllProductos(int IdDepto, int IdSucursal)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JledesmaProgramacionNcapasNetcoreContext context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {
                    var productoBuscado =
                        (
                                from SucursalProds in context.SucursalProductos

                                join Productos in context.Productos
                                on SucursalProds.IdProducto equals Productos.IdProducto

                                join Deptos in context.Departamentos
                                on Productos.IdDepartamento equals Deptos.IdDepartamento

                                join Areas in context.Areas
                                on Deptos.IdArea equals Areas.IdArea
                                where Deptos.IdDepartamento == IdDepto && SucursalProds.IdSucursal == IdSucursal
                                select new
                                {
                                    IdProducto = Productos.IdProducto,
                                    Nombre = Productos.Nombre,
                                    Cantidad = SucursalProds.Cantidad,
                                    Imagen = Productos.Imagen,
                                    PrecioUnit = Productos.PrecioUnitario,
                                    IdSucProd = SucursalProds.IdSucursalProducto
                                }
                             );
                    result.Objects = new List<object>();

                    if (productoBuscado != null)
                    {
                        if (productoBuscado.ToList().Count > 0)
                        {
                            foreach (var obj in productoBuscado)
                            {
                                ML.SucursalProducto ventaProducto = new ML.SucursalProducto();
                                ventaProducto.Producto = new ML.Producto();
                                ventaProducto.Sucursal = new ML.Sucursal();
                                ventaProducto.Producto.IdProducto = obj.IdProducto;
                                ventaProducto.Producto.Nombre = obj.Nombre;
                                ventaProducto.Producto.PrecioUnitario = obj.PrecioUnit;
                                ventaProducto.Producto.Imagen = obj.Imagen;
                                ventaProducto.Cantidad = obj.Cantidad.Value;
                                ventaProducto.IdSucursalProducto = obj.IdSucProd;

                                result.Object = ventaProducto;

                                result.Correct = true;

                                result.Objects.Add(ventaProducto);
                            }
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No se encontraron registros";
                        }
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontraron registros";
                    }

                }
            }
            catch (Exception ex)
            {

                result.Correct = false;
                result.ErrorMessage = ex.Message;

            }

            return result;
        }

        //Historial Compras Usuario
        public static ML.Result GetAllVentaProducto_Usuario(string IdAspNetUser)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JledesmaProgramacionNcapasNetcoreContext context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {
                    var productoBuscado =
                        (
                                from VentaProductosList in context.VentaProductos
                                join AspNetUserList in context.AspNetUsers
                                on VentaProductosList.IdUsuario equals AspNetUserList.Id
                                join SucursalProductosList in context.SucursalProductos
                                on VentaProductosList.IdSucursalProducto equals SucursalProductosList.IdSucursalProducto
                                join ProductoList in context.Productos
                                on SucursalProductosList.IdProducto equals ProductoList.IdProducto
                                where VentaProductosList.IdUsuario == IdAspNetUser
                                select new
                                {
                                    IdProductoVenta = VentaProductosList.IdProductoVenta,
                                    Nombre = ProductoList.Nombre,
                                    PrecioUnitario = ProductoList.PrecioUnitario,
                                    Imagen = ProductoList.Imagen,
                                    IdAspNetUser = AspNetUserList.Id,
                                    UsuarioName = AspNetUserList.UserName,
                                    CantidadComprada = VentaProductosList.CantidadProductoVenta,

                                }
                             );
                    result.Objects = new List<object>();

                    if (productoBuscado != null)
                    {
                        if (productoBuscado.ToList().Count > 0)
                        {
                            foreach (var obj in productoBuscado)
                            {
                                ML.VentaProducto ventaProducto = new ML.VentaProducto();

                                ventaProducto.IdProductoVenta = obj.IdProductoVenta;
                                ventaProducto.CantidadProductoVenta = obj.CantidadComprada;
                                ventaProducto.SucursalProductos = new ML.SucursalProducto();
                                ventaProducto.SucursalProductos.Producto = new ML.Producto();
                                ventaProducto.SucursalProductos.Producto.Nombre = obj.Nombre;
                                ventaProducto.SucursalProductos.Producto.PrecioUnitario = obj.PrecioUnitario;
                                ventaProducto.SucursalProductos.Producto.Imagen = obj.Imagen;
                                ventaProducto.IdUsuario = obj.IdAspNetUser;
                                ventaProducto.UsuarioNombre = obj.UsuarioName;

                                result.Object = ventaProducto;

                                result.Correct = true;

                                result.Objects.Add(ventaProducto);
                            }
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No se encontraron registros";
                        }
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontraron registros";
                    }

                }
            }
            catch (Exception ex)
            {

                result.Correct = false;
                result.ErrorMessage = ex.Message;

            }

            return result;
        }

        public static ML.Result AddLINQ(ML.VentaProducto ventaNueva)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JledesmaProgramacionNcapasNetcoreContext context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {

                    DL.VentaProducto ventaProductoInstance = new DL.VentaProducto();

                    ventaProductoInstance.CantidadProductoVenta = ventaNueva.CantidadProductoVenta;
                    ventaProductoInstance.IdUsuario = ventaNueva.IdUsuario;
                    ventaProductoInstance.IdSucursalProducto = ventaNueva.SucursalProductos.IdSucursalProducto;
                    context.VentaProductos.Add(ventaProductoInstance);

                    int rowsAffected = context.SaveChanges();

                    if (rowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al registrar Venta de Producto Nueva: " + ventaProductoInstance.IdProductoVenta;
                    }
                }
            }
            catch (Exception Ex)
            {
                //Error.Add(Ex);

                result.Correct = false;
                result.ErrorMessage = Ex.Message;
            }

            return result;
        }

        public static ML.Result UpdateLINQ(ML.VentaProducto productoRestar)
        {
            ML.Result result = new ML.Result();
            var producto = productoRestar.SucursalProductos;
            try
            {

                using (DL.JledesmaProgramacionNcapasNetcoreContext context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {

                    var query = (from aliasTableUsuarioEntityFramework in context.SucursalProductos
                                 where aliasTableUsuarioEntityFramework.IdSucursalProducto == producto.IdSucursalProducto
                                 select aliasTableUsuarioEntityFramework).SingleOrDefault();


                    if (query != null)
                    {
                        query.Cantidad -= productoRestar.CantidadProductoVenta;

                        context.SucursalProductos.Update(query);


                        int rowsAffected = context.SaveChanges();
                        result.Object = query.IdSucursal;
                        if (rowsAffected > 0)
                        {
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                        }
                    }

                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontró el grupo" + producto.IdSucursalProducto;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

    }
}
