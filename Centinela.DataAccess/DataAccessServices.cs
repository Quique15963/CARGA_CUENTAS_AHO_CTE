using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Centinela.Common;
using System.IO;
using System.Runtime.Serialization.Json;
using RestSharp;
using Newtonsoft.Json;
using Centinela.Entities;
using RestSharp.Authenticators;

namespace Centinela.DataAccess
{
    public static class DataAccessServices
    {
        public enum Authentication
        {
            None,
            Header,
            Basic,
            Complete,
            BizTalk
        }

        public static async Task<T> PostAuto<T>(string _urlBase, string _method, object _request, Authentication _authentication, AuthenticationConfiguration _configautentication)
        {
            try
            {
                HttpClientHandler httpClientHandler = new HttpClientHandler();
                httpClientHandler.SslProtocols = SslProtocols.Tls12;
                
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    httpClient.Timeout = TimeSpan.FromMinutes(5);
                    switch (_authentication)
                    {
                        case Authentication.None:
                            break;
                        case Authentication.Header:
                            httpClient.DefaultRequestHeaders.Add("USUARIO", _configautentication.userAuthentication);
                            httpClient.DefaultRequestHeaders.Add("PASSWORD", _configautentication.passwordAuthentication);
                            break;
                        case Authentication.Basic:
                            string _encodeBasic = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(_configautentication.userAuthentication + ":" + _configautentication.passwordAuthentication));
                            httpClient.DefaultRequestHeaders.Add("Correlation-Id", ""); //Ajustar El nro de Operacion
                            httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {_encodeBasic}");
                            break;
                        case Authentication.Complete:
                            string _encodeComplete = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(_configautentication.userAuthentication + ":" + _configautentication.passwordAuthentication));
                            httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {_encodeComplete}");
                            httpClient.DefaultRequestHeaders.Add("USUARIO", _configautentication.userAuthentication);
                            httpClient.DefaultRequestHeaders.Add("PASSWORD", _configautentication.passwordAuthentication);
                            httpClient.DefaultRequestHeaders.Add("Channel", _configautentication.channelAuthentication);
                            httpClient.DefaultRequestHeaders.Add("PublicToken", _configautentication.publicTokenAuthentication);
                            httpClient.DefaultRequestHeaders.Add("AppUserId", _configautentication.appUserIdAuthentication);
                            break;
                        default:
                            break;
                    }
                    var _json = JsonConvert.SerializeObject(_request);
                    var _content = new StringContent(_json, Encoding.UTF8, "application/json");
                    var _result = await httpClient.PostAsync(new Uri(_urlBase + _method), _content);
                    var _resultContent = await _result.Content.ReadAsStringAsync();
                    HttpStatusCode _statusCode = _result.StatusCode;
                    if (_statusCode == HttpStatusCode.OK)
                    {
                        var _responseService = _resultContent == null ? "" : _resultContent;
                        return JsonConvert.DeserializeObject<T>(_responseService);
                    }
                    else
                    {
                        throw new Exception($"ERROR => URL METHOD: {_urlBase + _method} ; STATUS: {_statusCode} ; CONTENT: {_resultContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"General Error: {ex.Message}");
            }
        }
    }
}
