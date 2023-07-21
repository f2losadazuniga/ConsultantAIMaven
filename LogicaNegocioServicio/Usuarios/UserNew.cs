using LogicaNegocioServicio.Comunes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.Usuarios
{
    public class UserNew
    {
        public int idUsuario{ get; set; }

        [Required(ErrorMessage = "La identificación es obligatorio")]
        public string numeroIdentificacion { get; set; }
        public string nombreApellido { get; set; }
        public string telefonos { get; set; }
        public string email { get; set; }
        public string idCiudad { get; set; }
        public string usuario { get; set; }
        public string pwd { get; set; }
        public int idEstado { get; set; }
        //public List<int> idPerfil { get; set; }
        public bool bloqueado { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idusuarioModifica { get; set; }
        public DateTime fechaModificacion { get; set; }
        public List<OptionJson> idPerfil { get; set; }
        public UserNew()
        {
            idUsuario = 0;
            idPerfil = new List<OptionJson>();
        }
    }
}
