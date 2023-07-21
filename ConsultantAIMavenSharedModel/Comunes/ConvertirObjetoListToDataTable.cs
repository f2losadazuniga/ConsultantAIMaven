using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ConsultantAIMavenSharedModel.Comunes
{
    public static class ConvertirObjetoListToDataTable
    {
        public static DataTable ConvertToDataTable<T>(List<T> objectList)
        {
            DataTable dataTable = new DataTable();
            if (objectList == null || objectList.Count == 0)
            {
                return dataTable;
            }

            // Obtener las propiedades del tipo T
            var properties = typeof(T).GetProperties();

            // Crear las columnas en la tabla DataTable basadas en las propiedades del tipo T
            foreach (var property in properties)
            {
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            // Agregar los datos de los objetos a la tabla DataTable
            foreach (var item in objectList)
            {
                DataRow row = dataTable.NewRow();
                foreach (var property in properties)
                {
                    row[property.Name] = property.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}
