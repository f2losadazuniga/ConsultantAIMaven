using ConnectionManagement.Data;
using ConsultantAIMavenSharedModel.Comunes;
using ConsultantAIMavenSharedModel.Usuarios;
using LogicaNegocioServicio.Comunes;
using LogicaNegocioServicio.Usuarios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.Autenticacion
{
    public class UsuarioDal
    {
        public string ConectionString { get; set; }
        public UsuarioDal(string conectionString)
        {
            ConectionString = conectionString;
        }

        public async Task<UserToken> ValidarUsuarioToken(UsuarioLogin infoUsaurio)
        {
            Connection db = new Connection(ConectionString);
            UserToken usuario = new UserToken();
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                if (!String.IsNullOrWhiteSpace(infoUsaurio.Usuario))
                {
                    db.SQLParametros.Add("@usuario", SqlDbType.VarChar, 50).Value = infoUsaurio.Usuario;

                }
                if (!String.IsNullOrWhiteSpace(infoUsaurio.Usuario))
                {
                    db.SQLParametros.Add("@pwd", SqlDbType.VarChar, 100).Value = infoUsaurio.Password;

                }
                var reader = await db.EjecutarReaderAsync("ValidarUsuarioSistemaToken", CommandType.StoredProcedure);
                if (reader != null)
                {
                    while (reader.Read())
                    {

                        if (!Convert.IsDBNull(reader["IdUsuario"])) {
                            if (!Convert.IsDBNull(reader["IdUsuario"])) { usuario.IdUsuario = int.Parse(reader["IdUsuario"].ToString()); }
                            if (!Convert.IsDBNull(reader["NombreApellido"])) { usuario.NombreApellido = reader["NombreApellido"].ToString(); }
                            if (!Convert.IsDBNull(reader["idPerfil"])) { usuario.IdPerfil = int.Parse(reader["idPerfil"].ToString()); }
                            if (!Convert.IsDBNull(reader["Perfil"])) { usuario.Perfil = reader["Perfil"].ToString(); }
                            if (!Convert.IsDBNull(reader["email"])) { usuario.Email = reader["email"].ToString(); }
                            if (!Convert.IsDBNull(reader["idEmpresa"])) { usuario.Empresa.IdEmpresa = int.Parse(reader["idEmpresa"].ToString()); }
                            if (!Convert.IsDBNull(reader["NombreEmpresa"])) { usuario.Empresa.Nombre = reader["NombreEmpresa"].ToString(); }
                            if (!Convert.IsDBNull(reader["fechaCambioClave"])) { usuario.FechaCambioClave = reader["fechaCambioClave"].ToString(); }

                        }



                    }
                }

            }

            catch (Exception ex)
            {
                throw new Exception("Se genero un error al consultar el usuario: " + ex.Message);
            }
            finally { }
            if (db != null)
            {
                db.Dispose();
            }

            return usuario;
        }

        public async Task<UserToken> ValidarUsuarioForgot(UserRequest usuario)
        {
            Connection db = new Connection(ConectionString);
            UserToken usuarioForgot = new UserToken();
            db.TiempoEsperaComando = 0;
            try
            {
                Guid g = Guid.NewGuid();
                var pws = UsuarioDal.GetSHA256(g.ToString());

                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                db.SQLParametros.Add("@pws", SqlDbType.VarChar, 150).Value = pws;


                if (!String.IsNullOrWhiteSpace(usuario.Usuario))
                {
                    db.SQLParametros.Add("@usuario", SqlDbType.VarChar, 50).Value = usuario.Usuario;

                }
                var reader = await db.EjecutarReaderAsync("ValidarUsuarioSistemaForgot", CommandType.StoredProcedure);
                if (reader != null)
                {
                    while (reader.Read())
                    {

                        if (!Convert.IsDBNull(reader["IdUsuario"]))
                        {
                            if (!Convert.IsDBNull(reader["IdUsuario"])) usuarioForgot.IdUsuario = int.Parse(reader["IdUsuario"].ToString());
                            if (!Convert.IsDBNull(reader["NombreApellido"])) usuarioForgot.NombreApellido = reader["NombreApellido"].ToString();
                            if (!Convert.IsDBNull(reader["idPerfil"])) usuarioForgot.IdPerfil = int.Parse(reader["idPerfil"].ToString());
                            if (!Convert.IsDBNull(reader["Perfil"])) usuarioForgot.Perfil = reader["Perfil"].ToString();
                            if (!Convert.IsDBNull(reader["email"])) usuarioForgot.Email = reader["email"].ToString();
                            if (!Convert.IsDBNull(reader["token"])) usuarioForgot.Token = reader["token"].ToString();
                          }
                    }
                }

            }

            catch (Exception ex)
            {
                throw new Exception("Se genero un error al consultar el usuario: " + ex.Message);
            }
            finally { }
            if (db != null)
            {
                db.Dispose();
            }

            return usuarioForgot;
        }

        public async Task<List<UserValidaToken>> ValidaTokenUrl(string token)
        {
            List<UserValidaToken> resultado = new List<UserValidaToken>();
            Connection db = new Connection(ConectionString);
            db.TiempoEsperaComando = 0;
            try
            {
                // Inicia validación de token
                if (!String.IsNullOrWhiteSpace(token))
                {
                    db.SQLParametros.Add("@token", SqlDbType.VarChar, 100).Value = token;
                }
                var reader = await db.EjecutarReaderAsync("ValidarTokenUsuario", CommandType.StoredProcedure);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var userValidaToken = new UserValidaToken();
                        if (!Convert.IsDBNull(reader["idUsuario"])) { userValidaToken.idUsuario = Convert.ToInt32(reader["idUsuario"]); }
                        if (!Convert.IsDBNull(reader["pwd"])) { userValidaToken.contrasenaActual = reader["pwd"].ToString(); }
                        if (!Convert.IsDBNull(reader["usuario"])) { userValidaToken.Usuario = reader["usuario"].ToString(); }
                        if (!Convert.IsDBNull(reader["result"])) { userValidaToken.result = Convert.ToInt32(reader["result"]); }
                        
                        resultado.Add(userValidaToken);
                    }
                }
            }

            catch (Exception ex)
            {
                throw new Exception("El token no es válido: " + ex.Message);
            }
            
            finally { }
            if (db != null)
            {
                db.Dispose();
            }

            return resultado;
        }

        public async Task<Users> ObtenerUsuarioPorId(int Id)
        {
            try
            {
                Connection db = new Connection(ConectionString);
                db.SQLParametros.Clear();
                db.SQLParametros.Add("@IdUser",SqlDbType.Int).Value = Id;
                var User = (await db.MapearClaseAsync<Users>("ConsultarUnUsuarioSistema")).FirstOrDefault();
                
                return User;
            }
            catch (Exception ex)
            {
                throw new Exception("Error obtener usuario: " + ex.Message);
            }
        }

        public async Task<List<UserValidaToken>> ActualizaNuevaContrasena(UserRecover userNew)
        {
            List<UserValidaToken> resultado = new List<UserValidaToken>();
            string password = userNew.pwd;
            int idUsuario = Convert.ToInt32(userNew.idUsuario);
            Connection db = new Connection(ConectionString);
            db.TiempoEsperaComando = 0;
            Regex letraMinus = new Regex(@"[a-z]");
            Regex letraMayus = new Regex(@"[A-Z]");
            Regex numeros = new Regex(@"[0-9]");
            Regex caracEsp = new Regex("[!\"#\\$%&'()*+,-./:;=?@\\[\\]^_`{|}~]");
            string longitudPass = "";
            string numeroPass = "";
            string mayusculaPass = "";
            string minusculaPass = "";
            string especialPass = "";
            string ultimoPass = "";
            string message = "";
            int passRepetido = 0;

            try
            {
                // Inicia validación de longitud, numero, mayúscula, minúscula, caracter especial y contraseñas anteriores
                if (!String.IsNullOrWhiteSpace(password))
                {
                    longitudPass = ConfigValues.seleccionarConfigValue("minLongitud", ConectionString);
                    numeroPass = ConfigValues.seleccionarConfigValue("numNumeros", ConectionString);
                    mayusculaPass = ConfigValues.seleccionarConfigValue("numMayusculas", ConectionString);
                    minusculaPass = ConfigValues.seleccionarConfigValue("numMinusculas", ConectionString);
                    especialPass = ConfigValues.seleccionarConfigValue("numEspeciales", ConectionString);
                    ultimoPass = ConfigValues.seleccionarConfigValue("NumValidacionUltimasContrasenas", ConectionString);
                }
                if (password.Length < Convert.ToInt32(longitudPass))
                {
                    message = "La contraseña debe tener mínimo 8 caractéres,";
                }
                if (!numeros.IsMatch(password) && numeros.Matches(password).Count() < Convert.ToInt32(numeroPass))
                {
                    message += " La contraseña debe tener " + numeroPass + " número(s),";
                }
                if (!letraMinus.IsMatch(password) && letraMinus.Matches(password).Count() < Convert.ToInt32(minusculaPass))
                {
                    message += " La contraseña debe tener  " + minusculaPass + " letra(s) minúscula(s),";
                }
                if (!letraMayus.IsMatch(password) && letraMayus.Matches(password).Count() < Convert.ToInt32(mayusculaPass))
                {
                    message += " La contraseña debe tener  " + mayusculaPass + " letra(s) mayúscula(s),";
                }
                if (!caracEsp.IsMatch(password) && caracEsp.Matches(password).Count() < Convert.ToInt32(especialPass))
                {
                    message += " La contraseña debe tener  " + especialPass + " caracter(es) especial(es)";
                }
                password = UsuarioDal.GetSHA256(password);
                db.SQLParametros.Add("@idUsuario", SqlDbType.Int, 100).Value = idUsuario;
                db.SQLParametros.Add("@pwd", SqlDbType.VarChar, 200).Value = password;
                var readerPass = db.EjecutarScalar("SP_ValidarContrasenaNoIgualAnteriores", CommandType.StoredProcedure);
                if (readerPass != null)
                {
                   passRepetido = Convert.ToInt32(readerPass); 
                }
                if (passRepetido == 1)
                {
                    message += " La contraseña no puede ser igual a la(s) última(s)  " + ultimoPass + " contraseña(s) anterior(es)";
                }
                if (message == "" || message == null)
                {
                    db.SQLParametros.Clear();
                    db.SQLParametros.Add("@Token", SqlDbType.VarChar).Value = userNew.token;
                    var UserData = await db.MapearClaseAsync<Users>("ConsultarUnUsuarioSistemaPorToken");   
                    if(UserData.FirstOrDefault() != null && UserData.FirstOrDefault().fechaExpiracionToken > DateTime.Now)
                    {
                        db.SQLParametros.Clear();
                        db.SQLParametros.Add("@idUsuario", SqlDbType.Int, 100).Value = UserData.FirstOrDefault().IdUsuario;
                        db.SQLParametros.Add("@pwd", SqlDbType.VarChar, 200).Value = password;
                        var reader = await db.EjecutarReaderAsync("CambioContrasenaForgot", CommandType.StoredProcedure);
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                var userValidaToken = new UserValidaToken();
                                if (!Convert.IsDBNull(reader["idUsuario"])) { userValidaToken.idUsuario = Convert.ToInt32(reader["idUsuario"]); }
                                if (!Convert.IsDBNull(reader["usuario"])) { userValidaToken.Usuario = reader["usuario"].ToString(); }
                                if (!Convert.IsDBNull(reader["resultado"])) { userValidaToken.result = Convert.ToInt32(reader["resultado"]); }
                                if (!Convert.IsDBNull(message)) { userValidaToken.message = "La contraseña se ha cambiado con éxito"; }

                                resultado.Add(userValidaToken);
                            }
                        }
                    }
                    else
                    {
                        resultado.Add(new UserValidaToken { message = "Token expirado" });
                    }
                    
                }
                else
                {
                    resultado.Add(new UserValidaToken { message = message });
                }

            }

            catch (Exception ex)
            {
                throw new Exception("El token no es válido: " + ex.Message);
            }

            finally { }
            if (db != null)
            {
                db.Dispose();
            }

            return resultado;
        }

        public static string GetSHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        public async Task<RespuestaServicio> CambioClaveUsuario(CambioClaveUsuario cambioClaveUsuario)
        {
            RespuestaServicio resultado = new RespuestaServicio();
            Connection db = new Connection(ConectionString);
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                db.IniciarTransaccion();
                db.SQLParametros.Add("@idUsuario", SqlDbType.Int).Value = cambioClaveUsuario.IdUsuario;
                db.SQLParametros.Add("@pwd", SqlDbType.VarChar, 200).Value = cambioClaveUsuario.NuevaClave;
                db.SQLParametros.Add("@Mensaje", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                db.SQLParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue;
                await db.EjecutarNonQueryAsync("CambioContrasenaForgot", CommandType.StoredProcedure);
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
                throw new Exception("Se generó un error al registrar el producto: " + ex.Message);
            }
            finally
            {
                db.CerrarConexion();
                if (db != null)
                {
                    db.Dispose();
                }
            }

            return resultado;
        }

        public async Task<RespuestaServicio> CambioClaveUsuarioNew(CambioClaveUsuario cambioClaveUsuario)
        {
            RespuestaServicio resultado = new RespuestaServicio();
            Connection db = new Connection(ConectionString);
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                db.IniciarTransaccion();
                db.SQLParametros.Add("@usuario", SqlDbType.VarChar, 250).Value = cambioClaveUsuario.Usuario;
                db.SQLParametros.Add("@identificacion", SqlDbType.VarChar, 250).Value = cambioClaveUsuario.Identificacion;
                db.SQLParametros.Add("@pwd", SqlDbType.VarChar, 200).Value = cambioClaveUsuario.NuevaClave;
                db.SQLParametros.Add("@Mensaje", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                db.SQLParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue;
                await db.EjecutarNonQueryAsync("CambioClave", CommandType.StoredProcedure);
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
                throw new Exception("Se generó un error al registrar el producto: " + ex.Message);
            }
            finally
            {
                db.CerrarConexion();
                if (db != null)
                {
                    db.Dispose();
                }
            }

            return resultado;
        }


    }
}
