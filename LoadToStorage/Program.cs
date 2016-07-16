using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KafkaNet;
using KafkaNet.Common;
using KafkaNet.Protocol;
using KafkaNet.Model;

namespace LoadToStorage
{
    class Program
    {

        static bool upload(Message message)
        {
            string message_contents = System.Text.Encoding.Default.GetString(message.Value);
            try
            {
                var split_contents = message_contents.Split(',');
                var company = new Company_Staging() { Id = Convert.ToInt32(split_contents[0]), Name = split_contents[1], Owner = split_contents[2]};
                using(var db = new dbDataContext())
                {
                    Console.WriteLine(company.Name);
                    db.Company_Stagings.InsertOnSubmit(company);
                    db.SubmitChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        static long latest_position()
        {
            using(var db_conn = new dbDataContext())
            {
                try
                {
                    long max_pos = (from row in db_conn.Loads
                                   select row.offset_pos).Max();
                    return max_pos;
                }
                catch
                {
                    return 0;
                }

            }
        }

        static void Main(string[] args)
        {
            var options = new KafkaOptions(new Uri("http://localhost:9092"), new Uri("http://localhost:9092"));
            var router = new BrokerRouter(options);
            var consumer = new Consumer(new ConsumerOptions("TestHarness", router));
            
            long offset_id = latest_position();
            consumer.SetOffsetPosition(new OffsetPosition(1, latest_position()));

            foreach (var message in consumer.Consume())
            {
                upload(message);
            }

            using (var db = new dbDataContext())
            {
                var load = new Load() { Id = 1, offset_pos = consumer.GetOffsetPosition()[0].Offset };
                db.Loads.InsertOnSubmit(load);
                db.SubmitChanges();
            }
           
        }
    }
}
