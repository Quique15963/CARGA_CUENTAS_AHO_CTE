using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using Centinela.Common;
using Centinela.DataAccess;
using Centinela.Entities;
using Centinela.Entities.RegistroZConnect;
using Centinela.Logger;
using static Centinela.DataAccess.DataAccessServices;
using static Centinela.Entities.GenericResponse;

namespace Centinela.Business
{
    public class BusinessCentinela
    {
        private readonly Logs _logger;
        private readonly int _nroServicio;
        private readonly int _authorizationId_1; // Als cero
        private readonly int _authorizationId_2; // Als mora
        private readonly int _authorizationId_3; // Tc cero
        private readonly int _authorizationId_4; // Tc mora
        private readonly int _authorizationId_5; // Ajuste recupero

        private readonly string _canalCenCero;
        private readonly string _canalCenMora;
        private readonly string _canalCenAjuste;

        private readonly string _publicToken;
        private readonly string _appUserId;

        private readonly string _flagDebitoCta;
        private readonly DateTime _horaInicioCuenta;
        private readonly DateTime _horaFinCuenta;
        private readonly DateTime _horaInicioCuentaDelete;
        private readonly DateTime _horaFinCuentaDelete;
        private readonly decimal _montoMinimoCuenta;
        private readonly decimal _ITF;
        private decimal _tipoVenta;
        private decimal _tipoCompra;
        private DataAccessBizTalk _bisTalk;
        private DataAccesCentinela _conection;
        public BusinessCentinela()
        {
            _logger = Logs.Instance;
            this._authorizationId_1 = Convert.ToInt32(ManagerConfig.GetKeyConfigString("AUTHORIZATION_ID_1"));
            this._authorizationId_2 = Convert.ToInt32(ManagerConfig.GetKeyConfigString("AUTHORIZATION_ID_2"));
            this._authorizationId_3 = Convert.ToInt32(ManagerConfig.GetKeyConfigString("AUTHORIZATION_ID_3"));
            this._authorizationId_4 = Convert.ToInt32(ManagerConfig.GetKeyConfigString("AUTHORIZATION_ID_4"));
            this._authorizationId_5 = Convert.ToInt32(ManagerConfig.GetKeyConfigString("AUTHORIZATION_ID_5"));

            this._nroServicio = Convert.ToInt32(ManagerConfig.GetKeyConfigString("ROBOT"));
            this._flagDebitoCta = ManagerConfig.GetKeyConfigString("FLAG_DEBITO_CUENTA");
            this._canalCenCero = ManagerConfig.GetKeyConfigString("CANAL_CENCERO");
            this._canalCenMora = ManagerConfig.GetKeyConfigString("CANAL_CEONLINE");
            this._canalCenAjuste = ManagerConfig.GetKeyConfigString("CANAL_CEAJUSTE");

            this._publicToken = ManagerConfig.GetKeyConfigString("PUBLIC_TOKEN");
            this._appUserId = ManagerConfig.GetKeyConfigString("APP_USER_ID");

            this._horaInicioCuenta = Convert.ToDateTime(ManagerConfig.GetKeyConfigString("HORA_INICIO_PAGO_CUENTA"));
            this._horaFinCuenta = Convert.ToDateTime(ManagerConfig.GetKeyConfigString("HORA_FIN_PAGO_CUENTA"));
            this._horaInicioCuentaDelete = Convert.ToDateTime(ManagerConfig.GetKeyConfigString("HORA_INICIO_DELETE_DEBIT_TRIGGER"));
            this._horaFinCuentaDelete = Convert.ToDateTime(ManagerConfig.GetKeyConfigString("HORA_FIN_DELETE_DEBIT_TRIGGER"));

            this._montoMinimoCuenta = Convert.ToDecimal(ManagerConfig.GetKeyConfigString("MONTO_MINIMO_CUENTA"), CultureInfo.InvariantCulture);
            this._ITF = Convert.ToDecimal(ManagerConfig.GetKeyConfigString("ITF"), CultureInfo.InvariantCulture) + Convert.ToDecimal(0.0005, CultureInfo.InvariantCulture);
            this._bisTalk = new DataAccessBizTalk();
            this._conection = new DataAccesCentinela();
        }

        #region SECTION.00: ELIMINACION DE CEUNTAS X ZCONNECT

        public async Task ProcesoEliminarCuentasALS_CERO()
        {
            try
            {
                if ((DateTime.Now.TimeOfDay > this._horaInicioCuentaDelete.TimeOfDay) && (DateTime.Now.TimeOfDay <= this._horaFinCuentaDelete.TimeOfDay))
                {
                    _logger.Information("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                    _logger.Information("--------INICIO DEL PROCESO DE ELIMINACION DE CUENTAS POR CANAL CON DIA DE PAGO DIFERENTE AL DE HOY-------------");
                    _logger.Information("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");

                    await EliminarCuentasAsync(this._authorizationId_1, this._canalCenCero);
                    _conection.EliminarCuentasCentinelaCero(this._canalCenCero);

                    await EliminarCuentasAsync(this._authorizationId_2, this._canalCenMora);
                    await EliminarCuentasAsync(this._authorizationId_4, this._canalCenMora);

                    if (DateTime.Now.Day == 8)
                    {
                        await EliminarCuentasAsync(this._authorizationId_3, this._canalCenCero);
                    }

                    _logger.Information("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                    _logger.Information("----------FIN DEL PROCESO DE ELIMINACION DE CUENTAS DE CUENTAS CON DIA DE PAGO DIFERENTE AL DE HOY--------------");
                    _logger.Information("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error en el proceso de Eliminar las cuentas BIZTALK ZCONNECT O CENTINELA Als Cero" + ex);
                Logs.EnvioCorreo(new CorreoModel
                {
                    tipo = Tipo.Error,
                    proceso = "PROCESO ELIMINAR CUENTAS DE Biztalk ZCONNECT NOTIFICACIONES - GENERO ERROR EN EL PROCESO",
                    detalle = ex.Message
                });
            }
        }
        private async Task EliminarCuentasAsync(int authorizationId, string canal)
        {
            var resp = new EliminarCuentasDetalle
            {
                publicToken = _publicToken,
                appUserId = _appUserId,
                authorizationId = authorizationId,
                CreatedBy = canal
            };

            this._bisTalk.EliminarCuentasAlsBiztalk(resp);
        }

        #endregion

        #region SECTION.01: REGISTRO DE CEUNTAS X ZCONNECT
        public async Task ProcesoRegistroCuentas()
        {
            try
            {
                if ((DateTime.Now.TimeOfDay > this._horaInicioCuenta.TimeOfDay) && (DateTime.Now.TimeOfDay <= this._horaFinCuenta.TimeOfDay))
                {
                    _logger.Information("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                    _logger.Information("-----------------------------INICIO DEL PROCESO DE REGISTRO DE CUENTAS ZCONNECT--------------------------------");
                    _logger.Information("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");

                    var tasks = new List<Task>();

                    if (DateTime.Now.Day == 7)
                    {
                        tasks.Add(GetCuentasTC_Cero());
                    }

                    tasks.Add(GetCuentasALS_Cero());
                    tasks.Add(GetCuentasALS_Mora());
                    tasks.Add(GetCuentasTC_Mora());
                    tasks.Add(GetCuentasAjuste_Recupero());
                    tasks.Add(GetCuentasALS_TC_Mora_Codeudores());

                    await Task.WhenAll(tasks);

                    _logger.Information("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                    _logger.Information("-----------------------------FIN DEL PROCESO DE REGISTRO DE CUENTAS ZCONNECT-----------------------------------");
                    _logger.Information("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                }
            }
            catch (Exception ex)
            {
                Logs.EnvioCorreo(new CorreoModel
                {
                    tipo = Tipo.Error,
                    proceso = "PROCESO ELIMINAR CUENTAS DE DEBIT TRIGGER - ALS_CERO - CANAL : CENCERO",
                    detalle = ex.Message
                });
                _logger.Error("Error en el proceso de Eliminar las cuentas debit Trigger Als Cero" + ex);
            }
        }

        public async Task<RegistroCuentas> GetCuentasALS_Cero()
        {
            var _registroCuentas = new RegistroCuentas();

            try
            {
                DataTable ClientesALS0 = _conection.PendientesPagosALS_Cero(this._nroServicio);
                _registroCuentas.data = new List<RegistroCuentasDetalle>();
                _registroCuentas.totalPendientes = ClientesALS0.Rows.Count;

                string fechaFormateada = DateTime.Now.ToString("yyyyMMdd");

                foreach (DataRow item in ClientesALS0.Rows)
                {
                    var resp = new RegistroCuentasDetalle()
                    {
                        publicToken = this._publicToken,
                        appUserId = this._appUserId,
                        authorizationId = this._authorizationId_1, 
                        priority = 1,
                        accountNumber = item.Field<string>("CUENTA_COMERCIAL"),
                        cic = item.Field<string>("CIC"),
                        notificationType = "A", // Abono A - Debito D
                        dateStart = fechaFormateada,
                        dateEnd = fechaFormateada, 
                        createdBy = this._canalCenCero,
                    };
                    _registroCuentas.data.Add(resp);

                    var _als_saldo_response = this._bisTalk.RegistraCuentasAlsBiztalk(resp);
                    _logger.Information("ALS CERO - CIC: " + resp.cic + " NUMERO DE CUENTA: " + resp.accountNumber);

                }
            }
            catch (Exception ex)
            {
                Logs.EnvioCorreo(new CorreoModel
                {
                    tipo = Tipo.Error,
                    proceso = "CONSULTA PENDIENTES PAGO",
                    detalle = ex.Message
                });
                _logger.Error("Error Consulta Pendientes Pago" + ex);
            }
            return _registroCuentas;
        }

        public async Task<RegistroCuentas> GetCuentasALS_Mora()
        {
            var _registroCuentas = new RegistroCuentas();

            try
            {
                DataTable ClientesALS0 = _conection.PendientesPagosALS_Mora(this._nroServicio);
                _registroCuentas.data = new List<RegistroCuentasDetalle>();
                _registroCuentas.totalPendientes = ClientesALS0.Rows.Count;

                string fechaFormateada = DateTime.Now.ToString("yyyyMMdd");
                string fechaFormateadaEnd = DateTime.Now.AddYears(1).ToString("yyyyMMdd");

                foreach (DataRow item in ClientesALS0.Rows)
                {
                    var resp = new RegistroCuentasDetalle()
                    {
                        publicToken = this._publicToken,
                        appUserId = this._appUserId,
                        authorizationId = this._authorizationId_2,
                        priority = 1, 
                        accountNumber = item.Field<string>("CUENTA_COMERCIAL"),
                        cic = item.Field<string>("CIC"),
                        notificationType = "A", // Abono A - Debito D
                        dateStart = fechaFormateada,
                        dateEnd = fechaFormateadaEnd, 
                        createdBy = this._canalCenMora,
                    };

                    _registroCuentas.data.Add(resp);

                    var _als_saldo_response = this._bisTalk.RegistraCuentasAlsBiztalk(resp);
                    _logger.Information("ALS MORA - CIC: " + resp.cic + " NUMERO DE CUENTA: " + resp.accountNumber);

                }
            }
            catch (Exception ex)
            {
                Logs.EnvioCorreo(new CorreoModel
                {
                    tipo = Tipo.Error,
                    proceso = "CONSULTA PENDIENTES PAGO",
                    detalle = ex.Message
                });
                _logger.Error("Error Consulta Pendientes Pago" + ex);
            }
            return _registroCuentas;
        }
        public async Task<RegistroCuentas> GetCuentasTC_Cero()
        {
            var _registroCuentas = new RegistroCuentas();

            try
            {
                DataTable ClientesALS0 = _conection.PendientesPagosTC_Cero(this._nroServicio);
                _registroCuentas.data = new List<RegistroCuentasDetalle>();
                _registroCuentas.totalPendientes = ClientesALS0.Rows.Count;

                string fechaFormateada = DateTime.Now.ToString("yyyyMMdd");

                foreach (DataRow item in ClientesALS0.Rows)
                {
                    var resp = new RegistroCuentasDetalle()
                    {
                        publicToken = this._publicToken,
                        appUserId = this._appUserId,
                        authorizationId = this._authorizationId_3,
                        priority = 1,
                        accountNumber = item.Field<string>("CUENTA_COMERCIAL"),
                        cic = item.Field<string>("CIC"),
                        notificationType = "A", // Abono A - Debito D
                        dateStart = fechaFormateada,
                        dateEnd = fechaFormateada, 
                        createdBy = this._canalCenCero,

                    };

                    _registroCuentas.data.Add(resp);

                    var _als_saldo_response = this._bisTalk.RegistraCuentasAlsBiztalk(resp);
                    _logger.Information("TC CERO - CIC: " + resp.cic + " NUMERO DE CUENTA: " + resp.accountNumber);

                }
            }
            catch (Exception ex)
            {
                Logs.EnvioCorreo(new CorreoModel
                {
                    tipo = Tipo.Error,
                    proceso = "CONSULTA PENDIENTES PAGO",
                    detalle = ex.Message
                });
                _logger.Error("Error Consulta Pendientes Pago" + ex);
            }
            return _registroCuentas;
        }
        public async Task<RegistroCuentas> GetCuentasTC_Mora()
        {
            var _registroCuentas = new RegistroCuentas();

            try
            {
                DataTable ClientesALS0 = _conection.PendientesPagosTC_Mora(this._nroServicio);
                _registroCuentas.data = new List<RegistroCuentasDetalle>();
                _registroCuentas.totalPendientes = ClientesALS0.Rows.Count;

                string fechaFormateada = DateTime.Now.ToString("yyyyMMdd");
                string fechaFormateadaEnd = DateTime.Now.AddYears(1).ToString("yyyyMMdd");

                foreach (DataRow item in ClientesALS0.Rows)
                {
                    var resp = new RegistroCuentasDetalle()
                    {
                        publicToken = this._publicToken,
                        appUserId = this._appUserId,
                        authorizationId = this._authorizationId_4, 
                        priority = 1,
                        accountNumber = item.Field<string>("CUENTA_COMERCIAL"), 
                        cic = item.Field<string>("CIC"),
                        notificationType = "A", // Abono A - Debito D
                        dateStart = fechaFormateada,
                        dateEnd = fechaFormateadaEnd, 
                        createdBy = this._canalCenMora, 
                    };

                    _registroCuentas.data.Add(resp);

                    var _als_saldo_response = this._bisTalk.RegistraCuentasAlsBiztalk(resp);

                    _logger.Information("TC MORA - CIC: " + resp.cic + " NUMERO DE CUENTA: " + resp.accountNumber);

                }
            }
            catch (Exception ex)
            {
                Logs.EnvioCorreo(new CorreoModel
                {
                    tipo = Tipo.Error,
                    proceso = "CONSULTA PENDIENTES PAGO",
                    detalle = ex.Message
                });
                _logger.Error("Error Consulta Pendientes Pago" + ex);
            }
            return _registroCuentas;
        }

        public async Task<RegistroCuentas> GetCuentasAjuste_Recupero()
        {
            var _registroCuentas = new RegistroCuentas();

            try
            {
                DataTable ClientesALS0 = _conection.PendientesPagosAjuste_Recupero(this._nroServicio);
                _registroCuentas.data = new List<RegistroCuentasDetalle>();
                _registroCuentas.totalPendientes = ClientesALS0.Rows.Count;

                string fechaFormateada = DateTime.Now.ToString("yyyyMMdd");
                string fechaFormateadaEnd = DateTime.Now.AddYears(1).ToString("yyyyMMdd");

                foreach (DataRow item in ClientesALS0.Rows)
                {
                    var resp = new RegistroCuentasDetalle()
                    {
                        publicToken = this._publicToken,
                        appUserId = this._appUserId,
                        authorizationId = this._authorizationId_5,
                        priority = 1,
                        accountNumber = item.Field<string>("CUENTA"),
                        cic = item.Field<string>("CIC"),
                        notificationType = "A", // Abono A - Debito D
                        dateStart = fechaFormateada, 
                        dateEnd = fechaFormateadaEnd, 
                        createdBy = this._canalCenAjuste,
                    };

                    _registroCuentas.data.Add(resp);

                    var _als_saldo_response = this._bisTalk.RegistraCuentasAlsBiztalk(resp);
                    _logger.Information("AJUSTE RECUPERO - CIC: " + resp.cic + " NUMERO DE CUENTA: " + resp.accountNumber);

                }
            }
            catch (Exception ex)
            {
                Logs.EnvioCorreo(new CorreoModel
                {
                    tipo = Tipo.Error,
                    proceso = "CONSULTA PENDIENTES PAGO",
                    detalle = ex.Message
                });
                _logger.Error("Error Consulta Pendientes Pago" + ex);
            }
            return _registroCuentas;
        }
        public async Task<RegistroCuentas> GetCuentasALS_TC_Mora_Codeudores()
        {
            var _registroCuentas = new RegistroCuentas();

            try
            {
                DataTable ClientesALS0 = _conection.PendientesPagosALS_TC_Mora_Codeudor(this._nroServicio);
                _registroCuentas.data = new List<RegistroCuentasDetalle>();
                _registroCuentas.totalPendientes = ClientesALS0.Rows.Count;

                string fechaFormateada = DateTime.Now.ToString("yyyyMMdd");
                string fechaFormateadaEnd = DateTime.Now.AddYears(1).ToString("yyyyMMdd");

                foreach (DataRow item in ClientesALS0.Rows)
                {
                    var resp = new RegistroCuentasDetalle()
                    {
                        publicToken = this._publicToken,
                        appUserId = this._appUserId,
                        authorizationId = this._authorizationId_2,
                        priority = 1,
                        accountNumber = item.Field<string>("ACCOUNT_NUMBER"),
                        cic = item.Field<string>("CP_CIC"),
                        notificationType = "A", // Abono A - Debito D
                        dateStart = fechaFormateada,
                        dateEnd = fechaFormateadaEnd,
                        createdBy = this._canalCenMora,
                    };

                    _registroCuentas.data.Add(resp);

                    var _als_saldo_response = this._bisTalk.RegistraCuentasAlsBiztalk(resp);

                    _logger.Information("ALS MORA CODEUDORES - CIC: " + resp.cic + " NUMERO DE CUENTA: " + resp.accountNumber);
                }
            }
            catch (Exception ex)
            {
                Logs.EnvioCorreo(new CorreoModel
                {
                    tipo = Tipo.Error,
                    proceso = "CONSULTA PENDIENTES PAGO",
                    detalle = ex.Message
                });
                _logger.Error("Error Consulta Pendientes Pago" + ex);
            }
            return _registroCuentas;
        }

        #endregion


    }
}
