using Microsoft.AspNetCore.Authentication.JwtBearer;
using LogicaNegocioServicio.Autenticacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using LogicaNegocioServicio.Comunes;
using ConsultantAIMavenSharedModel.Usuarios;

namespace ApiIntegracionEntregasLogyTech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CuentasController : ControllerBase
    {
        private readonly IConfiguration _ConnectionString;
        //private readonly IUserHelper _userHelper;
        //private readonly IMailHelper _mailHelper;
        public CuentasController(
             IConfiguration Configuration)
        {
           
            this._ConnectionString = Configuration;
           
        }
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<ReponseToken>> Login([FromBody] UsuarioLogin userInfo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    userInfo.Password = UsuarioDal.GetSHA256(userInfo.Password);
                    UserToken ustoken = new UserToken();
                    UsuarioDal usuvali = new UsuarioDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                    ustoken = await usuvali.ValidarUsuarioToken(userInfo);

                    if (ustoken.IdUsuario == 0)
                    {
                        return BadRequest("usuario or password invalido");
                    }
                    else
                    {
                        return BuildToken(userInfo, ustoken);
                    }


                }
            }
            catch (Exception ex)
            {
                var emex = new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = "Error:  " + ex.Message.ToString()
                };
                return BadRequest(new JsonResult(emex));
            }

        }


        [HttpGet("RenovarToken")]   
        public async Task<ActionResult<ReponseToken>>RenovarToken()
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "unique_name").FirstOrDefault();
            var pasword = HttpContext.User.Claims.Where(claim => claim.Type == "pwd").FirstOrDefault();
            
            var usuaerio =  emailClaim.Value;
            var userInfo = new UsuarioLogin()
            {
                Usuario = usuaerio,
                Password= pasword.Value
            };
            UserToken ustoken = new UserToken();
            UsuarioDal usuvali = new UsuarioDal(_ConnectionString.GetConnectionString("DefaultConnection"));
            ustoken = await usuvali.ValidarUsuarioToken(userInfo);
            if (ustoken.IdUsuario == 0)
            {
                return BadRequest("usuario or password invalido");
            }
            else
            {
                return BuildToken(userInfo, ustoken);
            }

        }

      

        private ReponseToken BuildToken(UsuarioLogin userInfo, UserToken usToken)
        {
            ReponseToken tk = new ReponseToken();
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Usuario),
            new Claim("IdUsuario", usToken.IdUsuario.ToString()),
            new Claim("pwd", userInfo.Password.ToString()),
            new Claim("idEmpresa", usToken.Empresa.IdEmpresa.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_ConnectionString["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tiempo de expiración del token. En nuestro caso lo hacemos de una hora.
            // var expiration = DateTime.UtcNow.AddHours(1);
            var expiration = DateTime.UtcNow.AddMinutes(15000);

            JwtSecurityToken token = new JwtSecurityToken(
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            tk.Token = new JwtSecurityTokenHandler().WriteToken(token);
            tk.Expiration = expiration;
            return tk;
        }


    }

}
