using System;
using System.Linq;
using Live2k.Core.Model.Base;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Conventions;

namespace Live2k.Core.Serializer
{
    public class EntityDiscriminator : IDiscriminatorConvention
    {
        public EntityDiscriminator()
        {
        }

        public string ElementName => "_t";

        public Type GetActualType(IBsonReader bsonReader, Type nominalType)
        {
            var ret = nominalType;

            var bookmark = bsonReader.GetBookmark();
            bsonReader.ReadStartDocument();
            if (bsonReader.FindElement(ElementName))
            {
                var value = bsonReader.ReadString();

                ret = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes().Where(a=>a.IsSubclassOf(typeof(Node))))
                    .FirstOrDefault(a=>a.FullName == value);
            }

            bsonReader.ReturnToBookmark(bookmark);

            return ret;
        }

        public BsonValue GetDiscriminator(Type nominalType, Type actualType)
        {
            return actualType.FullName;
        }
    }
}
