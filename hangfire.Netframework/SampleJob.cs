﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace hangfire.Netframework
{
    public class SampleJob
    {
        private static int callcount;
        public static async Task Execute()
        {
            callcount = 0;
            Console.WriteLine("start");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var tasks = new List<Task>();
            for (int i = 0; i < 100; i++)
            {
                tasks.Add(new Task(() => writefile()));
            }

            tasks.ForEach(t => t.Start());

            await Task.WhenAll(tasks.ToArray());
            sw.Stop();
            Console.WriteLine($"all tasks done {callcount} api call in {sw.Elapsed}");
        }

        private static void writefile()
        {
            try
            {
                var index = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();
                //var index = DateTime.Now.ToString();
                Console.WriteLine("start:" + index);
                Random rand = new Random(DateTime.Now.Second);


                for (int i = 0; i < 10; i++)
                {
                    var result = Get("https://amazon.com");
                    callcount++;
                    Console.WriteLine("call " + index + "," + i.ToString() + ": " + result);
                }

                Console.WriteLine("api call ended for index:" + index);

            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
            }
        }

        private static string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return response.StatusDescription;
            }
        }
    }
}
