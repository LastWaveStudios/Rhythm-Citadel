
using AIBehaviourAPI.Exceptions;
using System;
using System.Collections.Generic;

namespace AIBehaviourAPI
{
    public abstract class ABehaviourGraph : ABehaviourEngine
    {
        public Node StartNode 
        { 
            get
            {
                if (_nodes.Count == 0)
                    return null;
                return _nodes[0];
            }
            protected set
            {
                if (!_nodes.Contains(value))
                    throw new EmptyGraphException(this, "The node is not in the graph");

                if (_nodes[0] != value)
                {
                    if (_nodes.Remove(value)) _nodes.Insert(0, value);
                }
            }
        }

        protected List<Node> _nodes = new List<Node>();

        protected Node CreateNode(Action action, string name)
        {
            Node node = new Node(action, name);
            return node;
        }

        protected void AddNode(Node node)
        {
            _nodes.Add(node);
            node.BehaviourGraph = this;
        }

        protected void ConnectNodes(Node origin, Node target)
        {
            if (origin == null && target == null)
                throw new EmptyGraphException(this, "The origin or target nodes are null");

            if (_nodes.Contains(origin) && _nodes.Contains(target))
                throw new EmptyGraphException(this, "THe origin or target nodes are not in the graph");

            origin.outputNodes.Add(target);
            target.inputNodes.Add(origin);
        }
    }
}