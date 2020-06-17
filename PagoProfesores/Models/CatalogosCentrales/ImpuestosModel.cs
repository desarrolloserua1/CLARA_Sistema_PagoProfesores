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
    public class ImpuestosModel : SuperModel
    {
        [Required(ErrorMessage = "Rango es requerido")]
        public float Rango { get; set; }
        public float LimiteInferior { get; set; }
        public float LimiteSuperior { get; set; }
        public float CuotaFija { get; set; }
        public float PorcentajeExcedente { get; set; }

        public string sql { get; set; }

        public ImpuestosModel()
        {

        }
        public bool Add()
        {
            try
            {
                sql = "INSERT INTO IMPUESTOSASIMILADOS(";
                sql += "Rango";
                sql += ",LimiteInferior";
                sql += ",LimiteSuperior";
                sql += ",CuotaFija";
                sql += ",PorcentajeExcedente";
                sql += ",Usuario";
                sql += ",Fecha_m";

                sql += ") VALUES(";
                sql += Rango + "";
                sql += "," + LimiteInferior;
                sql += "," + LimiteSuperior;
                sql += "," + CuotaFija;
                sql += "," + PorcentajeExcedente;
                sql += ",'" + this.sesion.nickName + "'";
                sql += "," + "GETDATE()";
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
                sql = "SELECT * FROM IMPUESTOSASIMILADOS WHERE Rango = " + Rango + "";
                ResultSet res = db.getTable(sql);

                if (res.Next())
                {
                    Rango =                 res.GetFloat("RANGO");
                    LimiteInferior =        res.GetFloat("LIMITEINFERIOR");
                    LimiteSuperior =        res.GetFloat("LIMITESUPERIOR");
                    CuotaFija =             res.GetFloat("CUOTAFIJA");
                    PorcentajeExcedente =   res.GetFloat("PORCENTAJEEXCEDENTE");
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
                sql = "UPDATE IMPUESTOSASIMILADOS SET ";
                sql += "LimiteInferior = " + LimiteInferior;
                sql += ",LimiteSuperior = " + LimiteSuperior;
                sql += ",CuotaFija = " + CuotaFija;
                sql += ",Porcentajeexcedente = " + PorcentajeExcedente;
                sql += ",Usuario = '" + this.sesion.nickName + "'";
                sql += ",Fecha_m = " + "GETDATE()";
                sql += " WHERE Rango = " + Rango + "";
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
                sql = "DELETE FROM IMPUESTOSASIMILADOS WHERE Rango = " + Rango + "";
                if (db.execute(sql)) { return true; } else { return false; }
            }
            catch
            {
                return false;
            }
        }

    }
}