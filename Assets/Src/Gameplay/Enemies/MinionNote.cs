using Gameplay.Enemies;
using Gameplay.Enemies.Behaviours;
using Gameplay.RhythmSystem;
using Gameplay.Waves;
using Gameplay.World;
using System;
using System.Collections;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utilities.ServiceLocator;


namespace Gameplay.Enemies
{
    public class MinionNote : AEnemy
    {
        private BaseMinionBehaviour _behaviour;
        private Tilemap _towerTilemap;
        private Tilemap _pathTilemap;

        private Vector3Int actualTile;
        private Vector3Int nextTile;

        private void Start()
        {
            _animator = GetComponent<Animator>();

            ServiceLocatorSubsystem.SubscribeToInitialice(TakeReferences);
            StartPosition();
            GameObject towerLayer = GameObject.FindGameObjectWithTag("TowerLayer");
            _towerTilemap = towerLayer.GetComponent<Tilemap>();

            GameObject pathLayer = GameObject.FindGameObjectWithTag("PathLayer");
            _pathTilemap = pathLayer.GetComponent<Tilemap>();

            StartStats();
        }
        private void StartPosition()
        {
            actualTile = _worldManager.GetCellFromWorldPos(Vector3.zero);
            transform.position = _worldManager.GetCellCenterWorld(actualTile);
            getNextTile();
        }

        protected override void OnBeat(bool isMeasure)
        {
            Debug.Log("On Beat");
        }
        protected virtual void StartStats()
        {
            _health = 10;
            _damage = 0;
            _vinylDrop = 0;
            _preparationBeats = 4;
            _resistanceMultiplayer = 0;

        }

        public override bool IsOverSomething()
        {
            GameObject[] minions = GameObject.FindGameObjectsWithTag("Minion");
            foreach (GameObject child in minions)
            {
                if (child.transform == transform) continue;
                if (_worldManager.GetCellFromWorldPos(child.transform.position) == _worldManager.GetCellFromWorldPos(transform.position)) return true;
            }
            return false;
        }

        protected override void SubscribeToRhythm()
        {
            _rhythmManager.onQuarter += OnRhythmUpdate;
            _timeOfNote = _rhythmManager.GetTimeOfANote(NoteDuration.Quarter) / 1000.0f;
        }

        protected override void InitializeBehaviour()
        {
            _behaviour = new BaseMinionBehaviour(this);
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

        public override void OnDeath()
        {
            onDeath.Invoke(this);
            gameObject.SetActive(false);
        }
        protected override void DesubscribeToRhythm()
        {
            _rhythmManager.onQuarter -= OnRhythmUpdate;
        }

        public void getNextTile() 
        {
            Vector3Int currentTile = _worldManager.GetCellFromWorldPos(transform.position);
            nextTile = _worldManager.GetRandomTile(currentTile);
        }

        private void updateSpeed()
        {
            if (_pathTilemap.HasTile(actualTile)) _preparationBeats = 2;
            else _preparationBeats = 4;
        }
        public override bool CanMove()
        {
            return (!_towerTilemap.HasTile(nextTile) && IsMovementPrepared());
        }

        public override bool isCollapsing()
        {
            return (_towerTilemap.HasTile(nextTile) && IsMovementPrepared());
        }

        public override void Collapse()
        {
            base.Collapse();
            getNextTile();
        }
        protected override IEnumerator MoveToNextTile(Func<float, float> easingFunction = null)
        {
            
            Vector3 originPos = transform.position;
            Vector3 targetPos = _worldManager.GetCellCenterWorld(nextTile);

            Vector3 tileSize = _worldManager.GetTileSize();
            //Offset for enemy stacking
            float offsetFactor = 0.3f;
            targetPos += new Vector3(UnityEngine.Random.Range(-tileSize.x * offsetFactor, tileSize.x * offsetFactor),
                                     UnityEngine.Random.Range(-tileSize.y * offsetFactor, tileSize.y * offsetFactor),
                                     0f);

            float jumpOffset;
            float targetJumpOffset;
            float originPosOffset;
            bool isHorizontal;
            bool isFlipedTheorigin = false;
            if (nextTile.x == actualTile.x)
            {
                if (nextTile.y > actualTile.y)
                {
                    jumpOffset = targetPos.x - _jumpHeight;
                }
                else jumpOffset = targetPos.x + _jumpHeight;
                targetJumpOffset = targetPos.x;
                originPosOffset = originPos.x;
                isHorizontal = true;
            }
            else
            {
                jumpOffset = targetPos.y + _jumpHeight;
                targetJumpOffset = targetPos.y;
                originPosOffset = originPos.y;
                isHorizontal = false;
            }

            Vector3 originScale = transform.localScale;
            Vector3 targetScale = _originScale;

            float t = 0.0f;
            while (t <= _moveTime)
            {
                float T;
                if (easingFunction == null) T = t / _moveTime;
                else T = easingFunction(t / _moveTime);
                transform.position = originPos * (1.0f - T) + targetPos * T;

                // Jump offset
                float jT = Utilities.EasingFunctions.NormalizeParabolaNotConvex(T);
                if (!isFlipedTheorigin && jT <= 0.5)
                {
                    originPosOffset = targetJumpOffset;
                    isFlipedTheorigin = true;
                }
                if (isHorizontal)
                {
                    transform.position = new Vector3(originPosOffset * (1.0f - jT) + jumpOffset * jT, transform.position.y, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, originPosOffset * (1.0f - jT) + jumpOffset * jT, transform.position.z);
                }
                // Scale offset
                float sT = Utilities.EasingFunctions.EaseInBounce(t / _moveTime);
                transform.localScale = originScale * (1.0f - sT) + targetScale * sT;

                t += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPos;   // Fix for center final positions
            transform.localScale = targetScale;
            actualTile = nextTile;
            getNextTile();
            updateSpeed();
        }
    }
}

