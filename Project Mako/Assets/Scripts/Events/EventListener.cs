using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityGameObjectEvent : UnityEvent<GameObject>
{

}
namespace Mako.Events
{
  public class EventListener : MonoBehaviour
  {
    public CustomEvent gEvent;
    public UnityGameObjectEvent response = new UnityGameObjectEvent();

    private void OnEnable()
    {
      gEvent.Register(this);
    }
    void OnDisable()
    {
      gEvent.Unregister(this);
    }

    public void OnEventOccurs(GameObject go)
    {
      response.Invoke(go);
    }
  }
}
