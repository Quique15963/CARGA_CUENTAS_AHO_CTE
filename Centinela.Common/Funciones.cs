using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centinela.Common
{
    public class Funciones
    {
        #region "Manejo de Cadenas"
        public static double StringToDouble(string val, string mensaje)
        {
            double retval;
            if (val == "")
            {
                retval = 0;
            }
            else
            {
                try
                {
                    retval = Double.Parse(val);
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                    retval = -1;
                }
            }
            return retval;
        }
        public static bool IsDecimal(string valor)
        {
            try
            {
                Convert.ToDecimal(valor);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static int StringToInt(string val, string mensaje)
        {
            int retval;
            if (val == "")
            {
                retval = 0;
            }
            else
            {
                try
                {
                    retval = int.Parse(val);
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                    retval = 0;
                }
            }
            return retval;
        }
        public static char AsciiToChar(Byte codAscii)
        {
            Char[] chars;
            Char retval = ' ';
            Byte[] bytes = new Byte[] { codAscii };

            ASCIIEncoding ascii = new ASCIIEncoding();

            int charCount = ascii.GetCharCount(bytes, 1, 1);
            chars = new Char[charCount];
            int charsDecodedCount = ascii.GetChars(bytes, 1, 1, chars, 0);

            foreach (Char c in chars)
            {
                retval = c;
            }
            return retval;
        }

        public static string Left(string cadena, int numCarac)
        {
            string retVal = "";

            retVal = cadena.Substring(0, numCarac);

            return retVal;
        }
        public static string Right(string cadena, int longitud)
        {
            int tam = cadena.Length;
            int cont = tam - longitud;
            if (cont < 0)
                cont = tam - 1;
            string cadena2 = "";
            for (; cont < tam; cont++)
                cadena2 += cadena[cont];
            return cadena2;
        }
        public static string Mid(string cadena, int start, int length)
        {
            if (string.IsNullOrEmpty(cadena) || length < 1 || start > cadena.Length)
                return "";
            if (length > cadena.Length - start)
            {
                length = cadena.Length - start + 1;
            }
            return cadena.Substring(start - 1, length);
        }
        public static string DATETIME_TO_DATE(DateTime dt)
        {
            string retval = "";
            retval = dt.Year + String.Format("{0:00}", dt.Month) + String.Format("{0:00}", dt.Day);
            return retval;
        }

        public static string DATETIME_TO_TIME(DateTime dt)
        {
            string retval = "";
            retval = String.Format("{0:00}", dt.Hour) + String.Format("{0:00}", dt.Minute) + String.Format("{0:00}", dt.Second);
            return retval;
        }

        public static bool IsNumeric(object value)
        {
            bool result = false;

            try
            {
                int i = Convert.ToInt32(value);
                result = true;
            }
            catch
            {
                throw;
            }
            return result;
        }

        public static string f_Nulo_NBSP(string v_valor)
        {

            if (v_valor == "&nbsp;" || v_valor == "&NBSP;")
            {
                return "";
            }
            else
            {
                return v_valor;
            }
        }
        public static string InvertirCadena(string cadena)
        {
            string cadenainvertida = "";
            for (int cont = cadena.Length - 1; cont >= 0; cont--)
            {
                cadenainvertida += cadena[cont];
            }
            return cadenainvertida;
        }
        public static string EliminarCaracteresEspeciales(string cadena)
        {
            StringBuilder cadena2 = new StringBuilder();
            for (int cont = 0; cont < cadena.Length; cont++)
            {
                char c = cadena[cont];
                switch (c)
                {
                    case 'Ñ':
                        cadena2.Append('N');
                        break;
                    case 'ñ':
                        cadena2.Append('n');
                        break;
                    case 'Á':
                    case 'Ä':
                        cadena2.Append('A');
                        break;
                    case 'É':
                    case 'Ë':
                        cadena2.Append('E');
                        break;
                    case 'Í':
                    case 'Ï':
                        cadena2.Append('I');
                        break;
                    case 'Ó':
                    case 'Ö':
                        cadena2.Append('O');
                        break;
                    case 'Ú':
                    case 'Ü':
                        cadena2.Append('U');
                        break;
                    case 'á':
                    case 'ä':
                        cadena2.Append('a');
                        break;
                    case 'é':
                    case 'ë':
                        cadena2.Append('e');
                        break;
                    case 'í':
                    case 'ï':
                        cadena2.Append('i');
                        break;
                    case 'ó':
                    case 'ö':
                        cadena2.Append('o');
                        break;
                    case 'ú':
                    case 'ü':
                        cadena2.Append('u');
                        break;
                    default:
                        cadena2.Append(c);
                        break;
                }
            }
            return cadena2.ToString();
        }

        public static string EliminarCaracteres(string Cadena, params char[] Caracteres)
        {
            string Cadena2 = String.Empty;
            for (int indice = 0; indice < Cadena.Length; indice++)
            {
                bool Existe = false;
                foreach (char caracter in Caracteres)
                {
                    if (Cadena[indice].CompareTo(caracter) == 0)
                    {
                        Existe = true;
                        break;
                    }
                }
                if (!Existe)
                    Cadena2 += Cadena[indice];
            }
            return Cadena2;
        }
        public static string FormatoMoneda(string cadena)
        {
            string cadena2 = Funciones.InvertirCadena(cadena);
            string cadenaformato = "";
            string punto = string.Empty;
            for (int cont3 = 0; cont3 < cadena2.Length; cont3++)
            {
                if (cadena2[cont3] == '.')
                {
                    cadenaformato = punto + ".";
                    cadena2 = Right(cadena2, cadena2.Length - punto.Length - 1);
                    break;
                }
                else
                    punto += cadena2[cont3];
            }
            for (int cont = 0, cont2 = 0; cont < cadena2.Length; cont++, cont2++)
            {
                if (cont2 == 3)
                {
                    if (cont < cadena2.Length)
                    {
                        if (cadena2[cont] != '-')
                        {
                            cadenaformato += ",";
                            cont2 = 0;
                        }
                    }
                    else
                    {
                        cadenaformato += ",";
                        cont2 = 0;
                    }
                }
                cadenaformato += cadena2[cont];
            }
            return Funciones.InvertirCadena(cadenaformato);
        }
        public static string EliminarCaracteresEspecialesASCII(string cadena, params char[] CaracteresAceptados)
        {
            StringBuilder cadena2 = new StringBuilder();
            for (int cont = 0; cont < cadena.Length; cont++)
            {
                char c = cadena[cont];
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    cadena2.Append(c);
                }
                else
                {
                    switch (c)
                    {
                        case 'Ñ':
                            cadena2.Append('N');
                            break;
                        case 'ñ':
                            cadena2.Append('n');
                            break;
                        case 'Á':
                        case 'Ä':
                            cadena2.Append('A');
                            break;
                        case 'É':
                        case 'Ë':
                            cadena2.Append('E');
                            break;
                        case 'Í':
                        case 'Ï':
                            cadena2.Append('I');
                            break;
                        case 'Ó':
                        case 'Ö':
                            cadena2.Append('O');
                            break;
                        case 'Ú':
                        case 'Ü':
                            cadena2.Append('U');
                            break;
                        case 'á':
                        case 'ä':
                            cadena2.Append('a');
                            break;
                        case 'é':
                        case 'ë':
                            cadena2.Append('e');
                            break;
                        case 'í':
                        case 'ï':
                            cadena2.Append('i');
                            break;
                        case 'ó':
                        case 'ö':
                            cadena2.Append('o');
                            break;
                        case 'ú':
                        case 'ü':
                            cadena2.Append('u');
                            break;
                        default:
                            if (CaracteresAceptados.Contains(c))
                            {
                                cadena2.Append(c);
                            }
                            break;
                    }
                }
            }
            return cadena2.ToString();
        }

        public static bool EsNumeroEntero(string num)
        {
            try
            {
                Convert.ToUInt32(num);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string FiltrarSoloNumeros(string cadena)
        {
            int tam = cadena.Length;
            string cadena2 = "";
            int cont;
            for (cont = 0; cont < tam; cont++)
            {
                if (Convert.ToInt32(cadena[cont]) >= 48 && Convert.ToInt32(cadena[cont]) <= 57)
                {
                    cadena2 = cadena2 + cadena[cont];
                }
            }
            return cadena2;
        }

        #endregion
        #region FormatoCuenta
        public static string FormatoCuenta(string cuenta)
        {
            string DG = Digitocontrol(cuenta);
            string CuentaF = "";
            if (cuenta.Substring(11, 1) == "0")
                CuentaF += cuenta.Substring(5, 3) + Right(cuenta, 7) + cuenta.Substring(4, 1) + DG;
            else
                CuentaF += cuenta.Substring(5, 3) + Right(cuenta, 8) + cuenta.Substring(4, 1) + DG;
            return CuentaF;
        }
        /// <summary>
        /// Metodo que permite, obtener el digito de control de una cuenta
        /// </summary>
        /// <param name="cuenta">Numero de cuenta en formato Comercial</param>
        /// <returns></returns>
        public static string Digitocontrol(string cuenta)
        {
            string c2;
            //Basado en el SP de digto de control
            string suc = cuenta.Substring(5, 3);
            string valor = "8";
            if (suc == "101")
            {
                suc = "000";
                valor = "0";
            }
            //13 caracteres cta cte/ 14 caracteres ahorro
            if (cuenta.Substring(11, 1) == "0")
                c2 = suc + cuenta.Substring(4, 1) + "0" + Right(cuenta, 7) + "0";
            else
                c2 = suc + valor + "0" + Right(cuenta, 8);
            int acu = 0;
            string des;
            while (c2.Length > 0)
            {
                des = Right(c2, 2);
                c2 = c2.Substring(0, c2.Length - des.Length);
                acu = acu + Convert.ToInt32(des);
            }
            string dig = Right(Convert.ToString(acu), 2);
            return dig;
        }
        /// <summary>
        /// Funcion para convertir una cuenta en formato Comercial a Formato RepExt
        /// </summary>         
        /// <param name="cuenta">Cuenta en Formato Comercial</param>
        /// <returns> Cuenta en Formato RepExt</returns>
        public static string FormatoCuentaRepExt(string cuenta)
        {
            cuenta = cuenta.Trim().Replace("-", "");
            string CuentaF = "1030";
            if (cuenta.Length == 14 || cuenta.Length == 12)
                CuentaF += cuenta.Substring(11, 1) + cuenta.Substring(0, 3) + "0001000000" + cuenta.Substring(3, 8);
            else if (cuenta.Length == 13 || cuenta.Length == 11)
                CuentaF += cuenta.Substring(10, 1) + cuenta.Substring(0, 3) + "00000000000" + cuenta.Substring(3, 7);
            return CuentaF;
        }
        /// <summary>
        /// Funcion que convierte de formato RepExt a formato para envio a servicios canales
        /// </summary>
        /// <param name="credit">Credito ALS en formato RepExt</param>
        /// <returns></returns>
        public static string FormatoCreditoSercan(string credito)
        {
            credito = credito.Trim();
            if (credito.Length == 26)
                return credito.Substring(2, 3) + "-" + credito.Substring(5, 3) + "-" + credito.Substring(18, 8);
            else
                throw new Exception("El credito no tiene la longitud de 26 caracteres");
        }
        #endregion
    }
}
