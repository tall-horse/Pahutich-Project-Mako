using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mako.QuestSystem
{
    public abstract class QuestStep : MonoBehaviour
    {
        private bool isFinished = false;

        protected void FinishQuestStep()
        {
            isFinished = true;

            //To DO: advance the quest forward

            Destroy(gameObject);
        }
    }
}
