using UnityEngine;
using Gameplay.RhythmSystem;
using System.Collections;
using UnityEngine.UIElements;
using System;


namespace UI.GameplayUI.VisualHelp
{
    public class BeatGearController : MonoBehaviour
    {
        private Vector3[] _targetsPositions;
        private int _index;
        private float _moveTime;
        private Func<float, float> _easingFunction;

        public void Init(Vector3[] targetsPositions, float moveTime, Func<float, float> easingFunction = null)
        {
            _targetsPositions = targetsPositions;
            _index = 0;
            gameObject.transform.position = _targetsPositions[0];
            _moveTime = moveTime;
            _easingFunction = easingFunction;
            gameObject.SetActive(false);
        }

        public void Go()
        {
            RhythmManager.Instance.onBeat += OnBeat;
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
            while (t <= _moveTime)
            {
                float T;
                if (_easingFunction == null) T = t / _moveTime;
                else T = _easingFunction(t / _moveTime);
                transform.position = originPos * (1 - T) + targetPos * T;
                t += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPos;   // Fix for center final positions

            if (mustFinish)
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            RhythmManager.Instance.onBeat -= OnBeat;
        }
    }
}