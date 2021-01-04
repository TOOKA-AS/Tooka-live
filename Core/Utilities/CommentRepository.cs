using System;
using System.Collections.Generic;
using Live2k.Core.Model;
using MongoDB.Driver;

namespace Live2k.Core.Utilities
{
    public class CommentRepository
    {
        private readonly IMongoDatabase database;

        public CommentRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public IMongoCollection<Comment> Collection => this.database.GetCollection<Comment>("Comments");

        public Comment Get(string id)
        {
            var filter = Builders<Comment>.Filter.Eq(a => a.Id, id);
            return Collection.Find(filter).FirstOrDefault();
        }

        public void Add(Comment comment)
        {
            Collection.InsertOne(comment);
        }

        public void Add(IEnumerable<Comment> comments)
        {
            Collection.InsertMany(comments);
        }
    }
}
