using System;
using System.Collections.Generic;
using System.Text;

namespace ConsultantAIMavenSharedModel.Usuarios
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public int IdUsuarioModifica { get; set; }
        public string numeroIdentificacion { get; set; }
        public string nombreApellido { get; set; }
        public string telefonos { get; set; }
        public string email { get; set; }
        public int idciudad { get; set; }
        public string usuario { get; set; }
        public string pwd { get; set; }
        public string Repitapwd { get; set; }
        public int IdEstado { get; set; }
        public string Estado { get; set; }
        public int IdPerfil { get; set; }
        public int IdEmpresa { get; set; }
        public bool Bloqueado { get; set; }
        public string Ciudad { get; set; }
        public string Perfil { get; set; }
        public string Empresa { get; set; }
        public int Fila { get; set; }


    }
}
