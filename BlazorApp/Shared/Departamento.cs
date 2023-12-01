using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared
{
    [Serializable]
    public class Departamento
    {
        public int IdDepartamento { get; set; } 
        public string Nombre { get; set; }

        //Referencia de Navegacion
        public BlazorApp.Shared.Area Area { get; set; }

        public List<object> Departamentos { get; set; }
    }
}
