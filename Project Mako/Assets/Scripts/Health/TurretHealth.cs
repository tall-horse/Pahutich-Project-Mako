using System.Collections;
using System.Collections.Generic;
using Mako.Health;
using Mako.Shooting;
using UnityEngine;

namespace Mako
{
    public class TurretHealth : NormalHealth
    {
        private Turret _turret;
        void Awake()
        {
            _turret = GetComponent<Turret>();
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
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
            _turret.playerInRange = true;
            _turret.ExtendRange();
        }
    }
}
