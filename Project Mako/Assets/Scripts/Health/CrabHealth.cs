using System.Collections;
using System.Collections.Generic;
using Mako.AI;
using Mako.Health;
using UnityEngine;

namespace Mako
{
    public class CrabHealth : NormalHealth
    {
        private CrabMonsterAI _crabMonsterAI;
        protected override void Awake()
        {
            base.Awake();
            _crabMonsterAI = GetComponent<CrabMonsterAI>();
        }
        protected override void OnEnable()
        {
            base.SubscribeEvents();
            healthSystem.OnRespondToFire += RespondToFire;
        }

        protected override void OnDisable()
        {
            base.UnsubscribeEvents();
            healthSystem.OnRespondToFire -= RespondToFire;
        }

        private void RespondToFire()
        {
            _crabMonsterAI.GoOnSound();
        }
    }
}
