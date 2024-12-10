using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Centinela.Entities;
using System.IO;
using Serilog;
using System.Reflection;
using System.Configuration;

namespace Centinela.Logger
{
    public class Logs
    {
        private static readonly Lazy<Logs> _instance = new Lazy<Logs>(() => new Logs());
        public static Logs Instance => _instance.Value;

        private static string correoA = ConfigurationManager.AppSettings["CORREO_OPERACIONES"];
        private static string correoAplicativo = ConfigurationManager.AppSettings["CORREO_APLICATIVO"];
        private static string ServidorCorreo = ConfigurationManager.AppSettings["SERVIDOR_SMTP"];
        private static string asunto = ConfigurationManager.AppSettings["ASUNTO"];
        private static string Cuerpo = ConfigurationManager.AppSettings["CUERPOCORREO"];
        private static string servicio = ConfigurationManager.AppSettings["SERVICIO"];
        private static string robot = ConfigurationManager.AppSettings["ROBOT"];
        private static string limit = ConfigurationManager.AppSettings["LIMIT_LOG"];
        private static string size = ConfigurationManager.AppSettings["SIZE_LOG"];

        private static string _pathLogFile = ConfigurationManager.AppSettings["Path_Log_File"];
        private static string _fileSizeLimit = ConfigurationManager.AppSettings["FileSizeLimitBytes"];
        private static string _structure = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] <{ThreadId}> {Message:lj}{NewLine}{Exception}";
        private static string _limit = ConfigurationManager.AppSettings["Limit"];
        private static string _level = ConfigurationManager.AppSettings["Level"];
        public const string header = "INFO: {0} DETALLE: {1}";

        #region 00: CONFIGURACION DE LOGS

        public Logs()
        {
            var pathLogFile = _pathLogFile;
            var level = _level;
            var fileSizeLimit = _fileSizeLimit != null ? long.Parse(_fileSizeLimit) : (long?)null;
            var logStructure = _structure;
            var limit = _limit != null ? int.Parse(_limit) : (int?)null;

            switch (level)
            {
                case "INFORMATION":
                    Log.Logger = new LoggerConfiguration()
                        .Enrich.With(new ThreadIdEnricher())
                        .WriteTo.File(_pathLogFile, rollingInterval: RollingInterval.Day, outputTemplate: _structure, rollOnFileSizeLimit: true, fileSizeLimitBytes: fileSizeLimit, retainedFileCountLimit: limit)
                        .WriteTo.Console()
                        .MinimumLevel.Information()
                        .CreateLogger();
                    break;
                case "FATAL":
                    Log.Logger = new LoggerConfiguration()
                        .Enrich.With(new ThreadIdEnricher())
                        .WriteTo.File(_pathLogFile, rollingInterval: RollingInterval.Day, outputTemplate: _structure, rollOnFileSizeLimit: true, fileSizeLimitBytes: fileSizeLimit, retainedFileCountLimit: limit)
                        .WriteTo.Console()
                        .MinimumLevel.Fatal()
                        .CreateLogger();
                    break;
                case "WARNING":
                    Log.Logger = new LoggerConfiguration()
                        .Enrich.With(new ThreadIdEnricher())
                        .WriteTo.File(_pathLogFile, rollingInterval: RollingInterval.Day, outputTemplate: _structure, rollOnFileSizeLimit: true, fileSizeLimitBytes: fileSizeLimit, retainedFileCountLimit: limit)
                        .WriteTo.Console()
                        .MinimumLevel.Warning()
                        .CreateLogger();
                    break;
                case "ERROR":
                    Log.Logger = new LoggerConfiguration()
                        .Enrich.With(new ThreadIdEnricher())
                        .WriteTo.File(_pathLogFile, rollingInterval: RollingInterval.Day, outputTemplate: _structure, rollOnFileSizeLimit: true, fileSizeLimitBytes: fileSizeLimit, retainedFileCountLimit: limit)
                        .WriteTo.Console()
                        .MinimumLevel.Error()
                        .CreateLogger();
                    break;
                case "DEBUG":
                    Log.Logger = new LoggerConfiguration()
                        .Enrich.With(new ThreadIdEnricher())
                        .WriteTo.File(_pathLogFile, rollingInterval: RollingInterval.Day, outputTemplate: _structure, rollOnFileSizeLimit: true, fileSizeLimitBytes: fileSizeLimit, retainedFileCountLimit: limit)
                        .WriteTo.Console()
                        .MinimumLevel.Debug()
                        .CreateLogger();
                    break;
                default:
                    Log.Logger = new LoggerConfiguration()
                        .Enrich.With(new ThreadIdEnricher())
                        .WriteTo.File(_pathLogFile, rollingInterval: RollingInterval.Day, outputTemplate: _structure, rollOnFileSizeLimit: true, fileSizeLimitBytes: fileSizeLimit, retainedFileCountLimit: limit)
                        .WriteTo.Console()
                        .MinimumLevel.Verbose()
                        .CreateLogger();
                    break;
            }
        }
        private static string GetStackTraceInfo()
        {
            var stackFrame = new StackTrace().GetFrame(2);
            string methodName = stackFrame.GetMethod().Name;
            string className = stackFrame.GetMethod().ReflectedType.FullName;
            return $"Class: \"{className}\" Method: \"{methodName}\"";
        }
        public void Information(string format, params object[] objects)
        {
            string location = GetStackTraceInfo();
            string message = string.Format(format, objects);
            Log.Information(string.Format(header, location, message));
        }
        public void Information(string message)
        {
            string location = GetStackTraceInfo();
            Log.Information(string.Format(header, location, message));
        }
        public void Fatal(string format, params object[] objects)
        {
            string message = string.Format(format, objects);
            string location = GetStackTraceInfo();
            Log.Fatal(string.Format(header, location, message));
        }
        public void Fatal(string message)
        {
            string location = GetStackTraceInfo();
            Log.Fatal(string.Format(header, location, message));
        }
        public void Warning(string format, params object[] objects)
        {
            string message = string.Format(format, objects);
            string location = GetStackTraceInfo();
            Log.Warning(string.Format(header, location, message));
        }
        public void Warning(string message)
        {
            string location = GetStackTraceInfo();
            Log.Warning(string.Format(header, location, message));
        }
        public void Error(string format, params object[] objects)
        {
            string message = string.Format(format, objects);
            string location = GetStackTraceInfo();
            Log.Error(string.Format(header, location, message));
        }
        public void Error(string message)
        {
            string location = GetStackTraceInfo();
            Log.Error(string.Format(header, location, message));
        }
        public void Debug(string format, params object[] objects)
        {
            string location = GetStackTraceInfo();
            string message = string.Format(format, objects);
            Log.Debug(string.Format(header, location, message));
        }
        public void Debug(string message)
        {
            string location = GetStackTraceInfo();
            Log.Debug(string.Format(header, location, message));
        }
        public void Verbose(string format, params object[] objects)
        {
            string message = string.Format(format, objects);
            string location = GetStackTraceInfo();
            Log.Verbose(string.Format(header, location, message));
        }
        public void Verbose(string message)
        {
            string location = GetStackTraceInfo();
            Log.Verbose(string.Format(header, location, message));
        }

        #endregion
        #region 01: CONFIGURACION DE ENVIO DE CORREO ELECTRONICOS
        public static void RegistroErrorLogEmail(string ErrorDetalle, Exception error)
        {
            Logs.EnviarmailsGenerico(ErrorDetalle + " " + error.Message.ToString(), correoA);
            var logger = Logs.Instance;
            logger.Error($"Error: {ErrorDetalle}" + error);
        }

        public static void EnviarmailsGenerico(string cuerpodetalle, string servicioCorreoPara)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(string.Format(Logs.correoAplicativo, Logs.servicio));
            message.To.Add(servicioCorreoPara);
            string _correo = string.Format(Logs.asunto, Logs.servicio);
            if (Logs.robot != "0")
                _correo += "_" + Logs.robot;
            message.Subject = _correo;
            message.Body = Logs.Cuerpo + ": " + cuerpodetalle;
            message.IsBodyHtml = false;
            message.Priority = MailPriority.Normal;
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = Logs.ServidorCorreo;
            smtpClient.Port = 25;
            try
            {
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                var logger = Logs.Instance;
                logger.Error($"Error en el envio de notificación al mail: " + servicioCorreoPara + ex);
            }
        }
        public static void EnviarmailsGenerico(string servicioCorreoPara, CorreoModel correo)
        {
            if (!(string.IsNullOrEmpty(servicioCorreoPara) || servicioCorreoPara == "_"))
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(string.Format(Logs.correoAplicativo, Logs.servicio));
                message.To.Add(servicioCorreoPara);
                string _correo = string.Format(Logs.asunto, Logs.servicio);
                if (Logs.robot != "0")
                    _correo += "_" + Logs.robot;
                message.Subject = _correo;
                message.IsBodyHtml = true;
                string body = "<table border='1'><tr><td><b>SERVICIO:</b></td><td>#SERVICIO#</td></tr><tr><td><b>TIPO:</b></td><td>#TIPO#</td></tr><tr><td><b>PROCESO:</b></td><td>#PROCESO#</td></tr><tr><td valign='top'><b>DETALLE:</b></td><td>#DETALLE#</td></tr><tr><td><b>EQUIPO:</b></td><td>#EQUIPO#</td></tr><tr><td><b>RUTA LOGS:</b></td><td>#LOGS#</td></tr></table>";
                body = body.Replace("#SERVICIO#", ConfigurationManager.AppSettings["SERVICIO"]);
                body = body.Replace("#TIPO#", correo.tipo.ToString().ToUpper());
                body = body.Replace("#PROCESO#", correo.proceso);
                body = body.Replace("#DETALLE#", correo.detalle);
                body = body.Replace("#EQUIPO#", Environment.MachineName.ToString());
                body = body.Replace("#LOGS#", $"{Logs.GetRutaLog()}\\{DateTime.Now.ToString("yyyy-MM-dd")}");
                message.Body = body;
                message.Priority = MailPriority.Normal;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = Logs.ServidorCorreo;
                smtpClient.Port = 25;
                try
                {
                    smtpClient.Send(message);
                }
                catch (Exception ex)
                {
                    var logger = Logs.Instance;
                    logger.Error($"Error en el envio de notificación al mail: " + servicioCorreoPara + "Exception: " + ex);
                }
            }
        }

        public static void EnvioCorreo(CorreoModel correo)
        {
            Logs.EnviarmailsGenerico(correoA, correo);
            if (correo.exception == null)
            {
                var logger = Logs.Instance;
                logger.Warning($"Advertencia: " + correo.detalle);
            }
            else
            {
                var logger = Logs.Instance;
                logger.Error($"Error: " + correo.detalle + correo.exception);
            }
        }

        public static string GetRutaLog()
        {
            string rutaCarpeta = ConfigurationManager.AppSettings["RUTA_LOG"].ToString();
            if (Logs.robot != "0")
                rutaCarpeta += "_" + Logs.robot;
            return rutaCarpeta;
        }
        #endregion

    }
}
