using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Centinela.Business;
using Centinela.Entities;
using Centinela.Logger;
using System.Configuration;

using System.Timers;

namespace CentinelaCargaCuentas
{
    public partial class CentinelaCargaAjuste : ServiceBase
    {
        private System.Timers.Timer Reloj;
        private readonly Logs _logger;
        public CentinelaCargaAjuste()
        {
            _logger = Logs.Instance;
            InitializeComponent();
            try
            {
#if DEBUG
                Proceso();
#endif
                this.Reloj = new System.Timers.Timer();
                this.Reloj.Elapsed += new ElapsedEventHandler(this.RelojElapsed);
                this.Reloj.Enabled = true;
                this.Reloj.Interval = Convert.ToDouble(ConfigurationSettings.AppSettings["INTERVALO_EJECUCION"]) * 60 * 1000;

            }
            catch (Exception ex)
            {
                Logs.EnvioCorreo(new CorreoModel
                {
                    tipo = Tipo.Error,
                    proceso = "Error general Centinela",
                    detalle = ex.Message
                });
                _logger.Error("Se produjo un error general: " + ex);

            }
        }

        protected override void OnStart(string[] args)
        {
            _logger.Information("**********************SE INICIO EL SERVICIO CENTINELA 0**************************");
        }

        protected override void OnStop()
        {
            _logger.Information("**********************SE APAGO EL SERVICIO CENTINELA 0**************************");
        }
        public void RelojElapsed(System.Object sender, System.Timers.ElapsedEventArgs e)
        {

            lock (this)
            {
                Proceso();
            }

        }
        private void Proceso()
        {
            try
            {
                lock (this)
                {
                    BusinessCentinela bc = new BusinessCentinela();
                    bc.ProcesoEliminarCuentasALS_CERO();
                    bc.ProcesoRegistroCuentas();


                }
            }
            catch (Exception ex)
            {
                _logger.Error("Se produjo un error general en el PROCESO: " + ex);
            }
        }
    }
}
