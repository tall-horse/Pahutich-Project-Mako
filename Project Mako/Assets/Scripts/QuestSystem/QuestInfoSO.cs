using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mako.QuestSystem
{
    [CreateAssetMenu(fileName = "QuestInfoSO", menuName = "ScriptableObjects/QuestInfo", order = 1)]
    public class QuestInfoSO : ScriptableObject
    {
        [field: SerializeField] public string Id {get; private set;}

        [Header("General")]
        public string displayName;

        [Header("Requirements")]
        public QuestInfoSO[] questPrerequisites;

        [Header("Steps")]
        public GameObject[] questStepPrefabs;

        [Header("Rewards")]
        public int goldReward;
        private void OnValidate()
        {
            #if UNITY_EDITOR
            Id = this.name;
            UnityEditor.EditorUtility.SetDirty(this);
            #endif
        }
    }
}
