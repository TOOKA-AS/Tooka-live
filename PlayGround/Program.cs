﻿using System;
using System.IO;
using System.Linq;
using Live2k.Core.Base;
using Live2k.Core.Basic;
using Live2k.Core.Basic.Commodities;
using Live2k.Core.Basic.Relationships;
using Live2k.Core.Serializer;
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


            //// TEST

            var db = _client.GetDatabase("Live2K");
            var collection = db.GetCollection<Node>("Nodes");
            //var method = db.GetType().GetMethod("GetCollection").MakeGenericMethod(Type.GetType("PlayGround.SDI"));
            //var collection = method.Invoke(db, new[] { "Nodes", null });
            var found = collection.Find(a => a.Label == "SDI");
            var temp = found.CountDocuments();
            var founditem = collection.Find(a => a.Label == "SDI")
                .ToList().FirstOrDefault();

            var castMethod = founditem.GetType().GetMethod("Cast").MakeGenericMethod(Type.GetType(founditem.ActualType));
            SDI asSDI = castMethod.Invoke(founditem, new object[0]) as SDI;

            //var number = founditem.CountDocuments();

            //// TEST


            // getting all sdis
            //var repository = new Repository(_client);
            //var result = repository.GetAll<SDI>();

            //var first = result.FirstOrDefault();
            //var firstSdi = first as SDI;


            //Test1();

            // make an SDI
            var sdi = new SDI();
            //sdi.Id = Guid.NewGuid().ToString();
            sdi.DataCode = "SDI-222-11-01";
            sdi.Section = "222";

            var xx = sdi.Revisions;

            // revise SDI
            var revision = sdi.Revise().Cast<SdiRevision>();
            //revision.Id = Guid.NewGuid().ToString();
            revision.NumberOfDocs = 20;
            revision.Tags.Add("ssss");
            var y = sdi["Revisions"];
            var x = sdi.Revisions;

            // Control object
            var co = new ControlObject();
            //co.Id = Guid.NewGuid().ToString();
            co.AvevaId = "=1234/4345";
            co.Section = "222";
            co.Area = "BC110";
            co.ControlObjectCode = "222-11-0001";
            co.Status = "A2";

            // Relationship to SDI
            var rel = new ReferenceRelationship();
            //rel.Id = Guid.NewGuid().ToString();
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

            var repos = new Repository(_client);
            repos.Add(sdi);
            repos.Add(co);
            repos.Add(rel);
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

            var repos = new Repository(_client);
            repos.Add(user);
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
