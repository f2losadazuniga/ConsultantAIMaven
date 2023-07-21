using ConnectionManagement.Data;
using ConsultantAIMavenSharedModel.Comunes;
using ConsultantAIMavenSharedModel.Usuarios;
using EntregasLogyTechSharedModel.Perfil;
using LogicaNegocioServicio.Autenticacion;
using LogicaNegocioServicio.Comunes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;


namespace LogicaNegocioServicio.Usuarios
{
    public class UsersDal
    {
        public string ConectionString { get; set; }
        public UsersDal(string conectionString)
        {
            ConectionString = conectionString;
        }

        public async Task<List<Users>> GetAllUsers()
        {
            List<Users> resultado = new List<Users>();
            Connection db = new Connection(ConectionString);
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                var reader = await db.EjecutarReaderAsync("ConsultarUsuarioSistema", CommandType.StoredProcedure);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var usuarios = new Users();

                        if (!Convert.IsDBNull(reader["idUsuario"])) { usuarios.IdUsuario = Convert.ToInt32(reader["idUsuario"]); }
                        if (!Convert.IsDBNull(reader["numeroIdentificacion"])) { usuarios.numeroIdentificacion = reader["numeroIdentificacion"].ToString(); }
                        if (!Convert.IsDBNull(reader["nombreApellido"])) { usuarios.nombreApellido = reader["nombreApellido"].ToString(); }
                        if (!Convert.IsDBNull(reader["telefonos"])) { usuarios.telefonos = reader["telefonos"].ToString(); }
                        if (!Convert.IsDBNull(reader["email"])) { usuarios.email = reader["email"].ToString(); }
                        if (!Convert.IsDBNull(reader["idCiudad"])) { usuarios.idciudad = reader["idCiudad"].ToString(); }
                        if (!Convert.IsDBNull(reader["usuario"])) { usuarios.usuario = reader["usuario"].ToString(); }
                        if (!Convert.IsDBNull(reader["pwd"])) { usuarios.pwd = reader["pwd"].ToString(); }
                        if (!Convert.IsDBNull(reader["idEstado"])) { usuarios.idEstado = Convert.ToInt32(reader["idEstado"]); }
                        if (!Convert.IsDBNull(reader["bloqueado"])) { usuarios.bloqueado = Convert.ToBoolean(reader["bloqueado"]); }
                        if (!Convert.IsDBNull(reader["fechaCreacion"])) { usuarios.fechaCreacion = Convert.ToDateTime(reader["fechaCreacion"]); }
                        if (!Convert.IsDBNull(reader["idUsuarioCreacion"])) { usuarios.idUsuarioCreacion = Convert.ToInt32(reader["idUsuarioCreacion"]); }
                        if (!Convert.IsDBNull(reader["idusuarioModifica"])) { usuarios.idusuarioModifica = Convert.ToInt32(reader["idusuarioModifica"]); }
                        if (!Convert.IsDBNull(reader["fechaModificacion"])) { usuarios.fechaModificacion = Convert.ToDateTime(reader["fechaModificacion"]); }
                        if (!Convert.IsDBNull(reader["fechaultimoIngreso"])) { usuarios.fechaultimoIngreso = Convert.ToDateTime(reader["fechaultimoIngreso"]); }
                        if (!Convert.IsDBNull(reader["fechaCambioClave"])) { usuarios.fechaCambioClave = reader["fechaCambioClave"].ToString(); }

                        try
                        {
                            Connection dbsub = new Connection(ConectionString);
                            dbsub.SQLParametros.Clear();
                            dbsub.SQLParametros.Add("@idUsuario", SqlDbType.Int).Value = usuarios.IdUsuario;
                            usuarios.Perfil = (await dbsub.MapearObjetoAsync<PerfilInfo>("ConsultarUsuarioSistemaPerfil"));

                        }
                        catch (Exception ex)
                        {

                            throw new Exception("Se genero un error al consultar los perfiles: " + ex.Message);
                        }

                        resultado.Add(usuarios);
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

        public async Task<List<Usuario>> ListaUsuarios()
        {
            List<Usuario> resultado = new List<Usuario>();
            Connection db = new Connection(ConectionString);
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                var reader = await db.EjecutarReaderAsync("ConsultarUsuarioSistema", CommandType.StoredProcedure);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var usuarios = new Usuario();

                        if (!Convert.IsDBNull(reader["idUsuario"])) { usuarios.IdUsuario = Convert.ToInt32(reader["idUsuario"]); }
                        if (!Convert.IsDBNull(reader["numeroIdentificacion"])) { usuarios.numeroIdentificacion = reader["numeroIdentificacion"].ToString(); }
                        if (!Convert.IsDBNull(reader["nombreApellido"])) { usuarios.nombreApellido = reader["nombreApellido"].ToString(); }
                        if (!Convert.IsDBNull(reader["telefonos"])) { usuarios.telefonos = reader["telefonos"].ToString(); }
                        if (!Convert.IsDBNull(reader["email"])) { usuarios.email = reader["email"].ToString(); }
                        if (!Convert.IsDBNull(reader["idCiudad"])) { usuarios.idciudad = Convert.ToInt32(reader["idCiudad"].ToString()); }
                        if (!Convert.IsDBNull(reader["usuario"])) { usuarios.usuario = reader["usuario"].ToString(); }
                        if (!Convert.IsDBNull(reader["pwd"])) { usuarios.pwd = reader["pwd"].ToString(); }
                        if (!Convert.IsDBNull(reader["idEstado"])) { usuarios.IdEstado = Convert.ToInt32(reader["idEstado"]); }
                        if (!Convert.IsDBNull(reader["bloqueado"])) { usuarios.Bloqueado = Convert.ToBoolean(reader["bloqueado"]); }
                        if (!Convert.IsDBNull(reader["idPerfil"])) { usuarios.IdPerfil = Convert.ToInt32(reader["idPerfil"]); }
                        if (!Convert.IsDBNull(reader["idEmpresa"])) { usuarios.IdEmpresa = Convert.ToInt32(reader["idEmpresa"]); }

                        resultado.Add(usuarios);
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

        public async Task<List<Usuario>> ListaUsuariosFiltrado(Usuario BusquedaUsuario)
        {
            List<Usuario> resultado = new List<Usuario>();
            Connection db = new Connection(ConectionString);
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                if (!String.IsNullOrWhiteSpace(BusquedaUsuario.numeroIdentificacion)) { db.SQLParametros.Add("@numeroIdentificacion", SqlDbType.VarChar, 50).Value = BusquedaUsuario.numeroIdentificacion; }
                if (!String.IsNullOrWhiteSpace(BusquedaUsuario.usuario)) { db.SQLParametros.Add("@usuario", SqlDbType.VarChar, 50).Value = BusquedaUsuario.usuario; }
                if (!String.IsNullOrWhiteSpace(BusquedaUsuario.nombreApellido)) { db.SQLParametros.Add("@nombreApellido", SqlDbType.VarChar, 50).Value = BusquedaUsuario.nombreApellido; }

                var reader = await db.EjecutarReaderAsync("ConsultarUsuarioSistemaFiltros", CommandType.StoredProcedure);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var usuarios = new Usuario();

                        if (!Convert.IsDBNull(reader["idUsuario"])) { usuarios.IdUsuario = Convert.ToInt32(reader["idUsuario"]); }
                        if (!Convert.IsDBNull(reader["numeroIdentificacion"])) { usuarios.numeroIdentificacion = reader["numeroIdentificacion"].ToString(); }
                        if (!Convert.IsDBNull(reader["nombreApellido"])) { usuarios.nombreApellido = reader["nombreApellido"].ToString(); }
                        if (!Convert.IsDBNull(reader["telefonos"])) { usuarios.telefonos = reader["telefonos"].ToString(); }
                        if (!Convert.IsDBNull(reader["email"])) { usuarios.email = reader["email"].ToString(); }
                        if (!Convert.IsDBNull(reader["Ciudad"])) { usuarios.Ciudad = reader["Ciudad"].ToString(); }
                        if (!Convert.IsDBNull(reader["usuario"])) { usuarios.usuario = reader["usuario"].ToString(); }
                        if (!Convert.IsDBNull(reader["idEstado"])) { usuarios.IdEstado = Convert.ToInt32(reader["idEstado"]); }
                        if (!Convert.IsDBNull(reader["Perfil"])) { usuarios.Perfil = reader["Perfil"].ToString(); }
                        if (!Convert.IsDBNull(reader["Empresa"])) { usuarios.Empresa = reader["Empresa"].ToString(); }

                        resultado.Add(usuarios);
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

        public async Task<RespuestaServicio> CrearUsuario(Usuario Usuario)
        {
            Connection db = new Connection(ConectionString);
            RespuestaServicio resultado = new RespuestaServicio();
            string defaultPassword = UsuarioDal.GetSHA256(Usuario.numeroIdentificacion);
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                db.IniciarTransaccion();
                if (!String.IsNullOrWhiteSpace(Usuario.numeroIdentificacion)) { db.SQLParametros.Add("@numeroIdentificacion", SqlDbType.VarChar, 50).Value = Usuario.numeroIdentificacion; }
                if (!String.IsNullOrWhiteSpace(Usuario.nombreApellido)) { db.SQLParametros.Add("@nombreApellido", SqlDbType.VarChar, 100).Value = Usuario.nombreApellido; }
                if (!String.IsNullOrWhiteSpace(Usuario.telefonos)) { db.SQLParametros.Add("@telefonos", SqlDbType.VarChar, 100).Value = Usuario.telefonos; }
                if (!String.IsNullOrWhiteSpace(Usuario.email)) { db.SQLParametros.Add("@email", SqlDbType.VarChar, 100).Value = Usuario.email; }
                if (!String.IsNullOrWhiteSpace(Usuario.usuario)) { db.SQLParametros.Add("@usuario", SqlDbType.VarChar, 100).Value = Usuario.usuario; }
                if (!String.IsNullOrWhiteSpace(defaultPassword)) { db.SQLParametros.Add("@pwd", SqlDbType.VarChar, 100).Value = defaultPassword; }
                db.SQLParametros.Add("@idCiudad", SqlDbType.VarChar, 100).Value = Usuario.idciudad;
                db.SQLParametros.Add("@idEstado", SqlDbType.Int, 100).Value = 1;
                db.SQLParametros.Add("@idPerfil", SqlDbType.Int, 100).Value = Convert.ToInt32(Usuario.IdPerfil);
                db.SQLParametros.Add("@bloqueado", SqlDbType.Bit).Value = false;// UsuNuevo.bloqueado;
                db.SQLParametros.Add("@fechaCreacion", SqlDbType.DateTime).Value = DateTime.Now;
                db.SQLParametros.Add("@idUsuarioCreacion", SqlDbType.Int, 100).Value = Convert.ToInt32(Usuario.IdUsuario);
                db.SQLParametros.Add("@idusuarioModifica", SqlDbType.Int, 100).Value = Convert.ToInt32(Usuario.IdUsuario);
                db.SQLParametros.Add("@fechaModificacion", SqlDbType.DateTime).Value = DateTime.Now;
                db.SQLParametros.Add("@idEmpresa", SqlDbType.Int).Value = Usuario.IdEmpresa;
                db.SQLParametros.Add("@Mensaje", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                db.SQLParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue;

                await db.EjecutarNonQueryAsync("InsertarUsuarioSistema", CommandType.StoredProcedure);
                resultado.Codigo = db.SQLParametros["@returnValue"].Value.ToString();
                resultado.Mensaje = db.SQLParametros["@Mensaje"].Value.ToString();

                if (resultado.Codigo == "200")
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

        public async Task<List<RespuestaServicio>> CrearUsuariosMasivos(List<UsuariosMasivos> Usuario, int IdUsuario)
        {
            Connection db = new Connection(ConectionString);
            List<RespuestaServicio> resultado = new List<RespuestaServicio>();
            DataTable dtUsuarios = new DataTable();
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                db.IniciarTransaccion();

                foreach (var items in Usuario)
                {
                    items.IdUsuario = IdUsuario;
                }

                dtUsuarios = ConvertirObjetoListToDataTable.ConvertToDataTable(Usuario);
                db.SQLParametros.Add(new SqlParameter("@ListaCreacionMasivos", dtUsuarios));
                db.SQLParametros.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;

                var reader = await db.EjecutarReaderAsync("CrearUsuariosMasivos", CommandType.StoredProcedure);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        RespuestaServicio respuestaServicio = new RespuestaServicio();
                        if (!Convert.IsDBNull(reader["Fila"])) { respuestaServicio.Codigo = reader["Fila"].ToString(); }
                        if (!Convert.IsDBNull(reader["Descripcion"])) { respuestaServicio.Mensaje = reader["Descripcion"].ToString(); }
                        resultado.Add(respuestaServicio);
                    }
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

        public async Task<RespuestaServicio> ActualizarUsuario(Usuario Usuario)
        {

            RespuestaServicio resultado = new RespuestaServicio();
            Connection db = new Connection(ConectionString);
            db.TiempoEsperaComando = 0;
            DataTable dtDetalle = new DataTable();
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                db.IniciarTransaccion();
                db.SQLParametros.Add("@idUsuario", SqlDbType.Int, 100).Value = Convert.ToInt32(Usuario.IdUsuario);
                if (!String.IsNullOrWhiteSpace(Usuario.numeroIdentificacion)) { db.SQLParametros.Add("@numeroIdentificacion", SqlDbType.VarChar, 50).Value = Usuario.numeroIdentificacion; }
                if (!String.IsNullOrWhiteSpace(Usuario.nombreApellido)) { db.SQLParametros.Add("@nombreApellido", SqlDbType.VarChar, 100).Value = Usuario.nombreApellido; }
                if (!String.IsNullOrWhiteSpace(Usuario.telefonos)) { db.SQLParametros.Add("@telefonos", SqlDbType.VarChar, 100).Value = Usuario.telefonos; }
                if (!String.IsNullOrWhiteSpace(Usuario.email)) { db.SQLParametros.Add("@email", SqlDbType.VarChar, 100).Value = Usuario.email; }
                db.SQLParametros.Add("@idCiudad", SqlDbType.VarChar, 100).Value = Usuario.idciudad;
                db.SQLParametros.Add("@IdEmpresa", SqlDbType.VarChar, 100).Value = Usuario.IdEmpresa;
                db.SQLParametros.Add("@IdPerfil", SqlDbType.VarChar, 100).Value = Usuario.IdPerfil;
                if (!String.IsNullOrWhiteSpace(Usuario.usuario)) { db.SQLParametros.Add("@usuario", SqlDbType.VarChar, 100).Value = Usuario.usuario; }
                db.SQLParametros.Add("@idEstado", SqlDbType.Int, 100).Value = Convert.ToInt32(Usuario.IdEstado);
                db.SQLParametros.Add("@idusuarioModifica", SqlDbType.Int, 100).Value = Convert.ToInt32(Usuario.IdUsuarioModifica);
                db.SQLParametros.Add("@fechaModificacion", SqlDbType.DateTime).Value = DateTime.Now;
                db.SQLParametros.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                db.SQLParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue;

                db.EjecutarNonQuery("ActualizarUsuarioSistema", CommandType.StoredProcedure);
                resultado.Codigo = db.SQLParametros["@returnValue"].Value.ToString();
                resultado.Mensaje = db.SQLParametros["@Mensaje"].Value.ToString();

                if (resultado.Codigo == "200")
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
                throw new Exception("Se genero un error al actualizar el usuario: " + ex.Message);
            }
            finally { }
            if (db != null)
            {
                db.Dispose();
            }

            return resultado;
        }

        public async Task<List<Users>> InsertUsers(UserNewInsert UsuNuevo, int idEmpresa)
        {
            List<Users> resultado = new List<Users>();
            Connection db = new Connection(ConectionString);
            string defaultPassword = UsuarioDal.GetSHA256("12345");// Juan Mejia: Password por defecto cuando se crea un usuario por primera vez
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                if (!String.IsNullOrWhiteSpace(UsuNuevo.numeroIdentificacion)) { db.SQLParametros.Add("@numeroIdentificacion", SqlDbType.VarChar, 50).Value = UsuNuevo.numeroIdentificacion; }
                if (!String.IsNullOrWhiteSpace(UsuNuevo.nombreApellido)) { db.SQLParametros.Add("@nombreApellido", SqlDbType.VarChar, 100).Value = UsuNuevo.nombreApellido; }
                if (!String.IsNullOrWhiteSpace(UsuNuevo.telefonos)) { db.SQLParametros.Add("@telefonos", SqlDbType.VarChar, 100).Value = UsuNuevo.telefonos; }
                if (!String.IsNullOrWhiteSpace(UsuNuevo.email)) { db.SQLParametros.Add("@email", SqlDbType.VarChar, 100).Value = UsuNuevo.email; }
                if (!String.IsNullOrWhiteSpace(UsuNuevo.idCiudad)) { db.SQLParametros.Add("@idCiudad", SqlDbType.VarChar, 100).Value = UsuNuevo.idCiudad; }
                if (!String.IsNullOrWhiteSpace(UsuNuevo.usuario)) { db.SQLParametros.Add("@usuario", SqlDbType.VarChar, 100).Value = UsuNuevo.usuario; }
                if (!String.IsNullOrWhiteSpace(defaultPassword)) { db.SQLParametros.Add("@pwd", SqlDbType.VarChar, 100).Value = defaultPassword; }
                db.SQLParametros.Add("@idEstado", SqlDbType.Int, 100).Value = 1;
                //db.SQLParametros.Add("@idPerfil", SqlDbType.Int, 100).Value = Convert.ToInt32(UsuNuevo.idPerfil);
                db.SQLParametros.Add("@bloqueado", SqlDbType.Bit).Value = false;// UsuNuevo.bloqueado;
                db.SQLParametros.Add("@fechaCreacion", SqlDbType.DateTime).Value = DateTime.Now;
                db.SQLParametros.Add("@idUsuarioCreacion", SqlDbType.Int, 100).Value = Convert.ToInt32(UsuNuevo.idusuarioModifica);
                db.SQLParametros.Add("@idusuarioModifica", SqlDbType.Int, 100).Value = Convert.ToInt32(UsuNuevo.idusuarioModifica);
                db.SQLParametros.Add("@fechaModificacion", SqlDbType.DateTime).Value = DateTime.Now;
                db.SQLParametros.Add("@idEmpresa", SqlDbType.Int).Value = idEmpresa;

                var reader = await db.EjecutarReaderAsync("InsertarUsuarioSistema", CommandType.StoredProcedure);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var usuarios = new Users();

                        if (!Convert.IsDBNull(reader["idUsuario"])) { usuarios.IdUsuario = Convert.ToInt32(reader["idUsuario"]); }
                        if (!Convert.IsDBNull(reader["numeroIdentificacion"])) { usuarios.numeroIdentificacion = reader["numeroIdentificacion"].ToString(); }
                        if (!Convert.IsDBNull(reader["nombreApellido"])) { usuarios.nombreApellido = reader["nombreApellido"].ToString(); }
                        if (!Convert.IsDBNull(reader["telefonos"])) { usuarios.telefonos = reader["telefonos"].ToString(); }
                        if (!Convert.IsDBNull(reader["email"])) { usuarios.email = reader["email"].ToString(); }
                        if (!Convert.IsDBNull(reader["idCiudad"])) { usuarios.idciudad = reader["idCiudad"].ToString(); }
                        if (!Convert.IsDBNull(reader["usuario"])) { usuarios.usuario = reader["usuario"].ToString(); }
                        if (!Convert.IsDBNull(reader["pwd"])) { usuarios.pwd = reader["pwd"].ToString(); }
                        if (!Convert.IsDBNull(reader["idEstado"])) { usuarios.idEstado = Convert.ToInt32(reader["idEstado"]); }
                        if (!Convert.IsDBNull(reader["bloqueado"])) { usuarios.bloqueado = Convert.ToBoolean(reader["bloqueado"]); }
                        if (!Convert.IsDBNull(reader["fechaCreacion"])) { usuarios.fechaCreacion = Convert.ToDateTime(reader["fechaCreacion"]); }
                        if (!Convert.IsDBNull(reader["idUsuarioCreacion"])) { usuarios.idUsuarioCreacion = Convert.ToInt32(reader["idUsuarioCreacion"]); }
                        if (!Convert.IsDBNull(reader["idusuarioModifica"])) { usuarios.idusuarioModifica = Convert.ToInt32(reader["idusuarioModifica"]); }
                        if (!Convert.IsDBNull(reader["fechaModificacion"])) { usuarios.fechaModificacion = Convert.ToDateTime(reader["fechaModificacion"]); }
                        if (!Convert.IsDBNull(reader["fechaultimoIngreso"])) { usuarios.fechaultimoIngreso = Convert.ToDateTime(reader["fechaultimoIngreso"]); }
                        if (!Convert.IsDBNull(reader["fechaCambioClave"])) { usuarios.fechaCambioClave = reader["fechaCambioClave"].ToString(); }

                        resultado.Add(usuarios);
                    }
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

        public async Task<Response> UpdateUsers(int idUsuario, UserNew UsuNuevo)
        {

            Response resultado = new Response();
            Connection db = new Connection(ConectionString);
            //var idPerfil = JsonSerializer.Deserialize<List<OptionJson>>(UsuNuevo.idPerfil);
            db.TiempoEsperaComando = 0;
            DataTable dtDetalle = new DataTable();
            try
            {
                if (UsuNuevo.idPerfil.Count > 0)
                {
                    dtDetalle = ConvertListToDataTable.ToDataTable1(UsuNuevo.idPerfil);

                }
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                db.SQLParametros.Add("@idUsuario", SqlDbType.Int, 100).Value = Convert.ToInt32(idUsuario);
                if (!String.IsNullOrWhiteSpace(UsuNuevo.numeroIdentificacion)) { db.SQLParametros.Add("@numeroIdentificacion", SqlDbType.VarChar, 50).Value = UsuNuevo.numeroIdentificacion; }
                if (!String.IsNullOrWhiteSpace(UsuNuevo.nombreApellido)) { db.SQLParametros.Add("@nombreApellido", SqlDbType.VarChar, 100).Value = UsuNuevo.nombreApellido; }
                if (!String.IsNullOrWhiteSpace(UsuNuevo.telefonos)) { db.SQLParametros.Add("@telefonos", SqlDbType.VarChar, 100).Value = UsuNuevo.telefonos; }
                if (!String.IsNullOrWhiteSpace(UsuNuevo.email)) { db.SQLParametros.Add("@email", SqlDbType.VarChar, 100).Value = UsuNuevo.email; }
                if (!String.IsNullOrWhiteSpace(UsuNuevo.idCiudad)) { db.SQLParametros.Add("@idCiudad", SqlDbType.VarChar, 100).Value = UsuNuevo.idCiudad; }
                if (!String.IsNullOrWhiteSpace(UsuNuevo.usuario)) { db.SQLParametros.Add("@usuario", SqlDbType.VarChar, 100).Value = UsuNuevo.usuario; }
                if (dtDetalle.Rows.Count > 0)
                {
                    db.SQLParametros.Add(new SqlParameter("@tbIdperfiles", dtDetalle));
                } //db.SQLParametros.Add("@idPerfil", SqlDbType.Int, 100).Value = Convert.ToInt32(UsuNuevo.idPerfil);
                db.SQLParametros.Add("@idEstado", SqlDbType.Int, 100).Value = Convert.ToInt32(UsuNuevo.idEstado);
                db.SQLParametros.Add("@idusuarioModifica", SqlDbType.Int, 100).Value = Convert.ToInt32(UsuNuevo.idusuarioModifica);
                db.SQLParametros.Add("@fechaModificacion", SqlDbType.DateTime).Value = DateTime.Now;
                db.SQLParametros.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                db.SQLParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue;

                db.EjecutarNonQuery("ActualizarUsuarioSistema", CommandType.StoredProcedure);
                resultado.Valor = Convert.ToInt16(db.SQLParametros["@returnValue"].Value);
                resultado.Message = db.SQLParametros["@Mensaje"].Value.ToString();
            }
            catch (Exception ex)
            {
                resultado.Valor = 100;
                resultado.Message = "Se genero un error al actualizar el usuario: " + ex.Message;
                throw new Exception("Se genero un error al actualizar el usuario: " + ex.Message);
            }
            finally { }
            if (db != null)
            {
                db.Dispose();
            }

            return resultado;
        }

        public async Task<List<Users>> InactiveUsers(int idUsuario, int idUsuarioModifica)
        {
            List<Users> resultado = new List<Users>();
            Connection db = new Connection(ConectionString);
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                db.SQLParametros.Add("@idUsuario", SqlDbType.Int, 100).Value = Convert.ToInt32(idUsuario);
                db.SQLParametros.Add("@idusuarioModifica", SqlDbType.Int, 100).Value = Convert.ToInt32(idUsuarioModifica);

                var reader = await db.EjecutarReaderAsync("InactivarUsuarioSistema", CommandType.StoredProcedure);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var usuarios = new Users();

                        if (!Convert.IsDBNull(reader["idUsuario"])) { usuarios.IdUsuario = Convert.ToInt32(reader["idUsuario"]); }
                        if (!Convert.IsDBNull(reader["numeroIdentificacion"])) { usuarios.numeroIdentificacion = reader["numeroIdentificacion"].ToString(); }
                        if (!Convert.IsDBNull(reader["nombreApellido"])) { usuarios.nombreApellido = reader["nombreApellido"].ToString(); }
                        if (!Convert.IsDBNull(reader["telefonos"])) { usuarios.telefonos = reader["telefonos"].ToString(); }
                        if (!Convert.IsDBNull(reader["email"])) { usuarios.email = reader["email"].ToString(); }
                        if (!Convert.IsDBNull(reader["idCiudad"])) { usuarios.idciudad = reader["idCiudad"].ToString(); }
                        if (!Convert.IsDBNull(reader["usuario"])) { usuarios.usuario = reader["usuario"].ToString(); }
                        if (!Convert.IsDBNull(reader["pwd"])) { usuarios.pwd = reader["pwd"].ToString(); }
                        if (!Convert.IsDBNull(reader["idEstado"])) { usuarios.idEstado = Convert.ToInt32(reader["idEstado"]); }
                        if (!Convert.IsDBNull(reader["bloqueado"])) { usuarios.bloqueado = Convert.ToBoolean(reader["bloqueado"]); }
                        if (!Convert.IsDBNull(reader["fechaCreacion"])) { usuarios.fechaCreacion = Convert.ToDateTime(reader["fechaCreacion"]); }
                        if (!Convert.IsDBNull(reader["idUsuarioCreacion"])) { usuarios.idUsuarioCreacion = Convert.ToInt32(reader["idUsuarioCreacion"]); }
                        if (!Convert.IsDBNull(reader["idusuarioModifica"])) { usuarios.idusuarioModifica = Convert.ToInt32(reader["idusuarioModifica"]); }
                        if (!Convert.IsDBNull(reader["fechaModificacion"])) { usuarios.fechaModificacion = Convert.ToDateTime(reader["fechaModificacion"]); }
                        if (!Convert.IsDBNull(reader["fechaultimoIngreso"])) { usuarios.fechaultimoIngreso = Convert.ToDateTime(reader["fechaultimoIngreso"]); }
                        if (!Convert.IsDBNull(reader["fechaCambioClave"])) { usuarios.fechaCambioClave = reader["fechaCambioClave"].ToString(); }

                        resultado.Add(usuarios);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Se genero un error al inactivar el usuario: " + ex.Message);
            }
            finally { }
            if (db != null)
            {
                db.Dispose();
            }

            return resultado;
        }

        //public async Task<RespuestaServicio> ConfirmaGestionMatorizado(EntregasMotorizado objEntregas, int idUsuario)
        //{
        //    RespuestaServicio resultado = new RespuestaServicio();
        //    DataTable dtDetalleEntrega = new DataTable();
        //    string cadenaConexioAz = ConfigValues.seleccionarConfigValue("CADENA_CONEXION_AZURE", ConectionString);
        //    string contenedoAz = ConfigValues.seleccionarConfigValue("CONTENEDOR_AZURE", ConectionString);

        //    Connection db = new Connection(ConectionString);
        //    try
        //    {
        //        if (objEntregas.Detalle.Count > 0)
        //        {
        //            try
        //            {
        //                Parallel.ForEach(objEntregas.Detalle, async (deta) =>
        //                {
        //                    if (!string.IsNullOrWhiteSpace(deta.ImageBase64))
        //                    {
        //                        string ruta = objEntregas.Sistema.ToLower() + "/" + objEntregas.Servicio;
        //                        AzureBlobStorageService ObjAzureBloc = new AzureBlobStorageService(cadenaConexioAz, contenedoAz);
        //                        deta.UrlImage = ObjAzureBloc.UploadImageAzure(deta.ImageBase64, ruta);
        //                        if (!string.IsNullOrWhiteSpace(deta.UrlImage))
        //                        {
        //                            deta.ImageBase64 = null;
        //                        }
        //                    }
        //                });
        //            }
        //            catch (Exception)
        //            {
        //            }

        //            dtDetalleEntrega = ConvertirObjetoListToDataTable.ConvertToDataTable(objEntregas.Detalle);
        //        }
        //        try
        //        {
        //            if (!string.IsNullOrWhiteSpace(objEntregas.ImageBase64))
        //            {
        //                AzureBlobStorageService ObjAzureBloc = new AzureBlobStorageService(cadenaConexioAz, contenedoAz);
        //                string ruta = objEntregas.Sistema.ToLower() + "/" + objEntregas.Servicio;
        //                objEntregas.UrlImage = await ObjAzureBloc.UploadImageAsync(objEntregas.ImageBase64, ruta);
        //                if (!string.IsNullOrWhiteSpace(objEntregas.UrlImage))
        //                {
        //                    objEntregas.ImageBase64 = null;
        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {

        //        }

        //        db.TiempoEsperaComando = 0;
        //        db.IniciarTransaccion();
        //        db.SQLParametros.Clear();
        //        db.SQLParametros.Add("@IdEntrega", SqlDbType.Decimal).Value = objEntregas.IdEntrega;
        //        db.SQLParametros.Add("@Idusuario", SqlDbType.Int).Value = idUsuario;
        //        db.SQLParametros.Add("@Latitud", SqlDbType.VarChar, 50).Value = objEntregas.Latitude;
        //        db.SQLParametros.Add("@Longitud", SqlDbType.VarChar, 50).Value = objEntregas.Longitude;
        //        if (objEntregas.IdNovedad > 0)
        //        {
        //            db.SQLParametros.Add("@IdNovedadEntrega", SqlDbType.Int).Value = objEntregas.IdNovedad;
        //        }
        //        db.SQLParametros.Add("@IdEstado", SqlDbType.Int).Value = objEntregas.IdEstado;
        //        //db.SQLParametros.Add("@ImageBase64", SqlDbType.Text, -1).Value = objEntregas.ImageBase64;
        //        db.SQLParametros.Add("@UrlImage", SqlDbType.VarChar, -1).Value = objEntregas.UrlImage;
        //        db.SQLParametros.Add("@Mensaje", SqlDbType.VarChar, 5000).Direction = ParameterDirection.Output;
        //        db.SQLParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue;
        //        if (dtDetalleEntrega.Rows.Count > 0)
        //        {
        //            db.SQLParametros.Add(new SqlParameter("@DetalleEntrega", dtDetalleEntrega));
        //        }
        //        await db.EjecutarNonQueryAsync("sp_ConfirmaGestionMatorizado", CommandType.StoredProcedure);
        //        resultado.Codigo = db.SQLParametros["@returnValue"].Value.ToString();
        //        resultado.Mensaje = db.SQLParametros["@Mensaje"].Value.ToString();
        //        if (resultado.Codigo == "200")
        //        {
        //            db.ConfirmarTransaccion();

        //            // Crear una copia independiente del objeto objEntregas para que no se vea afectados los hilos
        //            string jsonString = JsonSerializer.Serialize(objEntregas);
        //            EntregasMotorizado copiaObjEntregas = JsonSerializer.Deserialize<EntregasMotorizado>(jsonString);

        //            // Crear un nuevo hilo y pasar la copia del objeto
        //            Thread nuevoHilo = new Thread(() => ConfirmarEntregasPorSistema(copiaObjEntregas));
        //            nuevoHilo.Start();
        //        }
        //        else
        //        {
        //            db.AbortarTransaccion();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        db.AbortarTransaccion();
        //        resultado.Codigo = "500";
        //        resultado.Mensaje = "Error:" + ex.Message;
        //    }
        //    finally
        //    {
        //        db.CerrarConexion();
        //        if (db != null)
        //        {
        //            db.Dispose();
        //        }

        //    }
        //    return resultado;
        //}

    }
}
