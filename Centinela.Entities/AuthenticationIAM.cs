using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centinela.Entities
{
    public class AuthenticationConfiguration
    {
        public string userAuthentication { get; set; } = string.Empty;
        public string passwordAuthentication { get; set; } = string.Empty;
        public string publicTokenAuthentication { get; set; } = string.Empty;
        public string channelAuthentication { get; set; } = string.Empty;
        public string appUserIdAuthentication { get; set; } = string.Empty;
        public string ipAuthentication { get; set; } = string.Empty;
        public string server { get; set; } = string.Empty;
    }

}
