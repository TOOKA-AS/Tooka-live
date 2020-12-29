using System;
using System.Linq;
using Live2k.Core.Model.Base;
using Live2k.Core.Serializer;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Live2k.Core.Utilities
{
    public sealed class Mediator
    {
        private readonly IMongoDatabase _database;

        private Mediator()
        {
            this._database = InitializeDatabase();
            InstanciateRepositories();
            Factory = new Factory(this);
        }

        public Mediator(string userid) : this()
        {
            if (string.IsNullOrWhiteSpace(userid))
            {
                throw new ArgumentException($"'{nameof(userid)}' cannot be null or whitespace", nameof(userid));
            }

            SessionUser = GetSessionUser(userid) ??
                throw new InvalidOperationException("User could not be found");
        }

        public Mediator(User user) : this()
        {
            SessionUser = user;
            user.Save();
        }
        private User GetSessionUser(string userid)
        {
            return UserRepository.GetUser(userid);
        }

        private void InstanciateRepositories()
        {
            UserRepository = new UserRepository(this._database);
            CounterReposity = new DocumentCounterReposity(this._database);
            NodeRepository = new NodeRepository(this, this._database);
        }

        private IMongoDatabase InitializeDatabase()
        {
            BsonSerializer.RegisterDiscriminatorConvention(typeof(BaseProperty), new BasePropertyDiscriminator());
            BsonSerializer.RegisterDiscriminatorConvention(typeof(Entity), new EntityDiscriminator());

            MapEntities();

            // Mongo client
            var client = new MongoClient("mongodb+srv://m001-student:m001-mongodb-basics@" +
                "sandbox.aidnp.mongodb.net/temp?retryWrites=true&w=majority");

            return client.GetDatabase("Live2K");
        }

        private void MapEntities()
        {
            var entityTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(a => a.IsSubclassOf(typeof(Node))));
            foreach (var type in entityTypes)
            {
                var constructor = type.GetConstructor(System.Reflection.BindingFlags.Instance |
                                                      System.Reflection.BindingFlags.NonPublic,
                                                      null, new Type[] { typeof(Mediator), typeof(bool) }, null);
                var map = new BsonClassMap(type);
                map.AutoMap();
                map.SetCreator(() => constructor.Invoke(new object[] { this, true }));
                BsonClassMap.RegisterClassMap(map);
            }
        }

        internal User SessionUser { get; }
        internal UserRepository UserRepository { get; private set; }
        internal DocumentCounterReposity CounterReposity { get; private set; }
        internal NodeRepository NodeRepository { get; private set; }
        internal Factory Factory { get; }
    }
}
