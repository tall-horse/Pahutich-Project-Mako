using System.Collections;
using UnityEngine;

namespace Mako.HealthNamespace
{
    public class OneShotObject : Health, ISelfDesctructable
    {
        public IEnumerator SelfDestroy()
        {
            throw new System.NotImplementedException();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Shooting.Projectile>())
            {
                StartCoroutine(SelfDestroy());
            }
        }
    }
}
