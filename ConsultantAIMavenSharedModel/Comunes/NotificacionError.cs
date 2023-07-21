
using System.ComponentModel.DataAnnotations;
namespace ConsultantAIMavenSharedModel.Comunes
{
    public class NotificacionError
    {
        [Required(ErrorMessage = "El IdAplicacion es obligatorio")]
        public int IdAplicacion { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(50)]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "El Mensaje es obligatorio")]
        public string Mensaje { get; set; }
        public string BearerToken { get; set; }

        [StringLength(50)]
        public string Opcion { get; set; }

        public NotificacionError()
        {
            Codigo = "0";
        }
    }

}
