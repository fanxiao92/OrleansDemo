using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;

namespace OrleansDemo
{
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                await using var client = await ConnectClient();
                var response = await DoClientWork(client);
                Console.WriteLine($"\n\n{response}\n\n");
                Console.ReadLine();
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        static async Task<IClusterClient> ConnectClient()
        {
            IClusterClient client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await client.Connect();
            Console.WriteLine("Client successfully connected to silo host \n");
            return client;
        }

        static Task<string> DoClientWork(IClusterClient client)
        {
            var friend = client.GetGrain<IHello>(0);
            return friend.SayHello("Good morning, HelloGrani!");
        }
    }
}