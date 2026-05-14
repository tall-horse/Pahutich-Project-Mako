using System.Collections.Generic;
using UnityEngine;

namespace Mako.Events
{
  [CreateAssetMenu(fileName = "New Event", menuName = "Game Event", order = 52)]
  public class CustomEvent : ScriptableObject
  {
    private List<EventListener> eventListeners = new List<EventListener>();

    public void Register(EventListener listener)
    {
      eventListeners.Add(listener);
    }

    public void Unregister(EventListener listener)
    {
      eventListeners.Remove(listener);
    }

    public void Occurred(GameObject go)
    {
      for (int i = 0; i < eventListeners.Count; i++)
      {
        eventListeners[i].OnEventOccurs(go);
      }
    }
  }
}

