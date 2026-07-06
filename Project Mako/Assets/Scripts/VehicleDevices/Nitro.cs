using System.Collections.Generic;
using Mako.Input;
using Mako.State;
using UnityEngine;

namespace Mako.VehicleDevices
{
    public class Nitro : MonoBehaviour
    {
        private bool doingNitro = false;
        private Rigidbody _playerRigidbody;
        private AudioSource _audioSource;
        private bool _isPressingNitro;
        [SerializeField] private float nitroForce = 10f;
        [SerializeField] private float nitroFuelMax;
        [SerializeField] private float nitroRegenerationAbility;
        [SerializeField] private float nitroFuelCurrent;
        [SerializeField] private List<ParticleSystem> enginesVisuals;
        private void Awake()
        {
            nitroFuelCurrent = nitroFuelMax;
            enginesVisuals.ForEach(e => e.Stop());
        }

        public void Initialize(Rigidbody rigidbody, AudioSource audioSource)
        {
            _playerRigidbody = rigidbody;
            _audioSource = audioSource;
        }
        // Update is called once per frame
        void Update()
        {
            if (GameManager.GameIsPaused == true) return;

            _isPressingNitro = InputManager.Instance.actions.Player.Nitro.ReadValue<float>() > 0.1f;
            bool hasSuffientAmountOfFuel = nitroFuelCurrent > 0;
            doingNitro = _isPressingNitro && hasSuffientAmountOfFuel;
            if (doingNitro)
            {
                ActivateNitro();
            }
            else
            {
                DeactivateNitro();
            }

            if (nitroFuelCurrent > nitroFuelMax)
                nitroFuelCurrent = nitroFuelMax;
            if (nitroFuelCurrent < 0)
                nitroFuelCurrent = 0;
        }

        private void DeactivateNitro()
        {
            enginesVisuals.ForEach(e => e.Stop());
            if (_isPressingNitro == false)
            {
                nitroFuelCurrent += Time.deltaTime * nitroRegenerationAbility;
            }
            if (_audioSource.isPlaying)
                _audioSource.Stop();
        }

        private void ActivateNitro()
        {
            Vector3 nitroVector = transform.forward * nitroForce;
            _playerRigidbody.AddForce(nitroVector, ForceMode.Acceleration);
            enginesVisuals.ForEach(e => e.Play());
            nitroFuelCurrent -= Time.deltaTime * nitroRegenerationAbility;
            if (!_audioSource.isPlaying)
                _audioSource.Play();
        }
    }
}