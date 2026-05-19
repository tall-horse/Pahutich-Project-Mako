using Mako.Movement;
using UnityEngine;

namespace Mako.VehicleDevices
{
    public class Stabilizer : MonoBehaviour
    {
        private PlayerController playerController;
        private Rigidbody rb;
        [SerializeField] private float stability;
        [SerializeField] private float speed;
        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
        }
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            if (rb != null && !playerController.IsGrounded())
            {
                Vector3 predictedUp = Quaternion.AngleAxis(rb.angularVelocity.magnitude * Mathf.Rad2Deg * stability / speed, rb.angularVelocity) * transform.up;
                Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
                rb.AddTorque(torqueVector * speed * speed);
            }
        }
    }
}
