using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared
{
    public class Producto
    {
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El campo Producto es Obligatorio.")]
        [Range(0, 200, ErrorMessage = "no debe exeder 200 Caracteres o estar vació")]
        [RegularExpression(@"^[A-Z]+[a-z]*$", ErrorMessage = "Solo se Permiten Numeros")]
        public string? Nombre { get; set; }
        public string? CodigoBarras { get; set; }

        [RegularExpression(@"^[0-9]*\.[0-9]{2}$", ErrorMessage = "Solo se Permiten Numeros")]
        [Display(Name = "Precio Unitario")]
        [Required(ErrorMessage = "El campo Precio es Obligatorio.")]
        public decimal PrecioUnitario { get; set; }

        [Required(ErrorMessage = "El campo Stock es Obligatorio.")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Solo se Permiten Numeros")]
        [Range(0, 200, ErrorMessage = "Solo se permiten valores de 0 a 200")]
        public int Stock { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$", ErrorMessage = "Solo se Permiten Numeros")]
        [Range(0, 500, ErrorMessage = "No debe exeder 500 Caracteres o estar vació")]
        public string? Descripcion { get; set; }

        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Solo se Permiten Numeros")]
        [Range(0, 7000, ErrorMessage = "La Imagen tiene un tamaño superior a  7000 Bytes")]
        public byte[]? Imagen { get; set; }

        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Solo se Permiten Numeros")]
        public BlazorApp.Shared.Proveedor? Proveedor { get; set; }

        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Solo se Permiten Numeros")]
        public BlazorApp.Shared.Departamento? Departamento { get; set; }
        public BlazorApp.Shared.Sucursal? Sucursal { get; set; }

        public int DeptoNumero { get; set; }
        public int ProveedorNumero { get; set; }

        //Para MVC
        public List<object>? Productos { get; set; }
    }
}
