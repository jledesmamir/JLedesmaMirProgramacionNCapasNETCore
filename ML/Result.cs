using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    [Serializable]
    public class Result
    {
        //clase para Registrar / Analizar información de try catch
        public bool Correct { get; set; } // sí insertó o no
        public string? ErrorMessage { get; set; }  //mensaje de error
        public object? Object { get; set; } //Get ById por almacenar solo un objeto.
        public List<object>? Objects { get; set; } //Lista de Objetos para captrurar los registros.
        public Exception? Ex { get; set; }
    }
}
