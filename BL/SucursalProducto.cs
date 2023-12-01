using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class SucursalProducto
    {
        public static ML.Result AddLINQ(int idProducto)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JledesmaProgramacionNcapasNetcoreContext context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {
                    ML.Result resultGetAllSucursal = Sucursal.GetAllLINQ();


                    int rowsAffected = 0;
                    foreach (ML.Sucursal objeto in resultGetAllSucursal.Objects)
                    {
                        DL.SucursalProducto spNuevo = new DL.SucursalProducto();

                        spNuevo.IdProducto = idProducto;
                        spNuevo.IdSucursal = objeto.IdSucursal;
                        spNuevo.Cantidad = 0;
                        context.SucursalProductos.Add(spNuevo);
                        rowsAffected = context.SaveChanges();
                    }

                    if (rowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
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
        public static ML.Result AddLINQ_LoadProducts(int idSucursal)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JledesmaProgramacionNcapasNetcoreContext context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {
                    ML.Result resultGetAllProducto = Producto.GetAllLINQ();


                    int rowsAffected = 0;
                    foreach (ML.Producto objeto in resultGetAllProducto.Objects)
                    {
                        DL.SucursalProducto spNuevo = new DL.SucursalProducto();

                        spNuevo.IdSucursal = idSucursal;
                        spNuevo.IdProducto = objeto.IdProducto;
                        spNuevo.Cantidad = 0;
                        context.SucursalProductos.Add(spNuevo);
                        rowsAffected = context.SaveChanges();
                    }

                    if (rowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
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

                    var query = (from aliasTableUsuarioEntityFramework in context.SucursalProductos
                                 where aliasTableUsuarioEntityFramework.IdProducto == producto.IdProducto && aliasTableUsuarioEntityFramework.IdSucursal == producto.Sucursal.IdSucursal
                                 select aliasTableUsuarioEntityFramework).SingleOrDefault();


                    if (query != null)
                    {
                        query.Cantidad = producto.Stock;

                        //query.IdProveedorNavigation.IdProveedor = producto.Proveedor.IdProveedor;

                        //query.IdDepartamentoNavigation.IdDepartamento = producto.Departamento.IdDepartamento;

                        context.SucursalProductos.Update(query);


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
        public static ML.Result UpdateLINQ(ML.SucursalProducto producto)
        {
            ML.Result result = new ML.Result();

            try
            {

                using (DL.JledesmaProgramacionNcapasNetcoreContext context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {

                    var query = (from aliasTableUsuarioEntityFramework in context.SucursalProductos
                                 where aliasTableUsuarioEntityFramework.IdSucursalProducto == producto.IdSucursalProducto
                                 select aliasTableUsuarioEntityFramework).SingleOrDefault();


                    if (query != null)
                    {
                        query.Cantidad = producto.Cantidad;

                        //query.IdProveedorNavigation.IdProveedor = producto.Proveedor.IdProveedor;

                        //query.IdDepartamentoNavigation.IdDepartamento = producto.Departamento.IdDepartamento;

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

        public static ML.Result GetByIdLINQ(int IdProducto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JledesmaProgramacionNcapasNetcoreContext context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {

                    var productoBuscado =
                     (
                        from listadoProducto in context.SucursalProductos
                        join producto in context.Productos on listadoProducto.IdProducto equals producto.IdProducto
                        where listadoProducto.IdProducto == producto.IdProducto
                        select new
                        {
                            IdSucursal = listadoProducto.IdSucursal,
                            IdProducto = producto.IdProducto,
                            Nombre = producto.Nombre,
                            PrecioUnitario = producto.PrecioUnitario,
                            Stock = listadoProducto.Cantidad,

                        }
                     ).FirstOrDefault();
                    //Cambiar Para uno solo

                    if (productoBuscado != null)
                    {
                        ML.Producto producto = new ML.Producto();

                        producto.IdProducto = IdProducto;
                        producto.Nombre = productoBuscado.Nombre;
                        producto.PrecioUnitario = productoBuscado.PrecioUnitario;
                        producto.Sucursal = new ML.Sucursal();
                        producto.Sucursal.IdSucursal = productoBuscado.IdSucursal.Value;
                        producto.Stock = productoBuscado.Stock.Value;

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

        public static ML.Result GetByIdSucLINQ(int IdSucProd)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JledesmaProgramacionNcapasNetcoreContext context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {

                    var productoBuscado =
                     (
                        from listadoProducto in context.SucursalProductos
                        join producto in context.Productos on listadoProducto.IdProducto equals producto.IdProducto
                        where listadoProducto.IdSucursalProducto == IdSucProd
                        select new
                        {
                            IdSucursal = listadoProducto.IdSucursal,
                            IdProducto = listadoProducto.IdProducto,
                            Nombre = producto.Nombre,
                            PrecioUnitario = producto.PrecioUnitario,
                            Stock = listadoProducto.Cantidad,
                            Imagen = producto.Imagen

                        }
                     ).FirstOrDefault();
                    //Cambiar Para uno solo

                    if (productoBuscado != null)
                    {
                        ML.SucursalProducto sucProducto = new ML.SucursalProducto();

                        sucProducto.Producto = new ML.Producto();

                        sucProducto.IdSucursalProducto = IdSucProd;
                        sucProducto.Producto.IdProducto = productoBuscado.IdProducto.Value;
                        sucProducto.Producto.Nombre = productoBuscado.Nombre;
                        sucProducto.Producto.PrecioUnitario = productoBuscado.PrecioUnitario;
                        sucProducto.Producto.Imagen = productoBuscado.Imagen;

                        sucProducto.Sucursal = new ML.Sucursal();
                        sucProducto.Sucursal.IdSucursal = productoBuscado.IdSucursal.Value;
                        sucProducto.Cantidad = productoBuscado.Stock.Value;
                        result.Object = sucProducto;

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

        public static ML.Result GetAllProductslLINQ(int IdSucursal)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JledesmaProgramacionNcapasNetcoreContext context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {
                    var productoBuscado =
                        (
                                from productos in context.Productos
                                join SucursalProdsContext in context.SucursalProductos
                                on productos.IdProducto equals SucursalProdsContext.IdProducto
                                where SucursalProdsContext.IdSucursal == IdSucursal
                                select new
                                {
                                    IdProducto = SucursalProdsContext.IdProducto,
                                    Nombre = SucursalProdsContext.IdProductoNavigation.Nombre,
                                    PrecioUnitario = SucursalProdsContext.IdProductoNavigation.PrecioUnitario,
                                    Imagen = productos.Imagen,
                                    Cantidad = SucursalProdsContext.Cantidad,
                                    IdSucursal = SucursalProdsContext.IdSucursal,
                                    IdSucProd = SucursalProdsContext.IdSucursalProducto

                                }
                             );
                    result.Objects = new List<object>();

                    if (productoBuscado != null)
                    {
                        if (productoBuscado.ToList().Count > 0)
                        {
                            foreach (var obj in productoBuscado)
                            {
                                ML.SucursalProducto mo = new ML.SucursalProducto();
                                mo.Producto = new ML.Producto();
                                mo.Sucursal = new ML.Sucursal();

                                mo.Cantidad = obj.Cantidad.Value;

                                mo.Producto.IdProducto = obj.IdProducto.Value;
                                mo.Producto.Nombre = obj.Nombre;
                                mo.Producto.PrecioUnitario = obj.PrecioUnitario;
                                mo.Producto.Imagen = obj.Imagen;
                                mo.IdSucursalProducto = obj.IdSucProd;

                                mo.Sucursal.IdSucursal = obj.IdSucursal.Value;

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

        public static ML.Result DeleteLINQ(int IdProducto)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (var context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {


                    var query = (from aliasTableUsuarioEntityFramework in context.SucursalProductos
                                 where aliasTableUsuarioEntityFramework.IdProducto == IdProducto
                                 select aliasTableUsuarioEntityFramework).ToList();

                    foreach (var obj in query)
                    {
                        context.SucursalProductos.Remove(obj);
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

    }
}
