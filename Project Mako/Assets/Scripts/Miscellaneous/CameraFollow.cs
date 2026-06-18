using Mako.VehicleDevices;
using UnityEngine;

namespace Mako.Miscellaneous
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private Transform _target;
        [SerializeField] private Scanner _scanner;
        [SerializeField] private Vector3 _offset;

        [Header("Smoothing")]
        [SerializeField] private bool _smoothing;
        [SerializeField] private float _smoothTime = 0.15f;

        [Header("Look Ahead")]
        [SerializeField] private bool _lookAhead;
        [SerializeField] private float _lookAheadStrength = 0.25f;
        [SerializeField] private float _lookAheadMaxDistance = 5f;

        private Vector3 _velocity;

        private void LateUpdate()
        {
            Vector3 desired = _target.position + _offset;

            if (_lookAhead)
            {
                Vector3 aimOffset = _scanner.AimPoint - _target.position;
                aimOffset *= _lookAheadStrength;
                aimOffset = Vector3.ClampMagnitude(aimOffset, _lookAheadMaxDistance);
                desired += aimOffset;
            }

            if (_smoothing == false)
            {
                transform.position = desired;
                _velocity = Vector3.zero;
                return;
            }

            transform.position = Vector3.SmoothDamp(transform.position, desired, ref _velocity, _smoothTime);
        }
    }
}