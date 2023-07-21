using LogicaNegocioServicio.Autenticacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.Autenticacion
{
    public interface IUserToken
    {
        Task<string> GeneratePasswordResetTokenAsync(UserToken user);
    }
}
