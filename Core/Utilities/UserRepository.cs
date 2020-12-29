using System;
using Live2k.Core.Model.Base;
using MongoDB.Driver;

namespace Live2k.Core.Utilities
{
    public class UserRepository
    {
        private readonly IMongoDatabase _database;

        public UserRepository(IMongoDatabase database)
        {
            this._database = database;
        }

        public IMongoCollection<User> Collection => this._database.GetCollection<User>("Users"); 

        public User GetUser(string email)
        {
            var filter = Builders<User>.Filter.Eq(a => a.Id, email);
            return Collection.Find(filter).FirstOrDefault();
        }

        public void AddUser(User user)
        {
            Collection.InsertOne(user);
        }

        internal void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
