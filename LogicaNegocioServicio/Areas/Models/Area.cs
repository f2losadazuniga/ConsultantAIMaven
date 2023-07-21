using LogicaNegocioServicio.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.Areas.Models
{
    public class Area
    {
        public int idArea { get; set; }
        public int idEmpresa { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string reprecentante { get; set; }
        public int idUsuarioCreacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaModificacion { get; set; }
    }
    public class AreaAuth : Area
    {
        public int idAutorizacionIngresoArea { get; set; }
    }


    public class AreaUpdate
    {
        [DbTypeParam(System.Data.SqlDbType.Int)]
        [DbDirectionParam(ParameterDirection.Input)]
        public int IdArea { get; set; }

        [DbTypeParam(System.Data.SqlDbType.VarChar)]
        [DbDirectionParam(ParameterDirection.Input)]
        public string Nombre { get; set; }
        [DbTypeParam(System.Data.SqlDbType.VarChar)]
        [DbDirectionParam(ParameterDirection.Input)]
        public string Descripcion { get; set; }
        [DbTypeParam(System.Data.SqlDbType.VarChar)]
        [DbDirectionParam(ParameterDirection.Input)]
        public string Reprecentante { get; set; }

        [DbTypeParam(System.Data.SqlDbType.VarChar)]
        [DbDirectionParam(ParameterDirection.Output)]
        [DbSizeParam(3500)]
        public string Mensaje { get; set; }
    }


    public class AreaRequest
    {
        [DbTypeParam(System.Data.SqlDbType.Int)]
        [DbDirectionParam(ParameterDirection.Input)]
        public int IdEmpresa { get; set; }
        [DbTypeParam(System.Data.SqlDbType.VarChar)]
        [DbDirectionParam(ParameterDirection.Input)]
        public string Nombre { get; set; }
        [DbTypeParam(System.Data.SqlDbType.VarChar)]
        [DbDirectionParam(ParameterDirection.Input)]
        public string Descripcion { get; set; } = null;
        [DbTypeParam(System.Data.SqlDbType.VarChar)]
        [DbDirectionParam(ParameterDirection.Input)]
        public string Reprecentante { get; set; }
        [DbTypeParam(System.Data.SqlDbType.Int)]
        [DbDirectionParam(ParameterDirection.Input)]
        public int IdUsuarioCreacion { get; set; }
        [DbTypeParam(System.Data.SqlDbType.VarChar)]
        [DbDirectionParam(ParameterDirection.Output)]
        [DbSizeParam(3500)]
        public string Mensaje { get; set; }
    }
}
