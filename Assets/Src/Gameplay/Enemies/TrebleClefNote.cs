using System;
using System.Collections.Generic;
using Gameplay.Enemies.Behaviours;
using Gameplay.RhythmSystem;
using Gameplay.Waves;
using TMPro;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;
using UnityEngine;
using Utilities.ServiceLocator;
using Math = Unity.Mathematics.Geometry.Math;


namespace Gameplay.Enemies
{
    public class TrebleClefNote : AEnemy
    {
        [Header("Boss special effects")]
        [SerializeField] private int _hpToStealth = 10;
        [SerializeField] private int _maxHpRestore = 30;
        [SerializeField] private int _range = 3;
        [SerializeField] private float _thresholdForNoAction = 0.7f;

        private int _maxHealth;
        
        private WaveManager _waveManager;

        private List<int> _enemiesOnRangeIDs;
        
        private bool _isMeasure = false;
        public bool IsMeasure => _isMeasure; // For the Behaviour
        
        public float Health => _health;
        public int EnemiesOnRangeForMaxHealth => Mathf.CeilToInt((float)_hpToStealth / _maxHpRestore);

        private TrebleClefNoteBehaviour _behaviour;

        [SerializeField] public List<TMP_Text> perceptionTexts;
        [SerializeField] public List<TMP_Text> decisionFactorTexts;
        [SerializeField] public TMP_Text actionText;
        
        protected override void SubscribeToRhythm()
        {
            _rhythmManager.onBeat += OnBeat;
            _timeOfNote = _rhythmManager.GetTimeOfAMeasure() / 1000.0f;
        }
        
        protected override void InitializeBehaviour()
        {
            // Alittle tricky but this is called on the Init, method so the Services are Initialize
            _waveManager = ServiceLocatorSubsystem.Instance.GetService<WaveManager>();
            if (_waveManager == null)
            {
                Debug.LogError("TrebleClefNote::InitializeBehaviour: Wave Manager not found");
                return;
            }

            _enemiesOnRangeIDs = new List<int>();

            _maxHealth = _health;

            _behaviour = new TrebleClefNoteBehaviour(this, perceptionTexts, decisionFactorTexts, actionText);
        }

        protected override void PushDeath()
        {
            IsAlive = false;
            _behaviour.PushDeath();
        }

        protected override void OnRhythmUpdate()
        {
            _behaviour.UpdateBehaviour();
        }
        
        private void OnBeat(bool isMeasure)
        {
            _isMeasure = isMeasure;
            OnRhythmUpdate();
        }

        public override void OnDeath()
        {
            onDeath.Invoke(this);
            gameObject.SetActive(false);
        }
        protected override void DesubscribeToRhythm()
        {
            _rhythmManager.onBeat -= OnBeat;
        }
        
        public float GetAlliesOnRange()
        {
            Vector3Int tilePosition = GetTile();

            _enemiesOnRangeIDs.Clear();
            
            float count = 0.0f;
            for (int i = 0; i <= _waveManager.LastEnemySpawnedInCurrentWave; ++i)
            {
                AEnemy ally = _waveManager.GetEnemy(i);
                if (Utilities.Distances.ManhattanDistance(ally.GetTile(), tilePosition) <= _range)
                {
                    count++;
                    _enemiesOnRangeIDs.Add(i);
                }
            }

            return count;
        }

        public void StealthHealth()
        {
            int stealHealth = 0;
            foreach (int enemyIndex in _enemiesOnRangeIDs)
            {
                AEnemy enemy = _waveManager.GetEnemy(enemyIndex);
                if (enemy == null) continue;

                enemy.TakeDamage(DamageType.TrueDamage, _hpToStealth);
                stealHealth += _hpToStealth;
            }
            _health += (stealHealth <= _maxHpRestore? stealHealth : _maxHpRestore);
            
            // TODO: Do some particles or animation for the stealth of HP
            //Debug.Log("Stealth Health");
        }

        public float GetMaxHealth()
        {
            return _maxHealth;
        }

        public float GetCurrentPreparation()
        {
            return _currentPreparation;
        }

        public float GetMaxPreparation()
        {
            return _preparationBeats;
        }
        
        public float GetThresholdForNoAction()
        {
            return _thresholdForNoAction;
        }
    } 
}

