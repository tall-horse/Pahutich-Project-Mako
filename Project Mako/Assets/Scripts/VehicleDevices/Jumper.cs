using System.Collections;
using System.Collections.Generic;
using Mako.Input;
using Mako.Movement;
using UnityEngine;

namespace Mako.VehicleDevices
{
    public class Jumper : MonoBehaviour
    {
        [Header("Jump")]
        [SerializeField] private float _desiredJumpHeight = 5f;
        [SerializeField] private float _jumpCooldown = 1f;
        [SerializeField] private float _chargeDuration = 0.2f;
        [SerializeField] private AnimationCurve _jumpCurve;
        [SerializeField] private float _curveDuration = 0.2f;
        [SerializeField] private float _jumForce = 2500000f;

        [Header("Fuel")]
        [SerializeField] private float _jumpFuelCurrent;
        [SerializeField] private float _jumpFuelMax = 100f;
        [SerializeField] private float _fuelRegenerationRate = 10f;
        [SerializeField] private float _fuelConsumptionPerJump = 20f;

        [Header("Effects")]
        [SerializeField] private List<ParticleSystem> enginesVisuals;
        [SerializeField] private AudioSource _audioSource;

        private Rigidbody _jumpingRigidbody;
        private InputManager _inputManager;
        private PlayerController _playerController;

        private float _cooldownTimer = 0f;
        private bool _wasJumpPressed = false;
        private bool _isCharging = false;

        void Awake()
        {
            if (_jumpingRigidbody == null)
                _jumpingRigidbody = GetComponent<Rigidbody>();

            if (_jumpingRigidbody == null)
            {
                Debug.LogError("Jumper: No Rigidbody found. Please attach one or assign it in the inspector.");
                return;
            }

            if (_jumpingRigidbody.isKinematic)
            {
                Debug.LogWarning("Jumper: Rigidbody is kinematic; disabling for jump.");
                _jumpingRigidbody.isKinematic = false;
            }
            if (!_jumpingRigidbody.useGravity)
            {
                Debug.LogWarning("Jumper: Rigidbody useGravity is off. Enabling it.");
                _jumpingRigidbody.useGravity = true;
            }
            _jumpFuelCurrent = _jumpFuelMax;
            enginesVisuals.ForEach(e => e.Stop());
        }

        /// <summary>
        /// Call this from your vehicle controller after you have wired up the components.
        /// </summary>
        public void Initialize(InputManager input, PlayerController player,
                               Rigidbody rb, AudioSource audio)
        {
            _inputManager = input;
            _playerController = player;
            _jumpingRigidbody = rb ?? _jumpingRigidbody;
            _audioSource = audio;

            if (_jumpingRigidbody != null)
            {
                if (_jumpingRigidbody.isKinematic) _jumpingRigidbody.isKinematic = false;
                if (!_jumpingRigidbody.useGravity) _jumpingRigidbody.useGravity = true;
            }
        }

        void Update()
        {
            if (_cooldownTimer > 0f) _cooldownTimer -= Time.deltaTime;

            bool jumpPressed = _inputManager.Actions.Player.Jump.ReadValue<float>() > 0.1f;

            if (jumpPressed && !_wasJumpPressed)
            {
                bool canJump = _cooldownTimer <= 0f &&
                               _jumpFuelCurrent >= _fuelConsumptionPerJump &&
                               _playerController.IsGrounded() &&
                               !_isCharging;

                if (canJump)
                {
                    _isCharging = true;
                    StartCoroutine(ChargeAndJump());
                }
            }

            _wasJumpPressed = jumpPressed;
        }

        /// <summary>
        /// Optional charge delay, then perform the actual jump.
        /// </summary>
        private IEnumerator ChargeAndJump()
        {
            if (_chargeDuration > 0f)
            {
                float t = 0f;
                while (t < _chargeDuration)
                {
                    enginesVisuals.ForEach(e => e.Play());
                    t += Time.deltaTime;
                    yield return null;
                }
            }

            ActivateJump();

            _isCharging = false;
        }

        private void ActivateJump()
        {
            float gravity = Mathf.Abs(Physics.gravity.y);
            float initSpeed = Mathf.Sqrt(2f * gravity * _desiredJumpHeight);

            if (_jumpCurve != null && _jumpCurve.length > 0)
                StartCoroutine(ApplyJumpForceOverTime(_jumForce));
            else
                _jumpingRigidbody.AddForce(Vector3.up * _jumForce, ForceMode.Impulse);

            enginesVisuals.ForEach(e => e.Play());
            if (!_audioSource.isPlaying) _audioSource.Play();

            _jumpFuelCurrent -= _fuelConsumptionPerJump;
            _jumpFuelCurrent = Mathf.Max(0f, _jumpFuelCurrent);
            _cooldownTimer = _jumpCooldown;
        }

        private IEnumerator ApplyJumpForceOverTime(float impulse)
        {
            float t = 0f;
            while (t < _curveDuration)
            {
                float factor = _jumpCurve.Evaluate(t / _curveDuration);
                Vector3 force = Vector3.up * impulse * factor;
                _jumpingRigidbody.AddForce(force, ForceMode.Force);
                t += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(0.75f);
            enginesVisuals.ForEach(e => e.Stop());
        }

        void FixedUpdate()
        {
            if (_playerController.IsGrounded() && !_isCharging)
                _jumpFuelCurrent = Mathf.Min(_jumpFuelMax,
                                             _jumpFuelCurrent + Time.fixedDeltaTime * _fuelRegenerationRate);
        }
    }
}
