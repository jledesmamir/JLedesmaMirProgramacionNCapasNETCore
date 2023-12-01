using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using System.Xml.Schema;
using System.Collections;

namespace BL
{
    public class Email
    {
        public static ML.Result PopulateBody(string pathHTML, string userName, string password, ML.Usario usuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                string body = string.Empty;

                using (StreamReader reader = new StreamReader(pathHTML))
                {
                    body = reader.ReadToEnd();
                }
                List<string>listaHtmlProds= new List<string>();

                decimal total = 0;
                foreach (ML.VentaProducto producto in usuario.VentaProducto.VentaProductos) {
                    total = total + (producto.SucursalProductos.Producto.PrecioUnitario * producto.CantidadProductoVenta);
                listaHtmlProds.Add("<tr><td>"+producto.SucursalProductos.Producto.Nombre+ "</td><td>$"+producto.SucursalProductos.Producto.PrecioUnitario +"</td></tr>");
                }
                string combinedString = string.Join('\n', listaHtmlProds);

                body = body.Replace("{UserName}", userName);
                body = body.Replace("{Password}", password);
                body = body.Replace("{IdVentaProducto}", password);
                body = body.Replace("{Renglones}", combinedString);
                body = body.Replace("{TotalPago}", total.ToString());

                result.Correct = true;
                result.Object = body;

            }
            catch (Exception ex)
            {
                result.Ex = ex;
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;

        }
        public static ML.Result PopulateBody(string pathHTML, string userName, string password)
        {
            ML.Result result = new ML.Result();

            try
            {
                string body = string.Empty;

                using (StreamReader reader = new StreamReader(pathHTML))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{UserName}", userName);
                body = body.Replace("{Password}", password);
                body = body.Replace("{IdVentaProducto}", password);
                body = body.Replace("{Renglones}", password);
                body = body.Replace("{TotalPago}", password);

                result.Correct = true;
                result.Object = body;

            }
            catch (Exception ex)
            {
                result.Ex = ex;
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;

        }

        public static ML.Result SendEmail(ML.Email email)
        {
            ML.Result result = new ML.Result();

            try
            {
                #region Failed
                // Create the message.
                //MailMessage mailNew = new MailMessage();
                // Set the message properties.
                //MailAddress from = new MailAddress(email.From, email.FromDisplayName);

                //mailNew.From = from;
                //mailNew.To.Add(email.To);
                //mailNew.Subject = email.Subject;
                //mailNew.IsBodyHtml = true;
                //mailNew.Body = email.Body;

                //SmtpClient smtp = new SmtpClient();
                //smtp.Host = email.Host;
                //smtp.Port = email.Port;
                //smtp.EnableSsl = true;

                //smtp.Credentials = new System.Net.NetworkCredential(email.User, email.Password); 
                #endregion
                var mailNew = new MimeMessage();
                mailNew.From.Add(MailboxAddress.Parse(email.User));
                mailNew.To.Add(MailboxAddress.Parse(email.To));
                mailNew.Subject = email.Subject;
                mailNew.Body = new TextPart(TextFormat.Html) { Text = email.Body };


                var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect(email.Host, email.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(email.User, email.Password);



                smtp.Send(mailNew);
                smtp.Disconnect(true);

                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Ex = ex;
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }

}
