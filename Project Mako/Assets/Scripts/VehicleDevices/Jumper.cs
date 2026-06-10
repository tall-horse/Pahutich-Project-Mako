using System.Collections.Generic;
using Mako.Input;
using Mako.Movement;
using UnityEngine;

namespace Mako.VehicleDevices
{
    public class Jumper : MonoBehaviour
    {
        private bool isJumping = false;
        private Rigidbody _jumpingRigidbody;
        private AudioSource _audioSource;
        private PlayerController _playerController;
        private InputManager _inputManager;
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float jumpFuelMax;
        [SerializeField] private float fuelRegenerationAbility;
        [SerializeField] private float jumpFuelCurrent;
        [SerializeField] private List<ParticleSystem> enginesVisuals;
        private void Awake()
        {
            jumpFuelCurrent = jumpFuelMax;
            enginesVisuals.ForEach(e => e.Stop());
        }
        public void Initialize(InputManager inputManager, PlayerController playerController, Rigidbody rigidbody, AudioSource audioSource)
        {
            _inputManager = inputManager;
            _playerController = playerController;
            _jumpingRigidbody = rigidbody;
            _audioSource = audioSource;
        }

        // Update is called once per frame
        void Update()
        {
            bool jumpInputActivated = _inputManager.Actions.Player.Jump.ReadValue<float>() > 0.1f;
            bool hasSufficientAmountOfFuel = jumpFuelCurrent > 0;

            if (jumpInputActivated && hasSufficientAmountOfFuel)
            {
                isJumping = true;
            }
            else
            {
                isJumping = false;
            }

            if (jumpFuelCurrent > jumpFuelMax)
                jumpFuelCurrent = jumpFuelMax;
            if (jumpFuelCurrent < 0)
                jumpFuelCurrent = 0;
        }
        private void FixedUpdate()
        {
            if (isJumping)
            {
                ActivateJump();
            }
            else
            {
                DeactivateJump();
            }
        }
        private void ActivateJump()
        {
            Vector3 jumpVector = transform.up * jumpForce;
            _jumpingRigidbody.AddForce(jumpVector, ForceMode.Impulse);
            enginesVisuals.ForEach(e => e.Play());
            jumpFuelCurrent -= Time.deltaTime * fuelRegenerationAbility;
            if (!_audioSource.isPlaying)
                _audioSource.Play();
        }

        private void DeactivateJump()
        {
            isJumping = false;
            enginesVisuals.ForEach(e => e.Stop());
            if (_playerController.IsGrounded())
                jumpFuelCurrent += Time.deltaTime * fuelRegenerationAbility;
            if (_audioSource.isPlaying)
                _audioSource.Stop();
        }
    }
}
