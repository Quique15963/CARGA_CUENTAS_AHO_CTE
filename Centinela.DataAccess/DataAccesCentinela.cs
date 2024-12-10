using System;
using System.Data;

namespace Centinela.DataAccess
{
    public class DataAccesCentinela
    {
        private readonly string strConexion_cen = DataAccess.Conexion("CENT");
        private readonly string strConexion_repxt = DataAccess.Conexion("REPEXT");
        private readonly string strConexion_trigger = DataAccess.Conexion("TRIGGER");

        #region SECTION.00: OBTENER CUENTAS X REGISTRADAS
        public DataTable PendientesPagosALS_Cero(int servicio)
        {
            DataTable DatosCentinela = new DataTable();

            try
            {
                string SP = "[cent].[ALS_SELECT_COBRO_DIA_0_GET]";

                StoreProcedure storeProcedure = new StoreProcedure(SP);
                storeProcedure.AgregarParametro("@SERVICIO", servicio, Direccion.Input);

                DatosCentinela = storeProcedure.RealizarConsulta(strConexion_cen);
                if (storeProcedure.Error != String.Empty)
                {
                    throw new Exception("Procedimiento Almacenado: " + SP + ", Descripcion:" + storeProcedure.Error.Trim());
                }
            }
            catch (Exception)
            {
                throw;
            }
            return DatosCentinela;
        }
        public DataTable PendientesPagosALS_Mora(int servicio)
        {
            DataTable DatosCentinela = new DataTable();

            try
            {
                string SP = "[cent].[ALS_SELECT_COBRO_DIA_MORA_GET]"; 

                StoreProcedure storeProcedure = new StoreProcedure(SP);
                storeProcedure.AgregarParametro("@SERVICIO", servicio, Direccion.Input);

                DatosCentinela = storeProcedure.RealizarConsulta(strConexion_cen);
                if (storeProcedure.Error != String.Empty)
                {
                    throw new Exception("Procedimiento Almacenado: " + SP + ", Descripcion:" + storeProcedure.Error.Trim());
                }
            }
            catch (Exception)
            {
                throw;
            }
            return DatosCentinela;
        }
        public DataTable PendientesPagosTC_Cero(int servicio)
        {
            DataTable DatosCentinela = new DataTable();

            try
            {
                string SP = "[cent].[TC_SELECT_COBRO_DIA_0_GET]";

                StoreProcedure storeProcedure = new StoreProcedure(SP);
                storeProcedure.AgregarParametro("@SERVICIO", servicio, Direccion.Input);

                DatosCentinela = storeProcedure.RealizarConsulta(strConexion_cen);
                if (storeProcedure.Error != String.Empty)
                {
                    throw new Exception("Procedimiento Almacenado: " + SP + ", Descripcion:" + storeProcedure.Error.Trim());
                }
            }
            catch (Exception)
            {
                throw;
            }
            return DatosCentinela;
        }

        public DataTable PendientesPagosTC_Mora(int servicio)
        {
            DataTable DatosCentinela = new DataTable();

            try
            {
                string SP = "[cent].[TC_SELECT_COBRO_DIA_MORA_GET]";

                StoreProcedure storeProcedure = new StoreProcedure(SP);
                storeProcedure.AgregarParametro("@SERVICIO", servicio, Direccion.Input);

                DatosCentinela = storeProcedure.RealizarConsulta(strConexion_cen);
                if (storeProcedure.Error != String.Empty)
                {
                    throw new Exception("Procedimiento Almacenado: " + SP + ", Descripcion:" + storeProcedure.Error.Trim());
                }
            }
            catch (Exception)
            {
                throw;
            }
            return DatosCentinela;
        }

        public DataTable PendientesPagosAjuste_Recupero(int servicio)
        {
            DataTable DatosCentinela = new DataTable();

            try
            {
                string SP = "[cent].[AJUSTE_SELECT_COBRO_DIA_0_GET]";

                StoreProcedure storeProcedure = new StoreProcedure(SP);
                storeProcedure.AgregarParametro("@SERVICIO", servicio, Direccion.Input);

                DatosCentinela = storeProcedure.RealizarConsulta(strConexion_cen);
                if (storeProcedure.Error != String.Empty)
                {
                    throw new Exception("Procedimiento Almacenado: " + SP + ", Descripcion:" + storeProcedure.Error.Trim());
                }
            }
            catch (Exception)
            {
                throw;
            }
            return DatosCentinela;

        }

        public DataTable PendientesPagosALS_TC_Mora_Codeudor(int servicio)
        {
            DataTable DatosCentinela = new DataTable();

            try
            {
                string SP = "[cent].[ALS_TC_SELECT_COBRO_DIA_MORA_CODEUDORES_GET]";

                StoreProcedure storeProcedure = new StoreProcedure(SP);
                storeProcedure.AgregarParametro("@SERVICIO", servicio, Direccion.Input);

                DatosCentinela = storeProcedure.RealizarConsulta(strConexion_cen);
                if (storeProcedure.Error != String.Empty)
                {
                    throw new Exception("Procedimiento Almacenado: " + SP + ", Descripcion:" + storeProcedure.Error.Trim());
                }
            }
            catch (Exception)
            {
                throw;
            }
            return DatosCentinela;

        }

        public bool EliminarCuentasCentinelaCero(string canal)
        {
            bool response = false;
            try
            {
                string SP = "[cent].[DELETE_ALS_TC_CERO_CUENTAS_GET_X_DELETE]";

                StoreProcedure storeProcedure = new StoreProcedure(SP);
                storeProcedure.AgregarParametro("@CHANNEL", canal, Direccion.Input);

                var estado = storeProcedure.EjecutarStoreProcedure(strConexion_cen);

                if (storeProcedure.Error != String.Empty)
                {
                    throw new Exception("Procedimiento Almacenado: " + SP + ", Descripcion:" + storeProcedure.Error.Trim());
                }

                response = estado;
            }
            catch (Exception ex)
            {
                throw new Exception("Delete Centinela Cuenta Error General: " + ex);
            }
            return response;
        }

        #endregion

    }
}
