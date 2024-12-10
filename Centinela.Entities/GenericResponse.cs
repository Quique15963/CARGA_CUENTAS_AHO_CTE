using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centinela.Entities
{
    public class GenericResponse : IDisposable
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string code { get; set; }
        public string codextorno { get; set; }

        public GenericResponse()
        {
            this.code = Centinela.Common.ManagerCode.GetCode("ERROR_FATAL");
            this.message = Centinela.Common.ManagerMessage.GetMessage("ERROR_FATAL");
            this.success = false;
        }
        void IDisposable.Dispose() { }
        public class ResponseService
        {
            public string message { get; set; } = string.Empty;
            public int code { get; set; }
            public bool state { get; set; }
        }
    }
}
