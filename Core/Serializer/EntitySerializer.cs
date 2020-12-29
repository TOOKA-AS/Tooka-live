using System;
using System.Linq;
using Live2k.Core.Model.Base;
using Live2k.Core.Utilities;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Live2k.Core.Serializer
{
    internal class EntitySerializer<T> : ClassSerializerBase<T> where T: Entity
    {
        public override T Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context.Reader.GetCurrentBsonType() == MongoDB.Bson.BsonType.Document)
                context.Reader.ReadStartDocument();

            var id = context.Reader.ReadString();
            var typename = context.Reader.ReadString();

            //context.Reader.ReadEndDocument();

            var type =  AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes().Where(a => a.IsSubclassOf(typeof(Node))))
                    .FirstOrDefault(a => a.FullName == typename);
            var constructor = type.GetConstructor(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, new Type[] { typeof(Mediator), typeof(Factory) }, null);
            //base.
            //return constructor.Invoke(new object[] { Mediator.GetInstance(), new Factory(Mediator.GetInstance()) }) as T;
            return null;
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, T value)
        {
            base.Serialize(context, args, value);
        }
    }
}