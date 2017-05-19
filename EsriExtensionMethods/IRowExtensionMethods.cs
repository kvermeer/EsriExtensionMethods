using ESRI.ArcGIS.Geodatabase;
using System;

namespace EsriExtensionMethods
{
    public static class IRowExtensionMethods
    {
        /// <summary>
        /// Get value for given field in a table row
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static object GetValue(this IRow row, string fieldName)
        {
            row.VerifyFieldExists(fieldName);
            var value = HandleDbNullValues(row.Value[row.GetFieldIndex(fieldName)]);
            return value;
        }

        /// <summary>
        /// Search & Get Field in a input table row
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static IField GetField(this IRow row, string fieldName)
        {
            row.VerifyFieldExists(fieldName);
            return row.Fields.Field[row.GetFieldIndex(fieldName)];
        }

        /// <summary>
        /// Sets the value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static bool SetValue(this IRow row, string fieldName, object value)
        {
            row.VerifyFieldExists(fieldName);
            if (value == null)
                row.Value[row.GetFieldIndex(fieldName)] = DBNull.Value;
            else
                row.Value[row.GetFieldIndex(fieldName)] = value;
            return true;
        }

        private static void VerifyFieldExists(this IRow row, string fieldName)
        {
            if (row.GetFieldIndex(fieldName) == -1)
            {
                throw new Exception(string.Format("{0} '{1}' does not contain field '{2}' ",
                    row.Table is IFeatureClass ? "Featureclass" : "Table",
                    ((IDataset)row.Table).Name,
                    fieldName));
            }
        }

        private static int GetFieldIndex(this IRow row, string fieldName)
        {
            return row.Fields.FindField(fieldName);
        }

        private static object HandleDbNullValues(object val)
        {
            return val is DBNull
                ? null
                : val;
        }

    }
}
