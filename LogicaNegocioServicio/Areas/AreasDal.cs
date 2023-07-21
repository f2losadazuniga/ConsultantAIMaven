using LogicaNegocio;
using LogicaNegocioServicio.Areas.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.Areas
{
    public class AreasDal : GenericDal
    {
        public AreasDal(string ConnectionString) : base(ConnectionString)
        {

        }


        public async Task<List<AreaAuth>> ObtenerAreasDisponiblesAuth(decimal IdAutorizacionIngreso)
        {
            try
            {
                this.Connection.SQLParametros.Add($"@IdAutorizacionIngreso", SqlDbType.Decimal).SqlValue = IdAutorizacionIngreso;
                var Param = this.Connection.SQLParametros.Add("@Mensaje", SqlDbType.VarChar, 3500);
                Param.Direction = ParameterDirection.Output;
                return await this.Connection.MapearClaseAsync<AreaAuth>("ConsultarAreasDisponiblesAut");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Area>> ObtenerAreas(int IdEmpresa, int? IdArea = null)
        {
            try
            {   
                if(IdArea != null)
                    this.Connection.SQLParametros.Add("@IdArea",SqlDbType.Int).Value = IdArea;
                this.Connection.SQLParametros.Add($"@IdEmpresa", SqlDbType.Int).SqlValue = IdEmpresa;
                return await this.Connection.MapearClaseAsync<Area>("ConsultarAreas");
            }
            catch(Exception ex) 
            {
                return null;
            }
        }

        public async Task<List<AreaAuth>> ObtenerAreasAutorizadas(int IdAuth)
        {
            try
            {
                this.Connection.SQLParametros.Add("@IdAutorizacion", SqlDbType.Int).SqlValue  = IdAuth;
                return await this.Connection.MapearClaseAsync<AreaAuth>("ConsultarAreasPorAutorizacion");
            }
            catch(Exception ex) 
            {
                return null;
            }
        }


        public async Task<string> CreateArea(AreaRequest area)
        {
            try
            {
                var Params = this.GenerarParametrosSql(area);
                await this.Connection.EjecutarNonQueryAsync("InsertarArea", CommandType.StoredProcedure);

                return Params.FirstOrDefault(x => x.ParameterName == $"@{nameof(area.Mensaje)}").SqlValue.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> DeleteArea(int Id)
        {
            try
            {
                this.Connection.SQLParametros.Add("@IdArea",SqlDbType.Int).Value = Id;
                var Param = this.Connection.SQLParametros.Add("@Mensaje", SqlDbType.VarChar, 3500);
                Param.Direction = ParameterDirection.Output;
                this.Connection.EjecutarNonQuery("InactivarArea", CommandType.StoredProcedure);
                return Param.Value.ToString();
            }
            catch(Exception ex )
            {
                return ex.Message+"|error";
            }
        }


        public async Task<string> DeleteAreaAuth(int Id)
        {
            try
            {
                this.Connection.SQLParametros.Add("@IdAutorizacionIngresoArea", SqlDbType.Decimal).Value = Id;
                var Param = this.Connection.SQLParametros.Add("@Mensaje", SqlDbType.VarChar, 3500);
                Param.Direction = ParameterDirection.Output;
                await this.Connection.EjecutarNonQueryAsync("EliminarAreaAutorizada", CommandType.StoredProcedure);
                return Param.Value.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message + "|error";
            }
        }

        public async Task<string> ActualizarArea(AreaUpdate area)
        {
            try
            {
                var Params = this.GenerarParametrosSql(area);
                await this.Connection.EjecutarNonQueryAsync("ActualizarArea", CommandType.StoredProcedure);
                return Params.FirstOrDefault(x => x.ParameterName == $"@{nameof(area.Mensaje)}").SqlValue.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message + "|error";
            }
        }

        public async Task<string> AgregarAreaAuth(decimal IdAutorizacionIngreso, int IdArea, int IdUser)
        {
            try
            {
                this.Connection.SQLParametros.Add("@IdUser", SqlDbType.Int).Value = IdUser;
                this.Connection.SQLParametros.Add("@IdAutorizacionIngreso",SqlDbType.Decimal).Value = IdAutorizacionIngreso;
                this.Connection.SQLParametros.Add("@IdArea", SqlDbType.Int).Value = IdArea;
                var Param = this.Connection.SQLParametros.Add("@Mensaje", SqlDbType.VarChar, 3500);
                Param.Direction = ParameterDirection.Output;

                await this.Connection.EjecutarNonQueryAsync("AgregarAreaAuth", CommandType.StoredProcedure);
                return Param.SqlValue.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message + "|error";
            }
        }
    }
}
