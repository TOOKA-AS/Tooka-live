using System;
using Live2k.Core.Base;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Live2k.Core.Serializer
{
    public class BasePropertySerializer : SerializerBase<BaseProperty>
    {
        public override BaseProperty Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            context.Reader.ReadStartDocument();

            var title = context.Reader.ReadString();
            var description = context.Reader.ReadString();
            var value = ReadValue(context.Reader);

            context.Reader.ReadEndDocument();

            var type = typeof(Property<>).MakeGenericType(value.GetType());
            var constructor = type.GetConstructor(new Type[] { title.GetType(), description.GetType(), value.GetType() });

            return constructor.Invoke(new object[] { title, description, value }) as BaseProperty;
        }

        private object ReadValue(IBsonReader reader)
        {
            var type = reader.ReadBsonType();

            switch (type)
            {
                case BsonType.EndOfDocument:
                    break;
                case BsonType.Double:
                    return reader.ReadDouble();
                case BsonType.String:
                    return reader.ReadString();
                case BsonType.Document:
                    return EvaluateAsDocument(reader);
                case BsonType.Array:
                    break;
                case BsonType.Binary:
                    break;
                case BsonType.Undefined:
                    break;
                case BsonType.ObjectId:
                    break;
                case BsonType.Boolean:
                    break;
                case BsonType.DateTime:
                    return reader.ReadDateTime();
                case BsonType.Null:
                    return null;
                case BsonType.RegularExpression:
                    break;
                case BsonType.JavaScript:
                    break;
                case BsonType.Symbol:
                    break;
                case BsonType.JavaScriptWithScope:
                    break;
                case BsonType.Int32:
                    return reader.ReadInt32();
                case BsonType.Timestamp:
                    break;
                case BsonType.Int64:
                    return reader.ReadInt64();
                case BsonType.Decimal128:
                    break;
                case BsonType.MinKey:
                    break;
                case BsonType.MaxKey:
                    break;
                default:
                    break;
            }

            return null;
        }

        private object EvaluateAsDocument(IBsonReader reader)
        {
            throw new NotImplementedException();
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, BaseProperty value)
        {
            context.Writer.WriteStartDocument();
            context.Writer.WriteName(nameof(value.Title));
            context.Writer.WriteString(value.Title);
            context.Writer.WriteName(nameof(value.Description));
            context.Writer.WriteString(value.Description);

            // Write value
            WriteValue(context.Writer, value.GetValue());

            context.Writer.WriteEndDocument();
        }

        private void WriteValue(IBsonWriter writer, object value)
        {
            writer.WriteName("Value");
            switch (value)
            {
                case int _int:
                    writer.WriteInt32(_int);
                    break;
                case double _double:
                    writer.WriteDouble(_double);
                    break;
                case string _string:
                    writer.WriteString(_string);
                    break;
                case Entity _entity:
                    writer.WriteString(value.GetType().Name);
                    break;
                default:
                    writer.WriteString("Could not find type");
                    break;
            }
        }
    }
}
