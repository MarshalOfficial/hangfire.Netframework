using Hangfire;
using Hangfire.MemoryStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hangfire.Netframework
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GlobalConfiguration.Configuration.UseMemoryStorage();
            RecurringJob.AddOrUpdate("test", () => SampleJob.Execute(), Cron.Hourly);
            RecurringJob.Trigger("test");

            using (new BackgroundJobServer())
            {
                Console.ReadLine();
            }

            






            //keep the app open to scheduler do his jobs
            await Task.Delay(1000000000);
            await Task.Delay(1000000000);
            await Task.Delay(1000000000);
            await Task.Delay(1000000000);
        }
    }
}
