using Gameplay;
using Gameplay.RhythmSystem;
using Unity.VisualScripting;
using UnityEngine;


namespace UI.GameplayUI.VisualHelp
{
    public class GameplayVisualHelpController : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject _beatGearPrefab;
        [SerializeField] private GameObject _measureGearPrefab;

        [Header("Objects")]
        [SerializeField] private GameObject _gearsContainer;

        [Header("Transforms")]
        [SerializeField] private Transform _gear;
        [SerializeField] private Transform _spawnTransform;

        [Header("Configuration")]
        [SerializeField] private float _moveTime = 0.25f; // Time of the movement in seconds
        [SerializeField] private float _rotateCantity = 0.25f;
        [SerializeField] private float _rotateScale = 0.8f;

        private Vector3[] _targetsPositions;


        private void Start()
        {
            if (GameplayManager.Instance.IsInitialice && GameplayManager.Instance.Currentstate == GameplayState.Fight)
            {
                Init();
            }

            GameplayManager.Instance.onFightStateStart += Init;
            GameplayManager.Instance.onFightStateEnd += Disable;
        }

        public void Init()
        {
            gameObject.SetActive(true);
            uint beatsOnOneMeasure = RhythmManager.Instance.Signature.top;
            _targetsPositions = new Vector3[beatsOnOneMeasure + 1];
            Vector3 step = (_gear.transform.position - _spawnTransform.position) / (beatsOnOneMeasure);

            for (int i = 0; i <= beatsOnOneMeasure; ++i)
            {
                _targetsPositions[i] = _spawnTransform.position + (step * i);
            }

            RhythmManager.Instance.onBeat += OnBeat;
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            RhythmManager.Instance.onBeat -= OnBeat;
        }

        private void OnBeat(bool isMeasureBeat)
        {
            if (isMeasureBeat)
            {
                GenerateGear(_measureGearPrefab);
                return;
            }
            
            GenerateGear(_beatGearPrefab);
        }

        private void GenerateGear(GameObject prefab)
        {
            GameObject spawnedGear = GameObject.Instantiate(prefab, _spawnTransform.position, Quaternion.identity, _gearsContainer.transform);
            BeatGearController beatGearController = spawnedGear.GetComponent<BeatGearController>();
            if (beatGearController == null)
            {
                Debug.LogError($"GameplayVisualHelpController::GenerateGear The Gear spawned {prefab}, has not BeatGearController component");
                return;
            }
            beatGearController.Init(_targetsPositions, _moveTime, _rotateCantity * _rotateScale, Utilities.EasingFunctions.EaseOutQuart);
            beatGearController.Go();
        }
    }
}

