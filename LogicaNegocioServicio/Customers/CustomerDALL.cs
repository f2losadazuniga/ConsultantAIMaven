using ConnectionManagement.Data;
using System;
using System.Data;
using System.Threading.Tasks;
using EntregasLogyTechSharedModel.Customer;
using EntregasLogyTechSharedModel.Conversation;
using LogicaNegocioServicio.Conversation;
using System.Collections.Generic;

namespace LogicaNegocioServicio.Customers
{
    public class CustomerDALL
    {
        public string ConectionString { get; set; }
        public CustomerDALL(string conectionString)
        {
            ConectionString = conectionString;
        }
        public async Task<List<AllConversations>> InsertCustomern(Customer custome, int idUsuario)
        {
            Connection db = new Connection(ConectionString);
            DataTable dtFineTunes = new DataTable();
            db.TiempoEsperaComando = 0;
            try
            {
                db.SQLParametros.Clear();
                db.TiempoEsperaComando = 0;
                db.SQLParametros.Add("@Name", SqlDbType.VarChar, 150).Value = custome.Name;
                db.SQLParametros.Add("@PhotoProfile", SqlDbType.VarChar, -1).Value = custome.PhotoProfile;
                db.SQLParametros.Add("@Email", SqlDbType.VarChar, 100).Value = custome.Email;
                db.SQLParametros.Add("@CountryLocation", SqlDbType.VarChar, 200).Value = custome.CountryLocation;
                db.SQLParametros.Add("@UserId", SqlDbType.VarChar, 50).Value = custome.UserId;
                db.SQLParametros.Add("@idUser", SqlDbType.Int).Value = idUsuario;
                await db.EjecutarNonQueryAsync("InsertCustomern", CommandType.StoredProcedure);

                List<AllConversations> result = new List<AllConversations>();
                ChatDALL objChat = new ChatDALL(ConectionString);
                result = await objChat.GetAllChat(custome.Email, idUsuario);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Se genero un error al insertar el usuario: " + ex.Message);
            }
            finally
            {
                if (db != null)
                {
                    db.Dispose();
                }
            }
        }
    }
}
