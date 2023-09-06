// See https://aka.ms/new-console-template for more information

using StackExchange.Redis;

ConnectionMultiplexer connection = await ConnectionMultiplexer.ConnectAsync("localhost:1453");
ISubscriber subscriber = connection.GetSubscriber();

#region Pattern Matching
//await subscriber.SubscribeAsync("stock.*", (channel, message) =>
//{
//Console.WriteLine("message");
//});

#endregion
subscriber.SubscribeAsync("mychannel", (channel, message) =>
{
    Console.WriteLine(message);
});
Console.Read();