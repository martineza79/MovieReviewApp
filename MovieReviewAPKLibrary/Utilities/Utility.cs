using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace MovieReviewAPILibrary
{
    public static class Utility
    {
        public static string SerializeObject (Object obj)
        {
            return SerializeObject(obj, false);
        }

        public static string SerializeObject(Object obj, bool generateNamespace)
        {
            XmlSerializerNamespaces ns = null;
            XmlSerializer ser = new XmlSerializer(obj.GetType()); ;

            if (generateNamespace)
            {
                ns = new XmlSerializerNamespaces();
                ns.Add("", "");
            }

            StringBuilder sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb, new XmlWriterSettings() { OmitXmlDeclaration = true }))
            {
                if (ns == null)
                    ser.Serialize(writer, obj);
                else
                    ser.Serialize(writer, obj, ns);
                writer.Flush();
                writer.Close();
            }
            return sb.ToString();
        }

        public static string JsonSerilizeObject(Object obj)
        {
            JavaScriptSerializer objJsonSer = new JavaScriptSerializer();
            return objJsonSer.Serialize(obj);
        }

        public static T DeserializerObject<T>(string serialedObject)
        {
            TextReader objTextReader = new StringReader(serialedObject);
            XmlSerializer objXmlSer = new XmlSerializer(typeof(T));
            return ((T)(Convert.ChangeType(objXmlSer.Deserialize(objTextReader), typeof(T))));
        }

        public static T JsonDeserializeObject<T>(string serializedObject)
        {
            JavaScriptSerializer objJsonSer = new JavaScriptSerializer();
            return ((T)(Convert.ChangeType(objJsonSer.Deserialize<T> (serializedObject), typeof(T))));
        }

        public static DataTable ToDataTable<T>(this IList<T> objList)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable objDataTable = new DataTable();
            foreach (PropertyDescriptor property in properties)
                objDataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            if (objList != null)
            {
                foreach (T item in objList)
                {
                    DataRow row = objDataTable.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    objDataTable.Rows.Add(row);
                }
            }
            return objDataTable;
        }

        public static DataTable ConvertEntityListToDataTable<T>(this IList<T> objList, List<Type> excludedTypes)
        {
            if (excludedTypes == null)
                excludedTypes = new List<Type>();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable objDataTable = new DataTable();
            foreach (PropertyDescriptor property in properties)
                if (!excludedTypes.Contains(property.PropertyType))
                    objDataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            if (objList != null)
            {
                foreach (T item in objList)
                {
                    DataRow row = objDataTable.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    objDataTable.Rows.Add(row);
                }
            }
            return objDataTable;
        }

        private static T ConvertDataRowToEntity<T>(this DataRow tableRow) where T : new()
        {
            //Create a new type of the entity I want
            Type t = typeof(T);
            T returnObject = new T();

            foreach (DataColumn col in tableRow.Table.Columns)
            {
                string colName = col.ColumnName;

                //Look for the ojbect's property with the columns name, ignore case
                PropertyInfo pInfo = t.GetProperty(colName.ToLower(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.SetProperty);

                // did we find the property?
                if (pInfo != null)
                {
                    object val = tableRow[colName];

                    // is this a Nullable<> type
                    bool IsNullable = (Nullable.GetUnderlyingType(pInfo.PropertyType) != null);
                    if (IsNullable)
                    {
                        if (val is System.DBNull)
                        {
                            val = null;
                        }
                        else
                        {
                            // Convert the db type into the T we have in our Nullable<T> type
                            val = Convert.ChangeType(val, Nullable.GetUnderlyingType(pInfo.PropertyType));
                        }
                    }
                    else
                    {
                        // Convert the db type into the type of the property in our entity
                        if (pInfo.PropertyType.IsEnum)
                            val = Enum.ToObject(pInfo.PropertyType, val);
                        else
                            val = Convert.ChangeType(val, pInfo.PropertyType);
                    }

                    // Set the value of the property with the value from the db
                    pInfo.SetValue(returnObject, val, null);
               }
           }
           
            // return the entity object with values
            return returnObject;           
        }

        public static List<T> ConvertDataTableToEntityList<T>(this DataTable table) where T : new()
        {
            Type t = typeof(T);

            // Create a list of the entities we want to return
            List<T> returnObject = new List<T>();

            // Iterate through the DataTable's rows
            foreach (DataRow dr in table.Rows)
            {
                //Convert each row into an entity object and add to the list
                T newRow = dr.ConvertDataRowToEntity<T>();
                returnObject.Add(newRow);
            }

            // Return the finished list
            return returnObject;
        }

        public static string Base64Encode(string plainText)
        {
            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(plainText));
        }

        public static string Bases64Decode(string base64EncodedData)
        {
            return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(base64EncodedData));
        }

        public static string SerializeObjectUsingDataContractSerializer(Object obj)
        {
            var serializer = new DataContractSerializer(obj.GetType());
            using (var sw = new StringWriter())
            {
                using (var writer = new XmlTextWriter(sw))
                {
                    writer.Formatting = Formatting.None;
                    serializer.WriteObject(writer, obj);
                    writer.Flush();
                    return sw.ToString();
                }
            }
        }
    }
}
