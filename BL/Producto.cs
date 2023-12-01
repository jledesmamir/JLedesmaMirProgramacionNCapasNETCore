using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Producto
    {
        public static ML.Result AddLINQ(ML.Producto producto)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JledesmaProgramacionNcapasNetcoreContext context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {

                    DL.Producto productoNuevo = new DL.Producto();
                    productoNuevo.IdProveedor = producto.Proveedor.IdProveedor;
                    productoNuevo.IdDepartamento = producto.Departamento.IdDepartamento;

                    productoNuevo.Nombre = producto.Nombre;
                    productoNuevo.PrecioUnitario = producto.PrecioUnitario;
                    productoNuevo.CodigoBarras = producto.CodigoBarras;
                    productoNuevo.Descripcion = producto.Descripcion;
                    productoNuevo.Imagen = (producto.Imagen);

                    context.Productos.Add(productoNuevo);
                    int rowsAffected = context.SaveChanges();

                    int id = (from record in context.Productos
                              orderby record.IdProducto
                              select record.IdProducto).Last();


                    if (rowsAffected > 0)
                    {
                        result = BL.SucursalProducto.AddLINQ(id);
                        if (result.Correct)
                            result.Correct = true;
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "Fallo al cargar producto nuevo en todas las sucursales.";
                        }
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Fallo al cargar producto nuevo.";
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

        public static ML.Result UpdateLINQ(ML.Producto producto)
        {
            ML.Result result = new ML.Result();

            try
            {

                using (DL.JledesmaProgramacionNcapasNetcoreContext context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {

                    var query = (from aliasTableUsuarioEntityFramework in context.Productos
                                 where aliasTableUsuarioEntityFramework.IdProducto == producto.IdProducto
                                 select aliasTableUsuarioEntityFramework).SingleOrDefault();

                    if (query != null)
                    {
                        query.Nombre = producto.Nombre;
                        query.PrecioUnitario = producto.PrecioUnitario;
                        //query.Stock = producto.Stock;
                        query.Descripcion = producto.Descripcion;
                        query.CodigoBarras = producto.CodigoBarras;
                        query.Imagen = (producto.Imagen == null) ? null : producto.Imagen;
                        query.IdProveedor = producto.Proveedor.IdProveedor;

                        query.IdDepartamento = producto.Departamento.IdDepartamento;

                        context.Productos.Update(query);


                        int rowsAffected = context.SaveChanges();

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
                        result.ErrorMessage = "No se encontró el grupo" + producto.IdProducto;
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

        public static ML.Result DeleteLINQ(int IdSucursalProducto)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (var context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {


                    var query = (from aliasTableUsuarioEntityFramework in context.SucursalProductos
                                 where aliasTableUsuarioEntityFramework.IdSucursalProducto == IdSucursalProducto
                                 select aliasTableUsuarioEntityFramework).First();
                    if (query.IdSucursalProducto != 0)
                    {
                        context.SucursalProductos.Remove(query);
                        context.SaveChanges();
                    }
                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public static ML.Result GetByIdLINQ(int IdProducto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JledesmaProgramacionNcapasNetcoreContext context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {
                    var productoBuscado =
                     (
                        from listadoProducto in context.Productos
                        join producto in context.Productos on listadoProducto.IdProducto equals producto.IdProducto
                        where listadoProducto.IdProducto == IdProducto
                        select new
                        {
                            IdProducto = producto.IdProducto,
                            Nombre = producto.Nombre ?? "Unknown",
                            PrecioUnitario = producto.PrecioUnitario,
                            CodigoDeBarras = producto.CodigoBarras ?? "00000",
                            Descripcion = producto.Descripcion ?? "Unknown",
                            Imagen = producto.Imagen,
                            IdProveedor = (producto.IdProveedorNavigation.IdProveedor == null) ? 1 : producto.IdProveedorNavigation.IdProveedor,
                            IdDepartamento = (producto.IdDepartamentoNavigation.IdDepartamento == null) ? 1 : producto.IdDepartamentoNavigation.IdDepartamento,
                            IdArea = (producto.IdDepartamentoNavigation.IdAreaNavigation.IdArea == null) ? 1 : producto.IdDepartamentoNavigation.IdAreaNavigation.IdArea

                        }
                     ).FirstOrDefault();
                    //Cambiar Para uno solo

                    if (productoBuscado != null)
                    {
                        ML.Producto producto = new ML.Producto();

                        producto.IdProducto = IdProducto;
                        producto.Nombre = productoBuscado.Nombre;
                        producto.PrecioUnitario = productoBuscado.PrecioUnitario;
                        producto.CodigoBarras = productoBuscado.CodigoDeBarras;
                        producto.Descripcion = productoBuscado.Descripcion;
                        producto.Imagen = productoBuscado.Imagen;

                        producto.Proveedor = new ML.Proveedor();
                        producto.Proveedor.IdProveedor = (productoBuscado.IdProveedor == null) ? 1 : productoBuscado.IdProveedor;

                        producto.Departamento = new ML.Departamento();
                        producto.Departamento.IdDepartamento = (productoBuscado.IdDepartamento == null) ? 1 : productoBuscado.IdDepartamento;
                        producto.Departamento.Area = new ML.Area();
                        producto.Departamento.Area.IdArea = (productoBuscado.IdArea == null) ? 1 : productoBuscado.IdArea;

                        result.Object = producto;

                        result.Correct = true;
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

        public static ML.Result GetAllLINQ()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JledesmaProgramacionNcapasNetcoreContext context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {
                    var productoBuscado =
                     (
                        from listadoProducto in context.Productos
                        join producto in context.Productos on listadoProducto.IdProducto equals producto.IdProducto
                        where listadoProducto.IdProducto == producto.IdProducto
                        select new
                        {
                            IdSucursal = producto.IdProducto,
                            Nombre = producto.Nombre,
                            CodigoBarras = producto.CodigoBarras,
                            PrecioUnit = producto.PrecioUnitario,
                            Imagen = producto.Imagen
                        }
                     ).ToList();

                    result.Objects = new List<object>();

                    if (productoBuscado != null)
                    {
                        if (productoBuscado.Count > 0)
                        {
                            foreach (var obj in productoBuscado)
                            {
                                ML.Producto mo = new ML.Producto();

                                mo.IdProducto = obj.IdSucursal;
                                mo.Nombre = obj.Nombre;
                                mo.PrecioUnitario = obj.PrecioUnit;
                                mo.CodigoBarras = obj.CodigoBarras;
                                mo.Imagen = obj.Imagen;

                                result.Object = mo;

                                result.Correct = true;

                                result.Objects.Add(mo);
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
    }
}
