using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centinela.Entities.RegistroZConnect
{
    public class RegistroCuentas
    {
        public int totalPendientes { get; set; }
        public List<RegistroCuentasDetalle> data { get; set; }
    }
    public class RegistroCuentasDetalle
    {
        public string publicToken { get; set; }
        public string appUserId { get; set; }
        public int authorizationId { get; set; }
        public int priority { get; set; }
        public string accountNumber { get; set; } = string.Empty;
        public string cic { get; set; } = string.Empty;
        public string notificationType { get; set; } = string.Empty;
        public string dateStart { get; set; } = string.Empty;
        public string dateEnd { get; set; } = string.Empty;
        public string createdBy { get; set; } = string.Empty;


    }
}
