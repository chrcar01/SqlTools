using System;
using System.Data;
using System.Linq;
using FastMember;

namespace SqlTools.Tests
{
    public class FastmemberDataReaderObjectMapper : IDataReaderObjectMapper
    {
        public TModel Map<TModel>(IDataReader reader) where TModel : new()
        {
            TModel result = default;
            if (reader == null || reader.IsClosed)
            {
                return result;
            }
             
            result = new TModel();
            var accessor = TypeAccessor.Create(typeof(TModel));
            var members = accessor.GetMembers();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var member = members.FirstOrDefault(m => m.Name.Equals(reader.GetName(i), StringComparison.OrdinalIgnoreCase));
                var fieldName = member?.Name;
                if (members.Any(m => m.CanWrite && String.Equals(fieldName, m.Name, StringComparison.OrdinalIgnoreCase))
                    && !reader.IsDBNull(i))
                {
                    accessor[result, fieldName] = reader.GetValue(i);
                }
            }
            return result;
        }
    }
}