using ConsultantAIMavenSharedModel.Comunes;
using ConsultantAIMavenSharedModel.Usuarios;
using LogicaNegocioServicio.Autenticacion;
using LogicaNegocioServicio.Comunes;
using LogicaNegocioServicio.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiIntegracionEntregasLogyTech.Controllers
{
    [ApiController]
    [Route("api/")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
         private readonly IConfiguration _ConnectionString;

        public UsersController( IConfiguration Configuration)
        {
            this._ConnectionString = Configuration;         
        }


        [HttpGet("Usuarios/{Id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Users>> GetUsuario(int Id)
        {
            var usersDal = new UsuarioDal(_ConnectionString.GetConnectionString("DefaultConnection"));
            var User = await usersDal.ObtenerUsuarioPorId(Id);
            return Ok(User);
        }


        [HttpGet("Usuarios")]
        public async Task<ActionResult<List<Users>>> GetUsuarios()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                List<Users> resultado = new List<Users>();
                UsersDal mnu = new UsersDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                resultado = await mnu.GetAllUsers();
                return resultado;
            }
            catch (Exception ex)
            {
                var emex = new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = "Error:  " + ex.Message.ToString()
                };
                return new JsonResult(emex);
            }
        }

        [HttpGet("ListaUsuarios")]
        public async Task<ActionResult<List<Usuario>>> ListaUsuarios()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                List<Usuario> resultado = new List<Usuario>();
                UsersDal mnu = new UsersDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                resultado = await mnu.ListaUsuarios();
                return resultado;
            }
            catch (Exception ex)
            {
                var emex = new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = "Error:  " + ex.Message.ToString()
                };
                return new JsonResult(emex);
            }
        }

        [HttpPost("ListaUsuariosFiltrado")]
        public async Task<ActionResult<List<Usuario>>> ListaUsuariosFiltrado([FromBody] Usuario usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                List<Usuario> resultado = new List<Usuario>();
                UsersDal mnu = new UsersDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                resultado = await mnu.ListaUsuariosFiltrado(usuario);
                return resultado;
            }
            catch (Exception ex)
            {
                var emex = new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = "Error:  " + ex.Message.ToString()
                };
                return new JsonResult(emex);
            }
        }

        [HttpPost("CrearUsuario")]
        public async Task<ActionResult<RespuestaServicio>> CrearUsuario([FromBody] Usuario usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var identity = (ClaimsIdentity)User.Identity;
                var claims = identity.FindFirst(c => c.Type == "IdUsuario");
                var idUsuario = Convert.ToInt32(claims.Value);
                if (idUsuario <= 0)
                {
                    var em = new ErrorDetails()
                    {
                        StatusCode = 400,
                        Message = "Error: Se requiere un parametro de busqueda "
                    };
                    return new JsonResult(em);
                }
                RespuestaServicio resultado = new RespuestaServicio();
                UsersDal mnu = new UsersDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                resultado = await mnu.CrearUsuario(usuario);
                return resultado;
            }
            catch (Exception ex)
            {
                var emex = new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = "Error:  " + ex.Message.ToString()
                };
                return new JsonResult(emex);
            }
        }

        [HttpPost("ActualizarUsuario")]
        public async Task<ActionResult<RespuestaServicio>> ActualizarUsuario([FromBody] Usuario usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var identity = (ClaimsIdentity)User.Identity;
                var claims = identity.FindFirst(c => c.Type == "IdUsuario");
                var idUsuario = Convert.ToInt32(claims.Value);
                if (idUsuario <= 0)
                {
                    var em = new ErrorDetails()
                    {
                        StatusCode = 400,
                        Message = "Error: Se requiere un parametro de busqueda "
                    };
                    return new JsonResult(em);
                }
                RespuestaServicio resultado = new RespuestaServicio();
                UsersDal mnu = new UsersDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                resultado = await mnu.ActualizarUsuario(usuario);
                return resultado;
            }
            catch (Exception ex)
            {
                var emex = new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = "Error:  " + ex.Message.ToString()
                };
                return new JsonResult(emex);
            }
        }

        [HttpPost("CrearUsuariosMasivo")]
        public async Task<ActionResult<List<RespuestaServicio>>> CrearUsuariosMasivo([FromBody] List<UsuariosMasivos> usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var identity = (ClaimsIdentity)User.Identity;
                var claims = identity.FindFirst(c => c.Type == "IdUsuario");
                var idUsuario = Convert.ToInt32(claims.Value);
                if (idUsuario <= 0)
                {
                    var em = new ErrorDetails()
                    {
                        StatusCode = 400,
                        Message = "Error: Se requiere un parametro de busqueda "
                    };
                    return new JsonResult(em);
                }
                UsersDal mnu = new UsersDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                var resultado = await mnu.CrearUsuariosMasivos(usuario, idUsuario);
                return resultado;
            }
            catch (Exception ex)
            {
                var emex = new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = "Error:  " + ex.Message.ToString()
                };
                return new JsonResult(emex);
            }
        }

        [HttpPost("InsertaUsuario")]
        public async Task<ActionResult<Users>> InsertUser([FromBody] UserNewInsert userNew)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    
                    //userNew.pwd = UsuarioDal.GetSHA256(userNew.pwd);
                    List<Users> userNuevo = new List<Users>();
                    UsersDal usuInsert = new UsersDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                    userNuevo = await usuInsert.InsertUsers(userNew,
                        int.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "idEmpresa").Value));
                    return (userNuevo.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                string Message = "Error:  " + ex.Message.ToString() + $"{((ex.Message.Contains("Cannot insert duplicate key") ? "|El usuario ya existe" : ""))}";
               
                return BadRequest(Message);
            }          
        }

        [HttpPut("ActualizaUsuario/{idUsuario}")]
        public async Task<ActionResult<Response>> UpdateUser(int idUsuario, [FromBody] UserNew userNew)
        {
            Response respuesta = new Response();
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    
                    UsersDal usuUpdate = new UsersDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                    respuesta = await usuUpdate.UpdateUsers(idUsuario, userNew);
                    return (respuesta);
                }
            }
            catch (Exception ex)
            {
                respuesta.Valor = 100;
                respuesta.Message = "Error:  " + ex.Message.ToString();                                    
                return respuesta;
            }


        }

        [HttpDelete("EliminaUsuario/{idUsuario}/{idUsuarioModifica}")]
        public async Task<ActionResult<Users>> DeleteUser(int idUsuario, int idUsuarioModifica)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    List<Users> userDelete = new List<Users>();
                    UsersDal usuDelete = new UsersDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                    userDelete = await usuDelete.InactiveUsers(idUsuario, idUsuarioModifica);
                    return (userDelete.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }


        }

        [HttpPost("RecoverPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> RecoverPassword([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserToken usData = new UserToken();
            UsuarioDal usuvali = new UsuarioDal(_ConnectionString.GetConnectionString("DefaultConnection"));
            usData = await usuvali.ValidarUsuarioForgot(request);
            if (usData == null)
            {
                return BadRequest("usuario no existe");
            }
            MailManager objMail = new MailManager();
            objMail.EstablecerValoresPorDefecto(CommonTools.ObtenerValorConfiguracion("NombreServidor"), CommonTools.ObtenerValorConfiguracion("CuentaOrigen"), CommonTools.ObtenerValorConfiguracion("Credenciales"));

            objMail.Titulo = "Recuperación contraseña Logytech Mobile";
            objMail.Asunto = "Recuperación contraseña Logytech Mobile";
            objMail.TextoMensaje = String.Concat("Hola: ", usData.NombreApellido, Environment.NewLine, ", Por favor ingresar al link de abajo para recuperar la contraseña: ", Environment.NewLine, "</br><a style='margin:10px 0 10px 0;color:#ffffff;font-weight:bold;display:inline-block;padding:6px 10px;font-size:16px;text-align:center;background-image:none;border:1px solid transparent;border-radius:10px;-moz-border-radius:10px;-webkit-border-radius:10px;-khtml-border-radius:10px; background-color:#836493;' href= www.Recuperar.Clave'" + "'> RecuperarContraseña </a></br>", Environment.NewLine, "Si no solicitaste recuperar la contraseña, por favor ignora este email y tu contraseña seguirá siendo la misma.");
            objMail.Receptor.Add(usData.Email);
            objMail.FirmaMensaje = "Logytech Mobile S.A.S";
            // se envia notificacion de los pedidos que se vencen el dia siguiente a la fecha actual
            var te = Task.Run(() => objMail.EnviarMailNotificacionAsync());
            te.Wait();



            return Ok(new Response { IsSuccess = true });
        }

        [HttpGet("ValidacionTokenUrl/{token}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<UserValidaToken>>> ValidaToken(string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<UserValidaToken> usData = new List<UserValidaToken>();
            UsuarioDal usuvali = new UsuarioDal(_ConnectionString.GetConnectionString("DefaultConnection"));
            usData = await usuvali.ValidaTokenUrl(token);
            if (usData == null)
            {
                return BadRequest("usuario no existe");
            }
            return Ok(usData);
        }

        [HttpPost("CambioPasswordUser")]
        [AllowAnonymous]
        public async Task<ActionResult<List<UserValidaToken>>> CambioPasswordUser([FromBody] UserRecover userNew)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    List<UserValidaToken> userNuevo = new List<UserValidaToken>();
                    UsuarioDal usuUpdate = new UsuarioDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                    userNuevo = await usuUpdate.ActualizaNuevaContrasena(userNew);
                    if (userNuevo.FirstOrDefault().message.Contains("Token expirado"))
                        return BadRequest("El token expiro, realiza el proceso nuevamente");
                    if (userNuevo.FirstOrDefault().message != "La contraseña se ha cambiado con éxito")
                        return BadRequest(userNuevo.FirstOrDefault().message);
                    return (userNuevo);
                }
            }
            catch (Exception ex)
            {
                var emex = new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = "Error:  " + ex.Message.ToString()
                };
                return new JsonResult(emex);
            }


        }
    }

}
