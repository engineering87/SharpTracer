// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace SharpTracer.ServiceTest
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var guidTask1 = Guid.NewGuid().ToString();
            var guidTask2 = Guid.NewGuid().ToString();

            var task1 = Task.Factory.StartNew(() => SendTraceRequest(guidTask1));
            var task2 = Task.Factory.StartNew(() => SendTraceRequest(guidTask2));
            var task3 = Task.Factory.StartNew(() => SendHistoryRequest(guidTask1));

            Console.ReadKey();
        }

        private static async void SendTraceRequest(string taskId)
        {
            try
            {
                using var channel = GrpcChannel.ForAddress("https://localhost:5001");
                var client = new Tracer.TracerClient(channel);

                while (true)
                {
                    var request = new TracerRequest()
                    {
                        ServiceSourceId = taskId,
                        ServiceDestinationId = Guid.NewGuid().ToString(),
                        Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
                        Payload = "test"
                    };

                    Console.WriteLine(
                        $"Sending tracer request from {request.ServiceSourceId} to {request.ServiceDestinationId}");

                    var reply = await client.TraceAsync(request);

                    await Task.Delay(new Random().Next(500, 2000));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static async void SendHistoryRequest(string sorceId)
        {
            try
            {
                using var channel = GrpcChannel.ForAddress("https://localhost:5001");
                var client = new Tracer.TracerClient(channel);

                while (true)
                {
                    var request = new HistoryRequest()
                    {
                        SourceId = sorceId
                    };

                    Console.WriteLine(
                        $"Sending history request for {request.SourceId}");

                    var reply = await client.HistoryAsync(request);

                    foreach (var item in reply.Requests)
                    {
                        Console.WriteLine($"{item.ServiceSourceId} -> {item.ServiceDestinationId}");
                    }

                    await Task.Delay(new Random().Next(15000, 20000));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
