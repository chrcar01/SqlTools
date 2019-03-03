// <license>
// The MIT License (MIT)
// </license>
// <copyright company="TTRider Technologies, Inc.">
// Copyright (c) 2014-2017 All Rights Reserved
// </copyright>

using System.Collections.Concurrent;
using System.Data.Common;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass

namespace System.Data
{
    public static class AsyncAdapter
    {
        private static readonly ConcurrentDictionary<Type, ConnectionAdapter> ConnectionAdapters =
            new ConcurrentDictionary<Type, ConnectionAdapter>();
        private static readonly ConcurrentDictionary<Type, CommandAdapter> CommandAdapters =
            new ConcurrentDictionary<Type, CommandAdapter>();
        private static readonly ConcurrentDictionary<Type, DataReaderAdapter> DataReaderAdapters =
            new ConcurrentDictionary<Type, DataReaderAdapter>();

        private class ConnectionAdapter
        {
            internal readonly Func<IDbConnection, Task> OpenAsync;
            internal readonly Func<IDbConnection, CancellationToken, Task> OpenAsyncToken;

            internal ConnectionAdapter(Type type)
            {
                if (type.GetRuntimeMethod("OpenAsync", new Type[0]) != null)
                {
                    OpenAsync = async connection =>
                    {
                        dynamic cmd = connection;
                        await cmd.OpenAsync();
                    };
                }
                else
                {
                    OpenAsync = async connection => await Task.Run(() =>
                    {
                        connection.Open();
                    });
                }

                if (type.GetRuntimeMethod("OpenAsync", new[] { typeof(CancellationToken) }) != null)
                {
                    OpenAsyncToken = async (connection, token) =>
                    {
                        dynamic cmd = connection;
                        await cmd.OpenAsync(token);
                    };
                }
                else
                {
                    OpenAsyncToken = async (connection, token) => await Task.Run(() =>
                    {
                        connection.Open();
                    });
                }
            }
        }
        private class CommandAdapter
        {
            internal readonly Func<IDbCommand, Task<int>> ExecuteNonQueryAsync;
            internal readonly Func<IDbCommand, CancellationToken, Task<int>> ExecuteNonQueryAsyncToken;

            internal readonly Func<IDbCommand, Task<IDataReader>> ExecuteReaderAsync;
            internal readonly Func<IDbCommand, CancellationToken, Task<IDataReader>> ExecuteReaderAsyncToken;
            internal readonly Func<IDbCommand, CommandBehavior, Task<IDataReader>> ExecuteReaderAsyncBehavior;
            internal readonly Func<IDbCommand, CommandBehavior, CancellationToken, Task<IDataReader>> ExecuteReaderAsyncBehaviorToken;

            internal readonly Func<IDbCommand, Task<object>> ExecuteScalarAsync;
            internal readonly Func<IDbCommand, CancellationToken, Task<object>> ExecuteScalarAsyncToken;

            internal CommandAdapter(Type type)
            {
                #region ExecuteNonQueryAsync
                if (type.GetRuntimeMethod("ExecuteNonQueryAsync", new Type[0]) != null)
                {
                    ExecuteNonQueryAsync = async command =>
                    {
                        dynamic cmd = command;
                        return await cmd.ExecuteNonQueryAsync();
                    };
                }
                else
                {
                    ExecuteNonQueryAsync = async command => await Task.FromResult(command.ExecuteNonQuery());
                }

                if (type.GetRuntimeMethod("ExecuteNonQueryAsync", new[] { typeof(CancellationToken) }) != null)
                {
                    ExecuteNonQueryAsyncToken = async (command, token) =>
                    {
                        dynamic cmd = command;
                        return await cmd.ExecuteNonQueryAsync(token);
                    };
                }
                else
                {
                    ExecuteNonQueryAsyncToken = async (command, token) => await Task.FromResult(command.ExecuteNonQuery());
                }
                #endregion ExecuteNonQueryAsync

                #region ExecuteReaderAsync

                if (type.GetRuntimeMethod("ExecuteReaderAsync", new Type[0]) != null)
                {
                    ExecuteReaderAsync = async command =>
                    {
                        dynamic cmd = command;
                        return await cmd.ExecuteReaderAsync();
                    };
                }
                else
                {
                    ExecuteReaderAsync = async command => await Task.FromResult(command.ExecuteReader());
                }

                if (type.GetRuntimeMethod("ExecuteReaderAsync", new[] { typeof(CancellationToken) }) != null)
                {
                    ExecuteReaderAsyncToken = async (command, token) =>
                    {
                        dynamic cmd = command;
                        return await cmd.ExecuteReaderAsync(token);
                    };
                }
                else
                {
                    ExecuteReaderAsyncToken = async (command, token) => await Task.FromResult(command.ExecuteReader());
                }

                if (type.GetRuntimeMethod("ExecuteReaderAsync", new[] { typeof(CommandBehavior) }) != null)
                {
                    ExecuteReaderAsyncBehavior = async (command, behavior) =>
                    {
                        dynamic cmd = command;
                        return await cmd.ExecuteReaderAsync(behavior);
                    };
                }
                else
                {
                    ExecuteReaderAsyncBehavior = async (command, behavior) => await Task.FromResult(command.ExecuteReader());
                }

                if (type.GetRuntimeMethod("ExecuteReaderAsync", new[] { typeof(CommandBehavior), typeof(CancellationToken) }) != null)
                {
                    ExecuteReaderAsyncBehaviorToken = async (command, behavior, token) =>
                    {
                        dynamic cmd = command;
                        return await cmd.ExecuteReaderAsync(behavior, token);
                    };
                }
                else
                {
                    ExecuteReaderAsyncBehaviorToken = async (command, behavior, token) => await Task.FromResult(command.ExecuteReader());
                }

                #endregion ExecuteReaderAsync

                #region ExecuteScalarAsync

                if (type.GetRuntimeMethod("ExecuteScalarAsync", new Type[0]) != null)
                {
                    ExecuteScalarAsync = async command =>
                    {
                        dynamic cmd = command;
                        return await cmd.ExecuteScalarAsync();
                    };
                }
                else
                {
                    ExecuteScalarAsync = async command => await Task.FromResult(command.ExecuteScalarAsync());
                }

                if (type.GetRuntimeMethod("ExecuteScalarAsync", new[] { typeof(CancellationToken) }) != null)
                {
                    ExecuteScalarAsyncToken = async (command, token) =>
                    {
                        dynamic cmd = command;
                        return await cmd.ExecuteScalarAsync();
                    };
                }
                else
                {
                    ExecuteScalarAsyncToken = async (command, token) => await Task.FromResult(command.ExecuteScalarAsync());
                }

                #endregion ExecuteScalarAsync
            }
        }
        private class DataReaderAdapter
        {
            internal readonly Func<IDataReader, int, Task<bool>> IsDBNullAsync;
            internal readonly Func<IDataReader, int, CancellationToken, Task<bool>> IsDBNullAsyncToken;
            internal readonly Func<IDataReader, Task<bool>> NextResultAsync;
            internal readonly Func<IDataReader, CancellationToken, Task<bool>> NextResultAsyncToken;
            internal readonly Func<IDataReader, Task<bool>> ReadAsync;
            internal readonly Func<IDataReader, CancellationToken, Task<bool>> ReadAsyncToken;
            private readonly MethodInfo getFieldValueAsync;
            private readonly MethodInfo getFieldValueAsyncToken;
            internal DataReaderAdapter(Type type)
            {
                if (type.GetRuntimeMethod("IsDBNullAsync", new[] { typeof(int) }) != null)
                {
                    IsDBNullAsync = async (reader, ordinal) =>
                    {
                        dynamic cmd = reader;
                        return await cmd.IsDBNullAsync(ordinal);
                    };
                }
                else
                {
                    IsDBNullAsync = async (reader, ordinal) => await Task.FromResult(reader.IsDBNull(ordinal));
                }

                if (type.GetRuntimeMethod("IsDBNullAsync", new[] { typeof(int), typeof(CancellationToken) }) != null)
                {
                    IsDBNullAsyncToken = async (reader, ordinal, token) =>
                    {
                        dynamic cmd = reader;
                        return await cmd.IsDBNullAsync(ordinal, token);
                    };
                }
                else
                {
                    IsDBNullAsyncToken = async (reader, ordinal, token) => await Task.FromResult(reader.IsDBNull(ordinal));
                }

                if (type.GetRuntimeMethod("NextResultAsync", new Type[0]) != null)
                {
                    NextResultAsync = async reader =>
                    {
                        dynamic cmd = reader;
                        return await cmd.NextResultAsync();
                    };
                }
                else
                {
                    NextResultAsync = async reader => await Task.FromResult(reader.NextResult());
                }

                if (type.GetRuntimeMethod("NextResultAsync", new[] { typeof(int), typeof(CancellationToken) }) != null)
                {
                    NextResultAsyncToken = async (reader, token) =>
                    {
                        dynamic cmd = reader;
                        return await cmd.NextResultAsync(token);
                    };
                }
                else
                {
                    NextResultAsyncToken = async (reader, token) => await Task.FromResult(reader.NextResult());
                }

                if (type.GetRuntimeMethod("ReadAsync", new Type[0]) != null)
                {
                    ReadAsync = async reader =>
                    {
                        dynamic cmd = reader;
                        return await cmd.ReadAsync();
                    };
                }
                else
                {
                    ReadAsync = async reader => await Task.FromResult(reader.Read());
                }

                if (type.GetRuntimeMethod("ReadAsync", new[] { typeof(int), typeof(CancellationToken) }) != null)
                {
                    ReadAsyncToken = async (reader, token) =>
                    {
                        dynamic cmd = reader;
                        return await cmd.ReadAsync(token);
                    };
                }
                else
                {
                    ReadAsyncToken = async (reader, token) => await Task.FromResult(reader.Read());
                }

                // for template function we have to defer checks
                this.getFieldValueAsync = type.GetRuntimeMethod("GetFieldValueAsync", new[] { typeof(int) });
                this.getFieldValueAsyncToken = type.GetRuntimeMethod("GetFieldValueAsync", new[] { typeof(int), typeof(CancellationToken) });
            }


            internal Task<T> DoGetFieldValueAsync<T>(IDataReader reader, int ordinal)
            {
                if (this.getFieldValueAsync != null)
                {
                    var method = this.getFieldValueAsync.MakeGenericMethod(typeof(T));
                    return (Task<T>)method.Invoke(reader, new object[] { ordinal });
                }
                return Task.FromResult((T)reader.GetValue(ordinal));
            }
            internal Task<T> DoGetFieldValueAsync<T>(IDataReader reader, int ordinal, CancellationToken token)
            {
                if (this.getFieldValueAsyncToken != null)
                {
                    var method = this.getFieldValueAsyncToken.MakeGenericMethod(typeof(T));
                    return (Task<T>)method.Invoke(reader, new object[] { ordinal, token });
                }
                return Task.FromResult((T)reader.GetValue(ordinal));
            }
        }


        public static Task OpenAsync(this IDbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            var dbConnection = connection as DbConnection;
            if (dbConnection != null) return dbConnection.OpenAsync();
            return ConnectionAdapters.GetOrAdd(connection.GetType(),
                type => new ConnectionAdapter(type))
                    .OpenAsync(connection);
        }

        public static Task OpenAsync(this IDbConnection connection, CancellationToken token)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            var dbConnection = connection as DbConnection;
            if (dbConnection != null) return dbConnection.OpenAsync(token);
            return ConnectionAdapters.GetOrAdd(connection.GetType(),
                type => new ConnectionAdapter(type))
                    .OpenAsyncToken(connection, token);
        }


        public static Task<int> ExecuteNonQueryAsync(this IDbCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var dbCommand = command as DbCommand;
            if (dbCommand != null) return dbCommand.ExecuteNonQueryAsync();
            return CommandAdapters.GetOrAdd(command.GetType(),
                type => new CommandAdapter(type))
                    .ExecuteNonQueryAsync(command);
        }

        public static Task<int> ExecuteNonQueryAsync(this IDbCommand command, CancellationToken token)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var dbCommand = command as DbCommand;
            if (dbCommand != null) return dbCommand.ExecuteNonQueryAsync(token);
            return CommandAdapters.GetOrAdd(command.GetType(),
                type => new CommandAdapter(type))
                    .ExecuteNonQueryAsyncToken(command, token);
        }

        public static Task<IDataReader> ExecuteReaderAsync(this IDbCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var dbCommand = command as DbCommand;
            if (dbCommand != null) return dbCommand.ExecuteReaderAsync().ContinueWith<IDataReader>(t=>t.Result);
            return CommandAdapters.GetOrAdd(command.GetType(),
                type => new CommandAdapter(type))
                    .ExecuteReaderAsync(command);
        }

        public static Task<IDataReader> ExecuteReaderAsync(this IDbCommand command, CancellationToken token)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var dbCommand = command as DbCommand;
            if (dbCommand != null) return dbCommand.ExecuteReaderAsync(token).ContinueWith<IDataReader>(t => t.Result, token);
            return CommandAdapters.GetOrAdd(command.GetType(),
                type => new CommandAdapter(type))
                    .ExecuteReaderAsyncToken(command, token);
        }

        public static Task<IDataReader> ExecuteReaderAsync(this IDbCommand command, CommandBehavior commandBehavior)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var dbCommand = command as DbCommand;
            if (dbCommand != null) return dbCommand.ExecuteReaderAsync(commandBehavior).ContinueWith<IDataReader>(t => t.Result);
            return CommandAdapters.GetOrAdd(command.GetType(),
                type => new CommandAdapter(type))
                    .ExecuteReaderAsyncBehavior(command, commandBehavior);
        }

        public static Task<IDataReader> ExecuteReaderAsync(this IDbCommand command, CommandBehavior commandBehavior,
            CancellationToken token)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var dbCommand = command as DbCommand;
            if (dbCommand != null) return dbCommand.ExecuteReaderAsync(commandBehavior, token).ContinueWith<IDataReader>(t => t.Result, token);
            return CommandAdapters.GetOrAdd(command.GetType(),
                type => new CommandAdapter(type))
                    .ExecuteReaderAsyncBehaviorToken(command, commandBehavior, token);
        }

        public static Task<Object> ExecuteScalarAsync(this IDbCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var dbCommand = command as DbCommand;
            if (dbCommand != null) return dbCommand.ExecuteScalarAsync();
            return CommandAdapters.GetOrAdd(command.GetType(),
                type => new CommandAdapter(type))
                    .ExecuteScalarAsync(command);
        }

        public static Task<Object> ExecuteScalarAsync(this IDbCommand command, CancellationToken token)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var dbCommand = command as DbCommand;
            if (dbCommand != null) return dbCommand.ExecuteScalarAsync(token);
            return CommandAdapters.GetOrAdd(command.GetType(),
                type => new CommandAdapter(type))
                    .ExecuteScalarAsyncToken(command, token);
        }



        public static Task<T> GetFieldValueAsync<T>(this IDataReader reader, int ordinal)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            var dataReader = reader as DbDataReader;
            if (dataReader != null) return dataReader.GetFieldValueAsync<T>(ordinal);
            return DataReaderAdapters.GetOrAdd(reader.GetType(),
                type => new DataReaderAdapter(type))
                    .DoGetFieldValueAsync<T>(reader, ordinal);
        }
        public static Task<T> GetFieldValueAsync<T>(this IDataReader reader, int ordinal, CancellationToken token)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            var dataReader = reader as DbDataReader;
            if (dataReader != null) return dataReader.GetFieldValueAsync<T>(ordinal, token);
            return DataReaderAdapters.GetOrAdd(reader.GetType(),
                type => new DataReaderAdapter(type))
                    .DoGetFieldValueAsync<T>(reader, ordinal, token);
        }
        public static Task<bool> IsDBNullAsync(this IDataReader reader, int ordinal)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            var dataReader = reader as DbDataReader;
            if (dataReader != null) return dataReader.IsDBNullAsync(ordinal);
            return DataReaderAdapters.GetOrAdd(reader.GetType(),
                type => new DataReaderAdapter(type))
                    .IsDBNullAsync(reader, ordinal);
        }
        public static Task<bool> IsDBNullAsync(this IDataReader reader, int ordinal, CancellationToken token)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            var dataReader = reader as DbDataReader;
            if (dataReader != null) return dataReader.IsDBNullAsync(ordinal, token);
            return DataReaderAdapters.GetOrAdd(reader.GetType(),
                type => new DataReaderAdapter(type))
                    .IsDBNullAsyncToken(reader, ordinal, token);
        }
        public static Task<bool> NextResultAsync(this IDataReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            var dataReader = reader as DbDataReader;
            if (dataReader != null) return dataReader.NextResultAsync();
            return DataReaderAdapters.GetOrAdd(reader.GetType(),
                type => new DataReaderAdapter(type))
                    .NextResultAsync(reader);
        }
        public static Task<bool> NextResultAsync(this IDataReader reader, CancellationToken token)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            var dataReader = reader as DbDataReader;
            if (dataReader != null) return dataReader.NextResultAsync(token);
            return DataReaderAdapters.GetOrAdd(reader.GetType(),
                type => new DataReaderAdapter(type))
                    .NextResultAsyncToken(reader, token);
        }
        public static Task<bool> ReadAsync(this IDataReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            var dataReader = reader as DbDataReader;
            if (dataReader != null) return dataReader.ReadAsync();
            return DataReaderAdapters.GetOrAdd(reader.GetType(),
                type => new DataReaderAdapter(type))
                    .ReadAsync(reader);
        }
        public static Task<bool> ReadAsync(this IDataReader reader, CancellationToken token)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            var dataReader = reader as DbDataReader;
            if (dataReader != null) return dataReader.ReadAsync(token);
            return DataReaderAdapters.GetOrAdd(reader.GetType(),
                type => new DataReaderAdapter(type))
                    .ReadAsyncToken(reader, token);
        }
    }
}