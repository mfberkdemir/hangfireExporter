using Hangfire;
using Hangfire.Mongo;
using Hangfire.Storage;
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
        IStorageConnection api2;
        StringBuilder data;

        public metricsController()
        {
            GlobalConfiguration.Configuration.UseMongoStorage("mongodb://192.168.1.151:27017", "hang", new MongoStorageOptions
            {
                Prefix = "custom",
                MigrationOptions = new MongoMigrationOptions
                {
                    Strategy = MongoMigrationStrategy.Migrate,
                    BackupStrategy = MongoBackupStrategy.Collections
                }
            });

            api = JobStorage.Current.GetMonitoringApi();
            api2 = JobStorage.Current.GetConnection();
            data = new StringBuilder();
        }

        public string Get()
        {
            string argumentsHolder = Environment.GetEnvironmentVariable("dbprovider");

            data.AppendLine("# Help Servers Count ");
            data.AppendLine("hangfire_servers_count " + api.Servers().Count);
            data.AppendLine("# Help Succeeded Jobs Count");
            data.AppendLine("hangfire_succeeded_jobs_total_count " + api.SucceededListCount().ToString());
            data.AppendLine("# Help Deleted Jobs Count");
            data.AppendLine("hangfire_deleted_jobs_total_count " + api.DeletedListCount().ToString());
            data.AppendLine("# Help Failed Jobs Count");
            data.AppendLine("hangfire_failed_jobs_total_count " + api.FailedCount().ToString());
            data.AppendLine("# Help Processing Jobs Count");
            data.AppendLine("hangfire_processing_jobs_total_count " + api.ProcessingCount().ToString());
            data.AppendLine("# Help Scheduled Jobs Count");
            data.AppendLine("hangfire_scheduled_jobs_total_count " + api.ScheduledCount().ToString());
            data.AppendLine("# Help Succeeded Jobs List Count");
            data.AppendLine("hangfire_succeeded_jobs_total_count " + api.SucceededListCount().ToString());

            var recurCount = api2.GetRecurringJobs().Count;
            data.AppendLine("# Help Recurring Jobs Count");
            data.AppendLine("hangfire_recurring_jobs_count " + recurCount.ToString());

            var qdata = api.Queues();
            for (int i = 0; i < qdata.Count; i++)
            {
                data.AppendLine("# Help Queues Name");
                data.AppendLine("hangfire_" + i.ToString() + "_queues_name " + qdata[i].Name);
                data.AppendLine("# Help Queues Length ");
                data.AppendLine("hangfire_" + i.ToString() + "_queues_length " + qdata[i].Length.ToString());
                data.AppendLine("# Help Queues Fetched ");
                data.AppendLine("hangfire_" + i.ToString() + "_queues_fetched " + qdata[i].Fetched.ToString());
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            return data.ToString();


        }
    }
}
