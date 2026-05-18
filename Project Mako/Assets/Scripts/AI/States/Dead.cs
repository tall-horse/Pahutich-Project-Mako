using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Mako.AI
{
    public class Dead : State
    {
        public Dead(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
        {
            name = STATE.DEAD;
        }
        public override void Enter()
        {
            agent.isStopped = true;
            anim.ResetTrigger("isAttacking");
            anim.ResetTrigger("isWalking");
            anim.ResetTrigger("isIdle");
            anim.SetTrigger("isDead");
            base.Enter();
        }
    }
}