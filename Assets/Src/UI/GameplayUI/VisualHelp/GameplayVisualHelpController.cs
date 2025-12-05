using Gameplay;
using Gameplay.RhythmSystem;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Utilities.ServiceLocator;


namespace UI.GameplayUI.VisualHelp
{
    public class GameplayVisualHelpController : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject _beatGearPrefab;
        [SerializeField] private GameObject _measureGearPrefab;

        [Header("Objects")]
        [SerializeField] private RectTransform _gearsContainer;
        [SerializeField] private GameObject _bigGearGameObject;

        [Header("Transforms")]
        [SerializeField] private RectTransform _gear;
        [SerializeField] private RectTransform _spawnTransform;

        [Header("Configuration")]
        [SerializeField] private float _moveTime = 0.25f; // Time of the movement in seconds
        [SerializeField] private float _rotateCantity = 0.25f;
        [SerializeField] private float _rotateScale = 0.8f;

        [SerializeField] private Canvas _canvas;

        private Vector2[] _targetsPositions;

        private bool _mustSpawnGears = true;

        private RhythmManager _rhythmManager;

        public void Start()
        {
            ServiceLocatorSubsystem.SubscribeToInitialice(Init);
        }
        public void Init()
        {
            //Debug.Log("Gamplay Visual Help INIT");
            _rhythmManager = ServiceLocatorSubsystem.Instance.GetService<RhythmManager>();
            if (_rhythmManager == null)
            {
                Debug.LogError("GameplayVisualHelpController::Init: The RhythmManager is null");
                return;
            }

            gameObject.SetActive(true);
            uint beatsOnOneMeasure = _rhythmManager.Signature.top;
            // _targetsPositions = new Vector2[beatsOnOneMeasure + 1];
            //Vector2 step = (_gear.anchoredPosition - _spawnTransform.anchoredPosition) / (beatsOnOneMeasure);
            //  Debug.Log("GEAR" + _gear.anchoredPosition);
            // Debug.Log("SPAWN" + _spawnTransform.anchoredPosition);

            /*  for (int i = 0; i <= beatsOnOneMeasure; ++i)
              {
                  _targetsPositions[i] = _spawnTransform.anchoredPosition + (step * i);
                  //_targetsPositions[i] = _spawnTransform.anchoredPosition;
                 // Debug.Log("TARGET POSITION" + _targetsPositions[i]);
              }*/
            _targetsPositions = new Vector2[beatsOnOneMeasure + 1];

            // 1. Pasamos SPAWN y GEAR a coordenadas locales de DinamicGears
            Vector3 spawnLocal3 = _gearsContainer.InverseTransformPoint(_spawnTransform.position);
            Vector3 gearLocal3 = _gearsContainer.InverseTransformPoint(_gear.position);

            Vector2 spawnLocal = new Vector2(spawnLocal3.x, spawnLocal3.y);
            Vector2 gearLocal = new Vector2(gearLocal3.x, gearLocal3.y);

            // 2. Mismo sitio de partida y llegada, pero AHORA en el mismo sistema
            Vector2 step = (gearLocal - spawnLocal) / beatsOnOneMeasure;

            for (int i = 0; i <= beatsOnOneMeasure; ++i)
            {
                _targetsPositions[i] = spawnLocal + step * i;
            }

            Debug.Log($"SPAWN LOCAL {spawnLocal}");
            Debug.Log($"GEAR  LOCAL {gearLocal}");
            Debug.Log($"FIRST TARGET  {_targetsPositions[0]}");
            Debug.Log($"LAST  TARGET  {_targetsPositions[_targetsPositions.Length - 1]}");

            _rhythmManager.onBeat += OnBeat;
            _rhythmManager.onFinishRhythmNextMeasure += OnFinishRhythmNextMeasure;
            _rhythmManager.onEndRhythm += OnRhythmEnd;
        }

        private void OnRhythmEnd()
        {
            // Let the value start for the reactivation of the manager in the next wave
            _mustSpawnGears = true;
        }

        private void OnFinishRhythmNextMeasure()
        {
            _mustSpawnGears = false;
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            //_rhythmManager.onBeat -= OnBeat;
        }

        private void OnBeat(bool isMeasureBeat)
        {
            StartCoroutine(RotateBigGear());

            if (!_mustSpawnGears) return;

            if (isMeasureBeat)
            {
                GenerateGear(_measureGearPrefab);
                return;
            }

            GenerateGear(_beatGearPrefab);
        }

        private void GenerateGear(GameObject prefab)
        {
            GameObject go = Instantiate(prefab, _gearsContainer);
            RectTransform gearRect = (RectTransform)go.transform;

            // 1) Anclar el engranaje al lado derecho, centrado verticalmente en la barra
            gearRect.anchorMin = new Vector2(1f, 0.5f);
            gearRect.anchorMax = new Vector2(1f, 0.5f);
            gearRect.pivot = new Vector2(0.5f, 0.5f);
            gearRect.localScale = Vector3.one;

            // 2) Offset desde la derecha (en píxeles). 
            //    Por ejemplo, -20 para que quede 20 px dentro de la barra.
            float offsetDesdeDerecha = -20f;
            gearRect.anchoredPosition = new Vector2(offsetDesdeDerecha, 0f);

          //  Debug.Log($"SPAWN GEAR {gearRect.anchoredPosition}");


            BeatGearController beatGearController = gearRect.GetComponent<BeatGearController>();
            if (beatGearController == null)
            {
                Debug.LogError($"GameplayVisualHelpController::GenerateGear The Gear spawned {prefab}, has not BeatGearController component");
                return;
            }
            beatGearController.Init(_targetsPositions, _moveTime, _rotateCantity * _rotateScale, _rhythmManager, Utilities.EasingFunctions.EaseOutQuart);
            beatGearController.Go();
        }

        private IEnumerator RotateBigGear()
        {
            Quaternion originRot = _bigGearGameObject.transform.rotation;
            Quaternion targetRot = originRot * Quaternion.Euler(0f, 0f, _rotateCantity);
            float startZ = originRot.eulerAngles.z;
            float endZ = targetRot.eulerAngles.z;

            float t = 0;
            while (t <= _moveTime)
            {
                float currentZ = Mathf.LerpAngle(startZ, endZ, t / _moveTime);
                t += Time.deltaTime;
                _bigGearGameObject.GetComponent<RectTransform>().localEulerAngles = new Vector3(0f, 0f, currentZ);
                yield return null;
            }
            _bigGearGameObject.GetComponent<RectTransform>().localEulerAngles = new Vector3(0f, 0f, endZ);
        }
    }
}