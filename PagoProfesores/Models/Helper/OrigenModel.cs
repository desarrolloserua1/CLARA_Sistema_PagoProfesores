﻿using ConnectDB;
using PagoProfesores.Models;
using PagoProfesores.Models.Helper;
using Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace PagoProfesores.Models.Helper
{
    public class OrigenModel : SuperModel
    {

        public Dictionary<string, string> getOrigen()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            string sql = "SELECT CVE_ORIGEN,ORIGEN FROM ORIGENPERSONA ORDER BY CVE_ORIGEN ";
            ResultSet res = db.getTable(sql);
            while (res.Next())
                dict.Add(res.Get("CVE_ORIGEN"), res.Get("ORIGEN"));

            return dict;
        }





    }
}