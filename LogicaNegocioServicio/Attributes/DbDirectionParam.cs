using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.Attributes
{
    public class DbDirectionParam : Attribute
    {
        private readonly ParameterDirection direction;
        public ParameterDirection Direction { get { return direction; } }

        public DbDirectionParam(ParameterDirection direction)
        {
            this.direction = direction;
        }
    }
}
