﻿using ConnectDB;
using ConnectUrlToken;
using Factory;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PagoProfesores.Models.ConsultarBanner;
using Session;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PagoProfesores.Controllers.ConsultarBanner
{
    public class CatCalendarioRuaController : Controller
    {
        private database db;
        private List<Factory.Privileges> Privileges;
        private SessionDB sesion;

        public CatCalendarioRuaController()
        {
            db = new database();

            string[] scripts = { "js/Pagos/ConsultarBanner/CatCalendarioRua.js" };
            Scripts.SCRIPTS = scripts;

            Privileges = new List<Factory.Privileges> {
                 new Factory.Privileges { Permiso = 10005,  Element = "Controller" }, //PERMISO ACCESO REGISTRO DE CONTRATOS
                 new Factory.Privileges { Permiso = 10008,  Element = "formbtnsave" }, //PERMISO EDITAR REGISTRO DE CONTRATOS
                 new Factory.Privileges { Permiso = 10009,  Element = "formbtndelete" }, //PERMISO ELIMINAR REGISTRO DE CONTRATOS
            };
        }

        // GET: CatPeriodos
        public ActionResult Start()
        {
            if (sesion == null) { sesion = SessionDB.start(Request, Response, false, db); }

            CatCalendarioRuaModel model = new CatCalendarioRuaModel();
            if ((model.sesion = sesion) == null)
                return Content("");
            model.Clean();

            try
            {
                Main view = new Main();
                ViewBag.MainUser = view.CreateMenuInfoUser(sesion);
                ViewBag.Main = view.createMenu(22, 45, sesion);

                //sesion.vdata["TABLE_CALENDARIORUA"] = "CALENDARIORUA_TMP";
                sesion.vdata["TABLE_CALENDARIORUA"] = "QCalendarioRuaT01";
                sesion.saveSession();

                ViewBag.BlockingPanel_1 = Main.createBlockingPanel("blocking-panel-1");
                ViewBag.DataTable = CreateDataTable(10, 1, null, "PERIODO", "ASC", sesion);

                Scripts.SCRIPTS = new string[] { "js/ConsultarBanner/CatCalendarioRua/CatCalendarioRua.js" };
                ViewBag.Scripts = Scripts.addScript();

                ViewBag.Ciclo = CreaCombo("select * from ciclos", "CVE_CICLO", "CVE_CICLO", "");

                //Intercom
                ViewBag.User = sesion.nickName.ToString();
                ViewBag.Email = sesion.nickName.ToString();
                ViewBag.FechaReg = DateTime.Today;

                Log.write(this, "Start", LOG.CONSULTA, "Ingresa a pantalla 'Calendario RUA' ", sesion);

                return View(Factory.View.Access + "ConsultarBanner/CatCalendarioRua/Start.cshtml");
            }
            catch (Exception e)
            {
                ViewBag.Notification = Notification.Error(e.Message);
                Log.write(this, "Start", LOG.ERROR, "Ingresa a pantalla 'Calendario RUA' " + e.Message, sesion);

                return View(Factory.View.Access + "ConsultarBanner/CatCalendarioRua/Start.cshtml");
            }
        }

        //GET CREATE DATATABLE
        public string CreateDataTable(int show, int pg, string search, string orderby, string sort, SessionDB sesion)
        {
            if (sesion == null)
                if ((sesion = SessionDB.start(Request, Response, false, new database(), SESSION_BEHAVIOR.AJAX)) == null)
                    return string.Empty;

            DataTable table = new DataTable();
            string CheckIcon = "<i class=\"fa fa-check\"></i>";

            table.TABLE = sesion.vdata["TABLE_CALENDARIORUA"];
            table.COLUMNAS = new string[] { "Reg", "Año escolar", "Periodo", "Descripción", "Fecha Inicial", "Fecha Final", "Tipo de periodo" };
            table.CAMPOS = new string[] { "REGISTRADO", "CVE_CICLO", "PERIODO", "DESCRIPCION", "FECHA_INICIAL", "FECHA_FINAL", "TIPOPERIODO" };
            table.CAMPOSSEARCH = new string[] { "CVE_CICLO", "PERIODO", "DESCRIPCION", "FECHA_INICIAL", "FECHA_FINAL", "TIPOPERIODO" };
            table.dictColumnFormat["REGISTRADO"] = delegate (string str, ResultSet res) { return str == "True" ? CheckIcon : ""; };

            table.orderby = orderby;
            table.sort = sort;
            table.show = show;
            table.pg = pg;
            table.search = search;
            table.field_id = "PERIODO";

            table.enabledCheckbox = true;

            return table.CreateDataTable(sesion);
        }

        [HttpPost]
        public string CreaCombo(string Sql = "", string Clave = "", string Valor = "", string Inicial = "")
        {
            string Salida = "";
            CatPeriodosModel model = new CatPeriodosModel();
            Salida = model.ComboSql(Sql, Clave, Valor, Inicial);
            return Salida;
        }

        [HttpGet]
        public string Consultar(string Ciclo)
        {
            //SessionDB sesion;
            if ((sesion = SessionDB.start(Request, Response, false, new database(), SESSION_BEHAVIOR.AJAX)) == null)
                return string.Empty;

            string paURL = ConfigurationManager.AppSettings["xURL"];
            string paUser = ConfigurationManager.AppSettings["xUser"];
            string paSecret = ConfigurationManager.AppSettings["xSecret"];
            string paFormat = ConfigurationManager.AppSettings["xFormat"];

            ConnectUrlToken.ConnectUrlToken con = new ConnectUrlToken.ConnectUrlToken(paURL, paUser, paSecret, paFormat);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string str_json = serializer.Serialize(
                new ServiceCalendarioRua
                {
                    ciclo = Ciclo
                });

            Token token = con.getToken();

            try
            {
                CatCalendarioRuaModel[] models = con.connectX<CatCalendarioRuaModel[]>(token, "srvCalendarioRua", str_json);
                if (models.Length > 0)
                {
                    models[0].sesion = sesion;
                    models[0].Clean();
                    foreach (CatCalendarioRuaModel model in models)
                    {
                        model.CVE_CICLO = Ciclo;
                        model.sesion = sesion;
                        model.FECHA_R = System.DateTime.Today.ToString();
                        model.FECHA_M = System.DateTime.Today.ToString();
                        model.TMP = false;
                        model.REGISTRADO = model.exist() ? "1" : "0";
                        model.TMP = true;
                        model.addTmp();
                    }
                }
                sesion.vdata["TABLE_CALENDARIORUA"] = "QCalendarioRuaT01";
                sesion.saveSession();

                return "ok";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error." + ex.Message);
            }
            return "-1";
        }

        public string Importar(string periodos, string cveCiclo)
        {
            CatCalendarioRuaModel model = new CatCalendarioRuaModel();
            if ((model.sesion = SessionDB.start(Request, Response, false, model.db, SESSION_BEHAVIOR.AJAX)) == null)
                return string.Empty;

            if (model.Importar(periodos, cveCiclo))
            {
                Log.write(this, "Importar", LOG.EDICION, "periodos:" + periodos, model.sesion);
                return Notification.Succes("Los datos se han actualizado satisfactoriamente.");
            }
            else
                return Notification.Error("No se ha podido hacer la importación");
        }

        //#EXPORT EXCEL
        public void ExportExcel()
        {
            if (sesion == null) { sesion = SessionDB.start(Request, Response, false, db); }

            try
            {
                System.Data.DataTable tbl = new System.Data.DataTable();
                tbl.Columns.Add("Registrado", typeof(string));
                tbl.Columns.Add("Año escolar", typeof(string));
                tbl.Columns.Add("Periodo", typeof(string));
                tbl.Columns.Add("Descripción", typeof(string));
                tbl.Columns.Add("Fecha inicial", typeof(string));
                tbl.Columns.Add("Fecha final", typeof(string));
                tbl.Columns.Add("Tipo de periodo", typeof(string));

                ResultSet res = db.getTable("SELECT CASE WHEN REGISTRADO = 1 THEN 'Si' ELSE 'No' END REGISTRADO, CVE_CICLO, PERIODO, DESCRIPCION, FECHA_INICIAL, FECHA_FINAL, TIPOPERIODO FROM QCalendarioRuaT01");

                while (res.Next())
                {
                    // Here we add five DataRows.
                    tbl.Rows.Add(res.Get("REGISTRADO"), res.Get("CVE_CICLO"), res.Get("PERIODO"), res.Get("DESCRIPCION"), res.Get("FECHA_INICIAL"), res.Get("FECHA_FINAL"), res.Get("TIPOPERIODO"));
                }

                using (ExcelPackage pck = new ExcelPackage())
                {
                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Catálogo de calendario RUA banner");

                    //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                    ws.Cells["A1"].LoadFromDataTable(tbl, true);
                    ws.Cells["A1:G1"].AutoFitColumns();
                    //ws.Column(1).Width = 20;
                    //ws.Column(2).Width = 80;

                    //Format the header for column 1-3
                    using (ExcelRange rng = ws.Cells["A1:G1"])
                    {
                        rng.Style.Font.Bold = true;
                        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                        rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));  //Set color to dark blue
                        rng.Style.Font.Color.SetColor(Color.White);
                    }

                    //Example how to Format Column 1 as numeric 
                    using (ExcelRange col = ws.Cells[2, 1, 2 + tbl.Rows.Count, 1])
                    {
                        col.Style.Numberformat.Format = "#,##0.00";
                        col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }

                    //Write it back to the client
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=CatalogoCalendarioRuaBanner.xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                }

                Log.write(this, "Start", LOG.CONSULTA, "Exporta Excel Catálogo de calendario RUA banner", sesion);

            }
            catch (Exception e)
            {
                ViewBag.Notification = Notification.Error(e.Message);
                Log.write(this, "Start", LOG.ERROR, "Exporta Excel Catálogo de calendario RUA banner" + e.Message, sesion);

            }
        }
    }

    public class ServiceCalendarioRua
    {
        public string ciclo;
    }
}