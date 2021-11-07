using CommandLine;
using Dapper.FastCrud;
using Npgsql;
using RhymeDictionary.Migration.Migrators;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RhymeDictionary.Migration
{
    internal class Program
    {
        [Verb("chitanka", HelpText = "Migrate words from a Chitanka database to RhymeDictionary PostgreSQL database.")]
        private class ChitankaCommandLineOptions
        {
            [Option('s', "source", Required = true, HelpText = "Chitanka database .NET connection string.")]
            public string ChitankaConnectionString { get; set; }

            [Option('d', "destination", Required = true, HelpText = "PostgreSQL .NET connection string.")]
            public string PostgreSqlConnectionString { get; set; }
        }
        
        [Verb("sql-server", HelpText = "Migrate entire RhymeDictionary Microsoft SQL Server database to PostgreSQL.")]
        private class MicrosoftSqlServerCommandLineOptions
        {
            [Option('s', "source", Required = true, HelpText = "Microsoft SQL Server .NET connection string.")]
            public string SqlServerConnectionString { get; set; }

            [Option('d', "destination", Required = true, HelpText = "PostgreSQL .NET connection string.")]
            public string PostgreSqlConnectionString { get; set; }
        }

        private static async Task Main(string[] args)
        {
            var parser = Parser.Default.ParseArguments<ChitankaCommandLineOptions, MicrosoftSqlServerCommandLineOptions>(args);

            await parser.WithParsedAsync<ChitankaCommandLineOptions>(async o =>
            {
                var chitankaConnectionString = o.ChitankaConnectionString;
                var pgConnectionString = o.PostgreSqlConnectionString;
                await MigrateChitanka(chitankaConnectionString, pgConnectionString);
            });

            await parser.WithParsedAsync<MicrosoftSqlServerCommandLineOptions>(async o =>
            {
                var sqlConnectionString = o.SqlServerConnectionString;
                var pgConnectionString = o.PostgreSqlConnectionString;
                await MigrateRhymeDatabase(sqlConnectionString, pgConnectionString);
            });
        }

        private static async Task MigrateChitanka(string chitankaConnectionString, string pgConnectionString)
        {
            throw new NotImplementedException();
        }

        private static async Task MigrateRhymeDatabase(string sqlConnectionString, string pgConnectionString)
        {
            try
            {
                Dapper.FastCrud.OrmConfiguration.DefaultDialect = SqlDialect.PostgreSql;

                await using var sqlConnection = new SqlConnection(sqlConnectionString);
                await using var pgConnection = new NpgsqlConnection(pgConnectionString);

                await pgConnection.OpenAsync();
                await sqlConnection.OpenAsync();

                var migrators = new IMigrator[]
                {
                    new WordsMigrator(),
                    new SiteCommentsMigrator(),
                    new WordAdditionsMigrator(),
                    new WordSuggestionsMigrator(),
                    new WordSearchVisitsMigrator(),
                };

                foreach (var migrator in migrators)
                {
                    Console.WriteLine($"Migrating {migrator.GetType().Name}");

                    await migrator.Setup(sqlConnection, pgConnection);
                    await migrator.Migrate(sqlConnection, pgConnection, new ConsoleWritingProgress(TimeSpan.FromSeconds(0.5)));
                    await migrator.Teardown(sqlConnection, pgConnection);

                    Console.WriteLine($"Migrated {migrator.GetType().Name} successfully");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
