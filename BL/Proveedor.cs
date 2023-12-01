using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Proveedor
    {
        public static ML.Result GetAllLINQ()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JledesmaProgramacionNcapasNetcoreContext context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {
                    var productoBuscado =
                     (
                        from Proveedors in context.Proveedors
                        select new
                        {
                            IdProveedor = Proveedors.IdProveedor,
                            Nombre = Proveedors.Nombre,
                            Direcc = Proveedors.Direccion,
                            Telefono = Proveedors.Telefono,
                            PagWeb = Proveedors.PaginaWeb
                        }
                     ).ToList();

                    result.Objects = new List<object>();

                    if (productoBuscado != null)
                    {
                        if (productoBuscado.Count > 0)
                        {
                            foreach (var obj in productoBuscado)
                            {
                                ML.Proveedor mo = new ML.Proveedor();

                                mo.IdProveedor = obj.IdProveedor;
                                mo.Nombre = obj.Nombre;
                                mo.Telefono = obj.Telefono;
                                //mo.CodigoBarras = obj.CodigoBarras;
                                //mo.Imagen = obj.Imagen;

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
