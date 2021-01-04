using System;
using Live2k.Core.Model.Base;
using Live2k.Core.Utilities;
using MongoDB.Bson.Serialization.Attributes;

namespace Live2k.Core.Model
{
    public class Attachment
    {
        private Attachment()
        {

        }

        private Attachment(Mediator mediator, Node node, string name, string extension, byte[] filebytes)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Extension = extension;
            Signature = new UserSignature(mediator.SessionUser);
            OwnerNode = new NodeFootPrint(node);
            Uri = "https:\\iamfara.com";

            node.RegisterSessionAttachment(this);
        }

        public static Attachment New(Mediator mediator, Node node, string name, string extension, byte[] filebytes)
        {
            return new Attachment(mediator, node, name, extension, filebytes);
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }

        [BsonIgnore]
        public string FillName => string.Format("{0}.{1}", Name, Extension);
        public string Description { get; set; }
        public UserSignature Signature { get; set; }
        public NodeFootPrint OwnerNode { get; set; }
        public string Uri { get; set; }
    }
}
