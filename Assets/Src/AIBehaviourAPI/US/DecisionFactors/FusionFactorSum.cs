namespace AIBehaviourAPI.US.DecisionFactors
{
    public class FusionFactorSum : ANodeUS
    {
        public FusionFactorSum(string name, ANodeUS[] parents) : base(name)
        {
            _inputNodes.AddRange(parents);
        }

        public override float GetUtilityValue()
        {
            float sumValue = 0;
            foreach (ANodeUS parentNode in _inputNodes)
            {
                sumValue += parentNode.GetUtilityValue();
            }
            
            return sumValue / (float)_inputNodes.Count;
        }
    }
}