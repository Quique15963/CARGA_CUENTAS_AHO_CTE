using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centinela.Entities.ConsultaAls
{
    public class SaldoAlsBizTalk
    {

    }
    public class SaldoAlsBizTalkRequest
    {
        public string creditNumber { get; set; }
        public string channelDate { get; set; }
        public string channelHour { get; set; }
        public string employeeFlag { get; set; }
        public string correlationId { get; set; }
    }

    public class SaldoAlsBizTalkResponse
    {
        public string state { get; set; }
        public string message { get; set; }
        public SaldoAlsBizTalkData data { get; set; }

    }

    public class SaldoAlsBizTalkData
    {
        public string accountNotificationId { get; set; }
    }

    public class ConsultaAlsResponse : GenericResponse
    {
        public string nroCredito { get; set; }
        public string monedaDeuda { get; set; }
        public decimal montoDeuda { get; set; }
        public string fechaProximoPago { get; set; }
        public string nroCtaAfiliada { get; set; }
        public string hostOperationNumber { get; set; }
        public string correlationId { get; set; }
        public string accountNotificationId { get; set; }

    }
}
