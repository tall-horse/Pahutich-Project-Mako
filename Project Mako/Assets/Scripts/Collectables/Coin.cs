using System.Collections;
using Mako.Events;
using UnityEngine;

namespace Mako.Collectables
{
    public class Coin : MonoBehaviour, ICollectable
    {
        private MeshRenderer meshRenderer;
        private Collider hitBox;
        private AudioSource audioSource;
        private EventListener OnCollected;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            hitBox = GetComponent<Collider>();
            audioSource = GetComponent<AudioSource>();
            OnCollected = GetComponent<EventListener>();
        }
        public void Collect()
        {
            audioSource.Play();
            meshRenderer.enabled = false;
            hitBox.enabled = false;
            OnCollected.OnEventOccurs(gameObject);
            StartCoroutine(SelfDestroy());
        }
        private IEnumerator SelfDestroy()
        {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            var isPlayer = other.CompareTag("Player");
            if (isPlayer)
            {
                Collect();
            }
        }
    }
}
