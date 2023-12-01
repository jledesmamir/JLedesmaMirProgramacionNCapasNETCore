using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    [Serializable]
    public class Departamento
    {
        public int IdDepartamento { get; set; } 
        public string Nombre { get; set; }

        //Referencia de Navegacion
        public ML.Area Area { get; set; }

        public List<object> Departamentos { get; set; }
    }
}
