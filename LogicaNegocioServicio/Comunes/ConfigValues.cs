
using ConnectionManagement.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LogicaNegocioServicio.Comunes
{
   public class ConfigValues
    {
       

        public static string seleccionarConfigValue(string configKeyName, string cadenaConexion)
        {
            string configKeyValue = "";
            try
            {
                using (var dbManager = new Connection(cadenaConexion))
                {
                    dbManager.SQLParametros.AddWithValue("@nombre", configKeyName);
                    var reader = dbManager.EjecutarReader("ObtenerInfoConfigValues", CommandType.StoredProcedure);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            if (!Convert.IsDBNull(reader["valor"])) { configKeyValue = reader["valor"].ToString(); }                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar el valor de configuracion: "+ ex.Message);
            }
            return configKeyValue;
        }


    }

}
