namespace AIBehaviourAPI.US.DecisionFactors
{
    public class FusionFactorMax : ANodeUS
    {
        public FusionFactorMax(string name, ANodeUS[] parents) : base(name)
        {
            _inputNodes.AddRange(parents);
        }

        public override float GetUtilityValue()
        {
            if (_isCachedUtilityValueValid) return _cachedUtilityValue;
            
            float max = float.MinValue;
            foreach (ANodeUS parentNode in _inputNodes)
            {
                float value = parentNode.GetUtilityValue();
                if (value > max) max = value;
            }

            return max;
        }
    }
}