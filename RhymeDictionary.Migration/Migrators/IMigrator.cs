using Npgsql;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RhymeDictionary.Migration.Migrators
{
    internal interface IMigrator
    {
        Task Setup(SqlConnection sqlConnection, NpgsqlConnection pgConnection);
        Task Migrate(SqlConnection sqlConnection, NpgsqlConnection pgConnection, IProgress<(int, int)> progress);
        Task Teardown(SqlConnection sqlConnection, NpgsqlConnection pgConnection);
    }
}