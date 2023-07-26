using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.Autenticacion
{
    public class ReponseToken
    {
        public string Token { get; set; }
        public DateTime? Expiration { get; set; }

        public ReponseToken()
        {
            Token = string.Empty;
            Expiration = null;
        }

    }
}
