using System;
using System.Threading.Tasks;
using Common;
using GrainInterfaces;
using GrainInterfaces.Model;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Utils;

namespace OrleansDemo
{
    class Program
    {
        private static string joinedChannel = "general";
        private static string userName = "UserWithNoName";

        public static async Task<int> Main(string[] args)
        {
            try
            {
                await using var client = await ConnectClient();
                PrettyConsole.Line("==== CLIENT: Initialized ====", ConsoleColor.Cyan);
                PrettyConsole.Line("CLIENT: Write commands:", ConsoleColor.Cyan);
                Console.ReadLine();
                await Menu(client);
                PrettyConsole.Line("==== CLIENT: Shutting down ====", ConsoleColor.DarkRed);
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
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = Constants.ClusterId;
                    options.ServiceId = Constants.ServiceId;
                })
                .UseLocalhostClustering()
                .ConfigureApplicationParts(
                    parts => parts.AddApplicationPart(typeof(IChannel).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole())
                .AddSimpleMessageStreamProvider(Constants.ChatRoomStreamProvider)
                .Build();

            await client.Connect();
            Console.WriteLine("Client successfully connected to silo host \n");
            return client;
        }

        private static void PrintHints()
        {
            var menuColor = ConsoleColor.Magenta;
            PrettyConsole.Line("Type '/j <channel>' to join specific channel", menuColor);
            PrettyConsole.Line("Type '/n <username>' to set your user name", menuColor);
            PrettyConsole.Line("Type '/l' to leave specific channel", menuColor);
            PrettyConsole.Line("Type '<any text>' to send a message", menuColor);
            PrettyConsole.Line("Type '/h' to re-read channel history", menuColor);
            PrettyConsole.Line("Type '/m' to query members in the channel", menuColor);
            PrettyConsole.Line("Type '/exit' to exit client.", menuColor);
        }

        private static async Task Menu(IClusterClient client)
        {
            string input;
            PrintHints();

            do
            {
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) continue;

                if (input.StartsWith("/j"))
                {
                    await JoinChannel(client, input.Replace("/j", "").Trim());
                }
                else if (input.StartsWith("/n"))
                {
                    userName = input.Replace("/n", "").Trim();
                    PrettyConsole.Line($"Your user name is set to be {userName}", ConsoleColor.DarkGreen);
                }
                else if (input.StartsWith("/l"))
                {
                    await LeaveChannel(client);
                }
                else if (input.StartsWith("/h"))
                {
                    await ShowCurrentChannelHistory(client);
                }
                else if (input.StartsWith("/m"))
                {
                    await ShowChannelMembers(client);
                }
                else if (!input.StartsWith("/exit"))
                {
                    await SendMessage(client, input);
                }
            } while (input != "/exit");
        }

        private static async Task ShowChannelMembers(IClusterClient client)
        {
            var room = client.GetGrain<IChannel>(joinedChannel);
            var members = await room.GetMembers();

            PrettyConsole.Line($"====== Members for '{joinedChannel}' Channel ======", ConsoleColor.DarkGreen);
            foreach (var member in members)
            {
                PrettyConsole.Line(member, ConsoleColor.DarkGreen);
            }

            PrettyConsole.Line("============", ConsoleColor.DarkGreen);
        }

        private static async Task ShowCurrentChannelHistory(IClusterClient client)
        {
            var room = client.GetGrain<IChannel>(joinedChannel);
            var history = await room.ReadHistory(1000);

            PrettyConsole.Line($"====== History for '{joinedChannel}' Channel ======", ConsoleColor.DarkGreen);
            foreach (var chatMsg in history)
            {
                PrettyConsole.Line($" ({chatMsg.Created:g}) {chatMsg.Author}> {chatMsg.Text}",
                    ConsoleColor.DarkGreen);
            }

            PrettyConsole.Line("============", ConsoleColor.DarkGreen);
        }

        private static async Task SendMessage(IClusterClient client, string messageText)
        {
            var room = client.GetGrain<IChannel>(joinedChannel);
            await room.Message(new ChatMsg(userName, messageText));
        }

        private static async Task JoinChannel(IClusterClient client, string channelName)
        {
            if (joinedChannel == channelName)
            {
                PrettyConsole.Line(
                    $"You already joined channel {channelName}. Double joining a channel, which is implemented as a stream, would result in double subscription to the same stream, " +
                    $"which would result in receiving duplicated messages. For more information, please refer to Orleans streaming documentation.");
                return;
            }

            PrettyConsole.Line($"Joining to channel {channelName}");
            joinedChannel = channelName;
            var room = client.GetGrain<IChannel>(joinedChannel);
            var streamId = await room.Join(userName);
            var stream = client.GetStreamProvider(Constants.ChatRoomStreamProvider)
                .GetStream<ChatMsg>(streamId, Constants.CharRoomStreamNameSpace);
            //subscribe to the stream to receiver furthur messages sent to the chatroom
            await stream.SubscribeAsync(new StreamObserver(client.ServiceProvider.GetService<ILoggerFactory>()
                .CreateLogger($"{joinedChannel} channel")));
        }

        private static async Task LeaveChannel(IClusterClient client)
        {
            PrettyConsole.Line($"Leaving channel {joinedChannel}");
            var room = client.GetGrain<IChannel>(joinedChannel);
            var streamId = await room.Leave(userName);
            var stream = client.GetStreamProvider(Constants.ChatRoomStreamProvider)
                .GetStream<ChatMsg>(streamId, Constants.CharRoomStreamNameSpace);

            //unsubscribe from the channel/stream since client left, so that client won't
            //receive furture messages from this channel/stream
            var subscriptionHandles = await stream.GetAllSubscriptionHandles();
            foreach (var handle in subscriptionHandles)
            {
                await handle.UnsubscribeAsync();
            }
        }
    }
}