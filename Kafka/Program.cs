using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KafkaNet;
using KafkaNet.Common;
using KafkaNet.Protocol;
using KafkaNet.Model;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new KafkaOptions(new Uri("http://localhost:9092"), new Uri("http://localhost:9092"));
            var router = new BrokerRouter(options);
            var client = new Producer(router);

            client.SendMessageAsync("TestHarness", new[] { new Message("Hello", "World") }).Wait();

            System.Threading.Thread.Sleep(2000);

            var consumer = new Consumer(new ConsumerOptions("TestHarness", router));

            //Consume returns a blocking IEnumerable (ie: never ending stream)
            foreach (var message in consumer.Consume())
            {
                Console.WriteLine("Response: P{0},O{1} : {2}",
                    message.Meta.PartitionId, message.Meta.Offset, System.Text.Encoding.Default.GetString(message.Value));
            }
        }
    }
}
