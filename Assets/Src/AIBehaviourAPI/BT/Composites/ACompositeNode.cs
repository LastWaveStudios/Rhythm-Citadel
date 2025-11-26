using System;
using System.Collections.Generic;
using System.Xml;

namespace AIBehaviourAPI.Bt.Composites
{
    public abstract class ACompositeNode : ANodeBT
    {
        public ACompositeNode(string name, List<ANodeBT> childNodes) : base(name)
        {
            _outputNodes = childNodes;
            _condition = NodeAction;
        }

        protected abstract bool NodeAction();
    }
}