using Mako.Shooting;
using UnityEngine;
using UnityEngine.AI;

namespace Mako.AI
{
    public class Attack : State
    {
        float rotationSpeed = 2.0f;
        AudioSource shoot;
        private MeleeAttack _meleeAttack;
        private int _attackIndex;
        public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
        {
            name = STATE.ATTACK;
            _meleeAttack = npc.GetComponent<MeleeAttack>();
            shoot = npc.GetComponent<AudioSource>();
            _meleeAttack.OnReloadAttack += GenerateAttackIndex;
        }

        public override void Enter()
        {
            anim.SetTrigger("isAttacking");
            anim.SetFloat("Blend", _attackIndex);
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
            anim.SetFloat("Blend", 0);
            shoot.Stop();
            base.Exit();
        }
        public void GenerateAttackIndex()
        {
            _attackIndex = UnityEngine.Random.Range(1, 4);
            anim.SetFloat("Blend", _attackIndex);
        }
    }
}
