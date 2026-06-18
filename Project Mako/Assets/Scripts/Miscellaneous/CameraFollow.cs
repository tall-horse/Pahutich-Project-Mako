using UnityEngine;

namespace Mako.Miscellaneous
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _offset;
        [Header("Smoothing")]
        [SerializeField] private bool _smoothing;
        [SerializeField] private float _smoothTime = 0.15f;

        private Vector3 _velocity;

        private void LateUpdate()
        {
            Vector3 desired = _target.position + _offset;

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