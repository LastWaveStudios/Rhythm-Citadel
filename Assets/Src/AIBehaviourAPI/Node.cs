using System;
using System.Collections.Generic;
using System.Text;

namespace AIBehaviourAPI
{
    public class Node
    {
        public ABehaviourGraph BehaviourGraph { get; internal set; }

        private Action action = null;
        public string Name { get; private set; }

        public List<Node> inputNodes;
        public List<Node> outputNodes;

        public Node(Action action, string name)
        {
            this.action = action;
            inputNodes = new List<Node>();
            outputNodes = new List<Node>();
            Name = name;
        }

        public bool ExecuteIfBound()
        {
            if (action == null) return false;
            action();
            return true;
        }

        public bool IsInputNode(Node node)
        {
            return inputNodes.Contains(node);
        }

        public bool IsOutputNode(Node node)
        {
            return outputNodes.Contains(node);
        }


        public override string ToString()
        {
            return $"Node: {Name} with action {action.ToString()} have this inputNodes: ({NodeListToString(inputNodes)}); outputNodes: ({NodeListToString(outputNodes)})";
        }

        public static string NodeListToString(List<Node> nodes)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Nodes: ");
            for (int i = 0; i < nodes.Count; ++i)
            {
                builder.Append($"{nodes[i].Name}, ");
            }
            builder.Append($"{nodes[nodes.Count - 1].Name}");
            return builder.ToString();
        }
    }
}