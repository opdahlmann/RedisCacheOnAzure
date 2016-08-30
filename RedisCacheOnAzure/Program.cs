using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedisCacheOnAzure
{
    class Program
    {
        private static IDatabase db;
        private static ConnectionMultiplexer conn;
        private static IServer server;
        static void Main(string[] args)
        {
            conn = ConnectionMultiplexer.Connect("rd123.redis.cache.windows.net:6380,password=ag/8vnGG0C9QwUuZp61a8xrIvORaH9CsIjisqIPBx3I=,ssl=True,abortConnect=False");
            db = conn.GetDatabase();
            server = conn.GetServer("rd123.redis.cache.windows.net:6380");

            Timer t = new Timer(DisplayTimeEvent, null, 0, 5000);
            db.StringSet(DateTime.Now.ToFileTimeUtc().ToString(), "Some value", new TimeSpan(0, 2, 0));
            Console.ReadLine();

        }


        private static void DisplayTimeEvent(Object o)
        {
            Console.WriteLine("currently connected:" + server.Keys().Count());
            foreach (var item in server.Keys())
            {
                Console.WriteLine("key:" + item.ToString() + 
                 " - Value: " + db.StringGetWithExpiry(item).Value.ToString() + 
                 " - " + "Expires in: " + db.StringGetWithExpiry(item).Expiry.Value.Duration().ToString(@"dd\.hh\:mm\:ss"));
            }
            Console.WriteLine("");
        }
    }
}
