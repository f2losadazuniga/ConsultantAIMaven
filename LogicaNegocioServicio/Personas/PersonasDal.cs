using ConnectionManagement.Data;
using LogicaNegocio;
using LogicaNegocioServicio.Comunes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;


namespace LogicaNegocioServicio.Personas
{
    public class PersonasDal : GenericDal
    {
        public string ConectionString { get; set; }
        public PersonasDal(string conectionString) : base(conectionString)
        {
            ConectionString = conectionString;
        }

      
        public async Task<string> ValidarAutenticacionActiva(int IdPersona)
        {
            try
            {
                this.Connection.SQLParametros.Clear();
                this.Connection.SQLParametros.Add($"@IdPersona", System.Data.SqlDbType.Int).Value = IdPersona;
                var Mensaje = this.Connection.SQLParametros.Add("@Mensaje", System.Data.SqlDbType.VarChar,3500);
                Mensaje.Direction = ParameterDirection.Output;

                this.Connection.EjecutarNonQuery("ValidarAutorizacionActivaSp",CommandType.StoredProcedure);
                return Mensaje.Value.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
   public async Task<Response> InactivePersona(int idPersona, int idUsuarioModifica)
        {
            Response resultado = new Response();
            Connection db = new Connection(ConectionString);
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                db.SQLParametros.Add("@idPersona", SqlDbType.Int, 100).Value = Convert.ToInt32(idPersona);
                db.SQLParametros.Add("@idUsuarioModificacion", SqlDbType.Int, 100).Value = Convert.ToInt32(idUsuarioModifica);
                db.SQLParametros.Add("@fechaModificacion", SqlDbType.DateTime, 100).Value = DateTime.Now;
                db.SQLParametros.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                db.SQLParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue;

                db.EjecutarNonQuery("InactivarPersona", CommandType.StoredProcedure);
                resultado.Valor = Convert.ToInt16(db.SQLParametros["@returnValue"].Value);
                resultado.Message = db.SQLParametros["@Mensaje"].Value.ToString();
            }
            catch (Exception ex)
            {
                resultado.Valor = 100;
                resultado.Message = "Se genero un error al eliminar el resgistro: " + ex.Message;
            }
            finally { }
            if (db != null)
            {
                db.Dispose();
            }

            return resultado;
        }

        public async Task<List<TipoPersonas>> GetAllTipoPersonas()
        {
            List<TipoPersonas> resultado = new List<TipoPersonas>();
            Connection db = new Connection(ConectionString);
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                var reader = await db.EjecutarReaderAsync("ConsultarTipoPersona", CommandType.StoredProcedure);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var personas = new TipoPersonas();

                        if (!Convert.IsDBNull(reader["idTipoPersona"])) { personas.idTipoPersona = Convert.ToInt32(reader["idTipoPersona"]); }
                        if (!Convert.IsDBNull(reader["tipoPersona"])) { personas.tipoPersona = reader["tipoPersona"].ToString(); }
                        if (!Convert.IsDBNull(reader["idEstado"])) { personas.idEstado = Convert.ToInt32(reader["idEstado"]); }
                        if (!Convert.IsDBNull(reader["idUsuarioCreacion"])) { personas.idUsuarioCreacion = Convert.ToInt32(reader["idUsuarioCreacion"]); }
                        if (!Convert.IsDBNull(reader["fechaCreacion"])) { personas.fechaCreacion = DateTime.Now; }
                        if (!Convert.IsDBNull(reader["idusuarioModificacion"])) { personas.idusuarioModificacion = Convert.ToInt32(reader["idusuarioModificacion"]); }
                        if (!Convert.IsDBNull(reader["fechaModificacion"])) { personas.fechaModificacion = DateTime.Now; }

                        resultado.Add(personas);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Se genero un error al consultar los Tipo personas: " + ex.Message);
            }
            finally { }
            if (db != null)
            {
                db.Dispose();
            }

            return resultado;
        }

        public async Task<List<Persona>> GetAllPersonasDocumento(string numeroDocumento, int idEmpresa)
        {
            List<Persona> resultado = new List<Persona>();
            Connection db = new Connection(ConectionString);
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                if (!String.IsNullOrWhiteSpace(numeroDocumento.ToString())) { db.SQLParametros.Add("@numeroDocumento", SqlDbType.VarChar, 50).Value = numeroDocumento; }
                if (!String.IsNullOrWhiteSpace(idEmpresa.ToString())) { db.SQLParametros.Add("@idEmpresa", SqlDbType.Int, 50).Value = idEmpresa; }
                var reader = await db.EjecutarReaderAsync("ConsultarPersonasNumeroDocumentoyEmpresa", CommandType.StoredProcedure);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var personas = new Persona();

                        if (!Convert.IsDBNull(reader["idPersona"])) { personas.idPersona = Convert.ToInt32(reader["idPersona"]); }
                        if (!Convert.IsDBNull(reader["idTipoPersona"])) { personas.idTipoPersona = Convert.ToInt32(reader["idTipoPersona"]); }
                        if (!Convert.IsDBNull(reader["tipoPersona"])) { personas.tipoPersona = reader["tipoPersona"].ToString(); }
                        if (!Convert.IsDBNull(reader["numeroDocumento"])) { personas.numeroDocumento = reader["numeroDocumento"].ToString(); }
                        if (!Convert.IsDBNull(reader["nombre"])) { personas.nombre = reader["nombre"].ToString(); }
                        if (!Convert.IsDBNull(reader["apellido"])) { personas.apellido = reader["apellido"].ToString(); }
                        if (!Convert.IsDBNull(reader["correo"])) { personas.correo = reader["correo"].ToString(); }
                        if (!Convert.IsDBNull(reader["fechaNacimiento"])) { personas.fechaNacimiento = Convert.ToDateTime(reader["fechaNacimiento"]); }
                        if (!Convert.IsDBNull(reader["idEmpresa"])) { personas.idEmpresa = Convert.ToInt32(reader["idEmpresa"]); }
                        if (!Convert.IsDBNull(reader["idUsuarioCreacion"])) { personas.idUsuarioCreacion = Convert.ToInt32(reader["idUsuarioCreacion"]); }
                        if (!Convert.IsDBNull(reader["fechaCreacion"])) { personas.fechaCreacion = DateTime.Now; }
                        if (!Convert.IsDBNull(reader["idusuarioModificacion"])) { personas.idusuarioModificacion = Convert.ToInt32(reader["idusuarioModificacion"]); }
                        if (!Convert.IsDBNull(reader["fechaModificacion"])) { personas.fechaModificacion = DateTime.Now; }

                        resultado.Add(personas);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Se genero un error al consultar las personas: " + ex.Message);
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
