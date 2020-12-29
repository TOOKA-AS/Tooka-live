using System;
using Live2k.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Live2k.Core.Utilities
{
    public class DocumentCounterReposity
    {
        private readonly IMongoDatabase db;

        public DocumentCounterReposity(IMongoDatabase db)
        {
            this.db = db;
            NodeRepository.NewNodeAdded += Repository_NewEntityAdded;
        }

        private void Repository_NewEntityAdded(object sender, Model.Base.Entity e)
        {
            Increment(e.GetType());
        }

        private IMongoCollection<BsonDocument> GetCollection()
        {
            return db.GetCollection<BsonDocument>("Counter");
        }

        public int Count(Type type)
        {
            return CountIncrement(type, false);
        }

        private int CountIncrement(Type type, bool increment)
        {
            // Get counter collection
            var collection = GetCollection();
            var filter = Builders<BsonDocument>.Filter.Eq("Type", type.FullName);
            var counter = collection.Find(filter).FirstOrDefault() ?? GenerateDocument(type);
            var count = counter.GetElement("Count");
            var countValue = count.Value.AsInt32;

            if (!increment)
            {
                return countValue;
            }

            var update = Builders<BsonDocument>.Update.Set("Count", ++countValue);
            if (collection.UpdateOne(filter, update).ModifiedCount == 0)
            {
                var newCount = new BsonElement("Count", countValue);
                counter.SetElement(newCount);
                collection.InsertOne(counter);
            }
            
            return countValue++;
        }

        private int Increment(Type type)
        {
            return CountIncrement(type, true);
        }

        private BsonDocument GenerateDocument(Type type)
        {
            var doc = new BsonDocument();
            doc.Add(new BsonElement("Type", type.FullName));
            doc.Add(new BsonElement("Count", 0));
            return doc;
        }
    }
}
