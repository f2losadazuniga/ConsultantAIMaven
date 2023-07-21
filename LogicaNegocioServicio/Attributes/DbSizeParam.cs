using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.Attributes
{
    public class DbSizeParam : Attribute
    {
        private readonly int size;
        public int Size { get { return size; } }

        public DbSizeParam(int size)
        {
            this.size = size;
        }
    }
}
