using LogicaNegocioServicio.Autenticacion;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.Autenticacion
{
    public class UserToken : IUserToken
    {
        public int IdUsuario { get; set; }
        public string NombreApellido { get; set; }
         public int IdPerfil { get; set; }
        public string Perfil { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string FechaCambioClave { get; set; }
        public DateTime Expiration { get; set; }
        public EmpresaInfo Empresa { get; set; }
        public UserToken()
        {
            IdUsuario = 0;
            Empresa = new EmpresaInfo();
        }
        public async Task<string> GeneratePasswordResetTokenAsync(UserToken user)
        {

            return  "";
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
    }
   
}
