using ConnectionManagement.Data;
using LogicaNegocioServicio.Attributes;
using LogicaNegocioServicio.Comunes;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio
{
    public class GenericDal
    {
        protected Connection Connection;
        protected readonly string connectionString;
        public GenericDal(string ConnectionString)
        {
            Connection = new Connection(ConnectionString);
            connectionString = ConnectionString;
        }




        public List<SqlParameter> GenerarParametrosSql<T>(T obj) where T : class, new()
        {
            List<SqlParameter> Parameters = new List<SqlParameter>();
            typeof(T).GetProperties().Where(x => x.GetCustomAttribute<DbTypeParam>().Type != SqlDbType.Structured).ToList().ForEach(p =>
            {
                try
                {
                    
                    if (p.GetValue(obj) != null || p.GetCustomAttribute<DbDirectionParam>().Direction != ParameterDirection.Input)
                    {
                    
                        var param = new SqlParameter
                        {
                            Size = p.GetCustomAttribute<DbSizeParam>() != null ? p.GetCustomAttribute<DbSizeParam>().Size : 0,
                            SqlDbType = p.GetCustomAttribute<DbTypeParam>().Type,
                            Direction = p.GetCustomAttribute<DbDirectionParam>().Direction,
                            ParameterName = $"@{p.Name}",
                            Value = p.GetValue(obj)
                        };

                        Parameters.Add(param);
                    }
                        
                        
                } catch(Exception ex) 
                { }
                
            });
            this.Connection.SQLParametros.AddRange(Parameters.ToArray());
            return Parameters;
        }



        public List<SqlParameter> GenerarParametrosDataTablesSql<T,J>(T obj) where T : class, new() where J : class, new()
        {
            List<SqlParameter> Parameters = new List<SqlParameter>();
            typeof(T).GetProperties().Where(x => x.GetCustomAttribute<DbTypeParam>().Type == SqlDbType.Structured && x.PropertyType == typeof(List<J>)).ToList().ForEach(p =>
            {
                try
                {
                    if (p.GetValue(obj) != null || p.GetCustomAttribute<DbDirectionParam>().Direction != ParameterDirection.Input)
                    {
                        List<J> values = new List<J>();
                        values = (List<J>)p.GetValue(obj);

                        var param = new SqlParameter
                        {
                            Size = p.GetCustomAttribute<DbSizeParam>() != null ? p.GetCustomAttribute<DbSizeParam>().Size : 0,
                            SqlDbType = p.GetCustomAttribute<DbTypeParam>().Type,
                            Direction = p.GetCustomAttribute<DbDirectionParam>().Direction,
                            ParameterName = $"@{p.Name}",
                            Value =  ConvertListToDataTable.ToDataTable1(values)
                        };

                        Parameters.Add(param);
                    }


                }
                catch (Exception ex)
                { }

            });
            this.Connection.SQLParametros.AddRange(Parameters.ToArray());
            return Parameters;
        }
    }
}
