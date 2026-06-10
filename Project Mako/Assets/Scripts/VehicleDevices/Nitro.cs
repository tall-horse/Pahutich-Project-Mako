using System.Collections.Generic;
using Mako.Input;
using UnityEngine;

namespace Mako.VehicleDevices
{
    public class Nitro : MonoBehaviour
    {
        private bool doingNitro = false;
        private Rigidbody playerRigidbody;
        private AudioSource audioSource;
        private InputManager _inputManager;
        [SerializeField] private float nitroForce = 10f;
        [SerializeField] private float nitroFuelMax;
        [SerializeField] private float nitroRegenerationAbility;
        [SerializeField] private float nitroFuelCurrent;
        [SerializeField] private List<ParticleSystem> enginesVisuals;
        private void Awake()
        {
            playerRigidbody = GetComponentInParent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();
            nitroFuelCurrent = nitroFuelMax;
            enginesVisuals.ForEach(e => e.Stop());
        }

        private void Start()
        {
            _inputManager = InputManager.Instance;
        }
        // Update is called once per frame
        void Update()
        {
            bool isPressingNitro = _inputManager.Actions.Player.Nitro.ReadValue<float>() > 0.1f;
            bool hasSuffientAmountOfFuel = nitroFuelCurrent > 0;
            doingNitro = isPressingNitro && hasSuffientAmountOfFuel;
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
            nitroFuelCurrent += Time.deltaTime * nitroRegenerationAbility;
            if (audioSource.isPlaying)
                audioSource.Stop();
        }

        private void ActivateNitro()
        {
            Vector3 nitroVector = transform.forward * nitroForce;
            playerRigidbody.AddForce(nitroVector, ForceMode.Acceleration);
            enginesVisuals.ForEach(e => e.Play());
            nitroFuelCurrent -= Time.deltaTime * nitroRegenerationAbility;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }
}