using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Mako.AI
{
  public class Idle : State
  {
    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
      name = STATE.IDLE;
    }
    public override void Enter()
    {
      anim.SetTrigger("isIdle");
      base.Enter();
    }

    public override void Update()
    {
      if (heardPlayer || CanSeePlayer())
      {
        nextState = new Pursue(npc, agent, anim, player);
        stage = EVENT.EXIT;
      }
      if (CanAttackPlayer())
      {
        nextState = new Attack(npc, agent, anim, player);
        stage = EVENT.EXIT;
      }
    }
    public override void Exit()
    {
      anim.ResetTrigger("isIdle");
      base.Exit();
    }
  }
}
