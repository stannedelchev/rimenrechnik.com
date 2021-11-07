using Dapper;
using Dapper.FastCrud;
using Npgsql;
using RhymeDictionary.Migration.DapperFastCrudEntities;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RhymeDictionary.Migration.Migrators
{
    internal class WordAdditionsMigrator : IMigrator
    {
        public async Task Setup(SqlConnection sqlConnection, NpgsqlConnection pgConnection)
        {
            await pgConnection.ExecuteAsync(@"

DROP TABLE IF EXISTS public.""WordAdditions"";

CREATE TABLE public.""WordAdditions""
(
    ""Id"" uuid NOT NULL,
    ""Word"" text COLLATE pg_catalog.""default"" NOT NULL,
    ""WordId"" uuid,
    ""HasError"" boolean NOT NULL,
    ""ErrorMessage"" text COLLATE pg_catalog.""default"",
    ""DateAddedUtc"" timestamp without time zone NOT NULL,
    ""IpAddress"" text COLLATE pg_catalog.""default"" NOT NULL,
    CONSTRAINT ""WordAdditions_pkey"" PRIMARY KEY (""Id"")
)

TABLESPACE pg_default;
");
        }

        public async Task Migrate(SqlConnection sqlConnection, NpgsqlConnection pgConnection, IProgress<(int, int)> progress)
        {
            var wordAdditions = await sqlConnection.QueryAsync<WordAdditionEntity>("SELECT Id, Word, WordId, HasError, ErrorMessage, DateAddedUtc, IpAddress FROM WordAdditions");

            await using var transaction = await pgConnection.BeginTransactionAsync();

            foreach (var item in wordAdditions.WithProgress())
            {
                await pgConnection.InsertAsync(item.Item, so => so.AttachToTransaction(transaction));
                progress.Report((item.Position, item.Total));
            }

            await transaction.CommitAsync();
        }

        public Task Teardown(SqlConnection sqlConnection, NpgsqlConnection pgConnection) => Task.CompletedTask;
    }
}