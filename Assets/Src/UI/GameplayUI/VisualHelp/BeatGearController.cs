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
        private Vector2[] _targetsPositions;
        private int _index;
        private float _moveTime;
        private float _rotateCantity = 25.0f; // in Degrees
        private Func<float, float> _easingFunction;

        private RhythmManager _rhythmManager;

        public void Init(Vector2[] targetsPositions, float moveTime, float rotateCantity, RhythmManager rhythmManager, Func<float, float> easingFunction = null)
        {
            /*GameplayVisualHelpController[] found = FindObjectsOfType<GameplayVisualHelpController>();

            foreach (var s in found)
            {
                Debug.Log("El script está en: " + s.gameObject.name);
            }*/
            //Debug.Log("EL TARGET POSITION ES: " + targetsPositions);
            _targetsPositions = targetsPositions;
            _index = 0;
            RectTransform rect = GetComponent<RectTransform>();
            rect.anchoredPosition = _targetsPositions[0];
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
            /* _index++;
             Vector2 target = _targetsPositions[_index];

             bool mustFinish = _index == _targetsPositions.Length - 1;

             Debug.Log($"BEAT index={_index}  target={target}  mustFinish={mustFinish}");
             StartCoroutine(Move(_targetsPositions[++_index], _index == _targetsPositions.Length - 1 ? true : false));*/
            if(_index >= _targetsPositions.Length - 1)
        return; // ya ha terminado el recorrido

            _index++;

            Vector2 target = _targetsPositions[_index];
            bool mustFinish = _index == _targetsPositions.Length - 1;

            StartCoroutine(Move(target, mustFinish));
        }

        private IEnumerator Move(Vector2 targetPos, bool mustFinish)
        {
            /* RectTransform rect = GetComponent<RectTransform>();

             // Posición inicial en UI
             Vector2 originPos = rect.anchoredPosition;
             float fixedY = originPos.y;   // Esta Y no va a cambiar

             float t = 0.0f;

             Quaternion originRot = _gearToRotate.transform.rotation;
             Quaternion targetRot = originRot * Quaternion.Euler(0f, 0f, _rotateCantity);

             while (t <= _moveTime)
             {
                 float T = (_easingFunction == null) ? (t / _moveTime) : _easingFunction(t / _moveTime);

                 // Solo interpolamos la X
                 float newX = Mathf.Lerp(originPos.x, targetPos.x, T);

                 // Y siempre es la misma
                 rect.anchoredPosition = new Vector2(newX, fixedY);

                 Rotate(originRot, targetRot, t / _moveTime);

                 t += Time.deltaTime;
                 yield return null;
             }
             Debug.Log("SE ELIMINA: " + mustFinish);

             // Forzamos posición final correcta
             rect.anchoredPosition = new Vector2(targetPos.x, fixedY);

             if (mustFinish)
             {
                 Destroy(gameObject);
             }*/
            RectTransform rect = GetComponent<RectTransform>();

            Vector3 originPos3 = rect.localPosition;
            float fixedY = originPos3.y;
            float fixedZ = originPos3.z;

            float t = 0.0f;

            Quaternion originRot = _gearToRotate.transform.rotation;
            Quaternion targetRot = originRot * Quaternion.Euler(0f, 0f, _rotateCantity);

            while (t <= _moveTime)
            {
                float T = (_easingFunction == null) ? (t / _moveTime) : _easingFunction(t / _moveTime);

                float newX = Mathf.Lerp(originPos3.x, targetPos.x, T);
                rect.localPosition = new Vector3(newX, fixedY, fixedZ);

                Rotate(originRot, targetRot, t / _moveTime);

                t += Time.deltaTime;
                yield return null;
            }

            rect.localPosition = new Vector3(targetPos.x, fixedY, fixedZ);

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