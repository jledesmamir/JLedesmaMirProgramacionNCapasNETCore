using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BL
{
    public class Sucursal
    {
        public static ML.Result AddLINQ(ML.Sucursal sucursalNueva)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JledesmaProgramacionNcapasNetcoreContext context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {

                    DL.Sucursal sucursalInstance = new DL.Sucursal();

                    sucursalInstance.Nombre = sucursalNueva.Nombre;
                    sucursalInstance.Direccion = sucursalNueva.Direccion;
                    sucursalInstance.PaginaWeb = sucursalNueva.PaginaWeb;
                    context.Sucursals.Add(sucursalInstance);

                    int rowsAffected = context.SaveChanges();

                    if (rowsAffected > 0)
                    {
                        result = BL.SucursalProducto.AddLINQ_LoadProducts(sucursalInstance.IdSucursal);
                        if (result.Correct)
                            result.Correct = true;
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "Error al cargar productos a la sucursal " + sucursalInstance.Nombre + ".";
                        }
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al registrar Sucursal Nueva: " + sucursalInstance.Nombre;
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

        public static ML.Result UpdateLINQ(ML.Sucursal producto)
        {
            ML.Result result = new ML.Result();

            try
            {

                using (DL.JledesmaProgramacionNcapasNetcoreContext context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {

                    var query = (from aliasTableUsuarioEntityFramework in context.Sucursals
                                 where aliasTableUsuarioEntityFramework.IdSucursal == producto.IdSucursal
                                 select aliasTableUsuarioEntityFramework).SingleOrDefault();


                    if (query != null)
                    {
                        query.Nombre = producto.Nombre;
                        query.Direccion = producto.Direccion;
                        //query.Stock = producto.Stock;
                        query.PaginaWeb = producto.PaginaWeb;
                        context.Sucursals.Update(query);


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
                        result.ErrorMessage = "No se encontró el grupo" + producto.IdSucursal;
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

                    var query = (from aliasTableUsuarioEntityFramework in context.Sucursals
                                 where aliasTableUsuarioEntityFramework.IdSucursal == IdProducto
                                 select aliasTableUsuarioEntityFramework).First();

                    context.Sucursals.Remove(query);
                    context.SaveChanges();
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
        public static ML.Result GetByIdLINQ(int IdSucursal)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JledesmaProgramacionNcapasNetcoreContext context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {

                    var productoBuscado =
                     (
                        from listadoProducto in context.Sucursals
                        join producto in context.Sucursals on listadoProducto.IdSucursal equals producto.IdSucursal
                        where listadoProducto.IdSucursal == IdSucursal
                        select new
                        {
                            IdProducto = producto.IdSucursal,
                            Nombre = producto.Nombre,
                            PaginaWeb = producto.PaginaWeb,
                            Direccion = producto.Direccion,

                        }
                     ).FirstOrDefault();
                    //Cambiar Para uno solo

                    if (productoBuscado != null)
                    {
                        ML.Sucursal producto = new ML.Sucursal();

                        producto.IdSucursal = IdSucursal;
                        producto.Nombre = productoBuscado.Nombre;
                        producto.Direccion = productoBuscado.Direccion;
                        producto.PaginaWeb = productoBuscado.PaginaWeb;

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
                        from listadoProducto in context.Sucursals
                        join producto in context.Sucursals on listadoProducto.IdSucursal equals producto.IdSucursal
                        where listadoProducto.IdSucursal == producto.IdSucursal
                        select new
                        {
                            IdSucursal = producto.IdSucursal,
                            Nombre = producto.Nombre,
                            PaginaWeb = producto.PaginaWeb,
                            Direccion = producto.Direccion,
                        }
                     ).ToList();

                    result.Objects = new List<object>();

                    if (productoBuscado != null)
                    {
                        if (productoBuscado.Count > 0)
                        {
                            foreach (var obj in productoBuscado)
                            {
                                ML.Sucursal mo = new ML.Sucursal();

                                mo.IdSucursal = obj.IdSucursal;
                                mo.Nombre = obj.Nombre;
                                mo.PaginaWeb = obj.PaginaWeb;
                                mo.Direccion = obj.Direccion;

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