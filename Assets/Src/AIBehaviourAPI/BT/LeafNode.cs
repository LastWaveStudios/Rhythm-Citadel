using System;

namespace AIBehaviourAPI.Bt
{
    public class LeafNode : ANodeBT
    {
        public LeafNode(string name, Func<bool> action) : base(name, action) { }

        public LeafNode(string name, Action action, bool actionReturnConst) : base(name, () =>
        {
            action();
            return actionReturnConst;
        }) { }
    }
}