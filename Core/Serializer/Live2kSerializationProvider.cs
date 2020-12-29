using System;
using System.Reflection;
using Live2k.Core.Model.Base;
using MongoDB.Bson.Serialization;

namespace Live2k.Core.Serializer
{
    public class Live2kSerializationProvider : BsonSerializationProviderBase
    {
        public Live2kSerializationProvider()
        {
        }


        public override IBsonSerializer GetSerializer(Type type, IBsonSerializerRegistry serializerRegistry)
        {
            // if type is entity
            if (type == typeof(Entity) || type.IsSubclassOf(typeof(Entity)))
            {
                var genericSerializer = typeof(EntitySerializer<>).MakeGenericType(type);
                var constructor = genericSerializer.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                    null, new Type[0], null);
                var x = constructor.Invoke(new object[0]) as IBsonSerializer;
                return x;
            }

            return null;
        }
    }
}
