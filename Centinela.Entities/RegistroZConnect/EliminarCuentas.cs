using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centinela.Entities.RegistroZConnect
{
    public class EliminarCuentas
    {
        public int totalPendientes { get; set; }
        public List<EliminarCuentasDetalle> data { get; set; }
    }
    public class EliminarCuentasDetalle
    {
        public string publicToken  { get; set; } = string.Empty;
        public string appUserId { get; set; } = string.Empty;
        public int authorizationId { get; set; }
        public string accountNumber { get; set; } = string.Empty;
        public string cic { get; set; } = string.Empty;
        //
        public decimal priority { get; set; }
        public string notificationType { get; set; } = string.Empty;
        public string dateStart { get; set; } = string.Empty;
        public string dateEnd { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;

    }
}
