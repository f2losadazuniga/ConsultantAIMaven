using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultantAIMavenSharedModel.Usuarios
{
    public class userInfo
    {
        public int IdUsuario { get; set; }
        public int idPerfil { get; set; }
        public string nombreApellido { get; set; }
        public string perfil { get; set; }
        public string token { get; set; }
        public string fechaCambioClave { get; set; }

    }
}
