using System;
using System.Collections;
using System.Collections.Generic;
using Mako.Shooting;
using UnityEngine;

namespace Mako
{
    public class TurretAnimationController : MonoBehaviour
    {
        public event Action<Turret.TurretState, bool> OnTurretStateChange;
        public void TransitToCombatMode()
        {
            OnTurretStateChange?.Invoke(Turret.TurretState.Out, false);
        }
        public void TransitToTransit()
        {
            OnTurretStateChange?.Invoke(Turret.TurretState.Transitioning, false);
        }
        public void TransitToHiddenMode()
        {
            OnTurretStateChange?.Invoke(Turret.TurretState.Hidden, true);
        }
    }
}
