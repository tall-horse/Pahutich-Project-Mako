using System.Collections;
using Mako.Shooting;
using UnityEngine;

namespace Mako.Collectables
{
    public class WeaponCooler : MonoBehaviour, ICollectable
    {
        [SerializeField] private int amountToCool;
        private MeshRenderer meshRenderer;
        private Collider hitBox;
        private AudioSource audioSource;
        private Shooter playerGun;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            hitBox = GetComponent<Collider>();
            audioSource = GetComponent<AudioSource>();
            playerGun = FindObjectOfType<Mako.Movement.PlayerController>().gameObject.GetComponentInChildren<Shooter>();
        }
        public void Collect()
        {
            playerGun.currentOverheat -= amountToCool;
            audioSource.Play();
            meshRenderer.enabled = false;
            hitBox.enabled = false;
            StartCoroutine(SelfDestroy());
        }
        private IEnumerator SelfDestroy()
        {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            var encounteredGunOwner = other.GetComponentInChildren<Shooter>();
            if (encounteredGunOwner == playerGun)
            {
                Collect();
            }
        }
    }
}
