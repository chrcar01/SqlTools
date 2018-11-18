using System.Data;

namespace SqlTools
{
    /// <summary>
    /// Component capable of mapping a single result in a IDataReader to an instance of a model,
    /// mapping properties to the fields in the IDataReader.
    /// </summary>
    public interface IDataReaderObjectMapper
    {
        /// <summary>
        /// Maps fields in the reader to a model.
        /// </summary>
        /// <typeparam name="TModel">The type of the model being mapped.</typeparam>
        /// <param name="reader">An IDataReader containing source of data to map to the model.</param>
        /// <returns>New instance of the model with data extracted from the IDataReader.</returns>
        TModel Map<TModel>(IDataReader reader) where TModel : new();
    }
}
