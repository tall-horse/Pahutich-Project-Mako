using UnityEngine.Events;

namespace Mako.Health
{
  public class NormalHealth : Health
  {
    // Start is called before the first frame update
    protected override void Awake()
    {
      base.SetupHealthObject();
    }
  }
}
