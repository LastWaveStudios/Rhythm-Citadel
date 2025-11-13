using UnityEngine;
using Gameplay.RhythmSystem;
using System.Collections;
using UnityEngine.UIElements;
using System;


namespace UI.GameplayUI.VisualHelp
{
    public class BeatGearController : MonoBehaviour
    {
        [SerializeField] private GameObject _gearToRotate;
        private Vector3[] _targetsPositions;
        private int _index;
        private float _moveTime;
        private float _rotateCantity = 25.0f; // in Degrees
        private Func<float, float> _easingFunction;

        private RhythmManager _rhythmManager;

        public void Init(Vector3[] targetsPositions, float moveTime, float rotateCantity, RhythmManager rhythmManager, Func<float, float> easingFunction = null)
        {
            _targetsPositions = targetsPositions;
            _index = 0;
            gameObject.transform.position = _targetsPositions[0];
            _moveTime = moveTime;
            _rotateCantity = rotateCantity;
            _easingFunction = easingFunction;
            gameObject.SetActive(false);
            _rhythmManager = rhythmManager;
        }

        public void Go()
        {
            _rhythmManager.onBeat += OnBeat;
            gameObject.SetActive(true);
        }

        private void OnBeat(bool isMeasureBeat)
        {
            StartCoroutine(Move(_targetsPositions[++_index], _index == _targetsPositions.Length - 1? true : false));
        }

        private IEnumerator Move(Vector3 targetPos, bool mustFinish)
        {
            Vector3 originPos = transform.position;
            float t = 0.0f;
            Quaternion originRot = _gearToRotate.transform.rotation;
            Quaternion targetRot = originRot * Quaternion.Euler(0f, 0f, _rotateCantity);
            while (t <= _moveTime)
            {
                float T;
                if (_easingFunction == null) T = t / _moveTime;
                else T = _easingFunction(t / _moveTime);
                transform.position = originPos * (1 - T) + targetPos * T;
                Rotate(originRot, targetRot, t / _moveTime);
                t += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPos;   // Fix for center final positions

            if (mustFinish)
            {
                Destroy(gameObject);
            }
        }

        private void Rotate(Quaternion originRot, Quaternion targetRot, float T)
        {
            float startZ = originRot.eulerAngles.z;
            float endZ = targetRot.eulerAngles.z;
            float currentZ = Mathf.LerpAngle(startZ, endZ, T);
            _gearToRotate.GetComponent<RectTransform>().localEulerAngles = new Vector3(0f, 0f, currentZ);
        }

        private void OnDestroy()
        {
            _rhythmManager.onBeat -= OnBeat;
        }
    }
}