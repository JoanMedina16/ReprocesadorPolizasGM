using INFO.Objetos.SAT;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;
using INFO.Enums;
using System.Drawing;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace PD.Herramientas
{
    public class LOGI_Tools_PD
    {

        public void LogError(Exception oEx, string sMetodo, string sUsuarioID, string sClass, string sModulo, string sDatosAdicionales = "", string sEmpresa = "")
        {
            try
            {
                XmlDocument oDoc = new XmlDocument();
                string sPathBase = string.Format(@"{0}\Errores", AppDomain.CurrentDomain.BaseDirectory);
                if (!string.IsNullOrEmpty(sEmpresa))
                    sPathBase = string.Format(@"{0}\{1}", sPathBase, sEmpresa);
                string sFilePath = string.Format(@"{0}\{1}{2}.xml", sPathBase, DateTime.Now.ToString("ddMMyyyy"), DateTime.Now.Hour);
                if (!Directory.Exists(sPathBase))
                    Directory.CreateDirectory(sPathBase);
                if (!File.Exists(sFilePath))
                {
                    StreamWriter strwr = null;
                    strwr = File.CreateText(sFilePath);
                    strwr.WriteLine("<?xml version='1.0' encoding='UTF-8'?>");
                    strwr.WriteLine("<errores>");
                    strwr.WriteLine("</errores>");
                    strwr.Close();
                }
                oDoc.Load(sFilePath);

                XmlElement Usernodo = oDoc.CreateElement("usuario_id");
                XmlElement Metodonodo = oDoc.CreateElement("metodo");
                XmlElement Clasenodo = oDoc.CreateElement("clase");
                XmlElement ModuloNodo = oDoc.CreateElement("modulo");
                XmlElement Descripcionnodo = oDoc.CreateElement("descripcion");
                XmlElement Fechanodo = oDoc.CreateElement("fecha");
                XmlElement Otrosnodo = oDoc.CreateElement("otros_datos");

                Usernodo.InnerText = sUsuarioID;
                Metodonodo.InnerText = sMetodo;
                Clasenodo.InnerText = sClass;
                ModuloNodo.InnerText = sModulo;
                Fechanodo.InnerText = Convert.ToString(DateTime.Now);
                Descripcionnodo.InnerText = string.Format("Message: {0}. StackTrace: {1}. InnerException {2}. Source: {3}. Data: {4}. TargetSite: {5}", oEx.Message, oEx.StackTrace, oEx.InnerException, oEx.Source, oEx.Data, oEx.TargetSite);
                if (!string.IsNullOrEmpty(sDatosAdicionales))
                {
                    sDatosAdicionales = sDatosAdicionales.Replace("&", "&amp;");
                    sDatosAdicionales = sDatosAdicionales.Replace("\"", "&quot;");
                    sDatosAdicionales = sDatosAdicionales.Replace("<", "&alt;");
                    sDatosAdicionales = sDatosAdicionales.Replace(">", "&gt;");
                }
                Otrosnodo.InnerText = sDatosAdicionales;
                XmlNode oNodo = oDoc.DocumentElement;
                oNodo = oDoc.CreateElement("error");
                oNodo.AppendChild(Usernodo);
                oNodo.AppendChild(Metodonodo);
                oNodo.AppendChild(Clasenodo);
                oNodo.AppendChild(ModuloNodo);
                oNodo.AppendChild(Fechanodo);
                oNodo.AppendChild(Descripcionnodo);
                oNodo.AppendChild(Otrosnodo);
                oDoc.DocumentElement.AppendChild(oNodo);
                oDoc.Save(sFilePath);
            }
            catch { }
        }
        public void LogProceso(string sDescripcion, string sMetodo, string sClass, string sModulo, string sUsuarioID, string sDatosAdicionales = "", string sEmpresa = "")
        {
            try
            {
                XmlDocument oDoc = new XmlDocument();
                string sPathBase = string.Format(@"{0}\Procesos", AppDomain.CurrentDomain.BaseDirectory);
                if (!string.IsNullOrEmpty(sEmpresa))
                    sPathBase = string.Format(@"{0}\{1}", sPathBase, sEmpresa);
                string sFilePath = string.Format(@"{0}\{1}{2}.xml", sPathBase, DateTime.Now.ToString("ddMMyyyy"), DateTime.Now.Hour);
                if (!Directory.Exists(sPathBase))
                    Directory.CreateDirectory(sPathBase);
                if (!File.Exists(sFilePath))
                {
                    StreamWriter strwr = null;
                    strwr = File.CreateText(sFilePath);
                    strwr.WriteLine("<?xml version='1.0' encoding='UTF-8'?>");
                    strwr.WriteLine("<procesos>");
                    strwr.WriteLine("</procesos>");
                    strwr.Close();
                }
                oDoc.Load(sFilePath);

                XmlElement Usernodo = oDoc.CreateElement("usuario_id");
                XmlElement Metodonodo = oDoc.CreateElement("metodo");
                XmlElement Clasenodo = oDoc.CreateElement("clase");
                XmlElement ModuloNodo = oDoc.CreateElement("modulo");
                XmlElement Descripcionnodo = oDoc.CreateElement("descripcion");
                XmlElement Fechanodo = oDoc.CreateElement("fecha");
                XmlElement Otrosnodo = oDoc.CreateElement("otros_datos");

                Usernodo.InnerText = sUsuarioID;
                Metodonodo.InnerText = sMetodo;
                Clasenodo.InnerText = sClass;
                ModuloNodo.InnerText = sModulo;
                Fechanodo.InnerText = Convert.ToString(DateTime.Now);
                Descripcionnodo.InnerText = sDescripcion;
                if (!string.IsNullOrEmpty(sDatosAdicionales))
                {
                    sDatosAdicionales = sDatosAdicionales.Replace("&", "&amp;");
                    sDatosAdicionales = sDatosAdicionales.Replace("\"", "&quot;");
                    sDatosAdicionales = sDatosAdicionales.Replace("<", "&alt;");
                    sDatosAdicionales = sDatosAdicionales.Replace(">", "&gt;");
                }
                Otrosnodo.InnerText = sDatosAdicionales;
                XmlNode oNodo = oDoc.DocumentElement;
                oNodo = oDoc.CreateElement("proceso");
                oNodo.AppendChild(Usernodo);
                oNodo.AppendChild(Metodonodo);
                oNodo.AppendChild(Clasenodo);
                oNodo.AppendChild(ModuloNodo);
                oNodo.AppendChild(Fechanodo);
                oNodo.AppendChild(Descripcionnodo);
                oNodo.AppendChild(Otrosnodo);
                oDoc.DocumentElement.AppendChild(oNodo);
                oDoc.Save(sFilePath);
            }
            catch { }
        }

        public string GetEnumDefaultValue(Enum oEnum)
        {
            string sDefault = string.Empty;
            try
            {
                FieldInfo oField = oEnum.GetType().GetField(oEnum.ToString());
                DefaultValueAttribute[] oAtrr = (DefaultValueAttribute[])oField.GetCustomAttributes(typeof(DefaultValueAttribute), false);
                if (oAtrr != null && oAtrr.Length > 0)
                    sDefault = oAtrr[0].Value.ToString();
                else
                    sDefault = oEnum.ToString();
            }
            catch { }

            return sDefault;
        }


        public static bool CertificateValidationCallBack(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
        System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
                return true;
            if ((sslPolicyErrors & System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                if (chain != null && chain.ChainStatus != null)
                {
                    foreach (System.Security.Cryptography.X509Certificates.X509ChainStatus status in chain.ChainStatus)
                    {
                        if ((certificate.Subject == certificate.Issuer) &&
                           (status.Status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.UntrustedRoot))
                            continue;
                        else
                            if (status.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
                            return false;
                    }
                }
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Descripción: Metodo que devuelve en milisegundos la pausa que tomará cada hilo de 
        /// ejecución según la parametrización asignada a cada proceso
        /// 
        /// Fecha: 28/05/2021 
        /// </summary>
        /// <param name="iDias"></param>
        /// <param name="iHoras"></param>
        /// <param name="iMinutos"></param>
        /// <param name="iSegundos"></param>
        /// <returns></returns>
        public Int32 GetTiempoSleepSegmento(int iDias, int iHoras, int iMinutos, int iSegundos)
        {
            Int32 iSleep = -1;
            try
            {
                Int32 _iDias = (iDias * 24) * 60 * 60 * 1000;
                Int32 _iHoras = iHoras * 60 * 60 * 1000;
                Int32 _iMinutos = iMinutos * 60000;
                Int32 _iSegundos = iSegundos * 1000;
                iSleep = _iDias + _iHoras + _iMinutos + _iSegundos;
            }
            catch
            {
                iSleep = 60000 * 45;
            }

            return iSleep;
        }

        public bool DevuelveXMLObject(out string sresponse, ref LOGI_XMLS_INFO oFormato, string sPathXML = "", string sContentXML = "", bool bClearXML = false, string sEmisor = "", Decimal total = 0, Decimal IVA = 0, Decimal subtotal = 0)
        {
            bool bContinuar = false, bValidado = false;
            string rHex = string.Empty;
            sresponse = string.Empty;
            oFormato = new LOGI_XMLS_INFO();
            oFormato.Receptor = new PersonaCFDI();
            oFormato.Emisor = new PersonaCFDI();
            oFormato.Complemento = new ComplementoCFDI();
            oFormato.Conceptos = new List<LOGI_ConceptosCFDI_INFO>();
            oFormato.Impuestos = new ImpuestosCFDI();
            oFormato.Impuestos.Traslados = new List<TrasladoCFDI>();
            oFormato.Impuestos.Retenciones = new List<RetencionCFDI>();
            LOGI_ConceptosCFDI_INFO oConcepto;

            if (!string.IsNullOrEmpty(sPathXML))
            {
                //validamos que el directorio exista
                if (!File.Exists(sPathXML))
                    sresponse = string.Format("El archivo {0} no se ha encontrado o la ruta no es accesible para la aplicación.", sPathXML);
                else bValidado = true;
            }
            if (!string.IsNullOrEmpty(sContentXML))
                bValidado = true;

            if (bValidado)
            {
                try
                {
                    XmlReader xmlRead = null;

                    if (!string.IsNullOrEmpty(sPathXML))
                        xmlRead = new XmlTextReader(sPathXML);
                    if (!string.IsNullOrEmpty(sContentXML))
                    {
                        //sContentXML = sContentXML.Replace("&quot;", "\"");
                        sContentXML = sContentXML.Replace("908123", "98123");
                        StringReader sURI = new StringReader(sContentXML);
                        xmlRead = XmlReader.Create(sURI);
                    }
                    if (bClearXML)
                    {
                        rHex = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
                        sContentXML = File.ReadAllText(sPathXML);
                        sContentXML = Regex.Replace(sContentXML, rHex, string.Empty, RegexOptions.Compiled);
                        sContentXML = sContentXML.Replace("o;?", string.Empty);
                        sContentXML = sContentXML.Replace("Ã¡", "á").Replace("Ã©", "é").Replace("Ã­", "í").Replace("Ã³", "ó").Replace("Ãº", "ú");
                        StringReader sURI = new StringReader(sContentXML);
                        xmlRead = XmlReader.Create(sURI);
                    }

            
                    

                    while (xmlRead.Read())
                    {
                        if (xmlRead.Name.Equals("cfdi:Comprobante", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (xmlRead.HasAttributes)
                            {
                                while (xmlRead.MoveToNextAttribute())
                                {
                                    if (xmlRead.Name.Equals("Certificado", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Certificado = xmlRead.Value;
                                    if (xmlRead.Name.Equals("Fecha", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Fecha = xmlRead.Value;

                                    if (xmlRead.Name.Equals("Descuento", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Descuento = Convert.ToDecimal(xmlRead.Value);

                                    if (xmlRead.Name.Equals("Folio", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Folio = xmlRead.Value;
                                    if (xmlRead.Name.Equals("FormaPago", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.FormaPago = Convert.ToString(xmlRead.Value);
                                    if (xmlRead.Name.Equals("CondicionesdePago", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.CondicionesdePago = Convert.ToString(xmlRead.Value);
                                    if (xmlRead.Name.Equals("LugarExpedicion", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.LugarExpedicion = xmlRead.Value;
                                    if (xmlRead.Name.Equals("MetodoPago", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.MetodoPago = Convert.ToString(xmlRead.Value);
                                    if (xmlRead.Name.Equals("Moneda", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Moneda = Convert.ToString(xmlRead.Value);
                                    if (xmlRead.Name.Equals("NoCertificado", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.NoCertificado = xmlRead.Value;
                                    if (xmlRead.Name.Equals("Sello", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Sello = xmlRead.Value;
                                    if (xmlRead.Name.Equals("Serie", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Serie = xmlRead.Value;
                                    if (xmlRead.Name.Equals("SubTotal", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.SubTotal = Convert.ToDecimal(xmlRead.Value);
                                    if (xmlRead.Name.Equals("TipoDeComprobante", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.TipoDeComprobante = Convert.ToString(xmlRead.Value);
                                    if (xmlRead.Name.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Total = Convert.ToDecimal(xmlRead.Value);
                                    if (xmlRead.Name.Equals("Version", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Version = xmlRead.Value;
                                }
                            }
                        }
                        if (xmlRead.Name.Equals("cfdi:Emisor", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (xmlRead.HasAttributes)
                            {
                                while (xmlRead.MoveToNextAttribute())
                                {
                                    if (xmlRead.Name.Equals("Nombre", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Emisor.Nombre = xmlRead.Value;
                                    if (xmlRead.Name.Equals("RegimenFiscal", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Emisor.RegimenFiscal = Convert.ToString(xmlRead.Value);
                                    if (xmlRead.Name.Equals("Rfc", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Emisor.Rfc = xmlRead.Value;
                                }
                            }
                        }
                        if (xmlRead.Name.Equals("cfdi:Receptor", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (xmlRead.HasAttributes)
                            {
                                while (xmlRead.MoveToNextAttribute())
                                {
                                    if (xmlRead.Name.Equals("Nombre", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Receptor.Nombre = xmlRead.Value;
                                    if (xmlRead.Name.Equals("Rfc", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Receptor.Rfc = xmlRead.Value;
                                    if (xmlRead.Name.Equals("UsoCFDI", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Receptor.UsoCFDI = Convert.ToString(xmlRead.Value);
                                }
                            }
                        }



                        if (xmlRead.Name.Equals("cfdi:Concepto", StringComparison.InvariantCultureIgnoreCase))
                        {
                            oConcepto = new LOGI_ConceptosCFDI_INFO();
                            oConcepto.Impuestos = new List<ImpuestoConceptoCFDI>();
                            if (xmlRead.HasAttributes)
                            {
                                while (xmlRead.MoveToNextAttribute())
                                {
                                    if (xmlRead.Name.Equals("Cantidad", StringComparison.InvariantCultureIgnoreCase))
                                        oConcepto.Cantidad = Convert.ToDecimal(xmlRead.Value);

                                    if (xmlRead.Name.Equals("ClaveProdServ", StringComparison.InvariantCultureIgnoreCase))
                                        oConcepto.ClaveProdServ = xmlRead.Value;

                                    if (xmlRead.Name.Equals("ClaveUnidad", StringComparison.InvariantCultureIgnoreCase))
                                        oConcepto.ClaveUnidad = xmlRead.Value;

                                    if (xmlRead.Name.Equals("Descripcion", StringComparison.InvariantCultureIgnoreCase))
                                        oConcepto.Descripcion = xmlRead.Value;

                                    if (xmlRead.Name.Equals("Importe", StringComparison.InvariantCultureIgnoreCase))
                                        oConcepto.Importe = Convert.ToDecimal(xmlRead.Value);

                                    if (xmlRead.Name.Equals("ValorUnitario", StringComparison.InvariantCultureIgnoreCase))
                                        oConcepto.ValorUnitario = Convert.ToDecimal(xmlRead.Value);

                                    if (xmlRead.Name.Equals("NoIdentificacion", StringComparison.InvariantCultureIgnoreCase))
                                        oConcepto.NoIdentificacion = xmlRead.Value;
                                }

                                XmlReader xmlTagImpuesto = xmlRead;
                                xmlTagImpuesto.MoveToElement();
                                xmlTagImpuesto.Read();
                                if (xmlTagImpuesto.Name.Equals("cfdi:Impuestos", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    xmlTagImpuesto.MoveToElement();
                                    xmlTagImpuesto.Read();
                                    if (xmlTagImpuesto.Name.Equals("cfdi:Traslados", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        xmlTagImpuesto.MoveToElement();
                                        xmlTagImpuesto.Read();
                                        if (xmlTagImpuesto.Name.Equals("cfdi:Traslado", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            ImpuestoConceptoCFDI oImpuesto = new ImpuestoConceptoCFDI();
                                            if (xmlTagImpuesto.HasAttributes)
                                            {
                                                while (xmlTagImpuesto.MoveToNextAttribute())
                                                {
                                                    if (xmlTagImpuesto.Name.Equals("Base", StringComparison.InvariantCultureIgnoreCase))
                                                        oImpuesto.Base = Convert.ToDecimal(xmlTagImpuesto.Value);

                                                    if (xmlTagImpuesto.Name.Equals("Importe", StringComparison.InvariantCultureIgnoreCase))
                                                        oImpuesto.Importe = Convert.ToDecimal(xmlTagImpuesto.Value);

                                                    if (xmlTagImpuesto.Name.Equals("Impuesto", StringComparison.InvariantCultureIgnoreCase))
                                                        oImpuesto.Impuesto = Convert.ToString(xmlTagImpuesto.Value);

                                                    if (xmlTagImpuesto.Name.Equals("TasaOCuota", StringComparison.InvariantCultureIgnoreCase))
                                                        oImpuesto.TasaOCuota = Convert.ToDecimal(xmlTagImpuesto.Value);

                                                    if (xmlTagImpuesto.Name.Equals("TipoFactor", StringComparison.InvariantCultureIgnoreCase))
                                                        oImpuesto.TipoFactor = Convert.ToString(xmlTagImpuesto.Value);
                                                }

                                            }
                                            oConcepto.Impuestos.Add(oImpuesto);
                                        }
                                    }
                                }

                                oFormato.Conceptos.Add(oConcepto);
                            }
                        }

                        if (xmlRead.Name.Equals("cfdi:Impuestos", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (xmlRead.HasAttributes)
                            {
                                while (xmlRead.MoveToNextAttribute())
                                {
                                    if (xmlRead.Name.Equals("TotalImpuestosTrasladados", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Impuestos.TotalImpuestosTrasladados = Convert.ToDecimal(xmlRead.Value);
                                }
                            }

                        }

                        if (xmlRead.Name.Equals("cfdi:Traslados", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (xmlRead.Name.Equals("cfdi:Traslado", StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (xmlRead.HasAttributes)
                                {
                                    TrasladoCFDI oTraslado = new TrasladoCFDI();
                                    while (xmlRead.MoveToNextAttribute())
                                    {
                                        if (xmlRead.Name.Equals("Importe", StringComparison.InvariantCultureIgnoreCase))
                                            oTraslado.Importe = Convert.ToDecimal(xmlRead.Value);

                                        if (xmlRead.Name.Equals("Impuesto", StringComparison.InvariantCultureIgnoreCase))
                                            oTraslado.Impuesto = Convert.ToString(xmlRead.Value);

                                        if (xmlRead.Name.Equals("TasaOCuota", StringComparison.InvariantCultureIgnoreCase))
                                            oTraslado.TasaOCuota = Convert.ToDecimal(xmlRead.Value);

                                        if (xmlRead.Name.Equals("TipoFactor", StringComparison.InvariantCultureIgnoreCase))
                                            oTraslado.TipoFactor = Convert.ToString(xmlRead.Value);


                                    }
                                    if (oTraslado.Impuesto != null)
                                        oFormato.Impuestos.Traslados.Add(oTraslado);
                                }
                            }
                        }
                        if (xmlRead.Name.Equals("tfd:TimbreFiscalDigital", StringComparison.InvariantCultureIgnoreCase))
                        {
                            oFormato.Complemento.TimbreFiscalDigital = new TimbreFiscalCFDI();
                            if (xmlRead.HasAttributes)
                            {
                                while (xmlRead.MoveToNextAttribute())
                                {
                                    if (xmlRead.Name.Equals("uuid", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Complemento.TimbreFiscalDigital.UUID = xmlRead.Value;
                                    if (xmlRead.Name.Equals("noCertificadoSAT", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Complemento.TimbreFiscalDigital.NoCertificadoSAT = xmlRead.Value;
                                    if (xmlRead.Name.Equals("selloCFD", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Complemento.TimbreFiscalDigital.SelloCFD = xmlRead.Value;
                                    if (xmlRead.Name.Equals("selloSAT", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Complemento.TimbreFiscalDigital.SelloSAT = xmlRead.Value;
                                    if (xmlRead.Name.Equals("fechaTimbrado", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Complemento.TimbreFiscalDigital.FechaTimbrado = xmlRead.Value;
                                    if (xmlRead.Name.Equals("version", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Complemento.TimbreFiscalDigital.Version = xmlRead.Value;


                                }

                            }
                        }

                        if (xmlRead.Name.Equals("ecc12:EstadoDeCuentaCombustible", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (xmlRead.HasAttributes)
                            {
                                while (xmlRead.MoveToNextAttribute())
                                {
                                    if (xmlRead.Name.Equals("SubTotal", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.SubTotal = Convert.ToDecimal(xmlRead.Value);
                                    if (xmlRead.Name.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Total = Convert.ToDecimal(xmlRead.Value);
                                }
                            }
                        }

                        if (xmlRead.Name.Equals("edr:Traslado", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (xmlRead.HasAttributes)
                            {
                                while (xmlRead.MoveToNextAttribute())
                                {
                                    if (xmlRead.Name.Equals("Importe", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Impuestos.TotalImpuestosTrasladados = Convert.ToDecimal(xmlRead.Value);

                                    if (xmlRead.Name.Equals("Base", StringComparison.InvariantCultureIgnoreCase))
                                        oFormato.Subtotalbase = Convert.ToDecimal(xmlRead.Value);
                                }
                            }
                        }
                    }
                   

                        if (!string.IsNullOrEmpty(sEmisor))
                    {
                        if (!oFormato.Emisor.Rfc.Trim().Equals(sEmisor.Trim(), StringComparison.InvariantCultureIgnoreCase))
                            throw new Exception("El XML no es proporcionado por el proveedor seleccionado");
                    }
                    if (subtotal > 0)
                    {
                        if (!(ValidaDiff(subtotal, oFormato.SubTotal)))
                            throw new Exception("El importe del subtotal no cuadra con el del XML");
                    }

                    if (IVA > 0)
                    {
                        if (!(ValidaDiff(IVA, oFormato.Impuestos.TotalImpuestosTrasladados)))
                            throw new Exception("El importe del IVA no cuadra con el del XML");
                    }

                    if (total > 0)
                    {
                        if (!(ValidaDiff(total, oFormato.Total)))
                            throw new Exception("El importe del total no cuadra con el del XML");
                    }


                    rHex = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
                    oFormato.CFDIContent = !string.IsNullOrEmpty(sPathXML) ? File.ReadAllText(sPathXML) : sContentXML;
                    //oFormato.CFDIContent = File.ReadAllText(sPathXML);
                    oFormato.CFDIContent = Regex.Replace(oFormato.CFDIContent, rHex, string.Empty, RegexOptions.Compiled);
                    oFormato.CFDIContent = oFormato.CFDIContent.Replace("o;?", string.Empty);
                    //oFormato.CFDIContent = oFormato.CFDIContent.Replace("ï¿½", "í");
                    
                    oFormato.CFDIContent = oFormato.CFDIContent.Replace("vehï¿½culo", "vehículo");
                    oFormato.CFDIContent = oFormato.CFDIContent.Replace("Informaciï¿½n", "Información");
                    oFormato.CFDIContent = oFormato.CFDIContent.Replace("Ã¡", "á").Replace("Ã©", "é").Replace("Ã­", "í").Replace("Ã³", "ó").Replace("Ãº", "ú");
                    oFormato.CFDIContent = oFormato.CFDIContent.Replace("lt;br /gt;lt;br /gt;", "&lt;br /&gt;&lt;br /&gt;").Replace("quot;", "&quot;");
                    oFormato.CFDIContent = oFormato.CFDIContent.Replace("#xA;", " ");
                    oFormato.CFDIContent = oFormato.CFDIContent.Replace("LMA040227500", "LMA0402275Q6");
                    //oFormato.CFDIContent = oFormato.CFDIContent.Replace("\\\"", "\"");
                    //oFormato.CFDIContent = oFormato.CFDIContent.Replace("0x005C", string.Empty);
                    oFormato.CFDIContent = oFormato.CFDIContent.Replace("\r\n", " ");
                    oFormato.CFDIContent = oFormato.CFDIContent.Replace("\n", " ");
                    oFormato.CFDIContent = oFormato.CFDIContent.Replace("\t", " ");
                    //oFormato.CFDIContent = Regex.Replace(oFormato.CFDIContent, @"\\+", "");

                    xmlRead.Close();
                    bContinuar = true;
                }
                catch (Exception ex)
                {
                    bContinuar = false;
                    sresponse = string.Format("No se ha podido leer el archivo XML. ERROR {0}", ex.Message);
                }
            }
            return bContinuar;
        }

        public void m_ConsoleLine(RichTextBox rchConsole, string sCadena, eType oType, bool bColor = true)
        {
            try
            {

                Color oTextColor = Color.DarkRed;
                switch (oType)
                {
                    case eType.error:
                        oTextColor = Color.OrangeRed;
                        break;
                    case eType.warning:
                        oTextColor = Color.HotPink;
                        break;
                    case eType.proceso:
                        oTextColor = Color.CadetBlue;
                        break;
                    case eType.success:
                        oTextColor = Color.DarkGreen;
                        break;
                }
                if (!bColor)
                    oTextColor = Color.Black;

                if (rchConsole.InvokeRequired)
                {
                    rchConsole.BeginInvoke(new Action(delegate {
                        m_ConsoleLine(rchConsole, sCadena, oType, bColor);
                    }));
                    return;
                }
                string sDateTime = DateTime.Now.ToString("hh:mm:ss tt");
                string sFullLine = string.Format("{0} - {1}", sDateTime, sCadena);
                rchConsole.SelectionStart = rchConsole.Text.Length;
                rchConsole.SelectionColor = oTextColor;
                if (rchConsole.TextLength + sFullLine.Length > rchConsole.MaxLength)
                    m_LimpiaConsole(rchConsole);

                if (rchConsole.Lines.Length == 0)
                {
                    rchConsole.AppendText(sFullLine);
                    rchConsole.ScrollToCaret();
                    rchConsole.AppendText(Environment.NewLine);
                }
                else
                {

                    rchConsole.AppendText(sFullLine + Environment.NewLine);
                    rchConsole.ScrollToCaret();
                }

            }
            catch { }
        }

        public void m_LimpiaConsole(RichTextBox rchConsole)
        {
            try
            {
                if (rchConsole.InvokeRequired)
                {
                    rchConsole.BeginInvoke(new Action(delegate {
                        m_LimpiaConsole(rchConsole);
                    }));
                    return;
                }
                rchConsole.ResetText();
            }
            catch { }
        }
        

        public BasicHttpBinding crearBasicHttpBinding(string _nombre)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Name = _nombre;
            binding.CloseTimeout = System.TimeSpan.Parse("00:05:00");
            binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            binding.MaxBufferPoolSize = 31457280011;
            binding.MaxBufferSize = 1048576999;
            binding.MaxReceivedMessageSize = 1048576999;
            binding.OpenTimeout = System.TimeSpan.Parse("00:05:00");
            binding.ReceiveTimeout = System.TimeSpan.Parse("00:10:00");
            binding.SendTimeout = System.TimeSpan.Parse("00:05:00");
            binding.TransferMode = TransferMode.Buffered;
            binding.ReaderQuotas.MaxArrayLength = 524288999;
            binding.ReaderQuotas.MaxBytesPerRead = 1048576009;
            binding.ReaderQuotas.MaxDepth = 1024;
            binding.ReaderQuotas.MaxNameTableCharCount = 1638400099;
            binding.ReaderQuotas.MaxStringContentLength =  1048576009;
            binding.MaxBufferPoolSize = 31457280099;
            binding.MaxReceivedMessageSize= 1048576999;
            binding.Security.Mode = BasicHttpSecurityMode.Transport;
            binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

            //CustomBinding binding;
           /* TextMessageEncodingBindingElement messageEncodingBiding;
            HttpsTransportBindingElement transportingBinding;
            BindingElement[] bindingElementArray;

            messageEncodingBiding = new TextMessageEncodingBindingElement(MessageVersion.Soap12, Encoding.UTF8);
            messageEncodingBiding.ReaderQuotas.MaxStringContentLength = 1048576009;
            messageEncodingBiding.ReaderQuotas.MaxArrayLength = 1048576009;

            transportingBinding = new HttpsTransportBindingElement();
            transportingBinding.MaxBufferPoolSize = 1048576009;
            transportingBinding.MaxReceivedMessageSize = 1048576009;

            //Llenar solo si requiere certificado
            //transportingBinding.RequireClientCertificate = true;

            bindingElementArray = new BindingElement[2];
            bindingElementArray[0] = messageEncodingBiding;
            bindingElementArray[1] = transportingBinding;

            binding = new CustomBinding(bindingElementArray);
            binding.SendTimeout = TimeSpan.MaxValue;*/

            return binding;

        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public String Base64ToText(String sBase64)
        {
            try
            {

                string cleanBase64 = CleanBase64String(sBase64);

                // 2. Decodificar y limpiar caracteres nulos
                byte[] data2 = Convert.FromBase64String(cleanBase64);

                string xmlContent = CleanXmlString(data2);
                File.WriteAllText("cfdi.xml", xmlContent, Encoding.UTF8);
              //  Console.WriteLine("XML guardado exitosamente");

                return xmlContent;
                //originales
                //byte[] data = Convert.FromBase64String(sBase64);
                //return  System.Text.Encoding.UTF8.GetString(data);

            }
            catch {
                return "";
            }
        }
        static string CleanBase64String(string base64)
        {
            // Eliminar todos los caracteres no base64
            StringBuilder clean = new StringBuilder();
            foreach (char c in base64)
            {
                if (char.IsLetterOrDigit(c) || c == '+' || c == '/' || c == '=')
                {
                    clean.Append(c);
                }
            }

            return clean.ToString();
        }

        static string CleanXmlString(byte[] data)
        {
            try
            {
                // Convertir bytes a string UTF-8, reemplazando caracteres inválidos
                string xml = Encoding.UTF8.GetString(data);

                // Eliminar BOM si existe
                xml = xml.TrimStart('\uFEFF');

                // Encontrar inicio real del XML
                int xmlStart = xml.IndexOf("<?xml");
                if (xmlStart > 0)
                {
                    xml = xml.Substring(xmlStart);
                }

                // Eliminar caracteres de control inválidos para XML (incluyendo 0x1C)
                xml = RemoveInvalidXmlChars(xml);

                // Reemplazar secuencias específicas problemáticas
                xml = xml.Replace("Ã\\x1C", "Ú")
                         .Replace("Ãa\\x0001", "Ú")
                         .Replace("Á\\x0001", "Ú");

                // Aplicar otros reemplazos del diccionario
                foreach (var replacement in SpecialCharReplacements)
                {
                    if (xml.Contains(replacement.Key))
                    {
                        xml = xml.Replace(replacement.Key, replacement.Value);
                    }
                }
                

                return xml;
            }
            catch (Exception ex)
            {
                // Fallback: intentar conversión más básica
                return Encoding.UTF8.GetString(data.Where(b => b != 0x00 && b != 0x1C).ToArray());
            }
        }
        static string RemoveInvalidXmlChars(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Patrón Regex para eliminar caracteres de control no permitidos en XML
            // Excepto: \t (0x09), \n (0x0A), \r (0x0D)
            // Elimina: 0x00-0x08, 0x0B, 0x0C, 0x0E-0x1F, 0x7F
            string pattern = @"[\x00-\x08\x0B\x0C\x0E-\x1F\x7F]";
            return Regex.Replace(input, pattern, "");
        }
        //static string CleanXmlString(byte[] data)
        //{
        //    // Convertir bytes a string eliminando caracteres nulos
        //    StringBuilder xmlBuilder = new StringBuilder();

        //    // Convertir bytes a string UTF-8
        //     Encoding.UTF8.GetString(data);
        //    foreach (byte b in data)
        //    {
        //        if (b != 0x00) // Eliminar caracteres nulos
        //        {
        //            xmlBuilder.Append((char)b);
        //        }
        //    }

        //    string xml = xmlBuilder.ToString();
        //    if (xml.StartsWith("\uFEFF", StringComparison.Ordinal))
        //    {
        //        xml = xml.Substring(1);
        //    }
        //    int startIndex = xml.IndexOf("<?xml");
        //    if (startIndex > 0)
        //    {
        //        xml = xml.Substring(startIndex);
        //    }
        //    foreach (var replacement in SpecialCharReplacements)
        //    {
        //        xml = xml.Replace(replacement.Key, replacement.Value);
        //    }

        //    return xml;
        //}
        private static readonly Dictionary<string, string> SpecialCharReplacements = new Dictionary<string, string>
    {
        // Caracteres mal codificados (UTF-8 interpretado como Latin1)
        { "Ã¡", "á" }, { "Ã©", "é" }, { "Ã­", "í" }, { "Ã³", "ó" }, { "Ãº", "ú" },
        { "Ã±", "ñ" }, { "Ã¼", "ü" },
        //{ "Ã", "Á" }, { "Ã‰", "É" }, { "Ã", "Í" }, { "Ã", "Ó" }, { "Ãš", "Ú" },
        //{ "Ã‘", "Ñ" }, { "Ãœ", "Ü" },
        
        //// Caracteres con acento circunflejo
        //{ "Â¡", "¡" }, { "Â¿", "¿" }, { "Âº", "º" }, { "Âª", "ª" },
        
        //// Comillas y símbolos
        //{ "â€œ", "\"" }, { "â€", "\"" }, { "â€˜", "'" }, { "â€™", "'" },
        //{ "â€¦", "..." }, { "â€”", "—" }, { "â€“", "–" },
        
        //// Caracteres especiales comunes
        //{ "&amp;", "&" }, { "&lt;", "<" }, { "&gt;", ">" }, { "&quot;", "\"" }, { "&apos;", "'" },
        
        // Para tu caso específico del CFDI
        { "Ã\u001c", "Ú" },  //     
        { "Ãa\u0001", "Ú" },  //       
        { "Á\u0001", "Ú" },  //
        { "Ã0", "É" },  // "PERIFÃ0 RICO" -> "PERIFÉRICO"
       // { "Ãa", "Ú" },  // "PACABTÃaN" -> "PACABTÚN"
        //{ "Ã1", "Á" },  // Variantes
        { "Ã2", "É" }
    };
        

        //static void ValidateAndDisplayXml(string xmlContent)
        //{
        //    try
        //    {
        //        XmlDocument xmlDoc = new XmlDocument();
        //        xmlDoc.LoadXml(xmlContent);

        //        Console.WriteLine("\n✅ XML válido!");

        //        // Ver el inicio del XML
        //        string first1000 = xmlContent.Length > 1000 ?
        //            xmlContent.Substring(0, 1000) + "..." : xmlContent;
        //        Console.WriteLine($"\nInicio del XML:\n{first1000}");
        //    }
        //    catch (XmlException ex)
        //    {
        //        Console.WriteLine($"❌ Error en XML: {ex.Message}");

        //        // Mostrar los primeros caracteres para depuración
        //        Console.WriteLine("\nPrimeros 100 caracteres del contenido:");
        //        for (int i = 0; i < Math.Min(100, xmlContent.Length); i++)
        //        {
        //            char c = xmlContent[i];
        //            Console.WriteLine($"Pos {i}: '{c}' (0x{((int)c):X2})");
        //        }
        //    }
        //}


        bool ValidaDiff(Decimal importea, Decimal importeb)
        {
            Decimal total = importea - importeb;
            if (Math.Abs(total) > 1)
                return false;
            return true;
        }

        public int GetRandom()
        {
            return new Random().Next(120000, 2400000);
        }
    }

}

