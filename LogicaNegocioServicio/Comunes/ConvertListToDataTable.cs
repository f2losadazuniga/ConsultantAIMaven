using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;


namespace LogicaNegocioServicio.Comunes
{
    public static class ConvertListToDataTable
    {
        public static DataTable ToDataTable1<T>(IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prp = props[i];
                table.Columns.Add(prp.Name, prp.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
        public static DataTable ToDataTable2<T>(IList<T> data, string Colum, Type tipo)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            if (props.Count == 0)
            {
                table.Columns.Add(Colum, tipo);
            }
            foreach (T item in data)
            {
                table.Rows.Add(item);
            }
            return table;
        }

        public static DataTable ToDataTable<T>(IList<T> data,string Colum,Type tipo )
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
           if (props.Count == 0)
            {
                table.Columns.Add(Colum, tipo);
            }            
            foreach (T item in data)
            {                
                table.Rows.Add(item);
            }
            return table;
        }


    }
}
