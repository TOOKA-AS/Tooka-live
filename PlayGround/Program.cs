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
            var co = new ControlObject("=1234/1234", "111", "BC110", "111-10-0001", "A2");

            // Make and owning relation
            var rel = new ParentRelationship(GetUser(), co);

        }

        static void Test1()
        {
            Deserialize();

            var user = new User("Faramarz", null, "Bodaghi");

            var y = user.PhoneNumbers;
            user.AddToListProperty("Phone numbers", "Mobile", new Phone("98100918", "Mobile"));

            var x = user.Birthday;


            user.AddToListProperty("Addresses", "Home address", new Address("", "Viken", "Høvik", "Kokkerudåsen", "1363"));
            var home = user.HomeAddress;

            user.Birthday = DateTime.Now;

            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            using (var writer = new StreamWriter("TestJson.json"))
            {
                serializer.Serialize(writer, user, typeof(User));
            }
        }

        static void Deserialize()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            serializer.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
            using (var reader = new StreamReader("TestJson.json"))
            {
                var x = serializer.Deserialize(reader, typeof(User));
            }
        }

        static User GetUser()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            serializer.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
            User user;
            using (var reader = new StreamReader("TestJson.json"))
            {
                user = serializer.Deserialize(reader, typeof(User)) as User;
            }

            return user;
        }
    }
}
