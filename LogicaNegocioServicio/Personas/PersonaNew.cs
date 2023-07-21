using System;
using System.ComponentModel.DataAnnotations;

namespace LogicaNegocioServicio.Personas
{
    public class PersonaNew
    {
        [Required(ErrorMessage = "La identificación es obligatorio")]
        public int idTipoPersona { get; set; }
        public string numeroDocumento { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string correo { get; set; }
        public DateTime? fechaNacimiento { get; set; }
        public int idEmpresa { get; set; }
        public int idUsuarioCreacion { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public string UrlFoto { get; set; }

    }

    public class PersonaUpdate : PersonaNew
    {
        public string UrlFotoOld { get; set; }

    }
}
