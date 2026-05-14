namespace Mako.QuestSystem
{
    public class CollectCoinsQuestStep : QuestStep
    {
        private int coinsCollected = 0;
        private int coinsToComplete = 5;

        private void OnEnable()
        {
            //Subscribe to coins collected
            //GameEventsManager.instance.miscEvents.onCoinCollected += CoinCollected;
        }

        private void OnDisable()
        {
            //Unsubscribe to coins collected
            //GameEventsManager.instance.miscEvents.onCoinCollected += CoinCollected;
        }

        public void CoinCollected()
        {
            if(coinsCollected < coinsToComplete)
            {
                coinsCollected++;
            }

            if(coinsCollected >= coinsToComplete)
            {
                FinishQuestStep();
            }
        }
    }
}
