using System.Collections.Generic;
using Cache;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Core
{
    public class ShelfShuffler
    {
        private ShelfInstances _shelfInstances;
        private int shuffleEndDelay = 1000;
        
        [Header("DI")] 
        private CacheGame cacheGame;

        public ShelfShuffler(ShelfInstances shelfInstances, CacheGame cacheGame)
        {
            _shelfInstances = shelfInstances;
            this.cacheGame = cacheGame;
        }

        public async void ShuffleItems()
        {
            SetBlendedStatusForAll(false);

            await UniTask.Delay(cacheGame.ConfigGame.delayBeforeStart * 1000);

            var shuffledItems = LocalUtils.Shuffle(_shelfInstances.GetShelfItems());
            var shelfTriggers = _shelfInstances.GetShelfTriggers();

            for (var i = 0; i < shelfTriggers.Count; i++)
            {
                if (shuffledItems[i] != null)
                {
                    shelfTriggers[i].AttachItem(shuffledItems[i]).Forget();
                }
                else
                {
                    Debug.Log("Out of item range");
                }
            }

            await UniTask.Delay(shuffleEndDelay);
            
            SetBlendedStatusForAll(true);
        }

        private void SetBlendedStatusForAll(bool state)
        {
            foreach (var item in _shelfInstances.GetShelfItems())
            {
                item.SetBlendedStatus(state);
            }
        }
    }

}