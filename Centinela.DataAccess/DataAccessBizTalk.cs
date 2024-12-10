using Centinela.Entities.ConsultaAls;
using Centinela.Entities;
using Centinela.Logger;
using Centinela.Security;
using System;
using static Centinela.DataAccess.DataAccessServices;
using Centinela.Common;

using Newtonsoft.Json;
using Centinela.Entities.RegistroZConnect;
using System.Security.Cryptography;

namespace Centinela.DataAccess
{
    public class DataAccessBizTalk
    {
        private readonly Logs _logger;
        private readonly string _UrlRegAlsBizTalk;
        private readonly string _UrlDelAlsBizTalk;
        private readonly string _UrlDelCanalAlsBizTalk;
        private readonly string _PublicTokenBiztalk;
        private readonly string _AppUserIdBiztalk;
        private readonly string _ChannelBiztalk;

        private readonly string _UrlSmartLink;
        private readonly string _UserBiztalk;
        private readonly string _PassBiztalk;
        private readonly string _Server;


        public DataAccessBizTalk()
        {
            _logger = Logs.Instance;
            this._UrlRegAlsBizTalk = ManagerConfig.GetKeyConfigString("URL_REG_CTA_BIZTALK");
            this._UrlDelAlsBizTalk = ManagerConfig.GetKeyConfigString("URL_DEL_ALS_BIZTALK");
            this._UrlDelCanalAlsBizTalk = ManagerConfig.GetKeyConfigString("URL_DEL_CAN_ALS_BIZTALK");
            this._UrlSmartLink = ManagerConfig.GetKeyConfigString("URL_SMART_LINK");
            this._UserBiztalk = ManagerConfig.GetKeyConfigString("CANAL_BIZTALK");
            this._Server = ManagerConfig.GetKeyConfigString("CENT_SERVER");
            string Pass = ManagerConfig.GetKeyConfigString("PASSWORD_BIZTALK");
            Pass = SegCrypt.EncryptDecrypt(false, Pass);
            string Password = ManagerConfig.GetKeyConfigString("PASSWORD_BIZTALK_API");
            this._PassBiztalk = Password;
            this._PublicTokenBiztalk = ManagerConfig.GetKeyConfigString("PUBLIC_TOKEN");
            this._AppUserIdBiztalk = ManagerConfig.GetKeyConfigString("APP_USER_ID");
            this._ChannelBiztalk = ManagerConfig.GetKeyConfigString("CHANNEL_BIZTALK");

        }

        
        public ConsultaAlsResponse EliminarCuentasAlsBiztalk(EliminarCuentasDetalle _requestCuenta)
        {
            SaldoAlsBizTalkResponse _responseBiztalk = new SaldoAlsBizTalkResponse();
            ConsultaAlsResponse response = new ConsultaAlsResponse();

            try
            {
                AuthenticationConfiguration _authentication = new AuthenticationConfiguration()
                {
                    userAuthentication = this._UserBiztalk,
                    passwordAuthentication = this._PassBiztalk,
                    publicTokenAuthentication = this._PublicTokenBiztalk,
                    appUserIdAuthentication = this._AppUserIdBiztalk,
                    channelAuthentication = this._ChannelBiztalk
                };

                _logger.Information("[" + _requestCuenta.cic + " - " + _requestCuenta.accountNumber + "], URL: " + _UrlDelCanalAlsBizTalk);
                _logger.Information("[" + _requestCuenta.cic + " - " + _requestCuenta.accountNumber + "], REQUEST: " + JsonConvert.SerializeObject(_requestCuenta));

                _responseBiztalk = DataAccessServices.PostAuto<SaldoAlsBizTalkResponse>(this._UrlDelCanalAlsBizTalk, "", _requestCuenta, Authentication.Complete, _authentication).Result;

                _logger.Information("[" + _requestCuenta.cic + " - " + _requestCuenta.accountNumber + "], RESPONSE: " + JsonConvert.SerializeObject(_responseBiztalk));

                response.success = _responseBiztalk.state == "00" ? true : false;
                response.message = _responseBiztalk.message;
                response.code = _responseBiztalk.state;

                if (response.success)
                {
                    response.nroCredito = _responseBiztalk.data.accountNotificationId;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                _logger.Error("[" + _requestCuenta.cic + " - " + _requestCuenta.accountNumber + "]" + ex);
            }

            return response;
        }

        public ConsultaAlsResponse RegistraCuentasAlsBiztalk(RegistroCuentasDetalle _requestCuenta)
        {
            SaldoAlsBizTalkResponse _responseBiztalk = new SaldoAlsBizTalkResponse();
            ConsultaAlsResponse response = new ConsultaAlsResponse();

            try
            {
                AuthenticationConfiguration _authentication = new AuthenticationConfiguration()
                {
                    userAuthentication = this._UserBiztalk,
                    passwordAuthentication = this._PassBiztalk,
                    publicTokenAuthentication = this._PublicTokenBiztalk,
                    appUserIdAuthentication = this._AppUserIdBiztalk,
                    channelAuthentication = this._ChannelBiztalk
                };

                _logger.Information("[" + _requestCuenta.cic + " - " + _requestCuenta.accountNumber + "], URL: " + _UrlRegAlsBizTalk);
                _logger.Information("[" + _requestCuenta.cic + " - " + _requestCuenta.accountNumber + "], REQUEST: " + JsonConvert.SerializeObject(_requestCuenta));

                _responseBiztalk = DataAccessServices.PostAuto<SaldoAlsBizTalkResponse>(this._UrlRegAlsBizTalk, "", _requestCuenta, Authentication.Complete, _authentication).Result;

                _logger.Information("[" + _requestCuenta.cic + " - " + _requestCuenta.accountNumber + "], RESPONSE: " + JsonConvert.SerializeObject(_responseBiztalk));

                response.success = _responseBiztalk.state == "00" ? true : false; 
                response.message = _responseBiztalk.message;
                response.code = _responseBiztalk.state;

                if (response.success)
                {
                    response.accountNotificationId = _responseBiztalk.data.accountNotificationId;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                _logger.Error("[" + _requestCuenta.cic + " - " + _requestCuenta.accountNumber + "]" + ex);
            }

            return response;
        }



    }
}
