using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centinela.Entities
{
    public enum Tipo
    {
        Informativo,
        Error
    }
    public class CorreoModel
    {
        public string proceso { get; set; }
        public string detalle { get; set; }
        public Tipo tipo { get; set; }
        public Exception exception { get; set; }
    }
}
