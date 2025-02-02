using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Mako.AI
{
  public class Pursue : State
  {
    public Pursue(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
      name = STATE.PURSUE;
      agent.speed = 5;
      agent.isStopped = false;
    }

    public override void Enter()
    {
      anim.SetTrigger("isWalking");
      base.Enter();
    }
    public override void Update()
    {
      agent.SetDestination(player.position);
      if (agent.hasPath)
      {
        if (CanAttackPlayer())
        {
          nextState = new Attack(npc, agent, anim, player);
          stage = EVENT.EXIT;
        }
      }
    }
    public override void Exit()
    {
      anim.ResetTrigger("isWalking");
      base.Exit();
    }
  }
}
