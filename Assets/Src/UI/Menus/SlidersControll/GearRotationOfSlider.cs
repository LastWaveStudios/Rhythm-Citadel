using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Menus.SlidersControll
{
    public class GearRotationOfSlider : MonoBehaviour
    {
        [SerializeField] private SliderController _sliderController;

        [Header("Gears references")]
        [SerializeField] private GameObject _bigGear;
        [SerializeField] private GameObject _middelGear;
        [SerializeField] private GameObject _smallGear;

        private void Start()
        {
            _sliderController.onSliderChange += Rotate;
            _sliderController.SubscribeToInitialize(Rotate);
        }

        private void Rotate(float sliderValue)
        {
            float targetAngle = sliderValue * 360.0f;
            RotateGear(_bigGear, targetAngle);
            RotateGear(_middelGear, -targetAngle);
            RotateGear(_smallGear, targetAngle);
        }

        private void RotateGear(GameObject gear, float targetAngle)
        {
            gear.transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
        }

        private void OnDestroy()
        {
            _sliderController.onSliderChange -= Rotate;
        }
    }
}