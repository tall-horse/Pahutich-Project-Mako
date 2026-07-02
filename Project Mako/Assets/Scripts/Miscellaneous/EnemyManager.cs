using System.Collections.Generic;
using Mako.AI;
using UnityEngine;

namespace Mako
{
    public class EnemyManager : MonoBehaviour
    {
        //now works only with crabs
        private Transform _player;
        [SerializeField] private List<CrabMonsterAI> _crabs;

        public void Initialize(Transform player)
        {
            _crabs = new();
            _player = player;
        }
        public void Register(CrabMonsterAI crab)
        {
            crab.player = _player;
            _crabs.Add(crab);
        }
        public void Deregister(CrabMonsterAI crab)
        {
            _crabs.Remove(crab);
        }
    }
}
