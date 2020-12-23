using System;
using System.IO;
using System.Linq;
using Live2k.Core.Model.Base;
using Live2k.Core.Model.Basic;
using Live2k.Core.Model.Basic.Commodities;
using Live2k.Core.Model.Basic.Relationships;
using Live2k.Core.Serializer;
using Live2k.Core.Utilities;
using Live2k.MongoDb;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace PlayGround
{
    class Program
    {
        private static MongoClient _client;

        static void Main(string[] args)
        {
            

            //BsonSerializer.RegisterSerializationProvider(new Live2kSerializationProvider());
            BsonSerializer.RegisterDiscriminatorConvention(typeof(BaseProperty), new BasePropertyDiscriminator());
            //BsonClassMap.RegisterClassMap<Entity>();
            //BsonClassMap.RegisterClassMap<Node>();
            //BsonClassMap.RegisterClassMap<Relationship>();
            //BsonClassMap.RegisterClassMap<Commodity>(cm =>
            //{
            //    cm.AutoMap();
            //    cm.SetIsRootClass(true);
            //    cm.AddKnownType(typeof(RevisableCommodity));
            //    cm.AddKnownType(typeof(RevisionCommodity));
            //    cm.AddKnownType(typeof(ControlObject));
            //});
            //BsonClassMap.RegisterClassMap<RevisableCommodity>(cm =>
            //{
            //    cm.AutoMap();
            //    cm.AddKnownType(typeof(SDI));
            //});
            //BsonClassMap.RegisterClassMap<RevisionCommodity>(cm =>
            //{
            //    cm.AutoMap();
            //    cm.AddKnownType(typeof(SdiRevision));
            //});
            //BsonClassMap.RegisterClassMap<SDI>();
            //BsonClassMap.RegisterClassMap<SdiRevision>();

            //BsonClassMap.RegisterClassMap<BaseProperty>(cm =>
            //{
            //    cm.AutoMap();
            //    cm.SetIsRootClass(true);
            //});
            //BsonClassMap.RegisterClassMap<Property<string>>();
            //BsonClassMap.RegisterClassMap<Property<DateTime>>();
            //BsonClassMap.RegisterClassMap<Property<int>>();
            //BsonClassMap.RegisterClassMap<Property<double>>();


            // connection to MongoDb
            _client = new MongoClient("mongodb+srv://m001-student:m001-mongodb-basics@sandbox.aidnp.mongodb.net/temp?retryWrites=true&w=majority");

            // Mediator
            var user = new User("faramarz.bodaghi@outlook.com");
            user.FirstName = "Faramarz";
            user.LastName = "Bodaghi";
            user.Birthday = new DateTime(1986, 9, 19);

            var mediator = new Mediator(user, new DocumentCounterReposity(_client.GetDatabase("Live2K")));
            var factory = new Factory(mediator);

            TestRepository(mediator);
            AddNewObjects(mediator, factory);
            GetFirstSDIAndChange(mediator, factory);

            //// TEST

            var db = _client.GetDatabase("Live2K");
            //var collection = db.GetCollection<Node>("Nodes");
            //var method = db.GetType().GetMethod("GetCollection").MakeGenericMethod(Type.GetType("PlayGround.SDI"));
            //var collection = method.Invoke(db, new[] { "Nodes", null });
            //var found = collection.Find(a => a.Label == "SDI");
            //var temp = found.CountDocuments();
            //var founditem = collection.Find(a => a.Label == "SDI")
            //    .ToList().FirstOrDefault();

            //var castMethod = founditem.GetType().GetMethod("Cast").MakeGenericMethod(Type.GetType(founditem.ActualType));
            //SDI asSDI = castMethod.Invoke(founditem, new object[0]) as SDI;
            //asSDI.Section = "44542";
            //asSDI.Label = "ChangedSDI";

            // Get sdi
            //var tempRepos = new Repository(mediator, _client);
            //var foundSdi = tempRepos.Get<SDI>(a => a.Label == "SDI-1");
            //foundSdi.Description = "Aram is observing";
            //foundSdi.Description = "Aram is not observing";
            //foundSdi.Revisions.FirstOrDefault().Description = "We are adding a desc here";
            //foundSdi.DataCode = "Changed";
            //var revision2 = foundSdi.Revise(mediator).Cast<SdiRevision>();
            //revision2.NumberOfDocs = 20;
            //revision2.Tags.Add("new rev");
            //tempRepos.Update(foundSdi);

            //var number = founditem.CountDocuments();

            //// TEST


            // getting all sdis
            //var repository = new Repository(_client);
            //var result = repository.GetAll<SDI>();

            //var first = result.FirstOrDefault();
            //var firstSdi = first as SDI;


            //Test1();




            // make an SDI
            //var sdi = new SDI(mediator);
            ////sdi.Id = Guid.NewGuid().ToString();
            //sdi.DataCode = "SDI-222-11-01";
            //sdi.Section = "2222";

            //var xx = sdi.Revisions;

            // revise SDI
            //var revision = sdi.Revise(new Factory(mediator)).Cast<SdiRevision>();
            //revision.Id = Guid.NewGuid().ToString();
            //revision.NumberOfDocs = 20;
            //revision.Tags.Add("dddd");
            //var y = sdi["Revisions"];
            //var x = sdi.Revisions;

            // Control object
            //var co = new ControlObject(mediator);
            //co.Id = Guid.NewGuid().ToString();
            //co.AvevaId = "=1234/4345";
            //co.Section = "222";
            //co.Area = "BC110";
            //co.ControlObjectCode = "222-11-0001";
            //co.Status = "A2";

            //// Relationship to SDI
            //var rel = new ReferenceRelationship();
            //rel.Id = Guid.NewGuid().ToString();
            //rel.SetNodes(co, sdi);

            //JsonSerializer serializer = new JsonSerializer();
            //serializer.TypeNameHandling = TypeNameHandling.Auto;
            //using (var writer = new StreamWriter("TestSDI.json"))
            //using (var coWriter = new StreamWriter("TestCo.json"))
            //using (var relWriter = new StreamWriter("TestRel.json"))
            //{
            //    serializer.Serialize(writer, sdi);
            //    serializer.Serialize(coWriter, co);
            //    serializer.Serialize(relWriter, rel);
            //}

            //var repos = new Repository(mediator, _client);
            //repos.Add(sdi);
            //repos.Add(co);
            //repos.AddEntity(rel);
        }

        private static void TestRepository(Mediator mediator)
        {
            var repos = new Repository(mediator, _client);
            var x = repos.Get(a => a.Label == "SDI-8");
        }

        private static void GetFirstSDIAndChange(Mediator mediator, Factory factory)
        {
            var repos = new Repository(mediator, _client);
            var sdi = repos.Get<SDI>(a => a.Label == "SDI-8");
            sdi.AddTag("Tag1", "Tag2", "Tag3");
            sdi.Revise(factory);
            sdi.RemoveTag("Tag1", "Tag2");
            sdi.AddTag("Hello");
            repos.Update(sdi);
        }

        private static void AddNewObjects(Mediator mediator, Factory factory)
        {
            // new SDI
            var sdi = factory.CreateNew<SDI>(null, "First sdi", new Tuple<string, object>("DataCode", "SDI-222-20-01"),
                                                                new Tuple<string, object>("Section", "222"));
            
            // add properties to active revision
            sdi.ActiveRevision.NumberOfDocs = 20;
            sdi.ActiveRevision.Description = "New Sdi revision";

            var repos = new Repository(mediator, _client);
            repos.Add(sdi);
        }

        //static void Test1()
        //{
        //    Deserialize();

        //    var user = new User();
        //    user.FirstName = "Faramarz";
        //    user.LastName = "Bodaghi";

        //    var phone = new Phone();
        //    phone.PhoneNumber = "98100918";
        //    phone.Tags.Add("Mobile");
        //    phone.Description = "Mobile";

        //    user.MobilePhone = phone;

        //    user.Birthday = new DateTime(1986, 9, 19);


        //    var address = new Address();
        //    address.City = "Høvik";
        //    address.Label = "Home address";
        //    address.Langtitude = 122442;
        //    address.Latitude = 33234;
        //    address.PostalCode = "1365";
        //    address.Provience = "Viken";
        //    address.Street = "Kokkerudåsen 21";
        //    address.Description = "Temporart home address";

        //    user.AddToListProperty("Addresses", "Home address", address);
        //    var home = user.HomeAddress;


        //    JsonSerializer serializer = new JsonSerializer();
        //    serializer.TypeNameHandling = TypeNameHandling.Auto;
        //    using (var writer = new StreamWriter("TestJson.json"))
        //    {
        //        serializer.Serialize(writer, user, typeof(User));
        //    }

        //    var repos = new Repository(_client);
        //    repos.Add(user);
        //}

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
