using System.Collections;
using System.Collections.Generic;
using Mako.Health;
using UnityEngine;

namespace Mako
{
    public class PlayerHealth : BasicHealth
    {
        protected override IEnumerator SelfDestroy()
        {
            yield return null;
        }
    }
}
