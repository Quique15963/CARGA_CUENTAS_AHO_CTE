using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Centinela.Common
{
    public static class ManagerConfig
    {
        public static string GetKeyConfigString(string strKeyNameIN)
        {
            string resultado = string.Empty;
            try
            {
                resultado = ConfigurationManager.AppSettings.Get(strKeyNameIN);
            }
            catch (Exception)
            {
                throw;
            }
            return resultado;
        }

        public static int GetKeyConfigInt(string strKeyNameIN)
        {
            int resultado = 0;
            try
            {
                resultado = Convert.ToInt32(ConfigurationManager.AppSettings.Get(strKeyNameIN).ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return resultado;
        }


    }
}
