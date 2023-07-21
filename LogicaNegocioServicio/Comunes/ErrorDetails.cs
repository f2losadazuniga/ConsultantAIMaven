using Newtonsoft.Json;

namespace LogicaNegocioServicio.Comunes
{
   public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            StatusCode = 400;
            return JsonConvert.SerializeObject(this);
        }
    }
}
