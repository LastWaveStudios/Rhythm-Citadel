using System;
using UnityEngine;
using Utilities.ServiceLocator;
using UnityEngine.UI;
using Gameplay.RhythmSystem;
using System.Collections.Generic;

namespace Gameplay.World
{
    public class Dancer : Utilities.ServiceLocator.AService
    {
        [SerializeField] private float _health = 100;
        private float _maxLife;
        public Action onDancerDeath;
        private SpriteRenderer _spriteRender;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private Image lifeFill;
        
        public float Health => _health;
        public float MaxLife => _maxLife;
        
        [SerializeField] private List<Sprite> normalFrames = new List<Sprite>();
        [SerializeField] private List<Sprite> L1DamagedFrames = new List<Sprite>();
        [SerializeField] private List<Sprite> L2DamagedFrames = new List<Sprite>();
        [SerializeField] private List<Sprite> L3DamagedFrames = new List<Sprite>();
        private int currentFrame = 0;

        private RhythmManager _rhythmManager;

        private void Awake()
        {
            _maxLife = _health;
        }
        public void Start()
        {
            //ServiceLocatorSubsystem.SubscribeToInitialice(Init);
            _spriteRender = GetComponent<SpriteRenderer>();
        }
        public override void Init()
        {
            _rhythmManager = ServiceLocatorSubsystem.Instance.GetService<RhythmManager>();
            if (_rhythmManager == null)
            {
                Debug.LogError("Dancer::Init: The rhythm manager is null");
                return;
            }
            _rhythmManager.onBeat += StepOneFrame;
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            lifeFill.fillAmount = _health/ _maxLife;
            CheckDeath();
            if (_health < 25)
            {
                _spriteRender.sprite = L3DamagedFrames[currentFrame];
            }
            else if (_health < 50)
            {
                _spriteRender.sprite = L2DamagedFrames[currentFrame];
            }
            else if (_health < 75)
            {
                _spriteRender.sprite = L1DamagedFrames[currentFrame];
            }
        }

        public void StepOneFrame(bool isFirstBeat)
        {
            if (normalFrames == null || normalFrames.Count == 0)
            {
                Debug.LogError("ATower is missing animation frames");
                return;
            }

            currentFrame++;

            if (currentFrame >= normalFrames.Count)
                currentFrame = 0;

            if (_health < 25)
            {
                _spriteRender.sprite = L3DamagedFrames[currentFrame];
            }
            else if (_health < 50)
            {
                _spriteRender.sprite = L2DamagedFrames[currentFrame];
            }
            else if (_health < 75)
            {
                _spriteRender.sprite = L1DamagedFrames[currentFrame];
            }
            else if (_health >= 75)
            {
                _spriteRender.sprite = normalFrames[currentFrame];
            }
            Debug.Log("Current frame: " + currentFrame);
        }

        public void CheckDeath()
        {
            if (_health <= 0)
                onDancerDeath.Invoke();

        }
    }
}

