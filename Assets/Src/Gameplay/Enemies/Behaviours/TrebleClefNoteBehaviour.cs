using System.Collections.Generic;
using AIBehaviourAPI.Bt;
using AIBehaviourAPI.US;
using AIBehaviourAPI.US.DecisionFactors;
using AIBehaviourAPI.US.UtilityFunctions;
using TMPro;
using UnityEngine;

namespace Gameplay.Enemies.Behaviours
{
    public class TrebleClefNoteBehaviour : IPushDeathPerception
    {
        private TrebleClefNote _enemy;

        private UtilitySystem _us;
        
        private List<TMP_Text> _perceptionTexts;
        private List<TMP_Text> _decisionFactorTexts;
        private TMP_Text _actionText;

        public TrebleClefNoteBehaviour(TrebleClefNote enemy, List<TMP_Text> perceptionTextList = null, List<TMP_Text> DFTextList = null, TMP_Text actionText = null)
        {
            _enemy = enemy;
            
            _perceptionTexts = perceptionTextList;
            _decisionFactorTexts = DFTextList;
            _actionText = actionText;

            _us = new UtilitySystem("Treble Clef Note US", USMode.Max, 19, 6, _perceptionTexts, _decisionFactorTexts, _actionText);

            UtilityPerception isAlivePerception = new UtilityPerception("Is Alive Perception", () => _enemy.IsAlive ? 0.0f : 1.0f);
            UtilityPerception isInTargetPerception = new UtilityPerception("Is In Target Perception", () => _enemy.IsInTarget() ? 1.0f : 0.0f);
            UtilityPerception alliesOnRangePerception = new UtilityPerception("Allies On Range Perception", () => Mathf.Clamp(_enemy.GetAlliesOnRange(), 0.0f, _enemy.EnemiesOnRangeForMaxHealth));
            UtilityPerception healthPerception = new UtilityPerception("Health Perception", () => Mathf.Clamp(_enemy.Health, 0.0f, _enemy.GetMaxHealth()));
            UtilityPerception isMovementPreparedPerception = new UtilityPerception("Is Movement Prepared Perception", () => _enemy.IsMovementPrepared() ? 1.0f : 0.0f);
            UtilityPerception isMeasurePerception = new UtilityPerception("Is Measure Perception", () => _enemy.IsMeasure ? 1.0f : 0.0f);
            UtilityPerception defaultActionValuePerception = new UtilityPerception("Default Action Value Perception", _enemy.GetThresholdForNoAction);

            // Instead of implement the boolean decisionFactors of the perceptions that are bool or the default we just go use them directly because are between 0 and 1 not process needed
            DecisionFactor alliesOnRangeDF = new DecisionFactor("Allies On Range Decision Factor", new Line(0.0f, (float)_enemy.EnemiesOnRangeForMaxHealth), alliesOnRangePerception);
            DecisionFactor healthNormalizationDF = new DecisionFactor("Health Normalization Decision Factor", new Line(0.0f, _enemy.GetMaxHealth()), healthPerception);
            DecisionFactor healthDF = new DecisionFactor("HP Decision Factor", new Sigmoid(-7.8f, 0.5f), healthNormalizationDF);

            FusionFactorMin attackFDF = new FusionFactorMin("Attack Fusion DF", new ANodeUS[] { isInTargetPerception, isMovementPreparedPerception, isMeasurePerception });
            FusionFactorWeightedSum stealthHealthFDF = new FusionFactorWeightedSum("Stealth HP Fusion DF", new ANodeUS[] { alliesOnRangeDF, healthDF },
                                                                                                                        new float[] { 0.3f, 0.7f });
            FusionFactorMin moveFDF = new FusionFactorMin("Move Fusion DF", new ANodeUS[] { isMovementPreparedPerception, isMeasurePerception });

            UtilityAction deadUA = new UtilityAction("Dead Action", _enemy.Death, isAlivePerception);
            UtilityAction attackUA = new UtilityAction("Attack Action", _enemy.Attack, attackFDF);
            UtilityAction stealthHealthUA = new UtilityAction("Stealth Health Action", _enemy.StealthHealth, stealthHealthFDF);
            UtilityAction moveUA = new UtilityAction("Move Action", () =>
            {
                _enemy.RestartMovementPreparation();
                _enemy.Move();
            }, moveFDF);
            UtilityAction prepareMoveUA = new UtilityAction("Prepare Move Action", _enemy.PrepareMovement, isMeasurePerception);
            UtilityAction defaultAction = new UtilityAction("Default Action", () => Debug.Log("Treble Clef Note action was nothing"), defaultActionValuePerception);

            _us.RegisterNode(isAlivePerception);
            _us.RegisterNode(isInTargetPerception);
            _us.RegisterNode(alliesOnRangePerception);
            _us.RegisterNode(healthPerception);
            _us.RegisterNode(isMovementPreparedPerception);
            _us.RegisterNode(isMeasurePerception);
            _us.RegisterNode(defaultActionValuePerception);

            _us.RegisterNode(alliesOnRangeDF);
            _us.RegisterNode(healthNormalizationDF);
            _us.RegisterNode(healthDF);

            _us.RegisterNode(attackFDF);
            _us.RegisterNode(stealthHealthFDF);

            _us.RegisterNode(moveFDF);

            _us.RegisterAction(deadUA);
            _us.RegisterAction(attackUA);
            _us.RegisterAction(stealthHealthUA);
            _us.RegisterAction(moveUA);
            _us.RegisterAction(prepareMoveUA);
            _us.RegisterAction(defaultAction);

            _us.Init();
        }


        public void UpdateBehaviour()
        {
            _us.Update();
        }
        
        public void PushDeath()
        {
            _us.Update();
        }
    }
}