using Dapper;
using Dapper.FastCrud;
using Npgsql;
using RhymeDictionary.Migration.DapperFastCrudEntities;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RhymeDictionary.Migration.Migrators
{
    internal class SiteCommentsMigrator : IMigrator
    {
        public async Task Setup(SqlConnection sqlConnection, NpgsqlConnection pgConnection)
        {
            await pgConnection.ExecuteAsync(@"

DROP TABLE IF EXISTS public.""SiteComments"";

CREATE TABLE public.""SiteComments""
(
    ""Id"" uuid NOT NULL,
    ""Email"" text COLLATE pg_catalog.""default"" NOT NULL,
    ""Text"" text COLLATE pg_catalog.""default"" NOT NULL,
    ""DateSavedUtc"" timestamp without time zone NOT NULL,
    CONSTRAINT ""SiteComments_pkey"" PRIMARY KEY(""Id"")
)

TABLESPACE pg_default;
");
        }

        public async Task Migrate(SqlConnection sqlConnection, NpgsqlConnection pgConnection, IProgress<(int, int)> progress)
        {
            var siteComments = sqlConnection.Query<SiteCommentEntity>("SELECT Id, Email, Text, DateSavedUtc FROM SiteComments");

            await using var transaction = await pgConnection.BeginTransactionAsync();

            foreach (var item in siteComments.WithProgress())
            {
                await pgConnection.InsertAsync(item.Item, so => so.AttachToTransaction(transaction));
                progress.Report((item.Position, item.Total));
            }

            await transaction.CommitAsync();
        }

        public Task Teardown(SqlConnection sqlConnection, NpgsqlConnection pgConnection) => Task.CompletedTask;
    }
}