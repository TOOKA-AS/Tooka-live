using System;
using System.IO;
using Live2k.Core.Basic;
using Live2k.Core.Basic.Commodities;
using Newtonsoft.Json;

namespace PlayGround
{
    class Program
    {
        static void Main(string[] args)
        {
            var user = new User("Faramarz", null, "Bodaghi");

            var y = user.PhoneNumbers;
            user.AddToListProperty("Phone numbers", "Mobile", new Phone("98100918", "Mobile"));

            var x = user.Birthday;


            user.AddToListProperty("Addresses", "Home address", new Address("Viken", "Høvik", "Kokkerudåsen", "1363"));
            var home = user.HomeAddress;

            user.Birthday = DateTime.Now;

            JsonSerializer serializer = new JsonSerializer();
            using (var writer = new StreamWriter("TestJson.json"))
            {
                serializer.Serialize(writer, (Commodity)user, typeof(Commodity));
            }
        }
    }
}
