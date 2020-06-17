﻿using PagoProfesores.Models.Helper;
using Session;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace PagoProfesores.Controllers.Helper
{
    public class EscuelasController : Controller
    {
        // GET: Escuelas
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string getEscuelas(string Escuela = "")
        {
            EscuelasModel model = new EscuelasModel();

            SessionDB sesion = SessionDB.start(Request, Response, false, model.db, SESSION_BEHAVIOR.AJAX);
            if (sesion == null)
                return "";

            StringBuilder sb = new StringBuilder();
            sb.Append("<option></option>");
            foreach (KeyValuePair<string, string> pair in model.getEscuelas())
            {
                sb.Append("<option value=\"").Append(pair.Key).Append("\">").Append(pair.Value).Append("</option>\n");
            }
            return sb.ToString();
        }
    }
}