using System;
using Newtonsoft.Json;

namespace Live2k.Core.Abstraction
{
    public class NodeFootPrint
    {
        [JsonIgnore]
        private Node _node;

        private NodeFootPrint()
        {
        }

        public NodeFootPrint(Node node)
        {
            _node = node ?? throw new ArgumentNullException(nameof(node));
            NodeId = node.Id ?? throw new ArgumentException($"Node is corrupted, {nameof(node.Id)} cannot be null");
            NodeLabel = node.Label ?? throw new ArgumentException($"Node is corrupted, {nameof(node.Label)} cannot be null");
            NodeDescription = node.Description;
        }

        public string NodeId { get; private set; }
        public string NodeLabel { get; private set; }
        public string NodeDescription { get; private set; }
    }
}
