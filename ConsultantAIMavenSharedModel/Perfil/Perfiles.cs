using System;
using System.ComponentModel.DataAnnotations;


namespace ConsultantAIMavenSharedModel.Perfil
{
    public class Perfiles
    {
        [Required(ErrorMessage = "El id Perfil es obligatorio")]
        public int idPerfil { get; set; }
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
