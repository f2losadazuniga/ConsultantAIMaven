using System;
using System.Collections.Generic;
using System.Text;

namespace LogicaNegocioServicio.Comunes
{
    public interface ILogger
    {
        string Log(LogModel logModel);
    }

}
