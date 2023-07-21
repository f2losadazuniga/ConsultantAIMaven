using ConnectionManagement.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ConsultantAIMavenSharedModel.Perfil;
namespace LogicaNegocioServicio.Perfil
{
    public class PerfilesDal
    {
        public string ConectionString { get; set; }
        public PerfilesDal(string conectionString)
        {
            ConectionString = conectionString;
        }

        public async Task<List<Perfiles>> GetAllPerfiles()
        {
            List<Perfiles> resultado = new List<Perfiles>();
            Connection db = new Connection(ConectionString);
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                
                var reader = await db.EjecutarReaderAsync("ConsultaPerfiles", CommandType.StoredProcedure);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var perfiles = new Perfiles();

                        perfiles.idPerfil = Convert.ToInt32(reader["idPerfil"]); 
                        if (!Convert.IsDBNull(reader["perfil"])) { perfiles.perfil= reader["perfil"].ToString(); }
      
                        resultado.Add(perfiles);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Se genero un error al consultar los perfiles: " + ex.Message);
            }
            finally { }
            if (db != null)
            {
                db.Dispose();
            }

            return resultado;
        }

        public async Task<List<Perfiles>> InsertPerfil( PerfilNew PerfilNuevo)
        {
            List<Perfiles> resultado = new List<Perfiles>();
            Connection db = new Connection(ConectionString);
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                //if (!String.IsNullOrWhiteSpace(UsuNuevo.numeroIdentificacion)){db.SQLParametros.Add("@numeroIdentificacion", SqlDbType.VarChar, 50).Value = UsuNuevo.numeroIdentificacion;}
                //if (!String.IsNullOrWhiteSpace(UsuNuevo.nombreApellido)){db.SQLParametros.Add("@nombreApellido", SqlDbType.VarChar, 100).Value = UsuNuevo.nombreApellido;}
                //if (!String.IsNullOrWhiteSpace(UsuNuevo.telefonos)) { db.SQLParametros.Add("@telefonos", SqlDbType.VarChar, 100).Value = UsuNuevo.telefonos; }
                //if (!String.IsNullOrWhiteSpace(UsuNuevo.email)) { db.SQLParametros.Add("@email", SqlDbType.VarChar, 100).Value = UsuNuevo.email; }
                //db.SQLParametros.Add("@idCiudad", SqlDbType.Int, 100).Value = UsuNuevo.idCiudad; 
                //if (!String.IsNullOrWhiteSpace(UsuNuevo.usuario)) { db.SQLParametros.Add("@usuario", SqlDbType.VarChar, 100).Value = UsuNuevo.usuario; }
                //if (!String.IsNullOrWhiteSpace(UsuNuevo.pwd)) { db.SQLParametros.Add("@pwd", SqlDbType.VarChar, 100).Value = UsuNuevo.pwd; }
                //db.SQLParametros.Add("@idEstado", SqlDbType.Int, 100).Value = UsuNuevo.idEstado;
                //db.SQLParametros.Add("@bloqueado", SqlDbType.Bit).Value = UsuNuevo.bloqueado;
                //db.SQLParametros.Add("@fechaCreacion", SqlDbType.DateTime).Value = UsuNuevo.fechaCreacion;
                //db.SQLParametros.Add("@idUsuarioCreacion", SqlDbType.Int, 100).Value = UsuNuevo.idUsuarioCreacion;
                //db.SQLParametros.Add("@idusuarioModifica", SqlDbType.Int, 100).Value = UsuNuevo.idusuarioModifica;
                //db.SQLParametros.Add("@fechaModificacion", SqlDbType.DateTime).Value = UsuNuevo.fechaModificacion;

                var reader = await db.EjecutarReaderAsync("InsertarPerfil", CommandType.StoredProcedure);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var perfiles = new Perfiles();

                        //if (!Convert.IsDBNull(reader["idUsuario"])) { usuarios.IdUsuario = Convert.ToInt32(reader["idUsuario"]); }
                        //if (!Convert.IsDBNull(reader["numeroIdentificacion"])) { usuarios.numeroIdentificacion = reader["numeroIdentificacion"].ToString(); }
                        //if (!Convert.IsDBNull(reader["nombreApellido"])) { usuarios.nombreApellido = reader["nombreApellido"].ToString(); }
                        //if (!Convert.IsDBNull(reader["telefonos"])) { usuarios.telefonos = reader["telefonos"].ToString(); }
                        //if (!Convert.IsDBNull(reader["email"])) { usuarios.email = reader["email"].ToString(); }
                        //if (!Convert.IsDBNull(reader["idCiudad"])) { usuarios.idCiudad = Convert.ToInt32(reader["idCiudad"]); }
                        //if (!Convert.IsDBNull(reader["usuario"])) { usuarios.usuario = reader["usuario"].ToString(); }
                        //if (!Convert.IsDBNull(reader["pwd"])) { usuarios.pwd = reader["pwd"].ToString(); }
                        //if (!Convert.IsDBNull(reader["idEstado"])) { usuarios.idEstado = Convert.ToInt32(reader["idEstado"]); }
                        //if (!Convert.IsDBNull(reader["bloqueado"])) { usuarios.bloqueado = Convert.ToBoolean(reader["bloqueado"]); }
                        //if (!Convert.IsDBNull(reader["fechaCreacion"])) { usuarios.fechaCreacion = Convert.ToDateTime(reader["fechaCreacion"]); }
                        //if (!Convert.IsDBNull(reader["idUsuarioCreacion"])) { usuarios.idUsuarioCreacion = Convert.ToInt32(reader["idUsuarioCreacion"]); }
                        //if (!Convert.IsDBNull(reader["idusuarioModifica"])) { usuarios.idusuarioModifica = Convert.ToInt32(reader["idusuarioModifica"]); }
                        //if (!Convert.IsDBNull(reader["fechaModificacion"])) { usuarios.fechaModificacion = Convert.ToDateTime(reader["fechaModificacion"]); }
                        //if (!Convert.IsDBNull(reader["fechaultimoIngreso"])) { usuarios.fechaultimoIngreso = Convert.ToDateTime(reader["fechaultimoIngreso"]); }
                        //if (!Convert.IsDBNull(reader["fechaCambioClave"])) { usuarios.fechaCambioClave = Convert.ToDateTime(reader["fechaCambioClave"]); }

                        resultado.Add(perfiles);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Se genero un error al insertarel perfil: " + ex.Message);
            }
            finally { }
            if (db != null)
            {
                db.Dispose();
            }

            return resultado;
        }

        public async Task<List<UsuarioPerfil>> InsertUsuarioPerfil(UsuarioPerfil UsuarioPerfilNuevo)
        {
            List<UsuarioPerfil> resultado = new List<UsuarioPerfil>();
            Connection db = new Connection(ConectionString);
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                if (!String.IsNullOrWhiteSpace(UsuarioPerfilNuevo.idUsuario.ToString())){db.SQLParametros.Add("@idUsuario", SqlDbType.Int, 50).Value = UsuarioPerfilNuevo.idUsuario;}
                if (!String.IsNullOrWhiteSpace(UsuarioPerfilNuevo.idPerfil.ToString())){db.SQLParametros.Add("@idPerfil", SqlDbType.Int, 100).Value = UsuarioPerfilNuevo.idPerfil;}
                if (!String.IsNullOrWhiteSpace(UsuarioPerfilNuevo.idUsuarioCreacion.ToString())) { db.SQLParametros.Add("@idUsuarioCreacion", SqlDbType.Int, 100).Value = UsuarioPerfilNuevo.idUsuarioCreacion; }
                if (!String.IsNullOrWhiteSpace(UsuarioPerfilNuevo.fechaCreacion.ToString())) { db.SQLParametros.Add("@fechaCreacion", SqlDbType.DateTime, 100).Value = DateTime.Now; }

                var reader = await db.EjecutarReaderAsync("InsertarUsuarioPerfil", CommandType.StoredProcedure);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var perfiles = new UsuarioPerfil();

                        if (!Convert.IsDBNull(reader["idUsuario"])) { perfiles.idUsuario = Convert.ToInt32(reader["idUsuario"]); }
                        if (!Convert.IsDBNull(reader["idPerfil"])) { perfiles.idPerfil = Convert.ToInt32(reader["idPerfil"]); }
                        if (!Convert.IsDBNull(reader["idUsuarioCreacion"])) { perfiles.idUsuarioCreacion = Convert.ToInt32(reader["idUsuarioCreacion"]); }
                        if (!Convert.IsDBNull(reader["fechaCreacion"])) { perfiles.fechaCreacion = Convert.ToDateTime(reader["fechaCreacion"]); }
                       
                        resultado.Add(perfiles);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Se genero un error al insertar el Usuarioperfil: " + ex.Message);
            }
            finally { }
            if (db != null)
            {
                db.Dispose();
            }

            return resultado;
        }

        public async Task<List<UsuarioPerfil>> UpdateUsuarioPerfil(int idUsuarioPerfil, UsuarioPerfil UsuPerfilNuevo)
        {
            List<UsuarioPerfil> resultado = new List<UsuarioPerfil>();
            Connection db = new Connection(ConectionString);
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                db.SQLParametros.Add("@idUsuarioPerfil", SqlDbType.Int, 100).Value = Convert.ToInt32(idUsuarioPerfil);
                if (!String.IsNullOrWhiteSpace(UsuPerfilNuevo.idUsuario.ToString())) { db.SQLParametros.Add("@idUsuario", SqlDbType.Int, 50).Value = UsuPerfilNuevo.idUsuario; }
                if (!String.IsNullOrWhiteSpace(UsuPerfilNuevo.idPerfil.ToString())) { db.SQLParametros.Add("@idPerfil", SqlDbType.Int, 100).Value = UsuPerfilNuevo.idPerfil; }
                if (!String.IsNullOrWhiteSpace(UsuPerfilNuevo.idUsuarioModificacion.ToString().ToString())) { db.SQLParametros.Add("@idUsuarioModificacion", SqlDbType.Int, 100).Value = UsuPerfilNuevo.idUsuarioModificacion; }
                if (!String.IsNullOrWhiteSpace(UsuPerfilNuevo.fechaModificacion.ToString())) { db.SQLParametros.Add("@fechaModificacion", SqlDbType.DateTime, 100).Value = DateTime.Now; }

                var reader = await db.EjecutarReaderAsync("ActualizarUsuarioPerfil", CommandType.StoredProcedure);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var usuarios = new UsuarioPerfil();

                        if (!Convert.IsDBNull(reader["idUsuario"])) { usuarios.idUsuario = Convert.ToInt32(reader["idUsuario"]); }
                        if (!Convert.IsDBNull(reader["idPerfil"])) { usuarios.idPerfil = Convert.ToInt32(reader["idPerfil"]); }
                        if (!Convert.IsDBNull(reader["idUsuarioModificacion"])) { usuarios.idUsuarioModificacion = Convert.ToInt32(reader["idUsuarioModificacion"]); }
                        if (!Convert.IsDBNull(reader["fechaModificacion"])) { usuarios.fechaModificacion = Convert.ToDateTime(reader["fechaModificacion"]); }

                        resultado.Add(usuarios);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Se genero un error al actualizar el usuarioPerfil: " + ex.Message);
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
