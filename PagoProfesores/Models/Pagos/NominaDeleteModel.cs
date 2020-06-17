using ConnectDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PagoProfesores.Models.Pagos
{
    public class NominaDeleteModel : SuperModel
    {
        public string Periodo { get; set; }
        public string CampusVPDI { get; set; }
        public string TipoPago { get; set; }
        public string PersonaID { get; set; }
        public string CentroCostosID { get; set; }
        public string EsquemaID { get; set; }
        public string MontoPagar { get; set; }
        public string TipoPagoCVE { get; set; }
        public string nominaID { get; set; }
        public string Escuela { get; set; }
        public string PartePeriodo { get; set; }
        public string ids { get; set; }
        public string sql { get; set; }
        public string bandera { get; set; }
        
        public bool deleteNominaAll()
        {
            try
            {
                bandera = "False";

                List<string> filtros = new List<string>();            

                if (Periodo != "" && Periodo != null) filtros.Add("PERIODO = '" + Periodo + "'");

              //  if (PartePeriodo != "" && PartePeriodo != null) filtros.Add("PARTEDELPERIODO = '" + PartePeriodo + "'");

               // if (Escuela != "" && Escuela != null) filtros.Add("CVE_ESCUELA = '" + Escuela + "'");

              //  if (TipoPago != "" && TipoPago != null) filtros.Add("CVE_TIPODEPAGO = '" + TipoPago + "'");

                if (CampusVPDI != "" && CampusVPDI != null) filtros.Add("CVE_SEDE = '" + CampusVPDI + "'");                           
             
                string JOIN = "";
                if (filtros.Count > 0)                
                    JOIN += " WHERE " + string.Join<string>(" AND ", filtros.ToArray());
               
                string sql2 = "SELECT * FROM NOMINA " + JOIN;
                ResultSet res = db.getTable(sql2);

                while (res.Next()) {
                    if(isDelete(res.Get("PERIODO"), res.Get("PARTEDELPERIODO"), res.Get("CVE_ESCUELA"), res.Get("ID_PERSONA"), res.Get("ID_ESQUEMA")))
                    {                      
                        sql = "DELETE FROM PA WHERE PERIODO = '"+ res.Get("PERIODO") + "'"
                            + " AND PARTEDELPERIODO = '" + res.Get("PARTEDELPERIODO") + "'"
                            + " AND CVE_ESCUELA = '" + res.Get("CVE_ESCUELA") + "'"
                            + " AND ID_PERSONA = '" + res.Get("ID_PERSONA") + "'";
                        db.execute(sql);

                        sql = "DELETE FROM ESTADODECUENTA WHERE PERIODO = '" + res.Get("PERIODO") + "'"
                            + " AND ID_ESQUEMA = '" + res.Get("ID_ESQUEMA") + "'"
                            + " AND ID_PERSONA = '" + res.Get("ID_PERSONA") + "'";
                        db.execute(sql);

                        sql = "DELETE FROM ESTADODECUENTA_DETALLE WHERE PERIODO = '" + res.Get("PERIODO") + "'"
                            + " AND ID_ESQUEMA = '" + res.Get("ID_ESQUEMA") + "'"
                            + " AND ID_PERSONA = '" + res.Get("ID_PERSONA") + "'";
                        db.execute(sql);

                        sql = "DELETE FROM ENTREGADECONTRATOS WHERE ID_PERSONA = '" + res.Get("ID_PERSONA") + "' AND  ID_ESQUEMA = " + res.Get("ID_ESQUEMA") + "  ";
                        db.execute(sql);

                        sql = "DELETE FROM NOMINA WHERE PERIODO = '" + Periodo + "'"
                            + " AND PARTEDELPERIODO = '" + res.Get("PARTEDELPERIODO") + "'"
                            + " AND CVE_ESCUELA = '" + res.Get("CVE_ESCUELA") + "'"
                            + " AND ID_PERSONA = '" + res.Get("ID_PERSONA") + "'";
                        db.execute(sql);



                    }
                    else
                        bandera = "True";
                }


                List<string> filtros_ = new List<string>();


              //  if (PartePeriodo != "" && PartePeriodo != null) filtros_.Add("PARTEDELPERIODO = '" + PartePeriodo + "'");

               // if (Escuela != "" && Escuela != null) filtros_.Add("CVE_ESCUELA = '" + Escuela + "'");

              //  if (TipoPago != "" && TipoPago != null) filtros_.Add("CVE_TIPODEPAGO = '" + TipoPago + "'");



                //script  Alex 
                /**********************************************************************/


                string sql_ = "SELECT * FROM ESTADODECUENTA WHERE PERIODO = '"+Periodo+ "' AND CVE_SEDE = '" + CampusVPDI + "' AND (UUID <> '' OR UUID IS NOT NULL) ";
                ResultSet res1 = db.getTable(sql_);

                if (res1.Count>0)
                { /*bandera = "True";*/ }
                else
                {

                    sql = "DELETE FROM ESTADODECUENTA WHERE PERIODO = '" + Periodo + "' AND CVE_SEDE = '" + CampusVPDI + "'";
                    db.execute(sql);                    


                    sql = "DELETE FROM NOMINA WHERE PERIODO = '" + Periodo + "' AND CVE_SEDE = '" + CampusVPDI + "'";
                    db.execute(sql);


                    sql = "DELETE FROM PA WHERE PERIODO = '" + Periodo + "' AND CVE_SEDE = '" + CampusVPDI + "'";
                    db.execute(sql);

                   // bandera = "False";

                }


                return true;


            }
            catch
            {
                return false;
            }
        }




      /*  public bool ScriptEliminarBasura()
        {
            try
            {
                bandera = "False";
            


                List<string> filtros_ = new List<string>();


                //  if (PartePeriodo != "" && PartePeriodo != null) filtros_.Add("PARTEDELPERIODO = '" + PartePeriodo + "'");

                // if (Escuela != "" && Escuela != null) filtros_.Add("CVE_ESCUELA = '" + Escuela + "'");

                //  if (TipoPago != "" && TipoPago != null) filtros_.Add("CVE_TIPODEPAGO = '" + TipoPago + "'");



           


                string sql_ = "SELECT * FROM ESTADODECUENTA WHERE PERIODO = '" + Periodo + "' AND CVE_SEDE = '" + CampusVPDI + "' AND (UUID <> '' OR UUID IS NOT NULL) ";
                ResultSet res1 = db.getTable(sql_);

                if (res1.Count > 0)
                { return false; }
                else
                {

                    sql = "DELETE FROM ESTADODECUENTA WHERE PERIODO = '" + Periodo + "' CVE_SEDE = '" + CampusVPDI + "'";
                    db.execute(sql);


                    sql = "DELETE FROM NOMINA WHERE PERIODO = '" + Periodo + "' CVE_SEDE = '" + CampusVPDI + "'";
                    db.execute(sql);


                    sql = "DELETE FROM PA WHERE PERIODO = '" + Periodo + "' CVE_SEDE = '" + CampusVPDI + "'";
                    db.execute(sql);

                    return true;


                }

              
            }
            catch
            {
                return false;
            }
        }*/




        public bool isDelete(string PERIODO, string PARTEPERIODO, string ID_ESCUELA, string ID_PERSONA, string ID_ESQUEMA)
        {
            sql = "SELECT * FROM ESTADODECUENTA WHERE PERIODO = '" + PERIODO + "'"
                + " AND ID_ESQUEMA = '" + ID_ESQUEMA + "'"
                + " AND ID_PERSONA = '" + ID_PERSONA + "'";

            ResultSet edocuenta = db.getTable(sql);

            while (edocuenta.Next())
            {
                if ((edocuenta.Get("FECHARECIBO") == "" || edocuenta.Get("FECHARECIBO") == null) && edocuenta.Get("PUBLICADO") == "False")
                { }
                else                
                    return false;
            }

            return true;
        }

        public bool deleteNominaSelected()
        {
            try
            {
                bandera = "False";
                string[] arrChecked = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string ID_NOMINA in arrChecked)
                {
                    string sql2 = "SELECT * FROM NOMINA WHERE ID_NOMINA = '"+ ID_NOMINA + "' AND CVE_SEDE = '" + CampusVPDI + "'";
                    ResultSet res = db.getTable(sql2);

                    if (res.Next() && isDelete(res.Get("PERIODO"), res.Get("PARTEDELPERIODO"), res.Get("CVE_ESCUELA"), res.Get("ID_PERSONA"), res.Get("ID_ESQUEMA")))
                    {
                        sql = "DELETE FROM PA WHERE PERIODO = '" + res.Get("PERIODO") + "'"
                            + " AND PARTEDELPERIODO = '" + res.Get("PARTEDELPERIODO") + "'"
                            + " AND CVE_ESCUELA = '" + res.Get("CVE_ESCUELA") + "'"
                            + " AND ID_PERSONA = '" + res.Get("ID_PERSONA") + "'";
                        db.execute(sql);

                        sql = "DELETE FROM ESTADODECUENTA WHERE PERIODO = '" + res.Get("PERIODO") + "'"
                            + " AND ID_ESQUEMA = '" + res.Get("ID_ESQUEMA") + "'"
                            + " AND ID_PERSONA = '" + res.Get("ID_PERSONA") + "'";
                        db.execute(sql);

                        sql = "DELETE FROM ESTADODECUENTA_DETALLE WHERE PERIODO = '" + res.Get("PERIODO") + "'"
                            + " AND ID_ESQUEMA = '" + res.Get("ID_ESQUEMA") + "'"
                            + " AND ID_PERSONA = '" + res.Get("ID_PERSONA") + "'";
                        db.execute(sql);

                        sql = "DELETE FROM ENTREGADECONTRATOS WHERE ID_PERSONA = '" + res.Get("ID_PERSONA") + "' AND  ID_ESQUEMA = " + res.Get("ID_ESQUEMA") + "  ";
                        db.execute(sql);

                        sql = "DELETE FROM NOMINA WHERE PERIODO = '" + res.Get("PERIODO") + "'"
                            + " AND PARTEDELPERIODO = '" + res.Get("PARTEDELPERIODO") + "'"
                            + " AND CVE_ESCUELA = '" + res.Get("CVE_ESCUELA") + "'"
                            + " AND ID_PERSONA = '" + res.Get("ID_PERSONA") + "'";
                        db.execute(sql);

                    }
                    else
                        bandera = "True";
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool insertaEntregaContratosXEsquemaPago()
        {
            bool exito = false;

            List<Parametros> lParamS = new List<Parametros>();
            Parametros paramPeriodo = new Parametros();
            Parametros paramCampusVPDI = new Parametros();
            Parametros paramTipoPago = new Parametros();
            
            try
            {
                paramPeriodo.nombreParametro = "@periodo";
                paramPeriodo.longitudParametro = 10;
                paramPeriodo.tipoParametro = SqlDbType.NVarChar;
                paramPeriodo.direccion = ParameterDirection.Input;
                paramPeriodo.value = Periodo;
                lParamS.Add(paramPeriodo);
                
                paramCampusVPDI.nombreParametro = "@campusVPDI";
                paramCampusVPDI.longitudParametro = 3;
                paramCampusVPDI.tipoParametro = SqlDbType.NVarChar;
                paramCampusVPDI.direccion = ParameterDirection.Input;
                paramCampusVPDI.value = CampusVPDI;
                lParamS.Add(paramCampusVPDI);

                paramTipoPago.nombreParametro = "@tipoPagoCVE";
                paramTipoPago.longitudParametro = 5;
                paramTipoPago.tipoParametro = SqlDbType.NVarChar;
                paramTipoPago.direccion = ParameterDirection.Input;
                paramTipoPago.value = TipoPago;
                lParamS.Add(paramTipoPago);
                
                exito = db.ExecuteStoreProcedure("sp_inserta_entregadecontratosXEsquemaPago", lParamS);

                if (exito == false)
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool calculaEstadocuentaXEsquemaPago()
        {
            bool exito = false;

            List<Parametros> lParamS = new List<Parametros>();
            Parametros paramPeriodo = new Parametros();
            Parametros paramCampusVPDI = new Parametros();
            Parametros paramTipoPago = new Parametros();
            
            try
            {
                paramPeriodo.nombreParametro = "@periodo";
                paramPeriodo.longitudParametro = 10;
                paramPeriodo.tipoParametro = SqlDbType.NVarChar;
                paramPeriodo.direccion = ParameterDirection.Input;
                paramPeriodo.value = Periodo;
                lParamS.Add(paramPeriodo);
                
                paramCampusVPDI.nombreParametro = "@campusVPDI";
                paramCampusVPDI.longitudParametro = 3;
                paramCampusVPDI.tipoParametro = SqlDbType.NVarChar;
                paramCampusVPDI.direccion = ParameterDirection.Input;
                paramCampusVPDI.value = CampusVPDI;
                lParamS.Add(paramCampusVPDI);

                paramTipoPago.nombreParametro = "@tipoPagoCVE";
                paramTipoPago.longitudParametro = 5;
                paramTipoPago.tipoParametro = SqlDbType.NVarChar;
                paramTipoPago.direccion = ParameterDirection.Input;
                paramTipoPago.value = TipoPago;
                lParamS.Add(paramTipoPago);
                
                exito = db.ExecuteStoreProcedure("sp_calcula_estadocuentaXEsquemaPago", lParamS);

                if (exito == false)
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool calculaEstadocuentaXRegistroNomina(string usuario)
        {
            List<Parametros> lParamS;
            Parametros paramPeriodo;
            Parametros paramCampusVPDI;
            Parametros paramTipoPagoCVE;
            Parametros paramEsquemaID;
            Parametros paramMontoPagar;
            Parametros paramPersonaID;
            Parametros paramCentroCostoID;
            Parametros paramNominaID;
            Parametros paramUsuario;
            Parametros paramPartePeriodo;

            bool exito = false;

            try
            {
                sql = " select ID_NOMINA, ID_PERSONA, ID_CENTRODECOSTOS, ID_ESQUEMA, MONTOAPAGAR, CVE_TIPODEPAGO "
                    + "   from NOMINA "
                    + "  where INDICADOR      <> 2 "
                    + "    and PERIODO         = '" + Periodo + "'"
                    + "    and CVE_SEDE        = '" + CampusVPDI + "' "
                     + "   and PARTEDELPERIODO = '" + PartePeriodo + "' ";

                if (TipoPago != "" && TipoPago != null) sql += " and CVE_TIPODEPAGO = '" + TipoPago + "' ";

                ResultSet res = db.getTable(sql);

                while (res.Next())
                {
                    PersonaID = res.Get("ID_PERSONA");
                    CentroCostosID = res.Get("ID_CENTRODECOSTOS");
                    EsquemaID = res.Get("ID_ESQUEMA");
                    MontoPagar = res.Get("MONTOAPAGAR");
                    TipoPagoCVE = res.Get("CVE_TIPODEPAGO");
                    nominaID = res.Get("ID_NOMINA");

                    lParamS = new List<Parametros>();

                    paramPeriodo = new Parametros();
                    paramPeriodo.nombreParametro = "@periodo";
                    paramPeriodo.longitudParametro = 10;
                    paramPeriodo.tipoParametro = SqlDbType.NVarChar;
                    paramPeriodo.direccion = ParameterDirection.Input;
                    paramPeriodo.value = Periodo;
                    lParamS.Add(paramPeriodo);
                    
                    paramPartePeriodo = new Parametros();
                    paramPartePeriodo.nombreParametro = "@partePeriodo";
                    paramPartePeriodo.longitudParametro = 3;
                    paramPartePeriodo.tipoParametro = SqlDbType.NVarChar;
                    paramPartePeriodo.direccion = ParameterDirection.Input;
                    paramPartePeriodo.value = PartePeriodo;
                    lParamS.Add(paramPartePeriodo);
                    
                    paramCampusVPDI = new Parametros();
                    paramCampusVPDI.nombreParametro = "@campusVPDI";
                    paramCampusVPDI.longitudParametro = 3;
                    paramCampusVPDI.tipoParametro = SqlDbType.NVarChar;
                    paramCampusVPDI.direccion = ParameterDirection.Input;
                    paramCampusVPDI.value = CampusVPDI;
                    lParamS.Add(paramCampusVPDI);

                    paramTipoPagoCVE = new Parametros();
                    paramTipoPagoCVE.nombreParametro = "@tipoPagoCVE";
                    paramTipoPagoCVE.longitudParametro = 5;
                    paramTipoPagoCVE.tipoParametro = SqlDbType.NVarChar;
                    paramTipoPagoCVE.direccion = ParameterDirection.Input;
                    paramTipoPagoCVE.value = TipoPagoCVE;
                    lParamS.Add(paramTipoPagoCVE);

                    paramEsquemaID = new Parametros();
                    paramEsquemaID.nombreParametro = "@esquemaID";
                    paramEsquemaID.tipoParametro = SqlDbType.BigInt;
                    paramEsquemaID.direccion = ParameterDirection.Input;
                    paramEsquemaID.value = EsquemaID;
                    lParamS.Add(paramEsquemaID);

                    paramMontoPagar = new Parametros();
                    paramMontoPagar.nombreParametro = "@montoapagar";
                    paramMontoPagar.tipoParametro = SqlDbType.Real;
                    paramMontoPagar.direccion = ParameterDirection.Input;
                    paramMontoPagar.value = MontoPagar;
                    lParamS.Add(paramMontoPagar);

                    paramPersonaID = new Parametros();
                    paramPersonaID.nombreParametro = "@personaID";
                    paramPersonaID.tipoParametro = SqlDbType.BigInt;
                    paramPersonaID.direccion = ParameterDirection.Input;
                    paramPersonaID.value = PersonaID;
                    lParamS.Add(paramPersonaID);

                    paramCentroCostoID = new Parametros();
                    paramCentroCostoID.nombreParametro = "@centroCostosID";
                    paramCentroCostoID.tipoParametro = SqlDbType.BigInt;
                    paramCentroCostoID.direccion = ParameterDirection.Input;
                    paramCentroCostoID.value = CentroCostosID;
                    lParamS.Add(paramCentroCostoID);

                    paramNominaID = new Parametros();
                    paramNominaID.nombreParametro = "@nominaID";
                    paramNominaID.tipoParametro = SqlDbType.Int;
                    paramNominaID.direccion = ParameterDirection.Input;
                    paramNominaID.value = nominaID;
                    lParamS.Add(paramNominaID);

                    paramUsuario = new Parametros();
                    paramUsuario.nombreParametro = "@usuario";
                    paramUsuario.longitudParametro = 180;
                    paramUsuario.tipoParametro = SqlDbType.NVarChar;
                    paramUsuario.direccion = ParameterDirection.Input;
                    paramUsuario.value = usuario;
                    lParamS.Add(paramUsuario);

                    exito = db.ExecuteStoreProcedure("sp_calcula_estadocuentaXRegistroNomina", lParamS);
                    if (exito == false)
                        return false;
                }
                return true;
            }
            catch (Exception E)
            {
                return false;
            }
        }
    }
}