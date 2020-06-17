using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Session;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using ConnectDB;

namespace PagoProfesores.Models.CatalogosporSede
{
    public class SedesModel : SuperModel
    {
        [Required(ErrorMessage = "Clave es requerido")]

        public string Cve_Sociedad { get; set; }
        public string Cve_Sede { get; set; }
        public string Sede { get; set; }
        public string Campus_Inb { get; set; }
        public string TipoContrato_Inb { get; set; }

        public string sql { get; set; } //update

        public SedesModel()
        {

        }
        public string ComboSql(string Sql, string cve, string valor)
        {
            string MySql = Sql;
            string Combo = "\r\n";

            ResultSet reader = db.getTable(MySql);
            try
            {
                while (reader.Next())
                {
                    Combo = Combo + "<option value =\"" + reader.Get(cve) + "\" >";
                    Combo += reader.Get(valor) + " </ option >\r\n";
                }
                return Combo;
            }
            catch
            {
                return "Error en consulta combo";
            }
        }

        public bool Add()
        {
            try
            {
                sql = "INSERT INTO SEDES(";

                sql += "Cve_Sociedad";
                sql += ",Cve_Sede";
                sql += ",Sede";
                sql += ",Campus_Inb";
                sql += ",TipoContrato_Inb";
                sql += ",Usuario";

                sql += ") VALUES(";
                sql += "'" + Cve_Sociedad + "'";
                sql += ",'"+ Cve_Sede + "'";
                sql += ",'" + Sede + "'";
                sql += ",'" + Campus_Inb + "'";
                sql += ",'" + TipoContrato_Inb + "'";
                sql += ",'" + this.sesion.nickName + "'";

                sql += ")";
                if (db.execute(sql))
                {
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
                sql = "SELECT * FROM SEDES WHERE Cve_Sede = '" + Cve_Sede + "'";
                ResultSet res = db.getTable(sql);

                if (res.Next())
                {
                    Cve_Sociedad =                  res.Get("CVE_SOCIEDAD");
                    Cve_Sede =                      res.Get("CVE_SEDE");
                    Sede =                          res.Get("SEDE");
                    Campus_Inb =                    res.Get("CAMPUS_INB");
                    TipoContrato_Inb =              res.Get("TIPOCONTRATO_INB");
                    
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
                sql = "UPDATE SEDES SET ";

                sql += "Cve_Sociedad = '" + Cve_Sociedad + "'";
                sql += ",Sede = '" + Sede + "'";
                sql += ",Campus_Inb = '" + Campus_Inb + "'";
                sql += ",TipoContrato_Inb = '" + TipoContrato_Inb + "'";
                sql += ",Usuario = '" + this.sesion.nickName + "'";
                sql += ",Fecha_M = " + "GETDATE()" + "";

                sql += " WHERE Cve_Sede = '" + Cve_Sede + "'";
                if (db.execute(sql)) { return true; } else { return false; }
            }
            catch
            {
                return false;
            }
        }

        public bool Delete()
        {
            try
            {
                sql = "DELETE FROM SEDES WHERE Cve_Sede = '" + Cve_Sede + "'";
                if (db.execute(sql)) { return true; } else { return false; }
            }
            catch
            {
                return false;
            }
        }

    }
}