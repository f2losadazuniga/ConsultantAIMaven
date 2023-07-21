
using System;
using System.ComponentModel.DataAnnotations;


namespace LogicaNegocioServicio.Personas
{
    public class TipoPersonas
    {
        [Required(ErrorMessage = "El id tipo persona es obligatorio")]
        public int idTipoPersona { get; set; }
        [Required(ErrorMessage = "El tipo Persona es obligatorio")]
        public string tipoPersona { get; set; }
        public int idEstado{ get; set; }
        public int idUsuarioCreacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int idusuarioModificacion { get; set; }
        public DateTime fechaModificacion { get; set; }

    }
}
