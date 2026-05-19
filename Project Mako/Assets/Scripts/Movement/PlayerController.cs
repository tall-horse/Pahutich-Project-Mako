using System;
using UnityEngine;
using TMPro;
using Mako.Miscellaneous;

namespace Mako.Movement
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float enginePower;
        [SerializeField] private float brakePower;
        [SerializeField] private float steerAngle;
        [SerializeField] private Transform centerOfMass;
        [SerializeField] private TextMeshProUGUI speedText;
        [SerializeField] private float airSteeringForce;
        [SerializeField] private float turnCarAfterXSeconds = 3f;
        [SerializeField] private Vector3 min;
        [SerializeField] private Vector3 max;
        [SerializeField] private GameObject weaponHolder;
        [SerializeField] private WheelCollider[] wheelColliders = new WheelCollider[6];
        private float speed = 0f;
        private float carUpsideDownTimer = 0f;
        private Rigidbody playerRigidbody;
        private CameraFollow cameraFollow;
        private AudioSource audioSource;
        //private OverheatBar overheatBar;
        Vector2 inputVector = Vector2.zero;
        private PlayerInputActions playerInputActions;
        //public event Action <Shooter> OnOverheatableWeaponSet;
        private void Awake()
        {
            playerInputActions = new PlayerInputActions();
            playerInputActions.Player.Enable();
            playerRigidbody = GetComponent<Rigidbody>();
            cameraFollow = FindObjectOfType<CameraFollow>();
            audioSource = GetComponent<AudioSource>();
            //overheatBar = FindObjectOfType<OverheatBar>();
        }

        // private void OnEnable()
        // {
        //   if(Inventory.instance == null)
        //   {
        //     Inventory.instance = FindObjectOfType(typeof(Inventory)) as Inventory;
        //   }
        //   GameObject primaryWeapon = Instantiate(Inventory.instance.GetPrimaryWeapon().gameObject,
        //   weaponHolder.transform.position, Quaternion.identity);
        //   GameObject secondaryWeapon = Instantiate(Inventory.instance.GetSecondaryWeapon().gameObject,
        //   weaponHolder.transform.position, Quaternion.identity);
        //   primaryWeapon.transform.parent = weaponHolder.transform;
        //   secondaryWeapon.transform.parent = weaponHolder.transform;
        // }
        private void Start()
        {
            playerRigidbody.centerOfMass = centerOfMass.transform.localPosition;
        }
        private void Update()
        {
            inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
            if (playerRigidbody != null)
            {
                speed = playerRigidbody.velocity.magnitude * 3.6f;
                speedText.text = "Speed: " + speed;
            }
            if (inputVector.y != 0)
            {
                if (!audioSource.isPlaying)
                    audioSource.Play();
            }
            else
                audioSource.Stop();
            ClampPosition();
        }

        private void ClampPosition()
        {
            float clampedX = Mathf.Clamp(transform.position.x, min.x, max.x);
            float clampedY = Mathf.Clamp(transform.position.y, min.y, max.y);
            float clampedZ = Mathf.Clamp(transform.position.z, min.z, max.z);

            transform.position = new Vector3(clampedX, clampedY, clampedZ);
        }

        void FixedUpdate()
        {

            CheckMotor(inputVector.y, inputVector.x);
            CheckSteer(inputVector.y, inputVector.x);
            CheckAir(inputVector.y, inputVector.x);
            CheckCarDown();
        }

        private void CheckMotor(float verticalInput, float horizontalInput)
        {
            RotateWheelsFB(verticalInput);
            BreakWheels(verticalInput, horizontalInput);
        }

        private void CheckAir(float verticalInput, float horizontalInput)
        {
            if (IsGrounded())
                return;

            Quaternion verticalAirRotation = Quaternion.AngleAxis(verticalInput * 40f, Vector3.right);
            Quaternion targetVerticalRotation = playerRigidbody.rotation * verticalAirRotation;

            playerRigidbody.MoveRotation(Quaternion.Lerp(playerRigidbody.rotation, targetVerticalRotation, 2.0f * Time.deltaTime));

            Quaternion horizontalAirRotation = Quaternion.AngleAxis(horizontalInput * 40f, Vector3.back);
            Quaternion targetHorizontalRotation = playerRigidbody.rotation * horizontalAirRotation;

            playerRigidbody.MoveRotation(Quaternion.Lerp(playerRigidbody.rotation, targetHorizontalRotation, 2.0f * Time.deltaTime));
        }

        private void BreakWheels(float verticalInput, float horizontalInput)
        {
            if (speed > 0.01f && (verticalInput == 0 && horizontalInput == 0) && Mathf.Abs(playerRigidbody.velocity.z) > 0.01f ||
            (playerRigidbody.transform.InverseTransformDirection(playerRigidbody.velocity).z >= 0.1f && verticalInput < 0 ||
            playerRigidbody.transform.InverseTransformDirection(playerRigidbody.velocity).z <= -0.1f && verticalInput > 0))
            {
                for (int i = 0; i < wheelColliders.Length; i++)
                {
                    if (wheelColliders[i] != null)
                        wheelColliders[i].brakeTorque = brakePower;
                }
            }
            else
            {
                for (int i = 0; i < wheelColliders.Length; i++)
                {
                    if (wheelColliders[i] != null)
                        wheelColliders[i].brakeTorque = 0;
                }
            }
        }

        private void RotateWheelsFB(float verticalInput)
        {
            for (int i = 0; i < wheelColliders.Length; i++)
            {
                if (wheelColliders[i] != null)
                    wheelColliders[i].motorTorque = enginePower * verticalInput;
            }
        }

        private void CheckSteer(float verticalInput, float horizontalInput)
        {
            if (wheelColliders[0] != null && wheelColliders[1] != null)
            {
                wheelColliders[0].steerAngle = steerAngle * horizontalInput;
                wheelColliders[1].steerAngle = steerAngle * horizontalInput;
            }
        }
        private void CheckCarDown()
        {
            if (Vector3.Dot(transform.up, Vector3.down) > 0 && !IsGrounded() && playerRigidbody.velocity.magnitude <= Math.Abs(0.1))
            {
                carUpsideDownTimer += Time.deltaTime;
                if (carUpsideDownTimer >= turnCarAfterXSeconds)
                {
                    transform.rotation = Quaternion.identity;
                    playerRigidbody.velocity = Vector3.zero;
                    playerRigidbody.angularVelocity = Vector3.zero;
                }
            }
            else
            {
                carUpsideDownTimer = 0f;
            }
        }

        public bool IsGrounded()
        {
            foreach (var item in wheelColliders)
            {
                if (item.isGrounded)
                    return true;
            }
            return false;
        }
    }
}
