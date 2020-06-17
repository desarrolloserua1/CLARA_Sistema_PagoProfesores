﻿using ConnectDB;
using Factory;
using PagoProfesores.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PagoProfesores.Models.Pagos
{
	public class EstadodeCuentaWebModel : SuperModel
	{
		public long ID_ESTADODECUENTA { get; set; }
        public string ID_PERSONA { get; set; }
        public string Estado { get; set; }
        public string IDSIU { get; set; }

		public string Profesor { get; set; }
		public string NoCuenta { get; set; }
        public string CuentaClabe { get; set; }
        public string Banco { get; set; }
		public string RFC { get; set; }
        public string Direccion { get; set; }

        public string Pensionados { get; set; }

        public string Fis_Recibide { get; set; }
		public string Fis_RFC { get; set; }
		public string Fis_Domicilio { get; set; }
		public string Fis_Concepto { get; set; }
		public string CveTipoDePago { get; set; }
        public string vdetalle_CveTipoDePago { get; set; }
        
        public string CveTipoFactura { get; set; }
        public FormatoXML XML { get; set; }
        public string Email_Sociedad { get; set; }

        public string FileNameXML { get; set; }
		public string FileNamePDF { get; set; }
		public bool ValidarXML { get; set; }
		public bool ValidarMySuite { get; set; }
		public string Sede { get; set; }
        public string RZ_RFC { get; set; }
        public string DatosFiscales { get; set; }

        public string BancosTotal { get; set; }
        public string RFCEntity { get; set; }

        public string CorreoO365 { get; set; }
        public string SedesAll { get; set; }

        public string UUID;
        public string sql;
        
        public string ID_LOG { get; set; }

        public void InitDatosP()
        {
            Profesor = "";              
            NoCuenta = "";
            CuentaClabe = "";
            Banco = "";
            RFC = "";               
            CveTipoDePago = "";
            CveTipoFactura = "";
            Direccion = "";
            DatosFiscales = "";
            RZ_RFC = "";
            CorreoO365 = "";
            SedesAll = "";
        }

        public void InitDatosFis()
        {
            Fis_Recibide = "";
            Fis_RFC = "";
            Fis_Domicilio = "";
            Fis_Concepto = "";
            Email_Sociedad = "";
        }

        public bool VerificarCuenta()
        {
            sql = "SELECT ID_PERSONA " +
                " FROM PERSONAS P WHERE P.IDSIU='" + IDSIU + "'" +
                " AND P.ID_PERSONA = '" + ID_PERSONA + "'" +
                " AND NOCUENTA_VALIDADO = 1" +
                " AND VALIDANOCUENTA = " + NoCuenta;

            ResultSet res = db.getTable(sql);

            if (res.Next())
                return true;
            else
            {
                //actualizar a 0 
                validaCuenta_2();
                return false;
            }
        }

        public void validaCuenta_1()
        {
            sql = "UPDATE PERSONAS SET "
                 + " NOCUENTA_VALIDADO = 0"
                 + " WHERE IDSIU= '" + IDSIU + "' AND ID_PERSONA = '" + ID_PERSONA + "'";

            db.execute(sql);
        }

        public void validaCuenta_2()
        {
            sql = "UPDATE PERSONAS SET "
                 + " VALIDANOCUENTA =''"
                 + ",NOCUENTA_VALIDADO = 0"
                 + " WHERE IDSIU= '" + IDSIU + "' AND ID_PERSONA = '" + ID_PERSONA + "'";

            db.execute(sql);
        }

        
        public void validaCuenta()
        {
            sql = "UPDATE PERSONAS SET "
                 + "VALIDANOCUENTA ='" + NoCuenta + "'"
                 + ",NOCUENTA_VALIDADO = 1"
                 //+ ",USUARIO='" + sesion.pkUser + "'"
                 + " WHERE IDSIU= '" + IDSIU + "' AND ID_PERSONA = '" + ID_PERSONA + "'";

            db.execute(sql);
        }

        public void GetDatos()
		{
            sql = "SELECT P.*," +
                " CONCAT(P.NOMBRES, ' ', P.APELLIDOS) AS 'NOMBRECOMPLETO' " +			
                " FROM QPERSONAS2 P WHERE P.IDSIU='" + IDSIU + "' AND  P.CVE_SEDE = '" + Sede + "'";

            ResultSet res = db.getTable(sql);

            if (res.Next())
			{
                ID_PERSONA = res.Get("ID_PERSONA");
                Profesor = res.Get("NOMBRECOMPLETO");
				NoCuenta = res.Get("NOCUENTA");
                CuentaClabe = res.Get("CUENTACLABE");
                RFC = res.Get("RFC");
                RZ_RFC = res.Get("RZ_RFC");
                DatosFiscales = res.Get("DATOSFISCALES");
                Banco = res.Get("BANCO");
				RFC = res.Get("RFC");
				CveTipoDePago = res.Get("CVE_TIPODEPAGO");
				CveTipoFactura = res.Get("CVE_TIPOFACTURA");
                Direccion = res.Get("DIRECCION_PAIS")+ " " + res.Get("DIRECCION_ESTADO") + " <br/>Del/Mun."+ res.Get("DIRECCION_ENTIDAD") + " Col." + res.Get("DIRECCION_COLONIA") + "<br/> Calle:" + res.Get("DIRECCION_CALLE") + " No.:"+ res.Get("DIRECCION_NUMERO") +" C.P."+ res.Get("DIRECCION_CP");
                CorreoO365 = res.Get("CORREO365");
                SedesAll = GetSedes(ID_PERSONA);
            } else InitDatosP();
        }

        public string GetPensiones()
        {
            sql =
                "SELECT *" +
                " FROM VPENSIONADOS WHERE ID_PERSONA ='" + ID_PERSONA + "'";
            ResultSet res = db.getTable(sql);

            bool datos = false;
            string table = "";

            string thead = "<h4>Pensión</h4>"+
                               "<table class=\"table table-invoice\">"+
                               "<thead>"+
                               "<tr>"+
                               "<th style=\"color:#000;\">BENEFICIARIO</th>"+
                               "<th style=\"color:#000;\">CUENTA</th>" +
                               "<th style=\"color:#000;\">CUENTA CLABE</th>" +
                               "<th style=\"color:#000;\">TIPOPENSION</th>" +
                               "<th style=\"color:#000;\">TIPODEPAGO</th>" +
                               "<th style=\"color:#000;\">PORCENTAJE</th>" +
                               "<th style=\"color:#000;\">CUOTA</th>" +
                               "<th style=\"color:#000;\">COMENTARIOS</th>" +
                               "</tr>" +
                               "</thead>"+
                               "<tbody>";

            string bodytable="";

            if (res.Next())
            {
                datos = true;
                bodytable += "<tr>" +
                             "<td>" + res.Get("BENEFICIARIO") + "</td>" +
                             "<td>" + res.Get("CUENTA") + "</td>" +
                             "<td>" + res.Get("CLABE") + "</td>" +
                             "<td>" + res.Get("TIPOPENSION") + "</td>" +
                             "<td>" + res.Get("TIPODEPAGO") + "</td>" +
                             "<td>" + res.Get("PORCENTAJE") + "</td>" +
                             "<td>" + res.Get("CUOTA") + "</td>" +
                             "<td>" + res.Get("COMENTARIOS") + "</td>" +
                             "</tr>";
            }

            string tend = " </tbody></table> ";

            if (datos) { table = thead + bodytable + tend; }

            return table;
        }

        public long GetIdPersona(string pIdSiu, string pCampus)
		{
            //sql = "SELECT ID_PERSONA FROM PERSONAS WHERE IDSIU='" + IDSIU + "'";

            sql = "SELECT P.ID_PERSONA"
                + "  FROM PERSONAS P INNER JOIN PERSONAS_SEDES PS ON PS.ID_PERSONA = P.ID_PERSONA"
                + " WHERE P.IDSIU     = '" + pIdSiu + "'"
                + "   AND PS.CVE_SEDE = '" + pCampus + "'";

            ResultSet res = db.getTable(sql);
			if (res.Next())
				return res.GetLong("ID_PERSONA");
			return 0;
		}

        public string GetSede(string pIdSiu)
        {
            sql = "select top 1 ps.CVE_SEDE " +
                  "  from personas p inner join personas_sedes ps on ps.ID_PERSONA = p.ID_PERSONA " +
                  " where idsiu = '" + pIdSiu + "'";

            ResultSet res = db.getTable(sql);
            if (res.Next())
                return res.Get("CVE_SEDE");
            return "UAN";
        }

        public string GetSedes(string pIdPersona)
        {
            string sedes = "";

            sql = "select ps.CVE_SEDE " +
                  "  from personas p inner join personas_sedes ps on ps.ID_PERSONA = p.ID_PERSONA " +
                  " where ps.id_persona = " + pIdPersona;

            ResultSet res = db.getTable(sql);
            if (res.Next())
                sedes += res.Get("CVE_SEDE") + ",";

            return sedes;
        }

        public void GetDatosFiscales()
		{
			sql = "SELECT SO.* FROM SOCIEDADES SO, SEDES SE WHERE SE.CVE_SEDE = '" + Sede + "' AND SO.CVE_SOCIEDAD = SE.CVE_SOCIEDAD ";

			ResultSet res = db.getTable(sql);
            if (res.Next())
            {
                Fis_Recibide = res.Get("SOCIEDAD");
                Fis_RFC = res.Get("RFC");
                Fis_Domicilio =
                    res.Get("DIRECCION_CALLE") + ", " +
                    res.Get("DIRECCION_COLONIA") + ", " +
                    res.Get("DIRECCION_ENTIDAD") + ", " +
                    res.Get("DIRECCION_ESTADO") + ", C.P. " +
                    res.Get("DIRECCION_CP");											
                Fis_Concepto = "HONORARIOS PROFESIONALES POR SERVICIOS ACAD&Eacute;MICOS";
                Email_Sociedad = res.Get("EMAIL_SOC");
            }
            else InitDatosFis();
        }

		public List<string> ValidarArchivoXML(FormatoXML XML)
		{
			List<string> errores = new List<string>();
            DateTime dt = Convert.ToDateTime(XML.Fecha);

            // FECHA
            if (dt.Year != DateTime.Now.Year)
				errores.Add("El año de la factura no corresponde al año en curso.");
			if (dt > DateTime.Now)
				errores.Add("La fecha de la factura es mayor a la fecha actual.");

			GetDatos();
			GetDatosFiscales();
			sql = "SELECT TOP 1 * FROM VESTADO_CUENTA WHERE ID_ESTADODECUENTA=" + ID_ESTADODECUENTA;
			ResultSet res = db.getTable(sql);
			if (res.Next())
			{
                // RFC Emisor
                if (DatosFiscales == "S")
                {
                    if (XML.RFC_Emisor != this.RZ_RFC.ToLower())
                    {
                        string RZ_RFC = "";
                        if (XML.RFC_Emisor != null) { RZ_RFC = XML.RFC_Emisor.ToUpper(); }
                        errores.Add("RFC emisor no coincide: " + RZ_RFC + " <span>&#8800;</span> " + this.RZ_RFC.ToUpper());
                    }
                }
                else
                {
                    if (XML.RFC_Emisor != this.RFC.ToLower())
                    {
                        string RFC = "";
                        if (XML.RFC_Emisor != null) { RFC = XML.RFC_Emisor.ToUpper(); }
                        errores.Add("RFC emisor no coincide: " + RFC + " <span>&#8800;</span> " + this.RFC.ToUpper());
                    }
                }

                // RFC Receptor
                if (XML.RFC_Receptor != this.Fis_RFC.ToLower())
                {
                    string RFC_R = "";
                    if (XML.RFC_Receptor != null) { RFC_R = XML.RFC_Receptor.ToUpper(); }
                    errores.Add("RFC receptor no coincide: " + RFC_R + " <span>&#8800;</span> " + this.Fis_RFC.ToUpper());
                }

				double value_xml;
				double value_DB;
                // importe
                double xml2_ = Math.Truncate(Double.Parse(XML.Importe));
                double db_ = Math.Truncate(Double.Parse(res.Get("MONTO")));
                double result = xml2_ - db_;

                if (((double.TryParse(XML.Importe, out value_xml) & double.TryParse(res.Get("MONTO"), out value_DB)) && value_xml == value_DB) || (result == -1 || result == 1 || result == 0)) { }
				else errores.Add("Importe no coincide " + value_xml.ToString("F") + " <span>&#8800;</span> " + value_DB.ToString("F"));


                if (vdetalle_CveTipoDePago == "FDI") { }
                else
                {

                    //IVA RET
                    double value_xml_IVARET;
                    double value_DB_IVARET;

                    double xml2_IVARET = Math.Truncate(Double.Parse(XML.IVA_Ret));
                    double db_IVARET = Math.Truncate(Double.Parse(res.Get("MONTO_IVARET")));
                    double result_IVARET = xml2_IVARET - db_IVARET;

                    if (((double.TryParse(XML.IVA_Ret, out value_xml_IVARET) & double.TryParse(res.Get("MONTO_IVARET"), out value_DB_IVARET)) && value_xml_IVARET == value_DB_IVARET) || (result_IVARET == -1 || result_IVARET == 1 || result_IVARET == 0)) { }
                    else errores.Add("IVARet no coincide " + value_xml_IVARET.ToString("F") + " <span>&#8800;</span> " + value_DB_IVARET.ToString("F"));



                    //ISR RET
                    double value_xml_ISRRET;
                    double value_DB_ISRRET;

                    double xml2_ISRRET = Math.Truncate(Double.Parse(XML.ISR_Ret));
                    double db_ISRRET = Math.Truncate(Double.Parse(res.Get("MONTO_ISRRET")));
                    double result_ISRRET = xml2_ISRRET - db_ISRRET;

                    if (((double.TryParse(XML.ISR_Ret, out value_xml_ISRRET) & double.TryParse(res.Get("MONTO_ISRRET"), out value_DB_ISRRET)) && value_xml_ISRRET == value_DB_ISRRET) || (result_ISRRET == -1 || result_ISRRET == 1 || result_ISRRET == 0)) { }
                    else errores.Add("ISRRet no coincide " + value_xml_ISRRET.ToString("F") + " <span>&#8800;</span> " + value_DB_ISRRET.ToString("F"));

                }
                
                if (errores.Count > 0)
                {
                    string msg = string.Join<string>(", \n", errores);                 

                    try
                    {
                        SaveLog(msg, 2);
                    }
                    catch (Exception) { }
                }

				// Validar folios fiscales unicos.
				/*List<EstadoCuentaFolios> listEstadoCuenta = ConsultaFoliosFiscales();
				foreach (EstadoCuentaFolios estadoCuenta in listEstadoCuenta)
				{
					if (XML.UUID == estadoCuenta.UUID)
					{
						errores.Add("El folio fiscal <strong>\"" + estadoCuenta.UUID + "\"</strong> ya existe para el concepto" +
							" <strong>\"" + estadoCuenta.Concepto + "\"</strong> del periodo <strong>\"" + estadoCuenta.Periodo + "\"</strong>");
						break;
					}
				}*/
			}

			this.XML = XML;
            return errores;
		}

        public void SaveLog(string msg, int nivel_error)//nivel_error = 1 (sintaxis: estandar de tags), nivel_error = 2(validaciónes:tag_xml y bd), nivel_error = 3(errores Mysuite),
        {
            sql = "SELECT * FROM LOG_SUBIR_XML WHERE ID_LOG = '" + ID_LOG + "'";
            ResultSet res = db.getTable(sql);

            string MENSAJE_ERROR = "";
            string MENSAJE_ERROR_INS = "";
            string _xml_ = "";
            string _pdf_ = "";

            if (nivel_error == 1) {
                MENSAJE_ERROR = ",MENSAJE_ERROR = '" + msg + "'";
                MENSAJE_ERROR_INS = "MENSAJE_ERROR";
            }
            else if (nivel_error == 2) {
                MENSAJE_ERROR = ",MENSAJE_ERROR_VALIDA = '" + msg + "'";
                MENSAJE_ERROR_INS = "MENSAJE_ERROR_VALIDA";
            }
            else if (nivel_error == 3) {
                MENSAJE_ERROR = ",MENSAJE_ERROR_MYSUITE = '" + msg + "'";
                MENSAJE_ERROR_INS = "MENSAJE_ERROR_MYSUITE";
            }
            else
            {
                MENSAJE_ERROR = "";
                 _xml_ = string.IsNullOrWhiteSpace(FileNameXML) ? "" : (",XML='" + FileNameXML + "'");
                 _pdf_ = string.IsNullOrWhiteSpace(FileNamePDF) ? "" : (",PDF='" + FileNamePDF + "'");
            }

            if (res.Next())
            {
                sql = "UPDATE LOG_SUBIR_XML SET "
                     + " FECHA_R=GETDATE()"
                     + ",RFC='" + RFC + "'"
                     + ",IDSIU='" + IDSIU + "'"
                     + ",CVE_SEDE='" + Sede + "'"
                     +  MENSAJE_ERROR 
                     + _xml_
                     + _pdf_                  
                      + ",USUARIO='" + sesion.pkUser + "'"
                     + " WHERE ID_LOG= '" + ID_LOG + "'";

                db.execute(sql);
            }
            else
            {
                string sql2 = "SELECT TOP 1 * FROM VESTADO_CUENTA WHERE ID_ESTADODECUENTA=" + ID_ESTADODECUENTA;
                res = db.getTable(sql2);
                string Concepto = "";
                string Periodo = "";

                if (res.Next())
                {
                    Periodo = res.Get("PERIODO");
                    Concepto = res.Get("CONCEPTO");
                    RFC = res.Get("RFC");
                }
                
                sql = "INSERT INTO LOG_SUBIR_XML(";
                sql += ""+ MENSAJE_ERROR_INS + "";
                sql += ",RFC";
                sql += ",IDSIU";
                sql += ",CVE_SEDE";
                sql += ",PERIODO";
                sql += ",CONCEPTO";
                sql += ",FECHA_R";              
                sql += ",USUARIO";
                sql += ") VALUES(";
                sql += "'" + msg + "'";
                sql += ",'" + RFC + "'";
                sql += ",'" + IDSIU + "'";
                sql += ",'" + Sede + "'";
                sql += ",'" + Periodo + "'";
                sql += ",'" + Concepto + "'";
                sql += ",GETDATE()";
                sql += ",'" + sesion.pkUser + "'";
                sql += ")";
                ID_LOG = db.executeId(sql);
            }
        }

		public bool SaveXMLyPDF()
		{
			string _xml_ = string.IsNullOrWhiteSpace(FileNameXML) ? "" : (",XML='" + FileNameXML + "',UUID='" + UUID + "'");
			string _pdf_ = string.IsNullOrWhiteSpace(FileNamePDF) ? "" : (",PDF='" + FileNamePDF + "'");

			if (_xml_ == "" && _pdf_ == "")
				return true;

			string FECHA = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

			sql = "UPDATE ESTADODECUENTA "
				+ "   SET FECHA_M           = GETDATE()"
				+ "       ,FECHARECIBO      = '" + FECHA + "'"
				+ "       ,FECHA_SOLICITADO = '" + FECHA + "'"
				+ "       ,USUARIO          = '" + sesion.nickName + "'" + _xml_ + _pdf_
				+ " WHERE ID_ESTADODECUENTA = " + ID_ESTADODECUENTA;

			return db.execute(sql);
		}

		public string CargarContrato(string idFormato)
		{
			sql = "SELECT TOP 1 FORMATO WHERE CVE_CONTRATO='" + idFormato + "'";
			ResultSet res = db.getTable(sql);
			if (res.Next())
				return res.Get("FORMATO");
			return string.Empty;
		}

		public bool SolicitarPago()
		{
			sql = "UPDATE ESTADODECUENTA SET SOLICITADO=1, FECHA_SOLICITADO=GETDATE(), FECHA_M=GETDATE(), USUARIO='" + sesion.nickName + "' WHERE ID_ESTADODECUENTA=" + ID_ESTADODECUENTA;
			return db.execute(sql);
		}

        public string GetComunicados()
        {
            string paragraphs = string.Empty;
            string comunicado = string.Empty;
            string concepto = string.Empty;
            string periodo = string.Empty;
            string esquema = string.Empty;
            string hr = "<hr />";
            int countHR = 1;
            
            sql = "select * "
                + "  from vcomunicado"
                + " where cve_sede   = '" + Sede + "'"
                + "   and (id_persona = 0 or id_persona = " + ID_PERSONA + ")"
                + "   and periodo in (select periodo "
                + "                     from estadodecuenta"
                + "                    where cve_sede   = '" + Sede + "'"
                + "                      and id_persona = " + ID_PERSONA + ")"
                + " order by periodo desc, pkconceptopago asc";

            ResultSet res = db.getTable(sql);

            if (res.Count > 0)
            {
                while (res.Next())
                {
                    periodo = res.Get("PERIODO");
                    esquema = res.Get("ESQUEMADEPAGO");
                    concepto = res.Get("CONCEPTO");
                    comunicado = res.Get("MENSAJE");

                    if (countHR > 1) paragraphs += hr;

                    paragraphs += "<p><b>" + periodo + "</b> - <b>" + esquema + "</b> - <b>" + concepto + "</b>: " + comunicado + "</p>";

                    countHR++;
                }
            }
            else paragraphs += "<p>No hay comunicados.</p>";

            return paragraphs;
        }

        public string GetPagosDashboardPendientes()
        {
            string table = "";

            sql = "SELECT TOP 25 * "
                + "  FROM VESTADO_CUENTA "
                + " WHERE CVE_SEDE   = '" + Sede + "'"
                + "   AND ID_PERSONA = "  + ID_PERSONA
                + "   AND PUBLICADO  = 1"
                + "   AND PADRE      = 0"
                + "   AND (FECHADEPOSITO IS NULL OR FECHADEPOSITO = '')";

            ResultSet res = db.getTable(sql);

            if (res.Count > 0)
            {
                string BANCOS = "";

                while (res.Next())
                {
                    if (res.GetInt("BLOQUEOS") > 0)
                        Estado = "<label class=\"label label-danger\">BLOQUEADO</label>";
                    else
                    {
                        var bloqueoContrato  = res.GetBool("BLOQUEOCONTRATO");

                        if (bloqueoContrato  && string.IsNullOrWhiteSpace(res.Get("FECHADEENTREGA")))
                            Estado = "<label class=\"label label-danger\">BLOQUEADO</label>";
                        else
                        {
                            if (string.IsNullOrWhiteSpace(res.Get("FECHARECIBO")) ||
                                string.IsNullOrWhiteSpace(res.Get("FECHA_SOLICITADO")))
                                Estado = "<label class=\"label label-warning\">PENDIENTE</label>";
                            else if (!string.IsNullOrWhiteSpace(res.Get("FECHARECIBO")) &&
                                !string.IsNullOrWhiteSpace(res.Get("FECHA_SOLICITADO")))
                                Estado = "<label class=\"label label-success\">ENTREGADO</label>";
                        }
                    }

                    BANCOS = "";

                    if (!string.IsNullOrWhiteSpace(res.Get("BANCOS")))
                        BANCOS = Double.Parse(res.Get("BANCOS")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX"));

                    table += "<tr>" +
                             "<td> " + res.Get("CVE_SEDE")        + " </td>" +
                             "<td> " + res.Get("PERIODO")         + " </td>" +
                             "<td> <a href=\"/EstadodeCuentaWeb/DetallePago?_idestadocuenta=" + res.Get("ID_ESTADODECUENTA") + " \">" + res.Get("CONCEPTO") + " </a></td>" +
                             "<td> " + res.Get("FECHARECIBOESQM") + " </td>" +
                             "<td> " + res.Get("FECHAPAGO")       + " </td>" +
                             "<td> " + BANCOS                     + " </td>" +
                             "<td> " + Estado                     + " </td>" +
                             "</tr>";
                }
            }
            else
                table = "<tr><td colspan=\"6\" class=\"dashboard_notrows\">NO EXISTEN PAGOS PENDIENTES</td></tr>";

            return table;
        }

        public string GetPagosDashboardDepositados()
        {
            string table = "";

            sql = "SELECT TOP 25 * FROM VESTADO_CUENTA WHERE CVE_SEDE = '" + Sede + "' AND ID_PERSONA =  " + ID_PERSONA + " AND PUBLICADO = 1 AND (FECHADEPOSITO <> NULL OR FECHADEPOSITO <> '')";

            ResultSet res = db.getTable(sql);

            string BANCOS = "";
            if (res.Count > 0)
            {
                while (res.Next())
                {
                    BANCOS = "";
                    if (!string.IsNullOrWhiteSpace(res.Get("BANCOS"))) { BANCOS = Double.Parse(res.Get("BANCOS")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")); }

                    table += "<tr >" +
                             "<td> " + res.Get("CVE_SEDE") + " </td>" +
                             "<td> " + res.Get("PERIODO") + " </td>" +
                             "<td> <a href=\"/EstadodeCuentaWeb/DetallePagoDepositado?_idestadocuenta=" + res.Get("ID_ESTADODECUENTA") + " \">" + res.Get("CONCEPTO") + " </a></td>" +
                             "<td><a href=\"javascript:; \"> " + res.Get("FECHADEPOSITO") + " </a></td>" +
                             "<td>" + BANCOS + "</td>" +
                             "</tr>";
                }
            }
            else table = "<tr><td colspan=\"6\" class=\"dashboard_notrows\">NO EXISTEN PAGOS DEPOSITADOS</td></tr>";

            return table;
        }

        public string GetDashboardContratos()
        {
            string table = "";

            sql = "SELECT TOP 25 * FROM VENTREGA_CONTRATOS WHERE CVE_SEDE = '" + Sede + "' AND ID_PERSONA =  " + ID_PERSONA + " AND (FECHADECONTRATO IS NULL OR FECHADECONTRATO = '')"; //modifico <> ''

            ResultSet res = db.getTable(sql);

            string btn_contrato = "";
            
            if (res.Count > 0)
            {
                while (res.Next())
                { 

                    if ( string.IsNullOrWhiteSpace(res.Get("FECHADECONTRATO")) )
                            Estado = "<label class=\"label label-warning\">PENDIENTE</label>";
                        else 
                            Estado = "<label class=\"label label-success\">ENTREGADO</label>";

                    btn_contrato = "<a href=\"javascript:void(0)\" onclick=\"formPage_Contratos.verContrato('" + res.Get("CVE_CONTRATO") +
                                       "','" + res.Get("CVE_SEDE") + "','" + res.Get("PERIODO") + "','" + res.Get("CVE_NIVEL") + "','" + res.Get("ID_ESQUEMA") + "','" + res.Get("IDSIU") + "');\" class=\"btn btn-xs btn-default m-r-5\"><i class=\"fa fa-print m-r-5\"></i> Imprimir</a>";


                    string MONTO = "";
                    if (!string.IsNullOrWhiteSpace(res.Get("MONTO"))){ MONTO = Double.Parse(res.Get("MONTO")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")); }
                   
                    table += "<li class=\"media media-sm\">" +
                              "<div class=\"media-body\">" +
                               btn_contrato + "<span style=\"font-size: 14px; font-weight: 500;\">FORMATO CONTRATO  /" + res.Get("CVE_SEDE") + " - " + res.Get("PERIODO") + " - (Monto: " + "<td>" + MONTO + "<td>" + ") "+ Estado + "" +
                              "<p class=\"m-b-5\">" +
                              "No.de Pagos: " + res.Get("NUMPAGOS") + ", No. Semanas " + res.Get("NOSEMANAS") + ", Fechas de Inicio: " + String.Format("{0:MM/dd/yyyy}", res.GetDateTime("FECHAINICIO")) + " - Fecha de Fin: " + String.Format("{0:MM/dd/yyyy}", res.GetDateTime("FECHAFIN")) +
                              "</p>" +
                              "</div>" +
                              "</li>";
                }
            }
            else
            {
                table = "<tr><td colspan=\"7\" class=\"dashboard_notrows\">NO EXISTEN CONTRATOS PENDIENTES</td></tr>";
            }

            return table;
        }

        public string getMensajeRecalculoAsimilados()
        {
            string alert = "";

            RecalculoAsimilados recalculo = new RecalculoAsimilados();
            
            sql = " SELECT *                    " +
                 "   FROM VESTADO_CUENTA       " +
                 "  WHERE CVE_SEDE          = '" + Sede + "'" +
                 "    AND ID_ESTADODECUENTA = " + ID_ESTADODECUENTA +
                 "    AND ID_PERSONA        = " + ID_PERSONA;

            ResultSet res = db.getTable(sql);
            
            /* _____________________________ bandera recalculo ___________________________ */
            List<string> listIDSIU = new List<string>();   
            
            while (res.Next())
                if (!listIDSIU.Contains(res.Get("IDSIU")))
                    listIDSIU.Add(res.Get("IDSIU"));

            foreach (string IDSIU in listIDSIU)
                recalculo.bandera_recalculo(IDSIU, Sede);
            /* ________________________________________________________ */

            res.ReStart();

            if (res.Next())
            {
                if (res.Get("RECALCULO") == "True")
                {
                    string message = "Estimado profesor, si usted recibe pagos por esquema de asimilados y recibe los pagos en diferentes exhibiciones durante el mes, el cálculo del ISR Retenido es sujeto a ajustes debido a que la retención por este esquema se calcula por el monto total de los pagos mensuales”.";
                    alert = "<div class=\"row\" style=\"margin: -20px 50px;\" ><div id =\"alerta\" class=\"alert alert-danger fade in\" style=\"width:; border:1px solid red \"><span data-dismiss=\"alert\" class=\"close\"></span><img src=\"../Content/images/icon-warning-red.png\" class=\"pull-left\" style=\"margin-top:-7px\">";
                    alert += "<p style=\"color:#c30909\">" + message + "</p></div></div>  <input type=\"hidden\" id=\"NotificationType\" value=\"ERROR\">";                   
                }

            }

            return alert;
        }

        public string GetEstadodeCuenta()
		{
            string TABLECONDICIONSQLD = null;
            ResultSet res3 = null;

            string tipoFactura = sesion.vdata["CVE_TIPOFACTURA"];
			int asimilados = 1;

            switch (tipoFactura)
			{
				case "A": // Asimilados
					asimilados = 1;
					break;
				case "H": // Honorarios
				case "F": // Facturas
					asimilados = 0;
					break;
			}
            
            sql = " SELECT *                    " +
			      "   FROM VESTADO_CUENTA       " +
                  "  WHERE CVE_SEDE          = '" + Sede              + "'" +
                  "    AND ID_ESTADODECUENTA = "  + ID_ESTADODECUENTA +
                  "    AND ID_PERSONA        = "  + ID_PERSONA;
			ResultSet res = db.getTable(sql);

			string table = "";

            //DateTime dtAhora = DateTime.Now;//Aplicar para ambiente LOCAL
            DateTime dtAhora = DateTime.Now.AddHours(-6);//Aplicar para ambiente TEST y PROD
            String strDate = "";
            strDate = dtAhora.ToString("yyyy-MM-dd");

            int resultFecha = 0;

            if (res.Count > 0) {
                while (res.Next()) {
                    if (res.Get("PADRE") == "0") {
                        table += "<tr style=\"background: #f0f3f4;\" >" +
                            "<td><b>" + res.Get("CVE_SEDE") + "</b></td>" +
                            "<td><b>" + res.Get("PERIODO") + "</b></td>" +
                            "<td><b>" + res.Get("CONCEPTO") + "</b></td>" +
                            "<td><b>" + Double.Parse(res.Get("MONTO")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</b></td>" +
                            "<td><b>" + Double.Parse(res.Get("MONTO_IVA")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</b></td>" +
                            "<td><b>" + Double.Parse(res.Get("MONTO_IVARET")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</b></td>" +
                            "<td><b>" + Double.Parse(res.Get("MONTO_ISRRET")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</b></td>" +
                            "<td><b>" + Double.Parse(res.Get("BANCOS")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</b></td>" +
                            "<td><b>" + res.Get("FECHAPAGO") + "</b></td>";

                        //dtFechReciboEsqm = Convert.ToDateTime(res.Get("FECHARECIBOESQM"));
                        //dtFechReciboEsqm = DateTime.ParseExact(res.Get("FECHARECIBOESQM"), "dd-MM-yyyy", null);
                        //resultFecha = DateTime.Compare(dtFechReciboEsqm, dtAhora);

                        if (DateTime.Parse(strDate) > DateTime.Parse(res.Get("FECHARECIBOESQM")))
                            resultFecha = 0;
                        else resultFecha = 1;

                        table += "<td> " + (resultFecha <= 0 ? "" : AgregaBotonAsimilados_HTML(res, asimilados)) + " </td>";
                        table += "</tr>";

                        //Detalle Estado de Cuenta
                        TABLECONDICIONSQLD = " ID_ESTADODECUENTA = " + res.Get("ID_ESTADODECUENTA");

                        sql = "SELECT * FROM VESTADO_CUENTA_DETALLE WHERE ID_PERSONA = " + ID_PERSONA + "";

                        if (TABLECONDICIONSQLD != "" && TABLECONDICIONSQLD != null)
                            sql += " AND " + TABLECONDICIONSQLD;

                        ResultSet detalle = db.getTable(sql);

                        while (detalle.Next())
                        {
                            table += "<tr>" +
                                "<td></td>" +
                                "<td>" + detalle.Get("PERIODO") + "</td>" +
                                "<td>" + detalle.Get("CONCEPTO") + "</td>" +
                                "<td>" + Double.Parse(detalle.Get("MONTO")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                "<td>" + Double.Parse(detalle.Get("MONTO_IVA")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                "<td>" + Double.Parse(detalle.Get("MONTO_IVARET")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                "<td>" + Double.Parse(detalle.Get("MONTO_ISRRET")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                "<td>" + Double.Parse(detalle.Get("BANCOS")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                "<td>" + detalle.Get("FECHAPAGO") + "</td>" +
                                "<td> </td>" +
                                "</tr>";
                        }

                        /** - LA BÚSQUEDA DE SUBPADRES - **/
                        ResultSet resH = this.GetRowsTableH(sesion.db, res.Get("ID_ESTADODECUENTA"));
                        while (resH.Next())
                        {
                            table += "<tr>" +
                                "<td>" + resH.Get("CVE_SEDE") + "</td>" +
                                "<td>" + resH.Get("PERIODO") + "</td>" +
                                "<td>" + resH.Get("CONCEPTO") + "</td>" +
                                "<td>" + Double.Parse(resH.Get("MONTO")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                "<td>" + Double.Parse(resH.Get("MONTO_IVA")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                "<td>" + Double.Parse(resH.Get("MONTO_IVARET")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                "<td>" + Double.Parse(resH.Get("MONTO_ISRRET")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                "<td>" + Double.Parse(resH.Get("BANCOS")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                "<td>" + resH.Get("FECHAPAGO") + "</td>" +
                                "<td></td>" +
                                "</tr>";

                            TABLECONDICIONSQLD = " ID_ESTADODECUENTA = " + resH.Get("ID_ESTADODECUENTA");

                            sql = "SELECT * FROM VESTADO_CUENTA_DETALLE WHERE ID_PERSONA = " + ID_PERSONA + "";

                            if (TABLECONDICIONSQLD != "" && TABLECONDICIONSQLD != null)
                                sql += " AND " + TABLECONDICIONSQLD;

                            res3 = db.getTable(sql);

                            while (res3.Next())
                            {
                                table += "<tr>" +
                                    "<td></td>" +
                                    "<td>" + res3.Get("PERIODO") + "</td>" +
                                    "<td>" + res3.Get("CONCEPTO") + "</td>" +
                                    "<td>" + Double.Parse(res3.Get("MONTO")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                    "<td>" + Double.Parse(res3.Get("MONTO_IVA")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                    "<td>" + Double.Parse(res3.Get("MONTO_IVARET")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                    "<td>" + Double.Parse(res3.Get("MONTO_ISRRET")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                    "<td>" + Double.Parse(res3.Get("BANCOS")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                    "<td>" + res3.Get("FECHAPAGO") + "</td>" +
                                    "<td> </td>" +
                                    "</tr>";
                            }
                        }
                    }
                }
            }
            else table = "<tr><td colspan=\"9\" class=\"dashboard_notrows\">NO EXISTEN ESTADO DE CUENTA</td></tr>";

            return table;
        }

        public string GetEstadodeCuentaDepositados()
        {
            string TABLECONDICIONSQLD = null;
            ResultSet res3 = null;

            string tipoFactura = sesion.vdata["CVE_TIPOFACTURA"];
			/*
            int asimilados = 1;
            switch (tipoFactura)
            {
                case "A": // Asimilados
                    asimilados = 1;
                    break;
                case "H": // Honorarios
                case "F": // Facturas
                    asimilados = 0;
                    break;
            }
			*/
            sql =
               "SELECT *" +
               " FROM VESTADO_CUENTA WHERE   CVE_SEDE = '" + Sede + "' AND  ID_ESTADODECUENTA =" + ID_ESTADODECUENTA + " AND ID_PERSONA = " + ID_PERSONA + "";
            ResultSet res = db.getTable(sql);
            string table = "";

            if (res.Count > 0)
            {
                while (res.Next())
                {
                    if (res.Get("PADRE") == "0")
                    {
                        table += "<tr style=\"background: #f0f3f4;\" >" +
                         "<td><b> " + res.Get("CVE_SEDE") + "</b> </td>" +
                         "<td> <b>" + res.Get("PERIODO") + "</b> </td>" +
                         "<td><b>" + res.Get("CONCEPTO") + "</b></td>" +
                         "<td><b>" + Double.Parse(res.Get("MONTO")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</b></td>" +
                         "<td><b>" + Double.Parse(res.Get("MONTO_IVA")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</b></td>" +
                         "<td><b>" + Double.Parse(res.Get("MONTO_IVARET")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</b></td>" +
                         "<td><b>" + Double.Parse(res.Get("MONTO_ISRRET")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</b></td>" +
                          "<td><b>" + res.Get("FECHAPAGO") + "</b></td>" +
                          "<td><b>" + Double.Parse(res.Get("BANCOS")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</b></td>" +
                         "<td><b>" + res.Get("FECHADEPOSITO") + "</b></td>" +
                         "</tr>";
                        
                        //Detalle Estado de Cuenta
                        TABLECONDICIONSQLD = " ID_ESTADODECUENTA = " + res.Get("ID_ESTADODECUENTA");

                        sql = "SELECT *" +
                                " FROM VESTADO_CUENTA_DETALLE WHERE ID_PERSONA = " + ID_PERSONA + "";

                        if (TABLECONDICIONSQLD != "" && TABLECONDICIONSQLD != null)
                            sql += " AND " + TABLECONDICIONSQLD;

                        ResultSet detalle = db.getTable(sql);

                        while (detalle.Next())
                        {
                            table += "<tr>" +
                                //   "<td> " + res.Get("CVE_SEDE") + " </td>" +
                                "<td></td>" +
                                "<td> " + detalle.Get("PERIODO") + " </td>" +
                                "<td>" + detalle.Get("CONCEPTO") + "</td>" +
                                "<td>" + Double.Parse(detalle.Get("MONTO")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                "<td>" + Double.Parse(detalle.Get("MONTO_IVA")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                "<td>" + Double.Parse(detalle.Get("MONTO_IVARET")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                "<td>" + Double.Parse(detalle.Get("MONTO_ISRRET")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                "<td>" + res.Get("FECHAPAGO") + "</td>" +
                                "<td>" + Double.Parse(detalle.Get("BANCOS")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                "<td> " + detalle.Get("FECHADEPOSITO") + " </td>" +
                                "</tr>";
                        }
                        
                        /** - LA BÚSQUEDA DE SUBPADRES - **/
                        ResultSet resH = this.GetRowsTableH(sesion.db, res.Get("ID_ESTADODECUENTA"));
                        while (resH.Next())
                        {
                            table += "<tr>" +
                                "<td> " + res.Get("CVE_SEDE") + " </td>" +
                                "<td> " + resH.Get("PERIODO") + " </td>" +
                                "<td>" + resH.Get("CONCEPTO") + "</td>" +
                                "<td>" + Double.Parse(resH.Get("MONTO")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                "<td>" + Double.Parse(resH.Get("MONTO_IVA")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                "<td>" + Double.Parse(resH.Get("MONTO_IVARET")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                "<td>" + Double.Parse(resH.Get("MONTO_ISRRET")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                "<td>" + res.Get("FECHAPAGO") + "</td>" +
                                "<td>" + Double.Parse(resH.Get("BANCOS")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                "<td> " + resH.Get("FECHADEPOSITO") + " </td>" +
                                "</tr>";

                            TABLECONDICIONSQLD = " ID_ESTADODECUENTA = " + resH.Get("ID_ESTADODECUENTA");
                            sql = "SELECT *" +
                                " FROM VESTADO_CUENTA_DETALLE WHERE ID_PERSONA = " + ID_PERSONA + "";

                            if (TABLECONDICIONSQLD != "" && TABLECONDICIONSQLD != null)
                                sql += " AND " + TABLECONDICIONSQLD;

                            res3 = db.getTable(sql);

                            while (res3.Next())
                            { 
                                table += "<tr>" +
                                    //    "<td> " + res.Get("CVE_SEDE") + " </td>" +
                                    "<td></td>" +
                                    "<td> " + res3.Get("PERIODO") + " </td>" +
                                    "<td>" + res3.Get("CONCEPTO") + "</td>" +
                                    "<td>" + Double.Parse(res3.Get("MONTO")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                    "<td>" + Double.Parse(res3.Get("MONTO_IVA")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                    "<td>" + Double.Parse(res3.Get("MONTO_IVARET")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                    "<td>" + Double.Parse(res3.Get("MONTO_ISRRET")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                    "<td>" + res.Get("FECHAPAGO") + "</td>" +
                                    "<td>" + Double.Parse(res3.Get("BANCOS")).ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                    "<td> " + res3.Get("FECHADEPOSITO") + " </td>" +
                                    "</tr>";
                            }
                        }
                    }
                }
            }
            else table = "<tr><td colspan=\"9\" class=\"dashboard_notrows\">NO EXISTEN ESTADO DE CUENTA</td></tr>";

            return table;
        }

        private ResultSet GetRowsTableH(database db, string IDCta = "")
        {
           // String sql = "SELECT * FROM VESTADO_CUENTA where PADRE = " + IDCta;
             String sql = "SELECT * FROM VESTADO_CUENTA where PADRE = " + IDCta + "  AND ID_PERSONA = " + ID_PERSONA + "";
            sql += " ORDER BY FECHAPAGO ";

            return db.getTable(sql);
        }

        public string AgregaBotonAsimilados_HTML(ResultSet res, int asimilados)
		{
			string FECHA_SOLICITADO = res.Get("FECHA_SOLICITADO");

            if (FECHA_SOLICITADO == "X")
                return "";

            string HTML;

            if (asimilados == 1)
            {
                if (res.GetInt("BLOQUEOS") > 0)
                    HTML = "";
                else
                {
                    if (res.GetBool("BLOQUEOCONTRATO") && string.IsNullOrWhiteSpace(res.Get("FECHADEENTREGA")))
                        HTML = "";
                    else
                    {
                        if (string.IsNullOrWhiteSpace(res.Get("FECHA_SOLICITADO")))
                            HTML = "<button type='button' class='btn btn-sm btn-primary' onclick=\"formPage_EstadoCuenta.solicitar(false, '" + res.Get("ID_ESTADODECUENTA") + "','" + res.Get("CONCEPTO") + "');\">Solicitar</button>";
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(res.Get("FECHARECIBO")))
                            {
                                string xml = res.Get("XML");//"/Upload/" + res.Get("XML") + "#" + DateTime.Now.Ticks;
                                string pdf = res.Get("PDF");

                                //CAMBIAR PARA CONVERTIR DE B64 A PDF
                                string boton_xml = "<button type='button' class='btn btn-sm btn-success' onclick='formPage_EstadoCuenta.verXML(this);' data-file='/Upload/" + xml + "#" + DateTime.Now.Ticks + "'>XML</button>";
                                string boton_pdf = "<button type='button' class='btn btn-sm btn-success' onclick='formPage_EstadoCuenta.verPDF(this);' data-file='/Upload/" + pdf + "#" + DateTime.Now.Ticks + "'>PDF</button>";
                                HTML = @"
                                       <table style='width:100px;'>
                                          <tr>
                                             <td class='cell_xml_pdf'>" + boton_xml + @"</td>
                                             <td class='cell_xml_pdf'>" + boton_pdf + @"</td>
                                          </tr>
                                       </table>";
                            }
                            else HTML = "Ya solicitado";
                        }
                    }
                }
            }
            else
            {//Honorarios
                string xml = res.Get("XML");//"/Upload/" + res.Get("XML") + "#" + DateTime.Now.Ticks;
                string pdf = res.Get("PDF");

                if (res.GetInt("BLOQUEOS") > 0)
                    HTML = "";
                else
                {
                    if (res.GetBool("BLOQUEOCONTRATO") && string.IsNullOrWhiteSpace(res.Get("FECHADEENTREGA")))
                        HTML = "";
                    else
                    {
                        if (string.IsNullOrWhiteSpace(res.Get("FECHARECIBO")) )
                        {
                            if (res.GetBool("MYSUITE"))
                                HTML = "<button type='button' class='btn btn-sm btn-primary' onclick=\"formPage_EstadoCuenta.subirXML(false, '" + res.Get("ID_ESTADODECUENTA") + "');\">subir Factura</button>";
                            else
                                HTML = "No tiene permisos para subir recibos";
                        }
                        else
                        {
                            string boton_xml = "<button type='button' class='btn btn-sm btn-success' onclick='formPage_EstadoCuenta.verXML(this);' data-file='/Upload/" + xml + "#" + DateTime.Now.Ticks + "'>XML</button>";
                            string boton_pdf = "<button type='button' class='btn btn-sm btn-success' onclick='formPage_EstadoCuenta.verPDF(this);' data-file='/Upload/" + pdf + "#" + DateTime.Now.Ticks + "'>PDF</button>";

                            HTML = @"
                                     <table style='width:100px;'>
                                         <tr>
                                             <td class='cell_xml_pdf'>" + boton_xml + @"</td>
                                             <td class='cell_xml_pdf'>" + boton_pdf + @"</td>
                                         </tr>
                                     </table>
                                     ";
                        }
                    }
                }
            }

            return HTML;
        }

        public string GetBloqueosDashboard()
        {


            string table = "";
            bool bloqueoContrato = false;
            sql =  "SELECT TOP 25 * FROM VESTADO_CUENTA WHERE CVE_SEDE = '" + Sede + "' AND ID_PERSONA =  " + ID_PERSONA + " AND PUBLICADO = 1 AND (FECHADEPOSITO IS NULL OR FECHADEPOSITO = '')";

            ResultSet bloqueo = db.getTable(sql);


            sql =
             "SELECT *" +
             " FROM VESTADO_CUENTA_DETALLE_BLOQUEOS WHERE CVE_SEDE = '" + Sede + "' AND ID_PERSONA ='" + ID_PERSONA + "'";
            ResultSet res = db.getTable(sql);

            if(bloqueo.Next())
            {
                if (bloqueo.GetBool("BLOQUEOCONTRATO") && string.IsNullOrWhiteSpace(bloqueo.Get("FECHADEENTREGA")))
                {
                    table += "<li>" +
                                    "<a href=\"/EstadodeCuentaWeb/DetallePago?_idestadocuenta=" + bloqueo.Get("ID_ESTADODECUENTA") + "\" class=\"todolist-container\" data-click=\"todolist\">" +
                                   "<div class=\"todolist-input\"><i class=\"fa fa-square-o\"></i></div>" +
                                   "<div class=\"todolist-title\">Bloqueo por falta de Contrato</div>" +
                                   "</a>" +
                                   "</li>";

                    bloqueoContrato = true;

                }

            }         

            if (res.Count > 0)
            {
                while (res.Next())
                {
                    table += "<li>" +
                              "<a href=\"/EstadodeCuentaWeb/DetallePago?_idestadocuenta=" + res.Get("ID_ESTADODECUENTA") + "\" class=\"todolist-container\" data-click=\"todolist\">" +
                              "<div class=\"todolist-input\"><i class=\"fa fa-square-o\"></i></div>" +
                              "<div class=\"todolist-title\">" + res.Get("BLOQUEODESCRIPCION") + "</div>" +
                              "</a>" +
                              "</li>";
                }

            }
            else
            {

                if (!bloqueoContrato)
                {
                    table = "<li><div  class=\"dashboard_notrows\">NO EXISTEN BLOQUEOS</div></li>";

                }

              
            }

            return table;
        }

        public string GetBloqueos()
        {
            sql =
                "SELECT CVE_BLOQUEO,CVE_SEDE,PERIODO,CONCEPTO,BLOQUEO,BLOQUEODESCRIPCION" +
                " FROM VESTADO_CUENTA_DETALLE_BLOQUEOS WHERE ID_PERSONA ='" + ID_PERSONA + "' AND  ID_ESTADODECUENTA =" + ID_ESTADODECUENTA + " GROUP BY CVE_BLOQUEO,CVE_SEDE,PERIODO,CONCEPTO,BLOQUEO,BLOQUEODESCRIPCION ";
            ResultSet res = db.getTable(sql);

            bool datos = false;
            string table = "";

            string thead = "<h4>Bloqueos</h4>" +
                               "<table class=\"table table-invoice\">" +
                               "<thead>" +
                               "<tr>" +
                               "<th style=\"color:#000;\">UNI</th>" +
                               "<th style=\"color:#000;\">PERIODO</th>" +
                               "<th style=\"color:#000;\">CONCEPTO</th>" +
                               "<th style=\"color:#000;\">BLOQUEO</th>" +
                               "<th style=\"color:#000;\">BLOQUEO DESCRIPCION</th>" +
                               "</tr>" +
                               "</thead>" +
                               "<tbody>";

            string bodytable = "";

            if (res.Count > 0)
            {
                while (res.Next())
                {
                    datos = true;
                    bodytable += "<tr>" +
                                 "<td>" + res.Get("CVE_SEDE") + "</td>" +
                                 "<td>" + res.Get("PERIODO") + "</td>" +
                                 "<td>" + res.Get("CONCEPTO") + "</td>" +
                                 "<td>" + res.Get("BLOQUEO") + "</td>" +
                                 "<td>" + res.Get("BLOQUEODESCRIPCION") + "</td>" +
                                 "</tr>";
                }

            }

            string tend = " </tbody></table> ";

            if (datos) { table = thead + bodytable + tend; }

            return table;

        }
        //--------------------CAMBIOS CARLOS--------------------//

        public string GetPagosxDepositar()
        {
            string numpagos = "";

            //string sql = "SELECT COUNT(*) MAX FROM VESTADO_CUENTA WHERE CVE_SEDE = '" + Sede + "' AND ID_PERSONA = " + ID_PERSONA+" AND PUBLICADO = 1 AND (FECHARECIBO <> NULL OR FECHARECIBO <> '')"; 
            string sql = "SELECT COUNT(*) MAX FROM VESTADO_CUENTA WHERE CVE_SEDE = '" + Sede + "' AND ID_PERSONA = " + ID_PERSONA + " AND PUBLICADO = 1 AND PADRE = 0 AND (FECHADEPOSITO IS NULL OR FECHADEPOSITO = '')"; //Modificación solicitada por Adrián el día 8 de marzo del 2019
            ResultSet res = db.getTable(sql);



            if (res.Count > 0)
            {
                while (res.Next())
                {

                    numpagos = res.Get("MAX");
                }


            }
            else { numpagos = "0"; }

                return numpagos;


        }

        public string GetEstadoPago()
        {
            sql = "SELECT * FROM VESTADO_CUENTA WHERE ID_ESTADODECUENTA =" + ID_ESTADODECUENTA + " AND ID_PERSONA = " + ID_PERSONA + "";
            ResultSet res = db.getTable(sql);
            string estado = "";
            if (res.Next())
            {
                if (res.GetInt("BLOQUEOS") > 0)
                    estado = "<label class=\"label label-danger\">BLOQUEADO</label>";
                else
                    if (res.GetBool("BLOQUEOCONTRATO") && string.IsNullOrWhiteSpace(res.Get("FECHADEENTREGA")))
                        estado = "<label class=\"label label-danger\">BLOQUEADO</label>";
                    else
                        if (string.IsNullOrWhiteSpace(res.Get("FECHARECIBO")) || string.IsNullOrWhiteSpace(res.Get("FECHA_SOLICITADO")))
                            estado = "<label class=\"label label-warning\">PENDIENTE</label>";
                        else if (!string.IsNullOrWhiteSpace(res.Get("FECHARECIBO")) && !string.IsNullOrWhiteSpace(res.Get("FECHA_SOLICITADO")))
                            estado = "<label class=\"label label-success\">ENTREGADO</label>";
            }
            return estado;
        }
        
        //---------------------- CAMBIOS EDGAR INICIO-----------------------//
        public string GetPagosPendientes()
        {
            string table = "";

            sql = "SELECT TOP 25 * FROM VESTADO_CUENTA WHERE  CVE_SEDE = '" + Sede + "' AND ID_PERSONA =  " + ID_PERSONA + " AND PUBLICADO = 1 AND (FECHADEPOSITO IS NULL OR FECHADEPOSITO = '') AND PADRE = 0 ";
            ResultSet res = db.getTable(sql);

            if (res.Count > 0)
            {
                while (res.Next())
                {
                    if (res.GetInt("BLOQUEOS") > 0)
                        Estado = "<label class=\"label label-danger\">BLOQUEADO</label>";
                    else
                        if (res.GetBool("BLOQUEOCONTRATO") && string.IsNullOrWhiteSpace(res.Get("FECHADEENTREGA")))
                            Estado = "<label class=\"label label-danger\">BLOQUEADO</label>";
                        else
                            if (string.IsNullOrWhiteSpace(res.Get("FECHARECIBO")) || string.IsNullOrWhiteSpace(res.Get("FECHA_SOLICITADO")))
                                Estado = "<label class=\"label label-warning\">PENDIENTE</label>";
                            else if (!string.IsNullOrWhiteSpace(res.Get("FECHARECIBO")) && !string.IsNullOrWhiteSpace(res.Get("FECHA_SOLICITADO")))
                                Estado = "<label class=\"label label-success\">ENTREGADO</label>";

                    table += "<tr>" +
                             "<td>" + res.Get("CVE_SEDE") + " </td>" +
                             "<td>" + res.Get("PERIODO") + " </td>" +
                             "<td><a href=\"/EstadodeCuentaWeb/DetallePago?_idestadocuenta=" + res.Get("ID_ESTADODECUENTA") + " \">" + res.Get("CONCEPTO") + " </a></td>" +
                             "<td>" + Estado + "</td>" +
                             "<td>" + string.Format("{0:C}", res.GetDecimal("MONTO")) + "</td>" +
                             "<td>" + string.Format("{0:C}", res.GetDecimal("MONTO_IVA")) + "</td>" +
                             "<td>" + string.Format("{0:C}", res.GetDecimal("MONTO_IVARET")) + "</td>" +
                             "<td>" + string.Format("{0:C}", res.GetDecimal("MONTO_ISRRET")) + "</td>" +
                             "<td>" + string.Format("{0:C}", res.GetDecimal("BANCOS")) + " </td>" +
                             "<td>" + res.Get("FECHAPAGO") + " </td></tr>";
                }
            }
            else
                table = "<tr><td colspan=\"10\" class=\"dashboard_notrows\">NO EXISTEN PAGOS PENDIENTES</td></tr>";
            
            return table;
        }

        public string GetPagosDepositados(HttpRequestBase Request)
        {
            string table = "";


            List<string> filtros = new List<string>();


            if (Request.Params["periodos"] != null && Request.Params["periodos"] != "") filtros.Add("PERIODO = '" + Request.Params["periodos"] + "'");


          //  if (Request.Params["fechaDepoF"] != null && Request.Params["fechaDepoF"] != "") filtros.Add("FECHADEPOSITO <= '" + Request.Params["fechaDepoF"] + "'");

            string union = "";
            string condition = "";
            if (filtros.Count > 0)
            {
                union = " AND ";
                condition = "" + union + " " + string.Join<string>(" AND ", filtros.ToArray());
            }


            sql =
                "SELECT TOP 25 * FROM VESTADO_CUENTA WHERE   CVE_SEDE = '" + Sede + "' AND  ID_PERSONA =  " + ID_PERSONA + " AND PUBLICADO = 1 AND (FECHADEPOSITO <> NULL OR FECHADEPOSITO <> '') " + condition + "";

            ResultSet res = db.getTable(sql);


            if (res.Count > 0)
            {


                while (res.Next())
                {


                    table += "<tr>" +
                             "<td> " + res.Get("CVE_SEDE") + " </td>" +
                             "<td> " + res.Get("PERIODO") + " </td>" +
                         //    "<td> <a href=\" \">" + res.Get("CONCEPTO") + " </a></td>" +
                         "<td> <a href=\"/EstadodeCuentaWeb/DetallePagoDepositado?_idestadocuenta=" + res.Get("ID_ESTADODECUENTA") + " \">" + res.Get("CONCEPTO") + " </a></td>" +
                         "<td>" + string.Format("{0:C}", res.GetDecimal("MONTO")) + "</td>" +
                                     "<td>" + string.Format("{0:C}", res.GetDecimal("MONTO_IVA")) + "</td>" +
                                     "<td>" + string.Format("{0:C}", res.GetDecimal("MONTO_IVARET"))  + "</td>" +
                                     "<td>" + string.Format("{0:C}", res.GetDecimal("MONTO_ISRRET")) + "</td>" +                                   
                                      "<td> " + string.Format("{0:C}", res.GetDecimal("BANCOS")) + " </td>" +

                             "<td><a href=\"javascript:; \"> " + res.Get("FECHADEPOSITO") + " </a></td>" +

                             "</tr>";

                }

            }
            else
            {
                table = "<tr><td colspan=\"9\" class=\"dashboard_notrows\">NO EXISTEN PAGOS PENDIENTES</td></tr>";
            }



            return table;
        }

        public string GetContratos(HttpRequestBase Request)
        {
            string table = "";


            List<string> filtros = new List<string>();


            if (Request.Params["periodos"] != null && Request.Params["periodos"] != "") filtros.Add("PERIODO = '" + Request.Params["periodos"] + "'");


            // if (Request.Params["fechaDepoF"] != null && Request.Params["fechaDepoF"] != "") filtros.Add("FECHAINICIO <= '" + Request.Params["fechaDepoF"] + "'");

            string union = "";
            string condition = "";
            if (filtros.Count > 0)
            {
                union = " AND ";
                condition = "" + union + " " + string.Join<string>(" AND ", filtros.ToArray());
            }


            sql =
                "SELECT TOP 25 * FROM VENTREGA_CONTRATOS WHERE CVE_SEDE = '" + Sede + "' AND  ID_PERSONA =  " + ID_PERSONA +" " + condition + "";

            ResultSet res = db.getTable(sql);
           // string estado = "";
            string btn_contrato = "";

            if (res.Count > 0)
            {


                while (res.Next())
                {

                    /*if (string.IsNullOrWhiteSpace(res.Get("FECHADECONTRATO")))
                        estado = "<label class=\"label label-warning\">PENDIENTE</label>";
                    else
                        estado = "<label class=\"label label-success\">ENTREGADO</label>";*/


                    btn_contrato = "<a href=\"javascript:void(0)\" onclick=\"formPage_Contratos.verContrato('" + res.Get("CVE_CONTRATO") +
                                      "','" + res.Get("CVE_SEDE") + "','" + res.Get("PERIODO") + "','" + res.Get("CVE_NIVEL") + "','" + res.Get("ID_ESQUEMA") + "','" + res.Get("IDSIU") + "');\" class=\"btn btn-xs btn-default m-r-5\"><i class=\"fa fa-print m-r-5\"></i> Imprimir</a>";

                   // btn_contrato = "<a href=\"\" onclick=\"formPage_Contratos.verContrato('" + res.Get("CVE_CONTRATO") +
                                  // "','" + res.Get("CVE_SEDE") + "','" + res.Get("PERIODO") + "','" + res.Get("CVE_NIVEL") + "','" + res.Get("ID_ESQUEMA") + "','" + res.Get("IDSIU") + "');\" class=\"btn btn-xs btn-default m-r-5\"><i class=\"fa fa-print m-r-5\"></i> Imprimir</a>";

                    string fecha_entrega = "";
                    if (res.Get("FECHADECONTRATO")=="" || res.Get("FECHADECONTRATO") == "NULL") {
                        Estado = "<label class=\"label label-warning\"><small>PENDIENTE</small></label>";                        

                    } else {
                        Estado = "<label class=\"label label-success\"><small>ENTREGADO</small></label>";
                        fecha_entrega = String.Format("{0:MM/dd/yyyy}", res.GetDateTime("FECHADECONTRATO"));
                    }



                    string v = res.Get("FECHADECONTRATO");

                     table += "<tr>" +
                             "<td> " + res.Get("CVE_SEDE") + " </td>" +

                             "<td> " + res.Get("PERIODO") + " </td>" +

                             "<td> "+ btn_contrato + "</td>" +

                             "<td> "+Estado+" </td>" +

                             "<td>" + String.Format("{0:MM/dd/yyyy}", res.GetDateTime("FECHAINICIO")) + "  </td>" +

                             "<td>" + String.Format("{0:MM/dd/yyyy}", res.GetDateTime("FECHAFIN")) + "  </td>" +

                             "<td> " + res.Get("NUMPAGOS") + " </td>" +

                             "<td> " + res.Get("NOSEMANAS") + " </td>" +

                             "<td> " + string.Format("{0:C}", res.GetDecimal("MONTO")) + " </td>" +

                             "<td>"+ fecha_entrega + "</td>" +
                             "</tr>";
                }

            }
            else
            {
                table = "<tr><td colspan=\"10\" class=\"dashboard_notrows\">NO EXISTEN CONTRATOS</td></tr>";
            }

            return table;
        }

        public string ConstanciasRetencionMensual(HttpRequestBase Request)
        {
            string table = "";
            List<string> filtros = new List<string>();

            if (Request.Params["fechaAnio"] != null && Request.Params["fechaAnio"] != "") filtros.Add("ANYODEPOSITO = '" + Request.Params["fechaAnio"] + "'");
            if (Request.Params["fechaMes"] != null && Request.Params["fechaMes"] != "") filtros.Add("MESDEPOSITO = '" + Request.Params["fechaMes"] + "'");

            string union = "";
            string condition = "";
            if (filtros.Count > 0)
            {
                union = " AND ";
                condition = "" + union + " " + string.Join<string>(" AND ", filtros.ToArray());
            }

            sql =
                "SELECT TOP 24 * FROM VESTADO_CUENTA_RETMENSUAL WHERE CVE_SEDE = '" + Sede + "' AND  ID_PERSONA =  " + ID_PERSONA + " AND (MESANYODEPOSITO <> '' OR MESANYODEPOSITO IS NOT NULL) AND (MESDEPOSITO <> '' OR MESDEPOSITO IS NOT NULL) AND (ANYODEPOSITO <> '' OR MESDEPOSITO IS NOT NULL)  " + condition + "   ";

            ResultSet res = db.getTable(sql);

            string boton = "";
            //string fecha_publicacion_strm;
            DateTime fecha_publicacion_dt;
            ResultSet publi_ret_mensual = null;

            var culture = new CultureInfo("en-US");

            if (res.Count > 0)
            {
                while (res.Next())
                {
                    sql =
                   "SELECT TOP 24 * FROM PUBLICACION_RETENCIONES_MENSUALES WHERE CVE_SEDE = '" + res.Get("CVE_SEDE") + "' AND CVE_CICLO  = '" + res.Get("ANYODEPOSITO") + "' AND MES ='" + res.Get("MESDEPOSITO") + "'";  
                    publi_ret_mensual = db.getTable(sql);

                    if (publi_ret_mensual.Next()) {

                        fecha_publicacion_dt = Convert.ToDateTime(publi_ret_mensual.Get("FECHA_PUBLICACION"));
                        
                        // fecha_publicacion_dt = Convert.ToDateTime("");
                        //  fecha_publicacion_strm = fecha_publicacion_dt == SuperModel.minDateTime ? "NULL" : ("'" + fecha_publicacion_dt.ToString("yyyy-MM-dd") + "'");
                        //   fecha_publicacion_dt = Convert.ToDateTime(fecha_publicacion_strm);                     

                        string FECHA_ACTUAL_stm = DateTime.Now.ToString("yyyy-MM-dd");
                        DateTime FECHA_ACTUAL = Convert.ToDateTime(FECHA_ACTUAL_stm, culture);

                        if (FECHA_ACTUAL>=  fecha_publicacion_dt )// JUNIO(fs) NO ENERO SI(fs)
                        {
                            boton = "<a href=\"javascript:void(0)\" class='btn btn-xs btn-default m-r-5' onclick=\"formPage_RetencionesMensuales.verRetencion" +
                            "('CR01','" + res.Get("CVE_SEDE") + " ', '" + res.Get("MESDEPOSITO") + "', '" + res.Get("MESDEPOSITO") + "', '" + res.Get("ANYODEPOSITO") +
                            "', '" + res.Get("MONTO") + "', '" + res.Get("MONTO_IVA") + "', '" + res.Get("MONTO_IVARET") + "', '" + res.Get("MONTO_ISRRET") + "');\"" +
                            " ><i class=\"fa fa-print m-r-5\"></i> Imprimir</a>";

                            table += "<tr>" +
                                     "<td> " + res.Get("CVE_SEDE") + " </td>" +
                                     "<td> " + res.Get("PERIODO") + " </td>" +
                                     "<td> " + boton + "</td>" +
                                     "<td> " + res.Get("ANYODEPOSITO") + " </td>" +
                                     "<td> " + this.GetMes(res.GetInt("MESDEPOSITO")) + " </td>" +
                                     "<td> " + res.Get("NUMPAGOS") + " </td>" +
                                     "<td> " + string.Format("{0:C}", res.GetDecimal("MONTO")) + " </td>" +
                                     "<td> " + string.Format("{0:C}", res.GetDecimal("MONTO_IVA")) + "</td>" +
                                     "<td> " + string.Format("{0:C}", res.GetDecimal("MONTO_IVARET")) + " </td>" +
                                     "<td>" + string.Format("{0:C}", res.GetDecimal("MONTO_ISRRET")) + "</td>" +
                                     "<td>" + string.Format("{0:C}", res.GetDecimal("BANCOS")) + "</td>" +
                                     "<td> " + res.Get("TIPODEPAGO") + "</td>" +
                                     "</tr>";
                        } else
                        {
                            //  table = "<tr><td colspan=\"12\" class=\"dashboard_notrows\">NO EXISTEN CONSTANCIAS DE RETENCION MENSUAL</td></tr>";
                        }
                    }/*else
                    {// no existe fecha de retension mensual
                        boton = "<a href=\"javascript:void(0)\" class='btn btn-xs btn-default m-r-5' onclick=\"formPage_RetencionesMensuales.verRetencion" +
                     "('CR01',' ', '" + res.Get("MESDEPOSITO") + "', '" + res.Get("MESDEPOSITO") + "', '" + res.Get("ANYODEPOSITO") +
                     "', '" + res.Get("MONTO") + "', '" + res.Get("MONTO_IVA") + "', '" + res.Get("MONTO_IVARET") + "', '" + res.Get("MONTO_ISRRET") + "');\"" +
                     " ><i class=\"fa fa-print m-r-5\"></i> Imprimir</a>";
                     
                    table += "<tr>" +
                                 "<td> " + res.Get("CVE_SEDE") + " </td>" +
                                 "<td> " + res.Get("PERIODO") + " </td>" +
                                 "<td> " + boton + "</td>" +
                                 "<td> " + res.Get("ANYODEPOSITO") + " </td>" +
                                 "<td> " + this.getMes(res.GetInt("MESDEPOSITO")) + " </td>" +
                                 "<td> " + res.Get("NUMPAGOS") + " </td>" +
                                 "<td> " + string.Format("{0:C}", res.GetDecimal("MONTO")) + " </td>" +
                                 "<td> " + string.Format("{0:C}", res.GetDecimal("MONTO_IVA")) + "</td>" +
                                 "<td> " + string.Format("{0:C}", res.GetDecimal("MONTO_IVARET")) + " </td>" +
                                 "<td>" + string.Format("{0:C}", res.GetDecimal("MONTO_ISRRET")) + "</td>" +
                                 "<td>" + string.Format("{0:C}", res.GetDecimal("BANCOS")) + "</td>" +
                                 "<td> " + res.Get("TIPODEPAGO") + "</td>" +
                                 "</tr>";
                    }*/
                }
            }
            else
            {
                table = "<tr><td colspan=\"12\" class=\"dashboard_notrows\">NO EXISTEN CONSTANCIAS DE RETENCION MENSUAL</td></tr>";
            }
            
            return table;
        }

        public string ConstanciasRetencionAnualPeriodo(HttpRequestBase Request)
        {
            string table = "";
            List<string> filtros = new List<string>();

            if (Request.Params["fechaAnio"] != null && Request.Params["fechaAnio"] != "") filtros.Add("ANIODEPOSITO = '" + Request.Params["fechaAnio"] + "'");

            string union = "";
            string condition = "";
            if (filtros.Count > 0)
            {
                union = " AND ";
                condition = "" + union + " " + string.Join<string>(" AND ", filtros.ToArray());
            }

            sql =
                "SELECT TOP 24 * FROM VESTADO_CUENTA_RETANUAL_PERIODO WHERE CVE_SEDE = '" + Sede + "' AND  ID_PERSONA =  " + ID_PERSONA + " AND (ANIODEPOSITO <> '' OR ANIODEPOSITO IS NOT NULL) AND (MESDEPOSITO_INI <> '' OR MESDEPOSITO_INI IS NOT NULL) AND (MESDEPOSITO_FIN <> '' OR MESDEPOSITO_FIN IS NOT NULL) " + condition + "";

            ResultSet res = db.getTable(sql);
            //string fecha_publicacion_strm;
            DateTime fecha_publicacion_dt;
            ResultSet publi_ret_anual = null;

            var culture = new CultureInfo("en-US");
            string boton = "";

            if (res.Count > 0)
            {
                while (res.Next())
                {
                    sql =
                  "SELECT TOP 24 * FROM PUBLICACION_RETENCIONES_ANUALES WHERE CVE_SEDE = '" + res.Get("CVE_SEDE") + "' AND CVE_CICLO  = '" + res.Get("ANIODEPOSITO") + "'";
                    publi_ret_anual = db.getTable(sql);

                    if (publi_ret_anual.Next())
                    {
                        fecha_publicacion_dt = Convert.ToDateTime(publi_ret_anual.Get("FECHA_PUBLICACION"));
                        //  fecha_publicacion_strm = fecha_publicacion_dt == SuperModel.minDateTime ? "NULL" : ("'" + fecha_publicacion_dt.ToString("yyyy-MM-dd") + "'");
                        //   fecha_publicacion_dt = Convert.ToDateTime(fecha_publicacion_strm);                     

                        string FECHA_ACTUAL_stm = DateTime.Now.ToString("yyyy-MM-dd");
                        DateTime FECHA_ACTUAL = Convert.ToDateTime(FECHA_ACTUAL_stm, culture);

                        if (FECHA_ACTUAL >= fecha_publicacion_dt)
                        {
                            boton = "<a href=\"javascript:void(0)\" class='btn btn-xs btn-default m-r-5' onclick=\"formPage_RetencionesAnualesPeriodo.verRetencion" +
                    "('CR01','" + res.Get("CVE_SEDE") + "', '" + res.Get("MESDEPOSITO_INI") + "', '" + res.Get("MESDEPOSITO_FIN") + "', '" + res.Get("ANIODEPOSITO") +
                    "', '" + res.Get("MONTO") + "', '" + res.Get("MONTO_IVA") + "', '" + res.Get("MONTO_IVARET") + "', '" + res.Get("MONTO_ISRRET") + "');\"" +
                    " ><i class=\"fa fa-print m-r-5\"></i> Imprimir</a>";

                            table += "<tr>" +
                                     "<td> " + res.Get("CVE_SEDE") + " </td>" +
                                     "<td> " + res.Get("PERIODO") + " </td>" +
                                     "<td> " + boton + "</td>" +
                                     "<td> " + res.Get("ANIODEPOSITO") + " </td>" +
                                     "<td> " + this.GetMes(res.GetInt("MESDEPOSITO_INI")) + " </td>" +
                                     "<td> " + this.GetMes(res.GetInt("MESDEPOSITO_FIN")) + " </td>" +
                                     "<td> " + res.Get("NUMPAGOS") + " </td>" +
                                     "<td> " + string.Format("{0:C}", res.GetDecimal("MONTO")) + " </td>" +
                                     "<td> " + string.Format("{0:C}", res.GetDecimal("MONTO_IVA")) + "</td>" +
                                     "<td> " + string.Format("{0:C}", res.GetDecimal("MONTO_IVARET")) + " </td>" +
                                     "<td>" + string.Format("{0:C}", res.GetDecimal("MONTO_ISRRET")) + "</td>" +
                                     "<td>" + string.Format("{0:C}", res.GetDecimal("BANCOS")) + "</td>" +
                                     "<td> " + res.Get("TIPODEPAGO") + "</td>" +
                                     "</tr>";
                        } else
                        {
                            //table = "<tr><td colspan=\"13\" class=\"dashboard_notrows\">NO EXISTEN CONSTANCIAS DE RETENCION ANUAL</td></tr>";
                        }
                    }/*else
                    {// no existe fecha d retension anual

                        boton = "<a href=\"javascript:void(0)\" class='btn btn-xs btn-default m-r-5' onclick=\"formPage_RetencionesAnuales.verRetencion" +
                "('CR01','" + res.Get("CVE_SEDE") + "', '" + res.Get("MESDEPOSITO_INI") + "', '" + res.Get("MESDEPOSITO_FIN") + "', '" + res.Get("ANIODEPOSITO") +
                "', '" + res.Get("MONTO") + "', '" + res.Get("MONTO_IVA") + "', '" + res.Get("MONTO_IVARET") + "', '" + res.Get("MONTO_ISRRET") + "');\"" +
                " ><i class=\"fa fa-print m-r-5\"></i> Imprimir</a>";
                
                        table += "<tr>" +
                                 "<td> " + res.Get("CVE_SEDE") + " </td>" +
                                 "<td> " + res.Get("PERIODO") + " </td>" +
                                 "<td> " + boton + "</td>" +
                                 "<td> " + res.Get("ANIODEPOSITO") + " </td>" +
                                 "<td> " + this.getMes(res.GetInt("MESDEPOSITO_INI")) + " </td>" +
                                 "<td> " + this.getMes(res.GetInt("MESDEPOSITO_FIN")) + " </td>" +
                                 "<td> " + res.Get("NUMPAGOS") + " </td>" +
                                 "<td> " + string.Format("{0:C}", res.GetDecimal("MONTO")) + " </td>" +
                                 "<td> " + string.Format("{0:C}", res.GetDecimal("MONTO_IVA")) + "</td>" +
                                 "<td> " + string.Format("{0:C}", res.GetDecimal("MONTO_IVARET")) + " </td>" +
                                 "<td>" + string.Format("{0:C}", res.GetDecimal("MONTO_ISRRET")) + "</td>" +
                                 "<td>" + string.Format("{0:C}", res.GetDecimal("BANCOS")) + "</td>" +
                                 "<td> " + res.Get("TIPODEPAGO") + "</td>" +
                                 "</tr>";
                    }*/
                }
            }
            else
            {
                table = "<tr><td colspan=\"13\" class=\"dashboard_notrows\">NO EXISTEN CONSTANCIAS DE RETENCION ANUAL</td></tr>";
            }

            return table;
        }

        public string ConstanciasRetencionAnual(HttpRequestBase Request)
        {
            string table = "";
            List<string> filtros = new List<string>();

            if (Request.Params["fechaAnio"] != null && Request.Params["fechaAnio"] != "") filtros.Add("ANIODEPOSITO = '" + Request.Params["fechaAnio"] + "'");
            
            string union = "";
            string condition = "";

            if (filtros.Count > 0)
            {
                union = " AND ";
                condition = "" + union + " " + string.Join<string>(" AND ", filtros.ToArray());
            }
            
            sql = "SELECT TOP 24 * FROM VESTADO_CUENTA_RETANUAL WHERE CVE_SEDE = '" + Sede + "' AND  ID_PERSONA =  " + ID_PERSONA + " AND (ANIODEPOSITO <> '' OR ANIODEPOSITO IS NOT NULL) " + condition + "";

            ResultSet res = db.getTable(sql);
            DateTime fecha_publicacion_dt;
            ResultSet publi_ret_anual = null;
            
            var culture = new CultureInfo("en-US");
            string boton = "";

            if (res.Count > 0)
            {
                while (res.Next())
                {
                    sql = "SELECT TOP 24 * FROM PUBLICACION_RETENCIONES_ANUALES WHERE CVE_SEDE = '" + res.Get("CVE_SEDE") + "' AND CVE_CICLO  = '" + res.Get("ANIODEPOSITO") + "'";
                    publi_ret_anual = db.getTable(sql);
                    
                    if (publi_ret_anual.Next())
                    {
                        fecha_publicacion_dt = Convert.ToDateTime(publi_ret_anual.Get("FECHA_PUBLICACION"));
                        string FECHA_ACTUAL_stm = DateTime.Now.ToString("yyyy-MM-dd");
                        DateTime FECHA_ACTUAL = Convert.ToDateTime(FECHA_ACTUAL_stm, culture);
                        
                        if (FECHA_ACTUAL >= fecha_publicacion_dt)
                        {
                            boton = "<a href=\"javascript:void(0)\" class='btn btn-xs btn-default m-r-5' onclick=\"formPage_RetencionesAnuales.verRetencion" +
                                "('CR01','" + res.Get("CVE_SEDE") + "', '" + res.Get("ANIODEPOSITO") +
                                "', '" + res.Get("MONTO") + "', '" + res.Get("MONTO_IVA") + "', '" + res.Get("MONTO_IVARET") + "', '" + res.Get("MONTO_ISRRET") + "');\"" +
                                " ><i class=\"fa fa-print m-r-5\"></i> Imprimir</a>";

                            table += "<tr>" +
                                     "<td> " + res.Get("CVE_SEDE") + " </td>" +
                                     //"<td> " + res.Get("PERIODO") + " </td>" +
                                     "<td> " + boton + "</td>" +
                                     "<td> " + res.Get("ANIODEPOSITO") + " </td>" +
                                     //"<td> " + this.GetMes(res.GetInt("MESDEPOSITO_INI")) + " </td>" +
                                     //"<td> " + this.GetMes(res.GetInt("MESDEPOSITO_FIN")) + " </td>" +
                                     "<td> " + res.Get("NUMPAGOS") + " </td>" +
                                     "<td> " + string.Format("{0:C}", res.GetDecimal("MONTO")) + " </td>" +
                                     "<td> " + string.Format("{0:C}", res.GetDecimal("MONTO_IVA")) + "</td>" +
                                     "<td> " + string.Format("{0:C}", res.GetDecimal("MONTO_IVARET")) + " </td>" +
                                     "<td>" + string.Format("{0:C}", res.GetDecimal("MONTO_ISRRET")) + "</td>" +
                                     "<td>" + string.Format("{0:C}", res.GetDecimal("BANCOS")) + "</td>" +
                                     "<td> " + res.Get("TIPODEPAGO") + "</td>" +
                                     "</tr>";
                        }
                    }
                }
            }
            else
                table = "<tr><td colspan=\"13\" class=\"dashboard_notrows\">NO EXISTEN CONSTANCIAS DE RETENCION ANUAL</td></tr>";

            return table;
        }

        /*********************************/
        public void GetTipodePago()
        {
            //sql = "SELECT DISTINCT CC.CVE_TIPODEPAGO FROM VESTADO_CUENTA_DETALLE VCD, CENTRODECOSTOS CC where VCD.ID_CENTRODECOSTOS = CC.ID_CENTRODECOSTOS AND VCD.IDSIU = '"+ IDSIU + "' AND VCD.CVE_SEDE = '"+ Sede + "'";
            sql = "SELECT DISTINCT CVE_TIPODEPAGO FROM VESTADO_CUENTA_DETALLE WHERE IDSIU = '" + IDSIU + "' AND CVE_SEDE = '" + Sede + "'"; //ET

            ResultSet res = db.getTable(sql);
            if (res.Next())            
                CveTipoDePago = res.Get("CVE_TIPODEPAGO");
                vdetalle_CveTipoDePago = res.Get("CVE_TIPODEPAGO");
        }
        /***********************/

        private string GetMes(int id_mes)
        {
            string _mes = "";

            switch (id_mes)
            {
                case 1:
                    _mes =  "ENERO";
                    break;
                   
                case 2:
                    _mes =  "FEBRERO";
                    break;
                case 3:
                    _mes = "MARZO";
                    break;
                case 4:
                    _mes = "ABRIL";
                    break;
                case 5:
                    _mes = "MAYO";
                    break;
                case 6:
                    _mes = "JUNIO";
                    break;
                case 7:
                    _mes = "JULIO";
                    break;
                case 8:
                    _mes = "AGOSTO";
                    break;
                case 9:
                    _mes = "SEPTIEMBRE";
                    break;
                case 10:
                    _mes = "OCTUBRE";
                    break;
                case 11:
                    _mes = "NOVIEMBRE";
                    break;
                case 12:
                    _mes = "DICIEMBRE";
                    break;
            }

            return _mes;
        }
        //---------------------- CAMBIOS EDGAR FIN-----------------------//

        public void GetDatosFiscalesEdoCta()
        {
            string server = "PROD";// ConfigurationManager.AppSettings["serverMySuite"];

            sql = "select * from vestado_cuenta where id_estadodecuenta = " + ID_ESTADODECUENTA;

            ResultSet res = db.getTable(sql);
            if (res.Next())
            {
                RFC = res.Get("RFC");
                BancosTotal = res.Get("BANCOS");
            }

            sql = "select * from timbrado_mysuite where server = '" + server + "' and camp_code = '" + Sede + "'";
            res = db.getTable(sql);
            if (res.Next())
                RFCEntity = res.Get("RFCENTITY");
                
        }

		public List<EstadoCuentaFolios> ConsultaFoliosFiscales()
		{
			List<EstadoCuentaFolios> list = new List<EstadoCuentaFolios>();

			string sql = "SELECT UUID,PERIODO,CONCEPTO FROM VESTADO_CUENTA" +
				" WHERE IDSIU='" + IDSIU + "' AND ID_ESTADODECUENTA<>" + ID_ESTADODECUENTA + " AND UUID IS NOT NULL";
			ResultSet res = db.getTable(sql);
			if (res.HasRows)
				while (res.Next())
				{
					list.Add(new EstadoCuentaFolios
					{
						UUID = res.Get("UUID").Trim(),
						Concepto = res.Get("CONCEPTO").Trim(),
						Periodo = res.Get("PERIODO").Trim(),
					});
				}
			return list;
		}

	}// </>

	public class FormatoXML
	{
		public string Fecha { get; set; }        // ok
		public string Importe { get; set; }      // ok
		public string RFC_Emisor { get; set; }
		public string RFC_Receptor { get; set; } // ok
		public string IVA_Ret { get; set; }
		public string ISR_Ret { get; set; }

        public string UUID { get; set; } // ok
        //public string FolioFiscal { get; set; } // ok

		public FormatoXML() { }
	}// </>

	public class EstadoCuentaFolios
	{
		public string UUID { get; set; }
		public string Periodo { get; set; }
		public string Concepto { get; set; }
		public override string ToString()
		{
			return "UUID=" + UUID + ", P=" + Periodo + ", C=" + Concepto;
		}
	}// </>
}