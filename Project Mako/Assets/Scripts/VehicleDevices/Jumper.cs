using System.Collections;
using System.Collections.Generic;
using Mako.Input;
using Mako.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mako.VehicleDevices
{
    /// <summary>
    /// Handles an instant or curve‑based “jump” for an armoured vehicle.
    /// </summary>
    public class Jumper : MonoBehaviour
    {
        [Header("Jump")]
        [Tooltip("Target height (in meters) that a single jump should reach.")]
        [SerializeField] private float _desiredJumpHeight = 5f;

        [Range(0.1f, 10f)]
        [Tooltip(
            "Multiplies the calculated jump impulse.\n" +
            "1 = exactly the height you set above.\n" +
            "2 = twice as high (double the velocity).")]
        [SerializeField] private float _jumpForceMultiplier = 1f;

        [Tooltip("If >0, this value overrides the calculated impulse.")]
        [SerializeField] private float _customJumpImpulse = -1f;

        [Header("Timing")]
        [SerializeField] private float _jumpCooldown = 1f;
        [SerializeField] private float _chargeDuration = 0.2f;
        [Tooltip("How long after the key is pressed that the engine particles should stop.")]
        [SerializeField] private float _particleStopDelay = 0.75f;

        [Header("Curve for physics")]
        [Tooltip(
            "If assigned, forces will be applied over time according to this curve.\n" +
            "The curve is evaluated from t=0 → 1 and the integral of the curve\n" +
            "must equal 1 for a stable jump height.")]
        [SerializeField] private AnimationCurve _jumpCurve;
        [Tooltip("Duration over which the curve will be applied (seconds).")]
        [SerializeField] private float _curveDuration = 0.2f;

        [Header("Fuel")]
        [Tooltip("Maximum amount of fuel the vehicle can hold.")]
        [SerializeField] private float _jumpFuelMax = 100f;
        [Tooltip("Current amount of fuel the vehicle holds.")]
        [SerializeField] private float _jumpFuelCurrent;
        [Tooltip("Rate at which fuel regenerates per second when grounded.")]
        [SerializeField] private float _fuelRegenerationRate = 10f;
        [Tooltip("Amount of fuel consumed by a single jump.")]
        [SerializeField] private float _fuelConsumptionPerJump = 20f;

        [Header("Effects")]
        [Tooltip("Particle systems that play while the vehicle is jumping.")]
        [SerializeField] private List<ParticleSystem> enginesVisuals;
        [Tooltip("Audio source that plays when a jump occurs.")]
        [SerializeField] private AudioSource _audioSource;

        private Rigidbody _jumpingRigidbody;
        private InputManager _inputManager;
        private PlayerController _playerController;

        private float _cooldownTimer = 0f;

        private bool _wasJumpPressed = false;
        private bool _isCharging = false;

        [Header("Advanced")]
        [Tooltip(
            "If true, the script will set the vertical velocity directly.\n" +
            "If false and a curve is supplied, it will use the curve for physics.")]
        [SerializeField] private bool _useVelocityChange = true;

        void Awake()
        {
            if (_jumpingRigidbody == null)
                _jumpingRigidbody = GetComponent<Rigidbody>();

            if (_jumpingRigidbody == null)
            {
                Debug.LogError("Jumper: No Rigidbody found. Attach one or assign it in the inspector.");
                return;
            }

            if (_jumpingRigidbody.isKinematic) _jumpingRigidbody.isKinematic = false;
            if (!_jumpingRigidbody.useGravity) _jumpingRigidbody.useGravity = true;

            _jumpFuelCurrent = _jumpFuelMax;
            enginesVisuals.ForEach(e => e.Stop());
        }
        private void OnEnable()
        {
            InputManager.Instance.actions.Player.Jump.started += ControlJump;
        }
        private void OnDisable()
        {

        }
        /// <summary>
        /// Call this from your vehicle controller after you have wired up the components.
        /// </summary>
        public void Initialize(PlayerController player,
                               Rigidbody rb, AudioSource audio)
        {
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
        }

        private void ControlJump(InputAction.CallbackContext obj)
        {
            if (!_wasJumpPressed)
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
            if (_jumpingRigidbody == null)
            {
                Debug.LogError("Jumper: No Rigidbody assigned – jump aborted.");
                return;
            }

            float gravity = Mathf.Abs(Physics.gravity.y);
            float deltaV = Mathf.Sqrt(2f * gravity * _desiredJumpHeight);

            if (_customJumpImpulse > 0f)
                deltaV = _customJumpImpulse / _jumpingRigidbody.mass;
            else
                deltaV *= _jumpForceMultiplier;

            if (_useVelocityChange || !_applyCurveForPhysics)
            {
                Vector3 newVel = _jumpingRigidbody.velocity;
                newVel.y = deltaV;
                _jumpingRigidbody.velocity = newVel;
            }
            else
            {
                float baseImpulse = deltaV * _jumpingRigidbody.mass;

                StartCoroutine(ApplyJumpForceOverTime(baseImpulse));
            }

            enginesVisuals.ForEach(e => e.Play());
            if (!_audioSource.isPlaying) _audioSource.Play();

            _jumpFuelCurrent -= _fuelConsumptionPerJump;
            _jumpFuelCurrent = Mathf.Max(0f, _jumpFuelCurrent);
            _cooldownTimer = _jumpCooldown;

            //Debug.Log($"[Jumper] Mass={_jumpingRigidbody.mass:F2}  Δv={deltaV:F2}  Height≈{Mathf.Pow(deltaV, 2) / (2f * gravity):F2} m");

            StartCoroutine(StopParticlesAfterDelay(_particleStopDelay));
        }

        /// <summary>
        /// If you supplied a curve, this coroutine applies a gradually increasing force
        /// over _curveDuration seconds.  The integration is done in FixedUpdate so the
        /// total impulse is stable regardless of frame‑rate.
        /// </summary>
        private IEnumerator ApplyJumpForceOverTime(float baseImpulse)
        {
            float t = 0f;
            while (t < _curveDuration)
            {
                float factor = _jumpCurve.Evaluate(t / _curveDuration);
                Vector3 force = transform.up * baseImpulse * factor;
                _jumpingRigidbody.AddForce(force, ForceMode.Force);
                t += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator StopParticlesAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            enginesVisuals.ForEach(e => e.Stop());
        }

        void FixedUpdate()
        {
            if (_playerController.IsGrounded() && !_isCharging)
                _jumpFuelCurrent = Mathf.Min(_jumpFuelMax,
                                             _jumpFuelCurrent + Time.fixedDeltaTime * _fuelRegenerationRate);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (_jumpingRigidbody == null) return;

            float gravity = Mathf.Abs(Physics.gravity.y);
            float deltaV = Mathf.Sqrt(2f * gravity * _desiredJumpHeight) * _jumpForceMultiplier;
            float expectedHeight = (deltaV * deltaV) / (2f * gravity);

            UnityEditor.EditorUtility.SetDirty(this);
            Debug.Log($"[Jumper] Expected max height with current settings: {expectedHeight:F2} m");
        }
#endif
        /// <summary>
        /// True if a curve is supplied and we should use it for physics.
        /// </summary>
        private bool _applyCurveForPhysics => _jumpCurve != null && _jumpCurve.length > 0;
    }
}
