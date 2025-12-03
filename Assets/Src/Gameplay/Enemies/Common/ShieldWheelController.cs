using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Enemies.Common
{
    public class ShieldWheelController : MonoBehaviour
    {
        private float _radius;
        private int _maxShields;
        private int _currentShields = 0;
        private float _shieldScaleFactor;
        private float _timeToRotate;
        private float _angleToRotate;
        private GameObject _shieldPrefab;

        private GameObject[] _shields;
        
        public void Init(float radius, int maxShields, float shieldScaleFactor, float timeToRotate, float angleToRotate, GameObject shieldPrefab)
        {
            _radius = radius;
            _maxShields = maxShields;
            _shieldScaleFactor = shieldScaleFactor;
            _timeToRotate =  timeToRotate;
            _angleToRotate = angleToRotate; // degrees
            _shieldPrefab = shieldPrefab;
            
            _shields = new GameObject[_maxShields];

            float angleIncrement = _maxShields / 2.0f * Mathf.PI;
            
            for (int i = 0; i < _maxShields; ++i)
            {
                _shields[i] = GameObject.Instantiate(_shieldPrefab, transform);
                _shields[i].transform.localScale = new Vector3(_shieldScaleFactor, _shieldScaleFactor, 1.0f);
                _shields[i].transform.localPosition = new Vector3(Mathf.Cos(i * angleIncrement), Mathf.Sin(i * angleIncrement), 0.0f) * _radius;
                _shields[i].SetActive(false);
            }
        }

        public void SetCurrentShields(int currentShields)
        {
            if (_currentShields == currentShields) return;
            
            _currentShields = Math.Min(currentShields, _maxShields);
            
            for (int i = 0; i < _currentShields; ++i)
            {
                _shields[i].SetActive(true);
            }

            for (int i = _currentShields; i < _maxShields; ++i)
            {
                _shields[i].SetActive(false);
            }
        }

        public void RotateShields()
        {
            StartCoroutine(RotateShields(Utilities.EasingFunctions.EaseOutQuart));
        }


        private IEnumerator RotateShields(Func<float, float> easingFunction = null)
        {
            
            Vector3[] originPositions = new Vector3[_maxShields];
            float[] originAngles = new float[_maxShields];
            float[] targetAngles = new float[_maxShields]; // Radians
            for (int i = 0; i < _maxShields; ++i)
            {
                originPositions[i] = _shields[i].transform.localPosition;
                originAngles[i] = MathF.Atan2(originPositions[i].y, originPositions[i].x) + Mathf.PI; // currentAngle in the range (0, 2 * PI)
                targetAngles[i] = (originAngles[i] + _angleToRotate * Mathf.Deg2Rad) % (2.0f * Mathf.PI);
            }
            
            float T;
            float t = 0;
            while (t <= _timeToRotate)
            {
                if (easingFunction == null) T = t / _timeToRotate;
                else T = easingFunction(t / _timeToRotate);
                for (int i = 0; i < _maxShields; ++i)
                {
                    float currentAngle = Mathf.LerpAngle(originAngles[i] * Mathf.Rad2Deg, targetAngles[i] * Mathf.Rad2Deg, T) * Mathf.Deg2Rad; // Radians
                    _shields[i].transform.localPosition = new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle), _shields[i].transform.localPosition.z) * _radius;
                    // Rotate to look to the center
                    float angleDeg = (Mathf.Atan2(_shields[i].transform.localPosition.y, _shields[i].transform.localPosition.x) + Mathf.PI) * Mathf.Rad2Deg;
                    _shields[i].transform.localEulerAngles = new Vector3(0, 0, angleDeg);
                }
                t += Time.deltaTime;
                yield return null;
            }

            for (int i = 0; i < _maxShields; ++i)
            {
                _shields[i].transform.localPosition = new Vector3(Mathf.Cos(targetAngles[i]), Mathf.Sin(targetAngles[i]), _shields[i].transform.localPosition.z) * _radius;
                // look to the center
                float angleDeg = (Mathf.Atan2(_shields[i].transform.localPosition.y, _shields[i].transform.localPosition.x) + Mathf.PI) * Mathf.Rad2Deg;
                _shields[i].transform.localEulerAngles = new Vector3(0, 0, angleDeg);
            }
        }
    }
}