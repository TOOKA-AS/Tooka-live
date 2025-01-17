﻿using System;
using Live2k.Core.Model.Base;
using Live2k.Core.Utilities;

namespace Live2k.Core.Model
{
    public class Comment
    {
        private Comment()
        {

        }

        private Comment(Mediator mediator, Node node, string body)
        {
            Id = Guid.NewGuid().ToString();
            Node = new NodeFootPrint(node);
            Signature = new UserSignature(mediator.SessionUser);
            Body = body;

            // Register comment on the node
            node.RegisterSessionComment(this);
        }

        public static Comment New(Mediator mediator, Node node, string body)
        {
            return new Comment(mediator, node, body);
        }

        /// <summary>
        /// Associated Id with the comment
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Id of the owner entity
        /// </summary>
        public NodeFootPrint Node { get; set; }

        /// <summary>
        /// User signature
        /// </summary>
        public UserSignature Signature { get; set; }

        /// <summary>
        /// Body of the comment
        /// </summary>
        public string Body { get; set; }
    }
}
