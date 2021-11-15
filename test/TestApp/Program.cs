using System;
using System.Threading.Tasks;
using ProtoBuf.Grpc.Client;
using Service.CoinMarketCapReader.Client;
using Service.CoinMarketCapReader.Grpc.Models;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Task.CompletedTask;
            GrpcClientFactory.AllowUnencryptedHttp2 = true;

            Console.Write("Press enter to start");
            Console.ReadLine();


            // var factory = new CoinMarketCapReaderClientFactory("http://localhost:5001");
            // var client = factory.GetHelloService();
            //
            // var resp = await  client.SayHelloAsync(new ApiKeyRequest(){Name = "Alex"});
            // Console.WriteLine(resp?.Message);

            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
