using System;
using System.Collections.Generic;
using System.Text;

namespace ConsultantAIMavenSharedModel.Usuarios
{
    public class CambioClaveUsuario
    {
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Identificacion { get; set; }
        public string ActualClave { get; set; }
        public string NuevaClave { get; set; }
        public string RepitaClave { get; set; }

    }
}
