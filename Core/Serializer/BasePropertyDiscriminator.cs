using System;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Conventions;

namespace Live2k.Core.Serializer
{
    public class BasePropertyDiscriminator : IDiscriminatorConvention
    {
        public BasePropertyDiscriminator()
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

                ret = Type.GetType(value);
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
