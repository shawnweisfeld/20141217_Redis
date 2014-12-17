using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRedis
{
    class Program
    {
        static ConnectionMultiplexer _connection;

        static void Main(string[] args)
        {
            ////Create a connection
            ////Do this only once for the whole app
            ////creating it is expensive, but it is designed to be shared
            _connection = ConnectionMultiplexer.Connect("localhost");

            ////get a database instance, this is cheap
            ////get a new one of these every time you need to do something
            IDatabase db = _connection.GetDatabase();


            //lets play with cache expiration
            Console.WriteLine("Writing a value to the cache with a 5 second expy.");
            db.StringSet("MyName", "Shawn", TimeSpan.FromSeconds(5));

            Console.WriteLine("Reading the value back immediately. MyName is {0}", db.StringGet("MyName"));

            Console.WriteLine("Sleeping for 7 seconds.");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));

            Console.WriteLine("MyName is {0}", db.StringGet("MyName"));


            //lets play with a hash 
            db.HashSet("Me", "FirstName", "Shawn");
            db.HashSet("Me", "LastName", "Weisfeld");
            db.HashSet("Me", "Home", "Austin, TX");

            foreach (var item in db.HashGetAll("Me"))
            {
                Console.WriteLine("Name: {0} Value: {1}", item.Name, item.Value);
            }
            


            //lets play with Pub/Sub
            var reciever = _connection.GetSubscriber();
            var sender = _connection.GetSubscriber();

            reciever.Subscribe("MyMessageChannel", (channel, payload) => 
            {
                Console.WriteLine("Value recived! {0}", payload);
            });

            
            for (int i = 0; i < 10; i++)
            {
                sender.Publish("MyMessageChannel", string.Format("Message {0}", i));   
            }

            Console.WriteLine("Done!");
            Console.ReadKey();




            reciever.UnsubscribeAll();
            sender.UnsubscribeAll();
        }
    }
}
