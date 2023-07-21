using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.Comunes
{
   public class JsonOptionObs 
    {
        public int Id { get;set; }
        public string Observacion { get;set; }
    }
   public class OptionJson : IParamsValues
    {
        public int value { get; set; }
    }
    public interface IParamsValues
    {
        int value { get; set;}
    }
}
