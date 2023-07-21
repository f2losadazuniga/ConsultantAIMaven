using ConnectionManagement.Data;
using ConsultantAIMavenSharedModel.Comunes;
using ConsultantAIMavenSharedModel.Usuarios;
using EntregasLogyTechSharedModel.FineTune;
using EntregasLogyTechSharedModel.Perfil;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.FineTunes
{
    public class FineTunesDal
    {
        public string ConectionString { get; set; }
        public FineTunesDal(string conectionString)
        {
            ConectionString = conectionString;
        }

        public async Task<List<AllFineTune>> GetFineTuneId(int Id)
        {
            List<AllFineTune> resultado = new List<AllFineTune>();
            Connection db = new Connection(ConectionString);
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                db.SQLParametros.Add("@Id", SqlDbType.Int).Value = Id;
                var reader = await db.EjecutarReaderAsync("GetFineTuneId", CommandType.StoredProcedure);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var fineTunes = new AllFineTune();

                        if (!Convert.IsDBNull(reader["Id"])) { fineTunes.Id = Convert.ToInt32(reader["Id"]); }
                        if (!Convert.IsDBNull(reader["Prompt"])) { fineTunes.Prompt = reader["Prompt"].ToString(); }
                        if (!Convert.IsDBNull(reader["Completion"])) { fineTunes.Completion = reader["Completion"].ToString(); }
                        resultado.Add(fineTunes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Se generó un error al consultar los FineTunes: " + ex.Message);
            }
            finally { }
            if (db != null)
            {
                db.Dispose();
            }

            return resultado;
        }

        public async Task<List<AllFineTune>> GetAllFineTunes()
        {
            List<AllFineTune> resultado = new List<AllFineTune>();
            Connection db = new Connection(ConectionString);
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                var reader = await db.EjecutarReaderAsync("GetAllFineTunes", CommandType.StoredProcedure);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var fineTunes = new AllFineTune();

                        if (!Convert.IsDBNull(reader["Id"])) { fineTunes.Id = Convert.ToInt32(reader["Id"]); }
                        if (!Convert.IsDBNull(reader["Prompt"])) { fineTunes.Prompt = reader["Prompt"].ToString(); }
                        if (!Convert.IsDBNull(reader["Completion"])) { fineTunes.Completion = reader["Completion"].ToString(); }
                        resultado.Add(fineTunes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Se genero un error al consultar los usuarios: " + ex.Message);
            }
            finally { }
            if (db != null)
            {
                db.Dispose();
            }

            return resultado;
        }

        public async Task<List<RespuestaServicio>> InsertAllFineTunes(List<DataFineTune> FineTunes)
        {
            Connection db = new Connection(ConectionString);
            List<RespuestaServicio> resultado = new List<RespuestaServicio>();
            DataTable dtFineTunes = new DataTable();
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                db.IniciarTransaccion();

                dtFineTunes = ConvertirObjetoListToDataTable.ConvertToDataTable(FineTunes);
                db.SQLParametros.Add(new SqlParameter("@FineTunesList", dtFineTunes));

                var reader = await db.EjecutarReaderAsync("InsertAllFineTunes", CommandType.StoredProcedure);
                if (reader != null)
                {
                    //while (reader.Read())
                    //{
                    //    RespuestaServicio respuestaServicio = new RespuestaServicio();
                    //    if (!Convert.IsDBNull(reader["Fila"])) { respuestaServicio.Codigo = reader["Fila"].ToString(); }
                    //    if (!Convert.IsDBNull(reader["Descripcion"])) { respuestaServicio.Mensaje = reader["Descripcion"].ToString(); }
                    //    resultado.Add(respuestaServicio);
                    //}
                }

                reader.CloseAsync();
                if (resultado.Count == 0)
                {
                    db.ConfirmarTransaccion();
                }
                else
                {
                    db.AbortarTransaccion();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Se genero un error al insertar el usuario: " + ex.Message);
            }
            finally { }
            if (db != null)
            {
                db.Dispose();
            }

            return resultado;
        }


    }
}
