using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.Attributes
{
    public class DbTypeParam : Attribute
    {
        public readonly SqlDbType type;
        public SqlDbType Type { get { return type; } }
        

        public DbTypeParam(SqlDbType type)
        {
            this.type = type;
        }
    }
}
