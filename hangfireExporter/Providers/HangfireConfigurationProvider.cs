using Hangfire;
using Hangfire.Azure.ServiceBusQueue;
using Hangfire.LiteDB;
using Hangfire.MemoryStorage;
using Hangfire.Mongo;
using Hangfire.MySql;
using Hangfire.PostgreSql;
using Hangfire.SqlServer;
using Hangfire.States;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using StackExchange.Redis;
using System;
using System.Transactions;

namespace hangfireExporter.Providers
{
    public static class HangfireConfigurationProvider
    {
        public static void GetMongoConfiguration(string connectionString, string mongoDatabaseName)
        {
            GlobalConfiguration.Configuration.UseMongoStorage(connectionString, mongoDatabaseName, new MongoStorageOptions
            {
                Prefix = "hangfire",
                MigrationOptions = new MongoMigrationOptions
                {
                    Strategy = MongoMigrationStrategy.Migrate,
                    BackupStrategy = MongoBackupStrategy.Collections
                }
            });
        }

        public static void GetSqlServerConfiguration(string connectionString)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage(@connectionString);
        }

        public static void GetRedisConfiguration(string redisIP)
        {
            ConnectionMultiplexer Redis = ConnectionMultiplexer.Connect(redisIP);
            GlobalConfiguration.Configuration.UseRedisStorage(Redis);
        }

        public static void GetAzureServiceBusQueueConfiguration(string connectionString)
        {
            var sqlStorage = new SqlServerStorage(connectionString);
            var azureConnectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");

            Action<QueueDescription> configureAction = qd =>
            {
                qd.MaxSizeInMegabytes = 5120;
                qd.DefaultMessageTimeToLive = new TimeSpan(0, 1, 0);
            };

            sqlStorage.UseServiceBusQueues(new ServiceBusQueueOptions
            {
                ConnectionString = azureConnectionString,
                Configure = configureAction,
                QueuePrefix = "hangfire-",
                Queues = new[] { EnqueuedState.DefaultQueue },
                CheckAndCreateQueues = false,
                LoopReceiveTimeout = TimeSpan.FromMilliseconds(500)
            });

            GlobalConfiguration.Configuration.UseStorage(sqlStorage);
        }

        public static void GetLiteDBConfiguration(string liteDBPath)
        {
            GlobalConfiguration.Configuration.UseLiteDbStorage(liteDBPath);
        }

        public static void GetMemoryStorageConfiguration()
        {
            GlobalConfiguration.Configuration.UseMemoryStorage();
        }

        public static void GetMySqlConfiguration(string connectionString)
        {
            GlobalConfiguration.Configuration.UseStorage(new MySqlStorage(connectionString, new MySqlStorageOptions
            {
                TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                QueuePollInterval = TimeSpan.FromSeconds(15),
                JobExpirationCheckInterval = TimeSpan.FromHours(1),
                CountersAggregateInterval = TimeSpan.FromMinutes(5),
                PrepareSchemaIfNecessary = true,
                DashboardJobListLimit = 50000,
                TransactionTimeout = TimeSpan.FromMinutes(1),
                TablesPrefix = "hangfire"
            }));
        }

        public static void GetPostgresConfiguration(string connectionString)
        {            
            GlobalConfiguration.Configuration.UsePostgreSqlStorage(connectionString);
        }
    }
}
