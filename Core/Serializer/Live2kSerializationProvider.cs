using System;
using Live2k.Core.Abstraction;
using MongoDB.Bson.Serialization;

namespace Live2k.Core.Serializer
{
    public class Live2kSerializationProvider : IBsonSerializationProvider
    {
        public Live2kSerializationProvider()
        {
        }

        public IBsonSerializer GetSerializer(Type type)
        {
            // if type is property
            if (type == typeof(BaseProperty) || type.IsSubclassOf(typeof(BaseProperty)))
            {
                return new BasePropertySerializer();
            }

            return null;
        }
    }
}
