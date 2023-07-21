
using EntregasLogyTechSharedModel.Perfil;
using System;
using System.Collections.Generic;

namespace ConsultantAIMavenSharedModel.Usuarios
{
    public class Users
    {
        public int IdUsuario { get; set; }
        public string numeroIdentificacion { get; set; }
        public string nombreApellido { get; set; }
        public string telefonos { get; set; }
        public string email { get; set; }
        public string idciudad { get; set; }
        public string usuario { get; set; }
        public string pwd { get; set; }
        public string bearerToken { get; set; }
        public int idEstado { get; set; }
        public bool bloqueado { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idusuarioModifica { get; set; }
        public DateTime fechaModificacion { get; set; }
        public DateTime fechaultimoIngreso { get; set; }
        public string fechaCambioClave { get; set; }
        public DateTime fechaExpiracionToken { get; set; }
        public PerfilInfo Perfil { get; set; }
        public Users()
        {
            IdUsuario = 0;
            Perfil = new PerfilInfo();
        }


    }
}
