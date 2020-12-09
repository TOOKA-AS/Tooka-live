using System;
using System.IO;
using Live2k.Core.Basic;
using Live2k.Core.Basic.Commodities;
using Live2k.Core.Basic.Relationships;
using Newtonsoft.Json;

namespace PlayGround
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test1();

            // make an SDI
            var sdi = new SDI();
            sdi.Id = Guid.NewGuid().ToString();
            sdi.DataCode = "SDI-111-11-01";
            sdi.Section = "111";

            var xx = sdi.Revisions;

            // revise SDI
            var revision = sdi.Revise<SdiRevision>();
            revision.Id = Guid.NewGuid().ToString();
            revision.NumberOfDocs = 20;
            revision.Tags.Add("ssss");
            var y = sdi["Revisions"];
            var x = sdi.Revisions;

            // Control object
            var co = new ControlObject();
            co.Id = Guid.NewGuid().ToString();
            co.AvevaId = "=1234/232";
            co.Section = "333";
            co.Area = "BC110";
            co.ControlObjectCode = "111-11-0001";
            co.Status = "A2";

            // Relationship to SDI
            var rel = new ReferenceRelationship();
            rel.Id = Guid.NewGuid().ToString();
            rel.SetNodes(co, sdi);

            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            using (var writer = new StreamWriter("TestSDI.json"))
            using (var coWriter = new StreamWriter("TestCo.json"))
            using (var relWriter = new StreamWriter("TestRel.json"))
            {
                serializer.Serialize(writer, sdi);
                serializer.Serialize(coWriter, co);
                serializer.Serialize(relWriter, rel);
            }
        }

        static void Test1()
        {
            Deserialize();

            var user = new User();
            user.FirstName = "Faramarz";
            user.LastName = "Bodaghi";

            var phone = new Phone();
            phone.PhoneNumber = "98100918";
            phone.Tags.Add("Mobile");
            phone.Description = "Mobile";

            user.MobilePhone = phone;

            user.Birthday = new DateTime(1986, 9, 19);


            var address = new Address();
            address.City = "Høvik";
            address.Label = "Home address";
            address.Langtitude = 122442;
            address.Latitude = 33234;
            address.PostalCode = "1365";
            address.Provience = "Viken";
            address.Street = "Kokkerudåsen 21";
            address.Description = "Temporart home address";

            user.AddToListProperty("Addresses", "Home address", address);
            var home = user.HomeAddress;


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
