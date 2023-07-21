using System;


namespace LogicaNegocioServicio.Personas
{
    public class Persona
    {
        public int idPersona { get; set; }     
        public int idTipoPersona { get; set; }
        public string tipoPersona { get; set; }
        public string numeroDocumento { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string correo { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public int idEmpresa { get; set; }
        public string empresa { get; set; }
        public int idUsuarioCreacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int idusuarioModificacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int IdEstado { get; set; }
        public string Estado { get; set; }
        public string UrlFoto { get; set; }
        public string UrlFotoOld { get; set; }
        public Persona()
        {
            idPersona = 0;
        }
    }
}
