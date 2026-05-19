using UnityEngine;

namespace Mako.Movement
{
    public class EnemyVehicle : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private float rotationSpeed = 180f;
        private Rigidbody enemyRigidbody;
        private float translation, rotation;
        private GameObject target;
        private void Awake()
        {
            enemyRigidbody = GetComponent<Rigidbody>();
            target = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            if (!target)
                return;

            Vector3 targetDirection = transform.position - target.transform.position;
            targetDirection.Normalize();

            rotation = Vector3.Cross(targetDirection, transform.forward).y;
        }

        private void FixedUpdate()
        {
            enemyRigidbody.angularVelocity = rotationSpeed * rotation * new Vector3(0, 1, 0) * Time.deltaTime;
            enemyRigidbody.velocity = transform.forward * movementSpeed;
        }
    }
}
