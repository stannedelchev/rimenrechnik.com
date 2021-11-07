using Dapper;
using Dapper.FastCrud;
using Npgsql;
using RhymeDictionary.Migration.DapperFastCrudEntities;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RhymeDictionary.Migration.Migrators
{
    internal class WordsMigrator : IMigrator
    {
        public async Task Setup(SqlConnection sqlConnection, NpgsqlConnection pgConnection)
        {
            await pgConnection.ExecuteAsync(@"


DROP TABLE IF EXISTS public.""Words"";

CREATE TABLE public.""Words""
(
    ""Id"" uuid NOT NULL,
    ""Normal"" text COLLATE pg_catalog.""default"" NOT NULL,
    ""Forward"" text COLLATE pg_catalog.""default"" NOT NULL,
	""Reverse"" text COLLATE pg_catalog.""default"" NOT NULL,
	""ReverseEnding"" text COLLATE pg_catalog.""default"" NULL,
    CONSTRAINT ""Words_pkey"" PRIMARY KEY (""Id"")
)

TABLESPACE pg_default;
");
        }

        public async Task Migrate(SqlConnection sqlConnection, NpgsqlConnection pgConnection, IProgress<(int, int)> progress)
        {
            var words = await sqlConnection.QueryAsync<WordEntity>("SELECT Id, Normal, Forward, Reverse, ReverseEnding FROM Words");

            await using var transaction = await pgConnection.BeginTransactionAsync();

            foreach (var item in words.WithProgress())
            {
                await pgConnection.InsertAsync(item.Item, so => so.AttachToTransaction(transaction));
                progress.Report((item.Position, item.Total));
            }

            await transaction.CommitAsync();
        }

        public Task Teardown(SqlConnection sqlConnection, NpgsqlConnection pgConnection) => Task.CompletedTask;
    }
}