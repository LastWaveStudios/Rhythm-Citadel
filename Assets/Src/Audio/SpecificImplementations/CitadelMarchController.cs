using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIBehaviourAPI.US;
using AIBehaviourAPI.US.DecisionFactors;
using AIBehaviourAPI.US.UtilityFunctions;
using Gameplay;
using Unity.VisualScripting;
using UnityEngine;
using Utilities.ServiceLocator;


namespace Audio.SpecificImplementations
{
    public class CitadelMarchController : AModularMusicController
    {
        private LevelStats _levelStats;
        
        private UtilitySystem _us;

        private List<int> _returnList;

        // This percentage is between 0 and 1, means the threshold for the parts to be reproduced the next measure (when the previous parts end)
        [SerializeField] private float _thresholdForReproduceThePart = 0.8f;
        
        protected override void Init()
        {
            base.Init();
            
            _levelStats = ServiceLocatorSubsystem.Instance.GetService<LevelStats>();
            if (_levelStats == null)
            {
                Debug.LogError("CitadelMarchController::Init: LevelStats not found");
                return;
            }
            
            _returnList = new List<int>(_clipsRhythms.Count);
            
            // Behaviour of the Music

            // It is a custom mode so i must to set the custom function for it
            _us = new UtilitySystem("Citadel March US controller", USMode.Custom);
            _us.SetCustomFunction(CustomUSFunction);

            UtilityPerception wavePercentageP = new UtilityPerception("Wave Percentage Perception", () => _levelStats.WavePercentageSpawned);
            UtilityPerception wavePercentageKilledP = new UtilityPerception("Wave Percentage Killed Perception", () => _levelStats.WavePercentageKilled);
            UtilityPerception levelPercentageP = new UtilityPerception("Level Percentage Perception", () => _levelStats.CurrentLevelPercentage);
            UtilityPerception dancerHpPercentageP = new UtilityPerception("dancer HP Percentage Perception", () => _levelStats.DancerHpPercentage);

            DecisionFactor lowWavePercentageDF = new DecisionFactor("Low Wave Percentage Decision Factor", new Line(1.0f, 0.0f), wavePercentageP);
            DecisionFactor midWavePercentageDF = new DecisionFactor("Mid Wave Percentage Decision Factor", new Parabola(false), wavePercentageP);
            DecisionFactor highWavePercentageDF = new DecisionFactor("High Wave Percentage Decision Factor", new Line(0.0f, 1.0f), wavePercentageP);
            DecisionFactor highWaveKilledPercentageDF = new DecisionFactor("High Wave Killed Percentage Decision Factor", new Line(0.0f, 1.0f), wavePercentageKilledP);
            DecisionFactor highLevelPercentageDF = new DecisionFactor("High Level Percentage Decision Factor", new Line(0.0f, 1.0f), levelPercentageP);
            DecisionFactor lowDancerHpPercentageDF = new DecisionFactor("Low Dancer HP Percentage Decision Factor", new Line(1.0f, 0.0f), dancerHpPercentageP);

            FusionFactorSum sumOfWaveAndWaveKilledPercentagesFDF = new FusionFactorSum("Wave and Wave Killed Sum Percentage Fusion Decision Factor", new ANodeUS[] { highWavePercentageDF, highWaveKilledPercentageDF });
            FusionFactorMax maxOfDancerHpAndLevelPercentageFDF = new FusionFactorMax("Dancer HP and Level Percentage Max Fusion Decision Factor", new ANodeUS[] { highLevelPercentageDF, lowDancerHpPercentageDF});

            UtilityAction part0Action = new UtilityAction("Part 0 Action", () => AddToReturnList(0), lowWavePercentageDF);
            UtilityAction part1Action = new UtilityAction("Part 1 Action", () => AddToReturnList(1), midWavePercentageDF);
            UtilityAction part2Action = new UtilityAction("Part 2 Action", () => AddToReturnList(2), highWavePercentageDF);
            UtilityAction part3Action = new UtilityAction("Part 3 Action", () => AddToReturnList(3), sumOfWaveAndWaveKilledPercentagesFDF);
            UtilityAction part4Action = new UtilityAction("Part 4 Action", () => AddToReturnList(4), highLevelPercentageDF);
            UtilityAction part5Action = new UtilityAction("Part 5 Action", () => AddToReturnList(5), maxOfDancerHpAndLevelPercentageFDF);
            
            _us.RegisterNode(wavePercentageP);
            _us.RegisterNode(wavePercentageKilledP);
            _us.RegisterNode(levelPercentageP);
            _us.RegisterNode(dancerHpPercentageP);
            
            _us.RegisterNode(lowWavePercentageDF);
            _us.RegisterNode(midWavePercentageDF);
            _us.RegisterNode(highWavePercentageDF);
            _us.RegisterNode(highWaveKilledPercentageDF);
            _us.RegisterNode(highLevelPercentageDF);
            _us.RegisterNode(lowDancerHpPercentageDF);
            
            _us.RegisterNode(sumOfWaveAndWaveKilledPercentagesFDF);
            _us.RegisterNode(maxOfDancerHpAndLevelPercentageFDF);
            
            _us.RegisterAction(part0Action);
            _us.RegisterAction(part1Action);
            _us.RegisterAction(part2Action);
            _us.RegisterAction(part3Action);
            _us.RegisterAction(part4Action);
            _us.RegisterAction(part5Action);
            
            _us.Init();
        }

        // Based on the order in wich the actions has been registered into the US, must return the indices of the actions that want to be executed
        private int[] CustomUSFunction(List<float> utilityValues)
        {
            List<int> actionsToExecute = new List<int>();
            StringBuilder builder = new StringBuilder();
            builder.Append("The utility Values are: [");
            for (int i = 0; i < utilityValues.Count; i++)
            {
                builder.Append($"({i} -> {utilityValues[i]}); ");
                if (utilityValues[i] >= _thresholdForReproduceThePart)
                {
                    actionsToExecute.Add(i);
                }
            }
            builder.Append($"] and the threshold are {_thresholdForReproduceThePart}");
            Debug.Log(builder.ToString());
            return actionsToExecute.ToArray();
        }

        private void AddToReturnList(int index)
        {
            _returnList.Add(index);
        }
        
        protected override int[] InitClips()
        {
            return new int[] { 0 };
        }

        protected override int[] SelectNextClips(int[] songPartThatWillEnd)
        {
            _returnList.Clear();
            
            _us.Update();
            
            // If reach here and the return list is empty or do not have the part 0, 1, 2, 3 means that there is no melody on it so just add the 0
            if (!_returnList.Any(x => x >= 0 && x <= 3))
            {
                _returnList.Add(0);
            }
            
            return _returnList.ToArray();
        }
    }
}