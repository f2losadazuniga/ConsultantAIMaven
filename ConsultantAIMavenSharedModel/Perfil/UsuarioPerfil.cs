using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultantAIMavenSharedModel.Perfil
{
    public class UsuarioPerfil
    {
        [Required(ErrorMessage = "El id Usuario es obligatorio")]
        public int idUsuario { get; set; }
        [Required(ErrorMessage = "El id del perfil es obligatorio")]
        public int idPerfil { get; set; }
        public int idUsuarioCreacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaModificacion { get; set; }
    }
}
