using System.Collections.Generic;
using System.Linq;
using Mako.Events;
using UnityEngine;

namespace Mako.Miscellaneous
{
  public class SoundSource : MonoBehaviour
  {
    [SerializeField] private float range;
    private List<SoundListener> soundListeners;
    public CustomEvent OnSoundDistributed;

    void Start()
    {
      GetListeners();
    }

    private void GetListeners()
    {
      soundListeners = FindObjectsOfType<SoundListener>().ToList();
      RemoveThisListener();
    }

    public void DistributeSound()
    {
      GetListeners();
      foreach (SoundListener sl in soundListeners)
      {
        if (Vector3.Distance(transform.position, sl.transform.position) <= range)
        {
          //OnSoundDistributed.Occurred(gameObject);
        }
      }
    }
    private void RemoveThisListener()
    {
      soundListeners.Remove(soundListeners.Find(sl => sl == this));
    }
    private void OnDrawGizmos()
    {
      Gizmos.color = Color.cyan;
      Gizmos.DrawWireSphere(transform.position, range);
    }
    private void OnCollisionEnter(Collision other) {
        DistributeSound();
    }
    private void OnDestroy()
    {
      RemoveThisListener();
    }
  }
}
