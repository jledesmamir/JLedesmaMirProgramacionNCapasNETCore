using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class AspNetUser
    {
        public static ML.Result GetByEmailLINQ(string IdUsuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JledesmaProgramacionNcapasNetcoreContext context = new DL.JledesmaProgramacionNcapasNetcoreContext())
                {

                    var usuarioBuscado =
                     (
                        from listadoUsuarios in context.AspNetUsers
                        where listadoUsuarios.Id == IdUsuario
                        select new
                        {
                            IdUsuario = listadoUsuarios.Id,
                            UserName = listadoUsuarios.UserName,

                        }
                     ).FirstOrDefault();
                    //Cambiar Para uno solo

                    if (usuarioBuscado != null)
                    {
                        ML.Usario aspNetUserInstance = new ML.Usario();
                        aspNetUserInstance.Id = usuarioBuscado.IdUsuario;
                        aspNetUserInstance.Username = usuarioBuscado.UserName;

                        result.Object = aspNetUserInstance;

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
    }
}
