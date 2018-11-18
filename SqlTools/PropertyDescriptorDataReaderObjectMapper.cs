using System;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace SqlTools
{
    /// <summary>
    /// Reflect over a model to determine the fields to pull out of the reader.
    /// </summary>
    public sealed class PropertyDescriptorDataReaderObjectMapper : IDataReaderObjectMapper
    {
        /// <summary>
        /// Maps fields in IDataReader to properties in TModel.
        /// </summary>
        /// <param name="reader">Source of the data being mapped.</param>
        /// <typeparam name="TModel">Target of the data being mapped.</typeparam>
        /// <returns>Null if the reader is null or closed.</returns>
        public TModel Map<TModel>(IDataReader reader) where TModel : new()
        {
            TModel result = default;
            if (reader == null || reader.IsClosed)
            {
                return result;
            }
             
            result = new TModel();
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(result);
            var ordinals = Enumerable.Range(0, reader.FieldCount);
            foreach (PropertyDescriptor prop in props)
            {
                
                var fieldExists = ordinals.Any(ordinal =>
                    String.Equals(reader.GetName(ordinal), prop.Name, StringComparison.OrdinalIgnoreCase));

                if (!fieldExists || reader[prop.Name] == DBNull.Value)
                {
                    continue;
                }

                prop.SetValue(result, ChangeType(reader[prop.Name], prop.PropertyType));
            }
            return result;
        }

        private object ChangeType(object value, Type conversionType)
        {
            if (conversionType == null)
            {
                throw new ArgumentNullException(nameof(conversionType));
            } 

            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (value == null)
                {
                    return null;
                }
                var nullableConverter = new NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }

            return Convert.ChangeType(value, conversionType);
        }
    }
}