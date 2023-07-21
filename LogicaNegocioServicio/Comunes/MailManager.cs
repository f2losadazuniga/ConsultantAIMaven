
using System;
//using System.Buffers.Text;
using System.Collections;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.Comunes
{
    public class MailManager
    {
        private MailMessage _manejadorMensaje;
        private SmtpClient _clienteSmtp;
        private string _nombreUsuario;
        private string _password;
        private string _dominio;
        private Attachment _adjuntos;
        private ArrayList _adjuntosUrl;



        private MailAddress _emisor;
        private MailAddressCollection _receptor = new MailAddressCollection();
        private MailAddressCollection _copia = new MailAddressCollection();
        private string _tituloMensaje;
        private string _textoMensaje;
        private string _firmaMensaje;
        private string _notaMensaje;
        private string _mensajeLegal;
        private StringBuilder _cuerpo = new StringBuilder();
        private ArrayList _adjuntosURL = new ArrayList();
        private MailMessage _mensaje = new MailMessage();
        private string _direccionOrigen;
        private string _direccionDestino;
        private ArrayList _rutaArchivos = new ArrayList();
        private Attachment _attach;
        private string _displayName;

        public SmtpClient ClienteSmtp
        {
            get
            {
                return _clienteSmtp;
            }
            set
            {
                _clienteSmtp = value;
            }
        }

        public MailAddressCollection Receptor
        {
            get
            {
                return _receptor;
            }
            set
            {
                _receptor = value;
            }
        }

        public string Titulo
        {
            get
            {
                return _tituloMensaje;
            }
            set
            {
                _tituloMensaje = value;
            }
        }

        public string FirmaMensaje
        {
            get
            {
                return _firmaMensaje;
            }
            set
            {
                _firmaMensaje = value;
            }
        }

        public string TextoMensaje
        {
            get
            {
                return _textoMensaje;
            }
            set
            {
                _textoMensaje = value;
            }
        }


        public MailManager() : base()
        {
            _clienteSmtp = new SmtpClient();
            _manejadorMensaje = new MailMessage();
            _nombreUsuario = "";
            _password = "";
            _dominio = "";
        }

        public void Dispose()
        {
            if (_manejadorMensaje != null)
                _manejadorMensaje.Dispose();
        }



        // Public ReadOnly Property ManejadorMensaje() As MailMessage
        // Get
        // Return _manejadorMensaje
        // End Get
        // End Property

        // Public ReadOnly Property ClienteSmtp() As SmtpClient
        // Get
        // Return _clienteSmtp
        // End Get
        // End Property

        public MailAddressCollection Destanatarios
        {
            get
            {
                return _manejadorMensaje.To;
            }
        }

        public MailAddressCollection DestanatariosCopia
        {
            get
            {
                return _manejadorMensaje.CC;
            }
        }

        public MailAddressCollection DestanatariosCopiaOculta
        {
            get
            {
                return _manejadorMensaje.Bcc;
            }
        }

        public string Cuerpo
        {
            get
            {
                return _manejadorMensaje.Body;
            }
            set
            {
                _manejadorMensaje.Body = value;
            }
        }

        public string Asunto
        {
            get
            {
                return _manejadorMensaje.Subject;
            }
            set
            {
                _manejadorMensaje.Subject = value;
            }
        }

        public bool CuerpoEsHtml
        {
            get
            {
                return _manejadorMensaje.IsBodyHtml;
            }
            set
            {
                _manejadorMensaje.IsBodyHtml = value;
            }
        }

        public MailPriority Prioridad
        {
            get
            {
                return _manejadorMensaje.Priority;
            }
            set
            {
                _manejadorMensaje.Priority = value;
            }
        }

        public string NombreUsuario
        {
            get
            {
                return _nombreUsuario;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
        }

        public string Dominio
        {
            get
            {
                return _dominio;
            }
        }

        public string ServidorCorreo
        {
            get
            {
                return _clienteSmtp.Host;
            }
            set
            {
                _clienteSmtp.Host = value;
            }
        }

        public bool HayDestinatario
        {
            get
            {
                int numDestinatario = _manejadorMensaje.To.Count;
                int numDestinatarioCopia = _manejadorMensaje.CC.Count;
                int numDestinatarioCopiaOculta = _manejadorMensaje.Bcc.Count;
                return System.Convert.ToBoolean(numDestinatario + numDestinatarioCopia + numDestinatarioCopiaOculta);
            }
        }

        public ArrayList AdjuntoUrl
        {
            get
            {
                return _adjuntosUrl;
            }
            set
            {
                _adjuntosUrl = value;
            }
        }

        public AlternateViewCollection VistaAlternativa
        {
            get
            {
                return _manejadorMensaje.AlternateViews;
            }
        }



        public void EstablecerCuentaOrigen(string direccion)
        {
            _manejadorMensaje.From = new MailAddress(direccion);
        }

        public void EstablecerCuentaOrigen(string direccion, string nombreAMostrar)
        {
            _manejadorMensaje.From = new MailAddress(direccion, nombreAMostrar);
        }

        public void AdicionarDestinatario(string direccion)
        {
            _manejadorMensaje.To.Add(direccion);
        }

        public void AdicionarDestinatario(string direccion, string nombreAMostrar)
        {
            _manejadorMensaje.To.Add(new MailAddress(direccion, nombreAMostrar));
        }

        public void AdicionarDestinatarioCopia(string direccion)
        {
            _manejadorMensaje.CC.Add(direccion);
        }

        public void AdicionarDestinatarioCopia(string direccion, string nombreAMostrar)
        {
            _manejadorMensaje.CC.Add(new MailAddress(direccion, nombreAMostrar));
        }

        public void AdicionarDestinatarioCopiaOculta(string direccion)
        {
            _manejadorMensaje.Bcc.Add(direccion);
        }

        public void AdicionarDestinatarioCopiaOculta(string direccion, string nombreAMostrar)
        {
            _manejadorMensaje.Bcc.Add(new MailAddress(direccion, nombreAMostrar));
        }

        public void EstablecerCuentaRespuesta(string direccion, string nombreAMostrar)
        {
            _manejadorMensaje.ReplyToList.Add(new MailAddress(direccion, nombreAMostrar));
        }

        public void EstablecerCredenciales(string nombreUsuario, string password, string dominio)
        {
            _clienteSmtp.Credentials = new NetworkCredential(nombreUsuario, password, dominio);
            _clienteSmtp.EnableSsl = true;
            _nombreUsuario = nombreUsuario;
            _password = password;
            _dominio = dominio;
        }

        public void AdjuntarArchivos()
        {
            foreach (string ruta in AdjuntoUrl)
            {
                if ((ruta != string.Empty))
                {
                    _adjuntos = new Attachment(ruta);
                    _manejadorMensaje.Attachments.Add(_adjuntos);
                }
            }
        }

        public void Enviar()
        {
            if (_manejadorMensaje.From == null || string.IsNullOrEmpty(_manejadorMensaje.From.Address))
                _manejadorMensaje.From = new MailAddress("system.notifier@logytechmobile.com", "Notificador de Eventos");
            {
                var withBlock = _clienteSmtp;
                withBlock.DeliveryMethod = SmtpDeliveryMethod.Network;
                withBlock.Send(_manejadorMensaje);
            }
        }

        public void LimpiarDestinatarios()
        {
            _manejadorMensaje.To.Clear();
        }

        public void LimpiarDestinatariosCopia()
        {
            _manejadorMensaje.CC.Clear();
        }

        public void LimpiarDestinatariosCopiaOculta()
        {
            _manejadorMensaje.Bcc.Clear();
        }

        public void LimpiarTodosLosDestinatarios()
        {
            _manejadorMensaje.To.Clear();
            _manejadorMensaje.CC.Clear();
            _manejadorMensaje.Bcc.Clear();
        }


        public void IncluirEncabezadoMensaje()
        {
            var firmaMensaje = "<br> Logytech Mobile S.A.S.";
            var notaMensaje = "Este mensaje fue generado automáticamente, si tiene alguna duda o inquietud respecto al mismo, por favor envía un correo electrónico al grupo <a href='mailto:itdevelopment@logytechmobile.com?subject=Inquietud Notificador'> IT Development </a>";
            System.Text.StringBuilder cuerpo = new System.Text.StringBuilder();


            {
                var withBlock = cuerpo;
                withBlock.Append("<HTML>");
                withBlock.Append("      <HEAD>");
                withBlock.Append("             <LINK href='include/styleBACK.css' type='text/css' rel='stylesheet'>");
                withBlock.Append("             <LINK href='Estilos/estiloContenido.css' type='text/css' rel='stylesheet'>");
                withBlock.Append("      </HEAD>");
                withBlock.Append("      <body class='cuerpo2'>");
                withBlock.Append("      <table width='100%' border='0' align='center' cellpadding='5' cellspacing='0' class='tabla'");
                withBlock.Append("             ID='Table1'>");
                withBlock.Append("             <tr>");
                withBlock.Append("                   <td width='20%' ><img src='https://apps.logytechmobile.com/notusils/images/logo_trans.png'>");
                withBlock.Append("                   </td>");
                withBlock.Append("                   <td align='center' bgcolor='#38610B' width='80%'><font size='3' face='arial' color='white'><b>" + _manejadorMensaje.Subject + "</b></font></td>");
                withBlock.Append("             </tr>");
                withBlock.Append("      </table>");
                withBlock.Append("      <br />");
                withBlock.Append("      <br />");

                if (DateTime.Now.Hour < 12)
                    withBlock.Append("  <font class='fuente'>Buenos Días, ");
                else if (DateTime.Now.Hour > 18)
                    withBlock.Append("  <font class='fuente'>Buenas Noches, ");
                else
                    withBlock.Append("  <font class='fuente'>Buenas Tardes, ");

                withBlock.Append("<br /><br />" + _manejadorMensaje.Body);
                withBlock.Append("      <br />");
                withBlock.Append("</font>");
                withBlock.Append("<br />       <font class='fuente'>Cordial Saludo,<br />");
                withBlock.Append("             <br><b>" + firmaMensaje + "</b><br /><br />");
                withBlock.Append("</font><br /><br /><br /><br /><br />");
                if (notaMensaje != "")
                    withBlock.Append("  <font class='fuente' size='1'><i>Nota: " + notaMensaje + ".</i></font");
                withBlock.Append("      </body>");
                withBlock.Append("</HTML>");
            }

            _manejadorMensaje.Body = cuerpo.ToString();
            _manejadorMensaje.BodyEncoding = Encoding.UTF7;
        }


      
        public void EstablecerValoresPorDefecto(string Host,string from, string Credenciales)
        {
            // Dim displayName As String
            if (string.IsNullOrEmpty(_displayName))
                _displayName = "Notificador LOGYTECH";

            // Set encoding values
            _mensaje.SubjectEncoding = Encoding.UTF8;
            _mensaje.BodyEncoding = Encoding.UTF8;

            // Set other values
            _mensaje.Priority = MailPriority.Normal;
            _mensaje.IsBodyHtml = true;
            _clienteSmtp.Host = Host;
            _mensaje.From = new MailAddress(from, _displayName);


            if (!string.IsNullOrWhiteSpace(Credenciales))
            {
                string[] credenciales;
                credenciales = Credenciales.Split(';');
                _nombreUsuario = credenciales[0];
                _password = credenciales[1];
                _dominio = credenciales[2];
            }

            _emisor = _mensaje.From;
            _receptor = _mensaje.To;
            // _firmaMensaje = "-- <br> ||| Dinatech Mobile S.A.S."
            _notaMensaje = "Este mensaje fue generado automáticamente, No responda a este correo electrónico </a>";
        }


        protected virtual void CrearCuerpoMensajeHTML()
        {
            //ConfigValues value = new ConfigValues("URL_IMAGEN_NOTIFICACION");
            string url = "http://www.logytechmobile.com/notusils/"; //value.ConfigKeyValue;
            {
                var withBlock = _cuerpo;
                withBlock.Append("<HTML>");
                withBlock.Append("	<HEAD>");
                withBlock.Append("	    <style>table, th, td { border: 1px solid black; border-collapse: collapse; } </style>");
                withBlock.Append("		<LINK href='" + url + "include/styleBACK.css' type='text/css' rel='stylesheet'>");
                withBlock.Append("	</HEAD>");
                withBlock.Append("	<body class='cuerpo2'>");
                withBlock.Append("	<table width='100%' border='0' align='center' cellpadding='5' cellspacing='0' class='tabla'");
                withBlock.Append("		ID='Table1'>");
                withBlock.Append("		<tr>");
                withBlock.Append("			<td width='20%' ><img src='" + url + "images/logo_trans.png'>");
                withBlock.Append("			</td>");
                withBlock.Append("		</tr>");
                withBlock.Append("	</table>");
                withBlock.Append("	<br />");
                withBlock.Append("	<br />");

                if (DateTime.Now.Hour < 12)
                    withBlock.Append("	<font class='fuente'>Buenos Días, ");
                else if (DateTime.Now.Hour > 18)
                    withBlock.Append("	<font class='fuente'>Buenas Noches, ");
                else
                    withBlock.Append("	<font class='fuente'>Buenas Tardes, ");

                withBlock.Append("<br /><br />" + _textoMensaje);
                withBlock.Append("	<br />");
                withBlock.Append("</font>");
                withBlock.Append("<br />	<font class='fuente'>Cordial Saludo,<br />");
                withBlock.Append("		<br><b>" + _firmaMensaje + "</b><br /><br />");
                withBlock.Append("</font><br /><br /><br /><br /><br />");
                if (_notaMensaje != "")
                    withBlock.Append("	<font class='fuente' size='1'><i>Nota: " + _notaMensaje + ".</i></font");
                withBlock.Append("	</body>");
                withBlock.Append("</HTML>");
            }

            _mensaje.Body = _cuerpo.ToString();

        }

        public async Task<bool> EnviarMailNotificacionAsync()
        {
            bool answer = false;
            try
            {
                foreach (MailAddress direccion in _receptor)
                {
                    if (!_mensaje.To.Contains(direccion))
                        _mensaje.To.Add(direccion);
                }

                foreach (MailAddress direccion in _copia)
                {
                    if (!_mensaje.CC.Contains(direccion))
                        _mensaje.CC.Add(direccion);
                }

                _mensaje.Subject = _manejadorMensaje.Subject;
                if (_emisor == null) { 
                    int sss = 3;
                //this.EstablecerValoresPorDefecto();
                }
                else
                    _mensaje.From = _emisor;

                if (_mensaje.IsBodyHtml == true)
                    CrearCuerpoMensajeHTML();
                else
                    _mensaje.Body = _textoMensaje;
                // Adjunta Archivos
                //AdjuntarArchivos();

                if (_nombreUsuario != null && _nombreUsuario.Length > 0 && _password != null && _password.Length > 0 && _dominio != null && _dominio.Length > 0)
                    EstablecerCredenciales(_nombreUsuario, _password, _dominio);

                {
                    var withBlock = ClienteSmtp;
                    withBlock.DeliveryMethod = SmtpDeliveryMethod.Network;
                    withBlock.Timeout = 99999;
                    withBlock.EnableSsl = true;
                    await withBlock.SendMailAsync(_mensaje);
                }

                answer = true;
            }
            catch (SmtpException smtpEx)
            {
                SmtpStatusCode statusCode = smtpEx.StatusCode;
                switch (statusCode)
                {
                    case SmtpStatusCode.ExceededStorageAllocation:
                        {
                            throw new Exception("El tamaño del mensaje es mayor al tamaño permitido por el buzón de correo del receptor");
                            break;
                        }

                    case SmtpStatusCode.GeneralFailure:
                        {
                            throw new Exception("No se pudo establecer conexión con el servidor SMTP de Logytech Mobile");
                            break;
                        }

                    case SmtpStatusCode.InsufficientStorage:
                        {
                            throw new Exception("El servidor SMTP de Logytech Mobile no tiene espacio suficiente para guardar el mensaje");
                            break;
                        }

                    case SmtpStatusCode.MailboxBusy:
                    case SmtpStatusCode.MailboxUnavailable:
                        {
                            throw new Exception("El buzón de correo del receptor está ocupado o no disponible, reintentando en 5 segundos");
                            break;
                        }

                    case SmtpStatusCode.TransactionFailed:
                        {
                            throw new Exception("Transacción fallida, reintentando en 5 segundos");

                            break;
                        }

                    case SmtpStatusCode.ServiceNotAvailable:
                        {
                            throw new Exception("Servicio no disponible temporalmente, reintentando en 5 segundos");
                            break;
                        }
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Correo electrónico no enviado: " + ex.Message);
            }

            return answer;
        }


    }

    
}
