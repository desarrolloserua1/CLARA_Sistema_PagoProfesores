using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConnectDB;
using System.Data.SqlClient;
using Session;
using System.Diagnostics;
using System.Collections;
using PagoProfesores.Models.Helper;
namespace PagoProfesores.Models.CatalogosCentrales
{
    public class MySuiteModel : SuperModel
    {

        public string requestor { get; set; }
        public string transaction { get; set; }
        public string country { get; set; }
        public string entity { get; set; }
        public string user { get; set; }
        public string username { get; set; }
        public string data1 { get; set; }
        public string data2 { get; set; }
        public string data3 { get; set; }
        public string mensaje { get; set; }
        public string file1 { get; set; }
        public string file2 { get; set; }
        public string success { get; set; }
        public string archivo { get; set; }
        public string uuid { get; set; }


        public bool Add()
        {
            try
            {
                string sql = "INSERT INTO MYSUITE(";               
                sql += " MYSUITE_REQUESTOR ";
                sql += ", MYSUTE_TRANSACTION ";
                sql += ", MYSUITE_COUNTRY ";
                sql += ", MYSUITE_ENTITY ";
                sql += ", MYSUITE_USER ";
                sql += ", MYSUITE_USERNAME ";
                sql += ", MYSUITE_DATA1 ";
                sql += ", MYSUITE_DATA2 ";
                sql += ", MYSUITE_DATA3 ";
                sql += ", MYSUITE_MENSAJE ";
                sql += ", MYSUITE_NOMBRE_FIEL1 ";
                sql += ", MYSUITE_NOMBRE_FIEL2 ";
                sql += ", MYSUITE_SUCCESS ";
                sql += ", MYSUITE_ARCHIVO ";
                sql += ", MYSUITE_UUID ";
                sql += ") VALUES(";
                sql += "'" + requestor + "'";
                sql += ",'" + transaction + "'";
                sql += ",'" + country + "'";
                sql += ",'" + entity + "'";
                sql += ",'" + user + "'";
                sql += ",'" + username + "'";
                sql += ",'" + data1 + "'";
                sql += ",'" + data2 + "'";
                sql += ",'" + data3 + "'";
                sql += ",'" + mensaje + "'";
                sql += ",'" + file1 + "'";
                sql += ",'" + file2 + "'";
                sql += ",'" + success + "'";
                sql += ",'" + archivo + "'";
                sql += ",'" + uuid + "'";

                sql += ")";   

                Debug.WriteLine("sql add MYSUITE: " + sql);
                if (db.execute(sql))
                {
                    Log.write(this, "MYSUITE add", LOG.REGISTRO, "SQL:" + sql, sesion);     
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }





        public bool Edit()
        {
            try
            {
                string sql = "SELECT * FROM MYSUITE WHERE PK1= 1 ";  //ID DEL PRIMERO REG
            //    Debug.WriteLine("sql edit MYSUITE: " + sql);
                ResultSet res = db.getTable(sql);
                if (res.Next())
                {                 

                    requestor = res.Get("MYSUITE_REQUESTOR");
                    transaction = res.Get("MYSUTE_TRANSACTION");
                    country = res.Get("MYSUITE_COUNTRY");
                    entity = res.Get("MYSUITE_ENTITY");
                    user = res.Get("MYSUITE_USER");
                    username = res.Get("MYSUITE_USERNAME");
                    data1 = res.Get("MYSUITE_DATA1");
                    data2 = res.Get("MYSUITE_DATA2");
                    data3 = res.Get("MYSUITE_DATA3");
                    mensaje = res.Get("MYSUITE_MENSAJE");
                    file1 = res.Get("MYSUITE_NOMBRE_FIEL1");
                    file2 = res.Get("MYSUITE_NOMBRE_FIEL2");
                    success = res.Get("MYSUITE_SUCCESS");
                    archivo = res.Get("MYSUITE_ARCHIVO");
                    uuid = res.Get("MYSUITE_UUID");

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }





        public bool Save()
        {
            try
            {
                string sql = "UPDATE MYSUITE SET ";              
                sql += "MYSUITE_REQUESTOR= '" + requestor + "'";
                sql += ",MYSUTE_TRANSACTION= '" + transaction + "'";
                sql += ",MYSUITE_COUNTRY= '" + country + "'";
                sql += ",MYSUITE_ENTITY= '" + entity + "'";
                sql += ",MYSUITE_USER= '" + user + "'";
                sql += ",MYSUITE_USERNAME= '" + username + "'";
                sql += ",MYSUITE_DATA1= '" + data1 + "'";
                sql += ",MYSUITE_DATA2= '" + data2 + "'";
                sql += ",MYSUITE_DATA3= '" + data3 + "'";
                sql += ",MYSUITE_MENSAJE= '" + mensaje + "'";
                sql += ",MYSUITE_NOMBRE_FIEL1= '" + file1 + "'";
                sql += ",MYSUITE_NOMBRE_FIEL2= '" + file2 + "'";
                sql += ",MYSUITE_SUCCESS= '" + success + "'";
                sql += ",MYSUITE_ARCHIVO= '" + archivo + "'";
                sql += ",MYSUITE_UUID= '" + uuid + "'";

              //  sql += " WHERE Cve_Sociedad='" + Cve_Sociedad + "'";  //EDITAR
                if (db.execute(sql))
                {
                    Log.write(this, "MySuite Save", LOG.REGISTRO, "SQL:" + sql, sesion);      //EDITAR
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }




    }
}