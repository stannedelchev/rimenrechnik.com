using Dapper;
using Dapper.FastCrud;
using Npgsql;
using RhymeDictionary.Migration.DapperFastCrudEntities;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RhymeDictionary.Migration.Migrators
{
    internal class WordSearchVisitsMigrator : IMigrator
    {
        public async Task Setup(SqlConnection sqlConnection, NpgsqlConnection pgConnection)
        {
            await pgConnection.ExecuteAsync(@"

DROP TABLE IF EXISTS public.""WordSearchVisits"";

CREATE TABLE public.""WordSearchVisits""
(
    ""Id"" uuid NOT NULL,
    ""Word"" text COLLATE pg_catalog.""default"" NOT NULL,
    ""DateSearchedUtc"" timestamp without time zone NOT NULL,
	""IpAddress"" text COLLATE pg_catalog.""default""  NOT NULL,
    CONSTRAINT ""WordSearchVisits_pkey"" PRIMARY KEY (""Id"")
)

TABLESPACE pg_default;
");
        }

        public async Task Migrate(SqlConnection sqlConnection, NpgsqlConnection pgConnection, IProgress<(int, int)> progress)
        {
            var totalItems = await sqlConnection.CountAsync<WordSearchVisitEntity>();
            var wordSearchVisits = sqlConnection.Query<WordSearchVisitEntity>(new CommandDefinition("SELECT Id, Word, DateSearchedUtc, IpAddress FROM WordSearchVisits", CommandFlags.Pipelined));

            await using var transaction = await pgConnection.BeginTransactionAsync();

            foreach (var item in wordSearchVisits.WithProgress(totalItems))
            {
                await pgConnection.InsertAsync(item.Item, so => so.AttachToTransaction(transaction));
                progress.Report((item.Position, item.Total));
            }

            await transaction.CommitAsync();
        }

        public Task Teardown(SqlConnection sqlConnection, NpgsqlConnection pgConnection) => Task.CompletedTask;
    }
}