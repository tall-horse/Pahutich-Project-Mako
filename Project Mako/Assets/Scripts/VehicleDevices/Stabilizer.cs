using Mako.Movement;
using UnityEngine;

namespace Mako.VehicleDevices
{
    public class Stabilizer : MonoBehaviour
    {
        private PlayerController _playerController;
        private Rigidbody _playerRigidbody;
        [SerializeField] private float stability;
        [SerializeField] private float speed;
        public void Initialize(PlayerController playerController, Rigidbody rigidbody)
        {
            _playerController = playerController;
            _playerRigidbody = rigidbody;
        }
        void FixedUpdate()
        {
            if (_playerRigidbody != null && !_playerController.IsGrounded())
            {
                Vector3 predictedUp = Quaternion.AngleAxis(_playerRigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * stability / speed, _playerRigidbody.angularVelocity) * transform.up;
                Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
                _playerRigidbody.AddTorque(torqueVector * speed * speed);
            }
        }
    }
}
