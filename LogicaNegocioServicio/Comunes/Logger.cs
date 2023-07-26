
using ConnectionManagement.Data;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.IO;
using System.IO.Compression;

namespace LogicaNegocioServicio.Comunes
{
    public class Logger : ILogger
    {
        private readonly IConfiguration configuration;
        private string connstr = string.Empty;

        public Logger(IConfiguration configuration)
        {
            this.configuration = configuration;
            connstr = configuration.GetConnectionString("DefaultConnection");
        }

        public static void RegistrarLog(LogModel model)
        {
           
            using (var dbManager = new Connection())
            {
                if (!string.IsNullOrEmpty(model.Peticion))
                {                   
                    dbManager.SQLParametros.AddWithValue("@Peticion", model.Peticion);
                }
                if (!string.IsNullOrEmpty(model.Respuesta))
                    dbManager.SQLParametros.AddWithValue("@Respuesta", model.Respuesta);
                if (!string.IsNullOrEmpty(model.Servicio))
                    dbManager.SQLParametros.AddWithValue("@Servicio", model.Servicio);
                dbManager.EjecutarNonQuery("RegistrarLogServicios", CommandType.StoredProcedure);
            }
        }

        public string Log(LogModel model)
        {
            try
            {
                using (var dbManager = new Connection())
                {
                    if (!string.IsNullOrEmpty(model.Peticion))
                        dbManager.SQLParametros.AddWithValue("@Peticion", model.Peticion);
                    if (!string.IsNullOrEmpty(model.Respuesta))
                        dbManager.SQLParametros.AddWithValue("@Respuesta", model.Respuesta);
                    dbManager.EjecutarNonQuery("RegistrarLogServicios", CommandType.StoredProcedure);
                }
            }
            catch (System.Exception e)
            {
                return e.Message;
            }
          
            return "1";
        }
    }

}
