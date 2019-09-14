using Hangfire;
using Hangfire.Storage;
using hangfireExporter.Providers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;

namespace hangfireExporter.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class metricsController : ControllerBase
    {
        IMonitoringApi api;
        StringBuilder data;

        public metricsController()
        {
            string dataProviderArg = Environment.GetEnvironmentVariable("dataProvider");
            string connStringArg = Environment.GetEnvironmentVariable("connectionString");
            string dbNameArg = Environment.GetEnvironmentVariable("dbName");

            switch (dataProviderArg)
            {
                case "mongo":
                    HangfireConfigurationProvider.GetMongoConfiguration(connStringArg, dbNameArg);
                    break;
                case "sqlserver":
                    HangfireConfigurationProvider.GetSqlServerConfiguration(connStringArg);
                    break;
                case "redis":
                    HangfireConfigurationProvider.GetRedisConfiguration(connStringArg);
                    break;
                case "azureservicebusqueue":
                    HangfireConfigurationProvider.GetAzureServiceBusQueueConfiguration(connStringArg);
                    break;
                case "litedb":
                    HangfireConfigurationProvider.GetLiteDBConfiguration(connStringArg);
                    break;
                case "memorystorage":
                    HangfireConfigurationProvider.GetMemoryStorageConfiguration();
                    break;
                case "mysql":
                    HangfireConfigurationProvider.GetMySqlConfiguration(connStringArg);
                    break;
                case "postgres":
                    HangfireConfigurationProvider.GetPostgresConfiguration(connStringArg);
                    break;
                default:
                    Console.WriteLine("Data Connection error. Please check connection string.");
                    break;
            }

            api = JobStorage.Current.GetMonitoringApi();
            data = new StringBuilder();
        }

        public string Get()
        {
            data.AppendLine("# Help Servers Count ");
            data.AppendLine("hangfire_servers_count " + api.GetStatistics().Servers.ToString());
            data.AppendLine("# Help Deleted Jobs Count");
            data.AppendLine("hangfire_deleted_jobs_total_count " + api.GetStatistics().Deleted.ToString());
            data.AppendLine("# Help Enqueued Jobs Count");
            data.AppendLine("hangfire_enqueued_jobs_total_count " + api.GetStatistics().Enqueued.ToString());
            data.AppendLine("# Help Failed Jobs Count");
            data.AppendLine("hangfire_failed_jobs_total_count " + api.GetStatistics().Failed.ToString());
            data.AppendLine("# Help Processing Jobs Count");
            data.AppendLine("hangfire_processing_jobs_total_count " + api.GetStatistics().Processing.ToString());
            data.AppendLine("# Help Queues Count");
            data.AppendLine("hangfire_queues_count " + api.GetStatistics().Queues.ToString());
            data.AppendLine("# Help Recurring Jobs Count");
            data.AppendLine("hangfire_recurring_jobs_count " + api.GetStatistics().Recurring.ToString());
            data.AppendLine("# Help Scheduled Jobs Count");
            data.AppendLine("hangfire_scheduled_jobs_total_count " + api.GetStatistics().Scheduled.ToString());
            data.AppendLine("# Help Succeeded Jobs List Count");
            data.AppendLine("hangfire_succeeded_jobs_total_count " + api.GetStatistics().Succeeded.ToString());
            
            data.AppendLine("# Help Failed Jobs By Dates Count");
            foreach (var item in api.FailedByDatesCount())
            {
                data.AppendLine("hangfire_failed_jobs_by_dates_count" +"{key="+ "\""+item.Key.ToShortDateString()+"\"} "+ item.Value);
            }

            data.AppendLine("# Help Succeeded Jobs By Dates Count");
            foreach (var item in api.SucceededByDatesCount())
            {
                data.AppendLine("hangfire_succeeded_jobs_by_dates_count" + "{key=" + "\"" + item.Key.ToShortDateString() + "\"} " + item.Value);
            }

            data.AppendLine("# Help Hourly Failed Jobs Count");
            foreach (var item in api.HourlyFailedJobs())
            {
                data.AppendLine("hangfire_hourly_failed_jobs_count" + "{key=" + "\"" + item.Key + "\"} " + item.Value);
            }

            data.AppendLine("# Help Hourly Succeeded Jobs Count");
            foreach (var item in api.HourlySucceededJobs())
            {
                data.AppendLine("hangfire_hourly_succeeded_jobs_count" + "{key=" + "\"" + item.Key + "\"} " + item.Value);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            return data.ToString();
        }
    }
}
