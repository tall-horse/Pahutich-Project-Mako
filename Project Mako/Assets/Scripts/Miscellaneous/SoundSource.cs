using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mako.Events;
using UnityEngine;

namespace Mako.Miscellaneous
{
  public class SoundSource : MonoBehaviour
  {
    [SerializeField] private float range;
    public CustomEvent OnSoundDistributed;

    // Update is called once per frame
    void Update()
    {

    }
    public void DistributeSound()
    {
      var soundListeners = GameObject.FindObjectsOfType<SoundListener>().ToList();
      foreach (SoundListener sl in soundListeners)
      {
        if(sl == GetComponent<SoundListener>())
        {
            soundListeners.Remove(sl);
        }
      }
      foreach (SoundListener sl in soundListeners)
      {
        if (Vector3.Distance(transform.position, sl.transform.position) <= range)
        {
          OnSoundDistributed.Occurred(gameObject);
        }
      }
    }
    private void OnDrawGizmos()
    {
      Gizmos.color = Color.cyan;
      Gizmos.DrawWireSphere(transform.position, range);
    }
    private void OnCollisionEnter(Collision other) {
        DistributeSound();
    }
  }
}
