using ConsultantAIMavenSharedModel.Comunes;
using System;
using System.Data;
using System.Threading.Tasks;
using ConnectionManagement.Data;
using EntregasLogyTechSharedModel.Conversation;
using EntregasLogyTechSharedModel.FineTune;
using System.Collections.Generic;
using ConsultantAIMavenSharedModel.Usuarios;
using Microsoft.VisualBasic;
using EntregasLogyTechSharedModel.Customer;

namespace LogicaNegocioServicio.Conversation
{
    public class ChatDALL
    {
        public string ConectionString { get; set; }
        public ChatDALL(string conectionString)
        {
            ConectionString = conectionString;
        }
        public async Task InsertConversation(Chat conversation, int idUsuario,string Answer)
        {
            Connection db = new Connection(ConectionString);
             DataTable dtFineTunes = new DataTable();
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                db.SQLParametros.Add("@idUser", SqlDbType.Int).Value = idUsuario;
                db.SQLParametros.Add("@Email", SqlDbType.VarChar, 100).Value = conversation.Email;
                db.SQLParametros.Add("@Ask", SqlDbType.VarChar, -1).Value = conversation.Message;
                db.SQLParametros.Add("@Answer", SqlDbType.VarChar, -1).Value = Answer;
                 await db.EjecutarNonQueryAsync("InsertConversation", CommandType.StoredProcedure);
               
            }
            catch (Exception ex)
            {
                throw new Exception("Se genero un error al insertar el usuario: " + ex.Message);
            }
            finally { }
            if (db != null)
            {
                db.Dispose();
            }

        }
        public async Task<RespuestaServicio> InsertReaction(RecordReaction Reaction, int idUsuario)
        {
            Connection db = new Connection(ConectionString);
            RespuestaServicio result = new RespuestaServicio();
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                db.SQLParametros.Add("@idUser", SqlDbType.Int).Value = idUsuario;
                db.SQLParametros.Add("@IdConversation", SqlDbType.Int).Value = Reaction.idConversation;
                db.SQLParametros.Add("@Reaction", SqlDbType.Bit, -1).Value = Reaction.Reaction;
                db.SQLParametros.Add("@Message", SqlDbType.VarChar, 200).Value = Reaction.Message;
                db.SQLParametros.Add("@Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                db.SQLParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue;
                db.IniciarTransaccion();
                await db.EjecutarNonQueryAsync("InsertReaction", CommandType.StoredProcedure);
                result.Codigo = db.SQLParametros["@returnValue"].Value.ToString();
                result.Mensaje = db.SQLParametros["@Mensaje"].Value.ToString();
                if (result.Codigo == "200")
                {
                    db.ConfirmarTransaccion();
                }
                else
                {
                    db.AbortarTransaccion();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("An error was generated when inserting the reaction " + ex.Message);
            }
            finally
            {
                if (db != null)
                {
                    db.Dispose();
                }
            }
        }

        public async Task<List<AllConversations>> GetAllChat( string email, int idUsuario)
        {
            List<AllConversations> resul = new List<AllConversations>();
            Connection db = new Connection(ConectionString);
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                db.SQLParametros.Add("@idUser", SqlDbType.Int).Value = idUsuario;
                db.SQLParametros.Add("@Email", SqlDbType.VarChar, 100).Value = email;
                var reader = await db.EjecutarReaderAsync("GetAllChat", CommandType.StoredProcedure);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var re = new AllConversations();

                        if (!Convert.IsDBNull(reader["idConversation"])) { re.IdConversation = Convert.ToInt32(reader["idConversation"]); }
                        if (!Convert.IsDBNull(reader["Ask"])) { re.Ask = reader["Ask"].ToString(); }
                        if (!Convert.IsDBNull(reader["Answer"])) { re.Answer = reader["Answer"].ToString(); }
                        if (!Convert.IsDBNull(reader["Reaction"])) { re.Reaction = Convert.ToBoolean(reader["Reaction"]); }
                        if (!Convert.IsDBNull(reader["Message"])) { re.Message = reader["Message"].ToString(); }
                        resul.Add(re);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Se genero un error al consultar los entrenamietos: " + ex.Message);
            }
            finally { }
            if (db != null)
            {
                db.Dispose();
            }

            return resul;
        }

    }
}
