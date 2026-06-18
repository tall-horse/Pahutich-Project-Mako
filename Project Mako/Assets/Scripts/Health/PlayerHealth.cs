using System.Collections;
using Mako.HealthNamespace;

namespace Mako
{
    public class PlayerHealth : Health, ISelfDesctructable
    {
        public IEnumerator SelfDestroy()
        {
            yield return null;
        }
    }
}
