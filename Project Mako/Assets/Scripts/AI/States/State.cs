using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mako.AI
{
  public class State
  {
    public enum STATE
    {
      IDLE, PURSUE, ATTACK
    }

    public enum EVENT
    {
      ENTER, UPDATE, EXIT
    }

    public STATE name;
    public bool heardPlayer;
    protected EVENT stage;
    protected GameObject npc;
    protected Animator anim;
    protected Transform player;
    protected State nextState;
    protected UnityEngine.AI.NavMeshAgent agent;

    float visDist = 18.0f;
    float attackDist = 5.0f;

    public State(GameObject _npc, UnityEngine.AI.NavMeshAgent _agent, Animator _anim, Transform _player)
    {
      npc = _npc;
      agent = _agent;
      anim = _anim;
      stage = EVENT.ENTER;
      player = _player;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }

    public virtual void Update() { stage = EVENT.UPDATE; }

    public virtual void Exit() { stage = EVENT.EXIT; }

    public State Process()
    {
      if (stage == EVENT.ENTER) Enter();
      if (stage == EVENT.UPDATE) Update();
      if (stage == EVENT.EXIT)
      {
        Exit();
        return nextState;
      }
      return this;
    }

    public bool CanSeePlayer()
    {
      Vector3 direction = player.position - npc.transform.position;
      float angle = Vector3.Angle(direction, npc.transform.forward);

      if (direction.magnitude < visDist)
      {
        return true;
      }
      return false;
    }

    public bool CanAttackPlayer()
    {
      Vector3 direction = player.position - npc.transform.position;
      if (direction.magnitude < attackDist)
      {
        return true;
      }
      return false;
    }
  }
}
