using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Mako.AI
{
  public class Attack : State
  {
    float rotationSpeed = 2.0f;
    AudioSource shoot;
    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
      name = STATE.ATTACK;
      shoot = npc.GetComponent<AudioSource>();
    }

    public override void Enter()
    {
      anim.SetTrigger("isAttacking");
      agent.isStopped = true;
      shoot.Play();
      base.Enter();
    }
    public override void Update()
    {
      Vector3 direction = player.position - npc.transform.position;
      float angle = Vector3.Angle(direction, npc.transform.forward);
      direction.y = 0;

      npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);

      if (!CanAttackPlayer())
      {
        nextState = new Pursue(npc, agent, anim, player);
        stage = EVENT.EXIT;
      }
    }
    public override void Exit()
    {
      anim.ResetTrigger("isAttacking");
      shoot.Stop();
      base.Exit();
    }
  }
}
