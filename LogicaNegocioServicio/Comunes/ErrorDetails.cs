using Newtonsoft.Json;

namespace LogicaNegocioServicio.Comunes
{
   public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ErrorDetails()
        {
            StatusCode = 400;            
        }
    }
}
