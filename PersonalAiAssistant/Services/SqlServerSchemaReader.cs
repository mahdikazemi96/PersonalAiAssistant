using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PersonalAiAssistant.Models;

namespace PersonalAiAssistant.Services;

public class SqlServerSchemaReader : IDatabaseSchemaReader
{
    private readonly string _connectionString;

    public SqlServerSchemaReader(
        IConfiguration configuration)
    {
        _connectionString =
            configuration.GetConnectionString("SqlServer")
            ?? throw new InvalidOperationException(
                "Connection string 'SqlServer' not found.");
    }

    public async Task<DatabaseSchema> ReadAsync()
    {
        var databaseSchema = new DatabaseSchema();

        await using var connection =
            new SqlConnection(_connectionString);

        await connection.OpenAsync();

        const string sql = @"
            SELECT
                t.TABLE_SCHEMA,
                t.TABLE_NAME,
                c.COLUMN_NAME,
                c.DATA_TYPE
            FROM INFORMATION_SCHEMA.TABLES t
            INNER JOIN INFORMATION_SCHEMA.COLUMNS c
                ON t.TABLE_SCHEMA = c.TABLE_SCHEMA
               AND t.TABLE_NAME = c.TABLE_NAME
            WHERE t.TABLE_TYPE IN ('BASE TABLE','VIEW')
            ORDER BY
                t.TABLE_SCHEMA,
                t.TABLE_NAME,
                c.ORDINAL_POSITION;";

        await using var command =
            new SqlCommand(sql, connection);

        await using var reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var schemaName = reader.GetString(0);
            var tableName = reader.GetString(1);
            var columnName = reader.GetString(2);
            var dataType = reader.GetString(3);

            var table =
                databaseSchema.Tables.FirstOrDefault(x =>
                    x.Schema.Equals(schemaName, StringComparison.OrdinalIgnoreCase) &&
                    x.Name.Equals(tableName, StringComparison.OrdinalIgnoreCase));

            if (table == null)
            {
                table = new TableSchema
                {
                    Schema = schemaName,
                    Name = tableName
                };

                databaseSchema.Tables.Add(table);
            }

            table.Columns.Add(new ColumnSchema
            {
                Name = columnName,
                DataType = dataType
            });
        }

        return databaseSchema;
    }
}