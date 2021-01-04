using System;
using System.Collections.Generic;
using Live2k.Core.Model;
using MongoDB.Driver;

namespace Live2k.Core.Utilities
{
    public class AttachmentRepository
    {
        private readonly IMongoDatabase _database;

        public AttachmentRepository(IMongoDatabase database)
        {
            this._database = database;
        }

        public IMongoCollection<Attachment> Collection => this._database.GetCollection<Attachment>("Attachments");

        public Attachment Get(string id)
        {
            var filter = Builders<Attachment>.Filter.Eq(a => a.Id, id);
            return Collection.Find(filter).FirstOrDefault();
        }

        public void Add(Attachment attachment)
        {
            Collection.InsertOne(attachment);
        }

        public void Add(IEnumerable<Attachment> attachments)
        {
            Collection.InsertMany(attachments);
        }
    }
}
