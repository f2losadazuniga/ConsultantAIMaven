using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultantAIMavenSharedModel.Perfil
{
    public class PerfilNew
    {

        [Required(ErrorMessage = "El nombre del perfil es obligatorio")]
        public string perfil { get; set; }
        public string descripcion { get; set; }
        public int idEstado { get; set; }
        public DateTime fecha { get; set; }
        public int idUsuarioCreacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaModificacion { get; set; }
    }
}
