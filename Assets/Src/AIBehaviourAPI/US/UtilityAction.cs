using System;

namespace AIBehaviourAPI.US
{
    public class UtilityAction : ANodeUS
    {
        private ANodeUS _parentNode => _inputNodes.Count > 0? _inputNodes[0] : null;
        
        public UtilityAction(string name, Action action, ANodeUS parent) : base(name)
        {
            _inputNodes.Add(parent);
            _action = action;
        }

        public override float GetUtilityValue()
        {
            if (_isCachedUtilityValueValid) return _cachedUtilityValue;
            
            return _parentNode.GetUtilityValue();
        }
    }
}