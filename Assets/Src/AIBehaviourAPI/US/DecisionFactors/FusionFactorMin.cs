namespace AIBehaviourAPI.US.DecisionFactors
{
    public class FusionFactorMin : ANodeUS
    {
        public FusionFactorMin(string name, ANodeUS[] parents) : base(name)
        {
            _inputNodes.AddRange(parents);
        }

        public override float GetUtilityValue()
        {
            if (_isCachedUtilityValueValid) return _cachedUtilityValue;
            
            float min = float.MaxValue;
            foreach (ANodeUS parentNode in _inputNodes)
            {
                float value = parentNode.GetUtilityValue();
                if (value < min) min = value;
            }

            return min;
        }
    }
}